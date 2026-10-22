using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.Rendering;

namespace NFPort.Editor
{
    /// <summary>
    /// Applies the Android player settings the port needs (IL2CPP, ARM64) the
    /// first time the upgraded project is opened, and on demand from the menu.
    /// </summary>
    [InitializeOnLoad]
    public static class NFPortSetup
    {
        const string Marker = "ProjectSettings/NFPortSetup.txt";
        public const string DefaultApplicationId = "com.UBI.A90.WW.arm64";

        static NFPortSetup()
        {
            if (!File.Exists(Marker)) EditorApplication.delayCall += Apply;
        }

        [MenuItem("NFPort/Apply Android ARM64 Player Settings")]
        public static void Apply()
        {
            var android = NamedBuildTarget.Android;

            // ARM64 on Android requires IL2CPP (Mono only ever supported ARMv7).
            PlayerSettings.SetScriptingBackend(android, ScriptingImplementation.IL2CPP);
            PlayerSettings.SetIl2CppCompilerConfiguration(android, Il2CppCompilerConfiguration.Release);
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64 | AndroidArchitecture.ARMv7;
            PlayerSettings.SetApiCompatibilityLevel(android, ApiCompatibilityLevel.NET_Standard);
            PlayerSettings.SetManagedStrippingLevel(android, ManagedStrippingLevel.Minimal);

            PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel22;
            PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevelAuto;

            // A new id, so it installs next to (not over) the original Ubisoft-signed APK.
            var id = PlayerSettings.GetApplicationIdentifier(android);
            if (string.IsNullOrEmpty(id) || id == "com.Company.ProductName" || id == "com.UBI.A90.WW")
                PlayerSettings.SetApplicationIdentifier(android, DefaultApplicationId);
            if (string.IsNullOrEmpty(PlayerSettings.bundleVersion) || PlayerSettings.bundleVersion == "1.0")
                PlayerSettings.bundleVersion = "1.0.5";
            PlayerSettings.Android.bundleVersionCode = Mathf.Max(PlayerSettings.Android.bundleVersionCode, 105);

            // Same as the original: auto-rotate between the two landscape orientations.
            PlayerSettings.defaultInterfaceOrientation = UIOrientation.AutoRotation;
            PlayerSettings.allowedAutorotateToLandscapeLeft = true;
            PlayerSettings.allowedAutorotateToLandscapeRight = true;
            PlayerSettings.allowedAutorotateToPortrait = false;
            PlayerSettings.allowedAutorotateToPortraitUpsideDown = false;

            // The original shaders are fixed-function ShaderLab; GLES3 first for old devices.
            PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget.Android, false);
            PlayerSettings.SetGraphicsAPIs(BuildTarget.Android, new[] { GraphicsDeviceType.OpenGLES3, GraphicsDeviceType.Vulkan });
            PlayerSettings.colorSpace = ColorSpace.Gamma;

            File.WriteAllText(Marker, "Android ARM64/IL2CPP settings applied by NFPortSetup.\n");
            AssetDatabase.SaveAssets();
            Debug.Log("NFPort: applied Android ARM64 (IL2CPP) player settings.");
        }
    }
}
