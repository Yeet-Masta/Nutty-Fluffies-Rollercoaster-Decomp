using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Keeps everything an orthographic camera draws in front of it while it
/// renders, so Unity sorts the game's transparent sprites in the right order.
///
/// The menus and HUD are MenuSprite quads (ZTest Always, ZWrite Off), so
/// their layering comes only from the order transparent objects are drawn
/// in: back to front along the camera's view direction, as in Unity 4.0.
/// GameScene's UICamera sits at z = -10 with a near clip plane of -50 and the
/// whole UI hangs behind it (z -10.4 to -11.6); LevelFactory gives the level
/// camera a negative near plane too, for its foreground layers. Unity 4.0
/// sorted those negative depths correctly, Unity 2022 does not: the GameScene
/// menus came out in the wrong order and flickered.
///
/// While such a camera renders, it is moved back along its view axis until
/// its near plane is in front of it, with near and far moved by the same
/// amount (an orthographic image doesn't change) and its children held in
/// place. Everything is put back after the camera has rendered, so scripts,
/// raycasts and clicks see the original setup.
/// </summary>
public static class NFPortTransparencySort
{
    const float MinNear = 0.3f;

    sealed class Saved
    {
        public Vector3 position;
        public float near, far;
        public Transform[] children;
        public Vector3[] childLocalPositions;
    }

    static readonly Dictionary<Camera, Saved> Shifted = new Dictionary<Camera, Saved>();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Install()
    {
        Camera.onPreCull += Shift;
        Camera.onPostRender += Restore;
    }

    static void Shift(Camera cam)
    {
        if (cam.cameraType != CameraType.Game || !cam.orthographic) return;
        Restore(cam); // a previous render that never finished
        var shift = MinNear - cam.nearClipPlane;
        if (shift <= 0f) return;

        var t = cam.transform;
        var saved = new Saved
        {
            position = t.position,
            near = cam.nearClipPlane,
            far = cam.farClipPlane,
            children = new Transform[t.childCount],
            childLocalPositions = new Vector3[t.childCount],
        };
        var childWorldPositions = new Vector3[t.childCount];
        for (var i = 0; i < t.childCount; i++)
        {
            saved.children[i] = t.GetChild(i);
            saved.childLocalPositions[i] = saved.children[i].localPosition;
            childWorldPositions[i] = saved.children[i].position;
        }
        Shifted[cam] = saved;

        t.position -= t.forward * shift;
        for (var i = 0; i < saved.children.Length; i++)
            saved.children[i].position = childWorldPositions[i];
        cam.nearClipPlane = saved.near + shift;
        cam.farClipPlane = saved.far + shift;
    }

    static void Restore(Camera cam)
    {
        if (!Shifted.TryGetValue(cam, out var saved)) return;
        Shifted.Remove(cam);
        if (cam == null) return;

        cam.transform.position = saved.position;
        for (var i = 0; i < saved.children.Length; i++)
            if (saved.children[i] != null)
                saved.children[i].localPosition = saved.childLocalPositions[i];
        cam.nearClipPlane = saved.near;
        cam.farClipPlane = saved.far;
    }
}
