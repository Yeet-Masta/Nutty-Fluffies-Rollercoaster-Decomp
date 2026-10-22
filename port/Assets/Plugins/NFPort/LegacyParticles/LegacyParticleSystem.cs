using UnityEngine;

/// <summary>
/// Shared setup for the legacy particle shims (ParticleEmitter,
/// ParticleAnimator, ParticleRenderer). Unity removed the legacy particle
/// system in 2018.3; the shims keep the original serialized settings and drive
/// a modern ParticleSystem on the same GameObject instead.
/// </summary>
public static class LegacyParticleSystem
{
    public const int MaxParticles = 2000;

    public static ParticleSystem Get(GameObject go)
    {
        var system = go.GetComponent<ParticleSystem>();
        if (system != null) return system;

        system = go.AddComponent<ParticleSystem>();
        system.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        var main = system.main;
        main.playOnAwake = false;
        main.loop = true;
        main.maxParticles = MaxParticles;
        main.startSpeed = 0f;
        main.gravityModifier = 0f;
        main.scalingMode = ParticleSystemScalingMode.Shape;

        // Emission is driven by ParticleEmitter through ParticleSystem.Emit.
        var emission = system.emission;
        emission.enabled = false;
        var shape = system.shape;
        shape.enabled = false;

        system.Play();
        return system;
    }

    public static Color32 FromRgba(uint rgba) =>
        new Color32((byte)rgba, (byte)(rgba >> 8), (byte)(rgba >> 16), (byte)(rgba >> 24));
}
