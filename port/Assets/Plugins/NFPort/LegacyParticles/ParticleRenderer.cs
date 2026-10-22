using UnityEngine;

/// <summary>
/// Replacement for the legacy ParticleRenderer: forwards its materials and
/// billboard settings to the ParticleSystemRenderer on the same GameObject.
/// </summary>
[DisallowMultipleComponent]
public class ParticleRenderer : MonoBehaviour
{
    public Material[] m_Materials = new Material[0];
    public float m_CameraVelocityScale;
    public int m_StretchParticles;
    public float m_LengthScale = 2f;
    public float m_VelocityScale;
    public float m_MaxParticleSize = 0.25f;
    public int uvAnimationXTile = 1;
    public int uvAnimationYTile = 1;
    public float uvAnimationCycles = 1f;

    ParticleSystemRenderer _renderer;

    ParticleSystemRenderer Renderer
    {
        get
        {
            if (_renderer == null) Setup();
            return _renderer;
        }
    }

    public Material[] materials
    {
        get => Renderer.materials;
        set => Renderer.materials = value;
    }

    public Material material
    {
        get => Renderer.material;
        set => Renderer.material = value;
    }

    public Material sharedMaterial
    {
        get => Renderer.sharedMaterial;
        set => Renderer.sharedMaterial = value;
    }

    void Awake()
    {
        if (_renderer == null) Setup();
    }

    void OnEnable()
    {
        if (_renderer != null) _renderer.enabled = true;
    }

    void OnDisable()
    {
        if (_renderer != null) _renderer.enabled = false;
    }

    void Setup()
    {
        var system = LegacyParticleSystem.Get(gameObject);
        _renderer = system.GetComponent<ParticleSystemRenderer>();
        _renderer.sharedMaterials = m_Materials;
        _renderer.maxParticleSize = m_MaxParticleSize;
        _renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        _renderer.receiveShadows = false;

        switch (m_StretchParticles)
        {
            case 1:
                _renderer.renderMode = ParticleSystemRenderMode.Stretch;
                _renderer.lengthScale = m_LengthScale;
                _renderer.velocityScale = m_VelocityScale;
                _renderer.cameraVelocityScale = m_CameraVelocityScale;
                break;
            case 2:
                _renderer.renderMode = ParticleSystemRenderMode.Billboard;
                _renderer.sortMode = ParticleSystemSortMode.Distance;
                break;
            case 3:
                _renderer.renderMode = ParticleSystemRenderMode.HorizontalBillboard;
                break;
            case 4:
                _renderer.renderMode = ParticleSystemRenderMode.VerticalBillboard;
                break;
            default:
                _renderer.renderMode = ParticleSystemRenderMode.Billboard;
                break;
        }

        var sheet = system.textureSheetAnimation;
        sheet.enabled = uvAnimationXTile * uvAnimationYTile > 1;
        if (sheet.enabled)
        {
            sheet.numTilesX = Mathf.Max(1, uvAnimationXTile);
            sheet.numTilesY = Mathf.Max(1, uvAnimationYTile);
            sheet.cycleCount = Mathf.Max(1, Mathf.RoundToInt(uvAnimationCycles));
        }
    }
}
