using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace NFPort.Editor
{
    /// <summary>
    /// Builds the port for each platform. From the command line:
    ///   Unity -batchmode -quit -projectPath &lt;project&gt; -buildTarget &lt;Android|Win64|Linux64|WebGL&gt;
    ///         -executeMethod NFPort.Editor.NFPortBuild.&lt;AndroidArm64|Windows64|Linux64|WebGL&gt; [-nfportOutput path]
    /// </summary>
    public static class NFPortBuild
    {
        static readonly string[] FallbackScenes =
        {
            "Assets/Game/Scenes/LogoAnimationScene.unity",
            "Assets/Game/Scenes/StoryScene.unity",
            "Assets/Game/Scenes/MenuScene.unity",
            "Assets/Game/Scenes/GameScene.unity",
        };

        [MenuItem("NFPort/Build Android ARM64 APK")]
        public static void AndroidArm64()
        {
            EditorUserBuildSettings.buildAppBundle = false;
            Build(BuildTargetGroup.Android, BuildTarget.Android, "Builds/NuttyFluffies-arm64.apk", "Android");
        }

        [MenuItem("NFPort/Build Windows (64-bit)")]
        public static void Windows64() =>
            Build(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64, "Builds/Windows/NuttyFluffies.exe", "Windows Build Support (Mono)");

        [MenuItem("NFPort/Build Linux (64-bit)")]
        public static void Linux64() =>
            Build(BuildTargetGroup.Standalone, BuildTarget.StandaloneLinux64, "Builds/Linux/NuttyFluffies.x86_64", "Linux Build Support (Mono)");

        [MenuItem("NFPort/Build WebGL")]
        public static void WebGL() =>
            Build(BuildTargetGroup.WebGL, BuildTarget.WebGL, "Builds/WebGL", "WebGL Build Support");

        static void Build(BuildTargetGroup group, BuildTarget target, string defaultOutput, string module)
        {
            if (!BuildPipeline.IsBuildTargetSupported(group, target))
                Fail($"NFPort: {target} is not installed. Add the \"{module}\" module to this Unity version in Unity Hub.");

            NFPortSetup.Apply();
            if (EditorUserBuildSettings.activeBuildTarget != target)
                EditorUserBuildSettings.SwitchActiveBuildTarget(group, target);

            var scenes = EditorBuildSettings.scenes.Where(s => s.enabled && File.Exists(s.path)).Select(s => s.path).ToArray();
            if (scenes.Length == 0) scenes = FallbackScenes.Where(File.Exists).ToArray();

            var output = ArgumentValue("-nfportOutput") ?? defaultOutput;
            var parent = target == BuildTarget.WebGL ? Path.GetFullPath(output) : Path.GetDirectoryName(Path.GetFullPath(output));
            Directory.CreateDirectory(parent);

            var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
            {
                scenes = scenes,
                locationPathName = output,
                target = target,
                targetGroup = group,
                options = BuildOptions.None,
            });

            var summary = report.summary;
            Debug.Log($"NFPort: {target} build {summary.result}, {summary.totalErrors} errors, output {output}");
            if (summary.result != BuildResult.Succeeded)
                Fail($"NFPort: {target} build failed, see the console for details.");
        }

        static void Fail(string message)
        {
            Debug.LogError(message);
            if (Application.isBatchMode) EditorApplication.Exit(1);
            throw new Exception(message);
        }

        static string ArgumentValue(string name)
        {
            var args = Environment.GetCommandLineArgs();
            var i = Array.IndexOf(args, name);
            return i >= 0 && i + 1 < args.Length ? args[i + 1] : null;
        }
    }
}
