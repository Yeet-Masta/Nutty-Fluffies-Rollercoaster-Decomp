using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

/// <summary>
/// Restores Unity 4.0's transparent draw order.
///
/// The menus and HUD are built from MenuSprite quads (Overlay queue, ZTest
/// Always, ZWrite Off), so their layering comes entirely from the order
/// transparent objects are drawn in. Unity 4.0 sorted them by distance from
/// the camera position for every camera. Unity 4.3+ sorts objects seen by
/// orthographic cameras (all of this game's menu/UI cameras) by view depth
/// only, so sprites at the same z tie, draw in an unstable order and flicker
/// or appear behind their backgrounds. Perspective sort mode is the old rule.
/// </summary>
public static class NFPortTransparencySort
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Install()
    {
        GraphicsSettings.transparencySortMode = TransparencySortMode.Perspective;
        SceneManager.sceneLoaded += (scene, mode) => ApplyToAllCameras();
    }

    public static void ApplyToAllCameras()
    {
        foreach (var cam in Object.FindObjectsOfType<Camera>(true))
            cam.transparencySortMode = TransparencySortMode.Perspective;
    }
}
