using UnityEngine;

/// <summary>
/// Drop-in replacement for Unity's removed legacy ParticleEmitter.
/// Field names match the legacy serialized property names (renamed where the
/// originals were not valid identifiers, see tools/convert_legacy_particles.py)
/// so the scene data carries over.
/// </summary>
[DisallowMultipleComponent]
public class ParticleEmitter : MonoBehaviour
{
    public bool m_Emit = true;
    public float minSize = 0.1f;
    public float maxSize = 0.1f;
    public float minEnergy = 3f;
    public float maxEnergy = 3f;
    public float minEmission = 50f;
    public float maxEmission = 50f;
    public Vector3 worldVelocity;
    public Vector3 localVelocity;
    public Vector3 rndVelocity;
    public float emitterVelocityScale = 0.05f;
    public Vector3 tangentVelocity;
    public float angularVelocity;
    public float rndAngularVelocity;
    public bool rndRotation;
    public bool simulateInWorldspace = true;
    public bool m_OneShot;

    ParticleSystem _system;
    float _pending;
    Vector3 _lastPosition;
    Vector3 _emitterVelocity;

    public bool emit
    {
        get => m_Emit;
        set => m_Emit = value;
    }

    public bool useWorldSpace
    {
        get => simulateInWorldspace;
        set
        {
            simulateInWorldspace = value;
            var main = System.main;
            main.simulationSpace = value ? ParticleSystemSimulationSpace.World : ParticleSystemSimulationSpace.Local;
        }
    }

    public int particleCount => System.particleCount;

    protected ParticleSystem System
    {
        get
        {
            if (_system == null) Setup();
            return _system;
        }
    }

    void Awake()
    {
        if (_system == null) Setup();
    }

    void Setup()
    {
        _system = LegacyParticleSystem.Get(gameObject);
        var main = _system.main;
        main.simulationSpace = simulateInWorldspace ? ParticleSystemSimulationSpace.World : ParticleSystemSimulationSpace.Local;
        main.startLifetime = new ParticleSystem.MinMaxCurve(minEnergy, maxEnergy);
        _lastPosition = transform.position;
    }

    void Update()
    {
        var dt = Time.deltaTime;
        if (dt > 0f)
        {
            var position = transform.position;
            _emitterVelocity = (position - _lastPosition) / dt;
            _lastPosition = position;
        }
        if (!m_Emit) return;

        if (m_OneShot)
        {
            if (System.particleCount == 0) Emit();
            return;
        }

        _pending += Random.Range(minEmission, maxEmission) * dt;
        var count = (int)_pending;
        if (count > 0)
        {
            _pending -= count;
            Emit(count);
        }
    }

    public void Emit() => Emit(Mathf.RoundToInt(Random.Range(minEmission, maxEmission)));

    public void Emit(int count)
    {
        var system = System;
        var world = simulateInWorldspace;
        for (var i = 0; i < count; i++)
        {
            var offset = SpawnOffset(out var direction);
            var velocity = transform.TransformDirection(localVelocity) + worldVelocity
                + new Vector3(
                    Random.Range(-0.5f, 0.5f) * rndVelocity.x,
                    Random.Range(-0.5f, 0.5f) * rndVelocity.y,
                    Random.Range(-0.5f, 0.5f) * rndVelocity.z)
                + _emitterVelocity * emitterVelocityScale;
            if (tangentVelocity != Vector3.zero && direction != Vector3.zero)
            {
                var tangent = Vector3.Cross(direction, Vector3.up);
                if (tangent.sqrMagnitude < 1e-6f) tangent = Vector3.Cross(direction, Vector3.right);
                velocity += transform.TransformDirection(Vector3.Scale(tangent.normalized, tangentVelocity));
            }

            var p = new ParticleSystem.EmitParams
            {
                position = world ? transform.TransformPoint(offset) : offset,
                velocity = world ? velocity : transform.InverseTransformDirection(velocity),
                startSize = Random.Range(minSize, maxSize),
                startLifetime = Random.Range(minEnergy, maxEnergy),
                startColor = Color.white,
                rotation = rndRotation ? Random.Range(0f, 360f) : 0f,
                angularVelocity = angularVelocity + Random.Range(-0.5f, 0.5f) * rndAngularVelocity,
            };
            system.Emit(p, 1);
        }
    }

    public void Emit(Vector3 pos, Vector3 velocity, float size, float energy, Color color) =>
        Emit(pos, velocity, size, energy, color, 0f, 0f);

    public void Emit(Vector3 pos, Vector3 velocity, float size, float energy, Color color, float rotation, float angularVelocity)
    {
        // Legacy explicit emission took positions/velocities in the emitter's simulation space.
        var p = new ParticleSystem.EmitParams
        {
            position = pos,
            velocity = velocity,
            startSize = size,
            startLifetime = energy,
            startColor = color,
            rotation = rotation,
            angularVelocity = angularVelocity,
        };
        System.Emit(p, 1);
    }

    public void ClearParticles() => System.Clear();

    /// <summary>Local-space spawn offset; the base emitter spawns at its origin.</summary>
    protected virtual Vector3 SpawnOffset(out Vector3 direction)
    {
        direction = Vector3.zero;
        return Vector3.zero;
    }
}
