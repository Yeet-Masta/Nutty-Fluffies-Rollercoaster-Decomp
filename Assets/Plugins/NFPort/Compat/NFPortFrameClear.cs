using UnityEngine;

/// <summary>
/// Clears the screen at the start of every frame.
///
/// None of the game's cameras clear colour in the menu and game scenes
/// (MenuScene's camera is "Depth only"; both GameScene cameras are "Don't
/// clear"), and the AspectUtility background camera that would have done it
/// isn't used in any scene. On 2013 phones the GPU effectively discarded each
/// frame; on modern swap chains (DXGI flip model, Vulkan, GLES3) the buffer
/// contents are undefined, which shows up as flicker and stale UI wherever
/// the game doesn't repaint every pixel. This adds the same kind of
/// background camera AspectUtility creates: lowest depth, renders nothing,
/// clears colour and depth to black.
/// </summary>
public static class NFPortFrameClear
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Install()
    {
        var go = new GameObject("NFPort Frame Clear");
        Object.DontDestroyOnLoad(go);
        var cam = go.AddComponent<Camera>();
        cam.depth = -100000f;
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = Color.black;
        cam.cullingMask = 0;
        cam.eventMask = 0;
        cam.orthographic = true;
        cam.useOcclusionCulling = false;
        cam.allowHDR = false;
        cam.allowMSAA = false;
    }
}
