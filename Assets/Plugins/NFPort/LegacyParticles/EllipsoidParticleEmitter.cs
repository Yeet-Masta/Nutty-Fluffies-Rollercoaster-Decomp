using UnityEngine;

/// <summary>Replacement for the legacy EllipsoidParticleEmitter.</summary>
public class EllipsoidParticleEmitter : ParticleEmitter
{
    public Vector3 m_Ellipsoid = Vector3.one;
    public float m_MinEmitterRange;

    public Vector3 ellipsoid
    {
        get => m_Ellipsoid;
        set => m_Ellipsoid = value;
    }

    public float minEmitterRange
    {
        get => m_MinEmitterRange;
        set => m_MinEmitterRange = value;
    }

    protected override Vector3 SpawnOffset(out Vector3 direction)
    {
        direction = Random.onUnitSphere;
        var radius = Mathf.Lerp(Mathf.Clamp01(m_MinEmitterRange), 1f, Random.value);
        return Vector3.Scale(direction * radius, m_Ellipsoid * 0.5f);
    }
}
