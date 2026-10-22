/// <summary>
/// Stand-in for Unity 4's iPhoneSettings.generation / iPhoneGeneration, which
/// the game uses for a few iPhone-specific performance tweaks. The modern
/// equivalent (UnityEngine.iOS) only exists when building for iOS, and on
/// every platform this port targets the original reported Unknown, so the
/// checks keep exactly their Android behaviour.
/// </summary>
public static class NFPortLegacyDevice
{
    public static NFPortLegacyDeviceGeneration generation => NFPortLegacyDeviceGeneration.Unknown;
}

/// <summary>Unity 4.0 iPhoneGeneration, same names and values.</summary>
public enum NFPortLegacyDeviceGeneration
{
    Unknown = 0,
    iPhone = 1,
    iPhone3G = 2,
    iPhone3GS = 3,
    iPodTouch1Gen = 4,
    iPodTouch2Gen = 5,
    iPodTouch3Gen = 6,
    iPad1Gen = 7,
    iPhone4 = 8,
    iPodTouch4Gen = 9,
    iPad2Gen = 10,
    iPhone4S = 11,
    iPad3Gen = 12,
    iPhone5 = 13,
    iPodTouch5Gen = 14,
    iPadMini1Gen = 15,
    iPad4Gen = 16,
    iPhoneUnknown = 10001,
    iPadUnknown = 10002,
    iPodTouchUnknown = 10003,
}
