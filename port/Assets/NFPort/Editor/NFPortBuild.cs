using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace NFPort.Editor
{
    /// <summary>
    /// Builds the ARM64 APK. From the command line:
    ///   Unity -batchmode -quit -projectPath &lt;project&gt; -executeMethod NFPort.Editor.NFPortBuild.AndroidArm64 [-nfportOutput path.apk]
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
            NFPortSetup.Apply();
            if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
                EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
            EditorUserBuildSettings.buildAppBundle = false;

            var scenes = EditorBuildSettings.scenes.Where(s => s.enabled && File.Exists(s.path)).Select(s => s.path).ToArray();
            if (scenes.Length == 0) scenes = FallbackScenes.Where(File.Exists).ToArray();

            var output = ArgumentValue("-nfportOutput") ?? "Builds/NuttyFluffies-arm64.apk";
            Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(output)));

            var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
            {
                scenes = scenes,
                locationPathName = output,
                target = BuildTarget.Android,
                targetGroup = BuildTargetGroup.Android,
                options = BuildOptions.None,
            });

            var summary = report.summary;
            Debug.Log($"NFPort: build {summary.result}, {summary.totalErrors} errors, output {output}");
            if (summary.result != BuildResult.Succeeded)
            {
                if (Application.isBatchMode) EditorApplication.Exit(1);
                throw new Exception("NFPort: Android build failed, see the console for details.");
            }
        }

        static string ArgumentValue(string name)
        {
            var args = Environment.GetCommandLineArgs();
            var i = Array.IndexOf(args, name);
            return i >= 0 && i + 1 < args.Length ? args[i + 1] : null;
        }
    }
}
