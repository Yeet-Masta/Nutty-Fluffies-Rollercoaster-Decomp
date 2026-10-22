using UnityEngine;

/// <summary>
/// Unity 4 (PhysX 2.8) moved colliders the moment their transform changed,
/// so a raycast always saw the current scene. Since 2018.3 transform changes
/// are only pushed to physics before the next simulation step unless
/// autoSyncTransforms is on. The menus animate, re-parent and re-activate
/// buttons and raycast for clicks in between, so restore the old behaviour.
/// </summary>
public static class NFPortPhysics
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Install()
    {
        Physics.autoSyncTransforms = true;
    }
}
