using UnityEngine;

/// <summary>
/// Replacement for the legacy ParticleAnimator: maps its colour animation,
/// size growth, forces and damping onto ParticleSystem modules.
/// </summary>
[DisallowMultipleComponent]
public class ParticleAnimator : MonoBehaviour
{
    public bool doesAnimateColor;
    public uint[] colorAnimationRgba = { 0xFFFFFFFF, 0xFFFFFFFF, 0xFFFFFFFF, 0xFFFFFFFF, 0xFFFFFFFF };
    public Vector3 worldRotationAxis;
    public Vector3 localRotationAxis;
    public float sizeGrow;
    public Vector3 rndForce;
    public Vector3 force;
    public float damping = 1f;
    public bool autodestruct;

    ParticleSystem _system;
    bool _hadParticles;

    public Color[] colorAnimation
    {
        get
        {
            var colors = new Color[colorAnimationRgba.Length];
            for (var i = 0; i < colors.Length; i++) colors[i] = LegacyParticleSystem.FromRgba(colorAnimationRgba[i]);
            return colors;
        }
        set
        {
            colorAnimationRgba = new uint[value.Length];
            for (var i = 0; i < value.Length; i++)
            {
                Color32 c = value[i];
                colorAnimationRgba[i] = c.r | (uint)c.g << 8 | (uint)c.b << 16 | (uint)c.a << 24;
            }
            if (_system != null) Apply();
        }
    }

    void Awake()
    {
        _system = LegacyParticleSystem.Get(gameObject);
        Apply();
    }

    void Apply()
    {
        var lifetime = _system.main.startLifetime;
        var averageLife = (lifetime.constantMin + lifetime.constantMax) * 0.5f;
        if (averageLife <= 0f) averageLife = lifetime.constant;

        var col = _system.colorOverLifetime;
        col.enabled = doesAnimateColor && colorAnimationRgba.Length > 0;
        if (col.enabled)
        {
            var n = Mathf.Min(colorAnimationRgba.Length, 8);
            var colorKeys = new GradientColorKey[n];
            var alphaKeys = new GradientAlphaKey[n];
            for (var i = 0; i < n; i++)
            {
                var t = n == 1 ? 0f : i / (float)(n - 1);
                Color c = LegacyParticleSystem.FromRgba(colorAnimationRgba[i]);
                colorKeys[i] = new GradientColorKey(c, t);
                alphaKeys[i] = new GradientAlphaKey(c.a, t);
            }
            var gradient = new Gradient();
            gradient.SetKeys(colorKeys, alphaKeys);
            col.color = gradient;
        }

        // Legacy size growth was exponential: size *= 1 + sizeGrow * dt.
        var sol = _system.sizeOverLifetime;
        sol.enabled = !Mathf.Approximately(sizeGrow, 0f) && averageLife > 0f;
        if (sol.enabled)
        {
            var curve = new AnimationCurve();
            for (var i = 0; i <= 4; i++)
            {
                var t = i / 4f;
                curve.AddKey(t, Mathf.Exp(sizeGrow * t * averageLife));
            }
            sol.size = new ParticleSystem.MinMaxCurve(1f, curve);
        }

        var fol = _system.forceOverLifetime;
        fol.enabled = force != Vector3.zero;
        if (fol.enabled)
        {
            fol.space = ParticleSystemSimulationSpace.World;
            fol.x = force.x;
            fol.y = force.y;
            fol.z = force.z;
        }

        var noise = _system.noise;
        noise.enabled = rndForce != Vector3.zero;
        if (noise.enabled)
        {
            noise.separateAxes = true;
            noise.strengthX = rndForce.x * 0.5f;
            noise.strengthY = rndForce.y * 0.5f;
            noise.strengthZ = rndForce.z * 0.5f;
            noise.frequency = 1f;
            noise.scrollSpeed = 1f;
            noise.damping = false;
        }

        var limit = _system.limitVelocityOverLifetime;
        limit.enabled = damping < 1f;
        if (limit.enabled)
        {
            limit.limit = float.MaxValue;
            limit.drag = -Mathf.Log(Mathf.Max(damping, 1e-4f));
            limit.multiplyDragByParticleSize = false;
            limit.multiplyDragByParticleVelocity = false;
        }

        var vol = _system.velocityOverLifetime;
        var orbit = worldRotationAxis + transform.TransformDirection(localRotationAxis);
        vol.enabled = orbit != Vector3.zero;
        if (vol.enabled)
        {
            vol.orbitalX = orbit.x * Mathf.Deg2Rad;
            vol.orbitalY = orbit.y * Mathf.Deg2Rad;
            vol.orbitalZ = orbit.z * Mathf.Deg2Rad;
        }
    }

    void LateUpdate()
    {
        if (!autodestruct) return;
        if (_system.particleCount > 0) _hadParticles = true;
        else if (_hadParticles) Destroy(gameObject);
    }
}
