using UnityEngine;

/// <summary>
/// Picks between the game's touch and mouse input paths. The Android build
/// hard-wired the touch path; the mouse path (KeyboardController, the mouse
/// FingerGestures prefab) is left over from the Mac version and is used on
/// desktop, in desktop browsers and in the editor.
/// </summary>
public static class NFPortInput
{
    public static bool UseTouch
    {
        get
        {
            if (Application.isEditor) return false;
            if (Application.isMobilePlatform) return true;
            // A WebGL build in a phone/tablet browser.
            return Application.platform == RuntimePlatform.WebGLPlayer && Input.touchSupported && !Input.mousePresent;
        }
    }

    /// <summary>A finished tap: first finger lifted, or the left mouse button released.</summary>
    public static bool TapEnded()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) return true;
        return !UseTouch && Input.GetMouseButtonUp(0);
    }
}
