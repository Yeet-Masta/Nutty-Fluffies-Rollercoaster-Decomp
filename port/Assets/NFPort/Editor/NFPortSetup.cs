using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.Rendering;

namespace NFPort.Editor
{
    /// <summary>
    /// Applies the player settings the port needs on each platform (Android
    /// ARM64/IL2CPP, Windows/Linux, WebGL) the first time the upgraded project
    /// is opened, and on demand from the menu.
    /// </summary>
    [InitializeOnLoad]
    public static class NFPortSetup
    {
        const string Marker = "ProjectSettings/NFPortSetup.txt";
        const int MarkerVersion = 3;
        public const string DefaultApplicationId = "com.UBI.A90.WW.arm64";

        static NFPortSetup()
        {
            if (!File.Exists(Marker) || !File.ReadAllText(Marker).Contains("version " + MarkerVersion))
                EditorApplication.delayCall += Apply;
        }

        [MenuItem("NFPort/Apply Player Settings (all platforms)")]
        public static void Apply()
        {
            ApplyCommon();
            ApplyAndroid();
            ApplyStandalone();
            ApplyWebGL();

            File.WriteAllText(Marker, "NFPort player settings applied by NFPortSetup, version " + MarkerVersion + ".\n");
            AssetDatabase.SaveAssets();
            Debug.Log("NFPort: applied player settings for Android (ARM64/IL2CPP), Windows/Linux and WebGL.");
        }

        static void ApplyCommon()
        {
            if (string.IsNullOrEmpty(PlayerSettings.bundleVersion) || PlayerSettings.bundleVersion == "1.0")
                PlayerSettings.bundleVersion = "1.0.5";
            // The original shaders are fixed-function ShaderLab authored for gamma.
            PlayerSettings.colorSpace = ColorSpace.Gamma;

            // Same as the original on phones: auto-rotate between the two landscape orientations.
            PlayerSettings.defaultInterfaceOrientation = UIOrientation.AutoRotation;
            PlayerSettings.allowedAutorotateToLandscapeLeft = true;
            PlayerSettings.allowedAutorotateToLandscapeRight = true;
            PlayerSettings.allowedAutorotateToPortrait = false;
            PlayerSettings.allowedAutorotateToPortraitUpsideDown = false;

            // The game's web services (news, version check, store data) are
            // plain http. Unity 4 allowed that; 2022 throws from the WWW
            // constructor unless it is enabled here.
            PlayerSettings.insecureHttpOption = InsecureHttpOption.AlwaysAllowed;
        }

        static void ApplyAndroid()
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
            PlayerSettings.Android.bundleVersionCode = Mathf.Max(PlayerSettings.Android.bundleVersionCode, 105);

            // GLES3 first for older devices; Vulkan as the alternative.
            PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget.Android, false);
            PlayerSettings.SetGraphicsAPIs(BuildTarget.Android, new[] { GraphicsDeviceType.OpenGLES3, GraphicsDeviceType.Vulkan });
        }

        static void ApplyStandalone()
        {
            var standalone = NamedBuildTarget.Standalone;

            // Mono needs no C++ toolchain; switch to IL2CPP here if you prefer it.
            PlayerSettings.SetScriptingBackend(standalone, ScriptingImplementation.Mono2x);
            PlayerSettings.SetApiCompatibilityLevel(standalone, ApiCompatibilityLevel.NET_Standard);
            PlayerSettings.SetManagedStrippingLevel(standalone, ManagedStrippingLevel.Minimal);

            PlayerSettings.fullScreenMode = FullScreenMode.FullScreenWindow;
            PlayerSettings.defaultScreenWidth = 1280;
            PlayerSettings.defaultScreenHeight = 720;
            PlayerSettings.resizableWindow = true;
            PlayerSettings.allowFullscreenSwitch = true;
            PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget.StandaloneWindows64, true);
            PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget.StandaloneLinux64, true);
        }

        static void ApplyWebGL()
        {
            var webgl = NamedBuildTarget.WebGL;

            PlayerSettings.SetApiCompatibilityLevel(webgl, ApiCompatibilityLevel.NET_Standard);
            PlayerSettings.SetManagedStrippingLevel(webgl, ManagedStrippingLevel.Minimal);
            PlayerSettings.defaultWebScreenWidth = 1280;
            PlayerSettings.defaultWebScreenHeight = 720;

            // The decompiled game relies on Unity logging-and-continuing after
            // exceptions (e.g. null references) like it does on other platforms;
            // the default "explicitly thrown only" mode would abort instead.
            PlayerSettings.WebGL.exceptionSupport = WebGLExceptionSupport.FullWithoutStacktrace;
            // Gzip with a JavaScript fallback works on any web host, including
            // ones that don't send Content-Encoding headers for Unity builds.
            PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Gzip;
            PlayerSettings.WebGL.decompressionFallback = true;
            PlayerSettings.WebGL.dataCaching = true;
        }
    }
}
