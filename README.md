# Nutty Fluffies Rollercoaster: ARM64 port

Tooling that turns the original Android release of **Nutty Fluffies
Rollercoaster 1.0.5** (RedLynx/Ubisoft, 2013, `com.UBI.A90.WW`) into a
**Unity 2022.3 LTS** project that builds a **64-bit (arm64-v8a)** APK, and
also **Windows**, **Linux** and **web (WebGL)** builds.

The original only ships `armeabi-v7a` Mono binaries from Unity 4.0, so it
won't install on 64-bit-only devices. Unity can only target ARM64 on Android
through IL2CPP, which first appeared in Unity 5. The game has to be
decompiled and moved to a modern engine.

No game code or assets are stored in this repository. Everything is
regenerated from **your own copy of the APK** by `tools/nfport.py`.

## What's in the original APK

| | |
|---|---|
| Engine | Unity **4.0.0f7**, Mono scripting backend, ARMv7 only |
| Scripts | `Assembly-CSharp-firstpass` (tk2d, FingerGestures, SDK bindings), `Assembly-CSharp`, and `Assembly-UnityScript` (most of the game, written in UnityScript/JavaScript) |
| Scenes | LogoAnimationScene → StoryScene → MenuScene → GameScene |
| Level data | `assets/GameResources` (level/theme/object packs: `.bin`, `.json`, `.txt`, `.png`), loaded at runtime with `WWW("jar:file://…!/assets/…")` |
| SDKs | Prime31 (Etcetera, Google IAB, Twitter, Facebook, Flurry, Game Center), Amazon IAP, Tapjoy, Pushwoosh, Urban Airship, Ubisoft cross-promotion/AppPerse |

## What the port changes

1. **Decompile.** [AssetRipper](https://github.com/AssetRipper/AssetRipper)
   recovers a Unity project (scenes, prefabs, meshes, textures, audio,
   materials, shaders) and decompiles all three script assemblies to C#.
2. **UnityScript runtime without code generation.** The decompiled
   UnityScript still calls `Boo.Lang` / `UnityScript.Lang`. Those runtimes
   use `System.Reflection.Emit`, which IL2CPP doesn't support.
   `port/Assets/Plugins/NFPort/UnityScriptCompat` reimplements the parts the
   game uses with reflection only: coroutine generators, `Array`, `for…in`
   enumeration, `==` and coercion semantics, `parseInt`/`parseFloat`.
3. **Legacy particles.** Unity 2018.3 removed `EllipsoidParticleEmitter`,
   `ParticleAnimator` and `ParticleRenderer`, and the game uses 78 of them.
   `tools/convert_legacy_particles.py` rewrites them in the scene YAML into
   shim components (`port/Assets/Plugins/NFPort/LegacyParticles`). The shims
   keep the original settings and drive a modern `ParticleSystem`, so the
   game's `emit`, `Emit()` and `ClearParticles()` calls keep working.
4. **Dead SDKs removed.** `tools/stub_sdks.py` turns every Amazon, Google
   IAB, Twitter, Facebook, Flurry, Tapjoy, Pushwoosh, Game Center,
   Etcetera, cross-promo and AppPerse binding into no-ops with the same API.
   The game code compiles unchanged and nothing calls into Java or native
   code that no longer exists. The .NET 2.0 `P31RestKit.dll` is replaced by
   `Compat/Prime31Compat.cs` (MiniJSON, manager base classes).
5. **API upgrade.** `tools/codemods.py` fixes decompiler artifacts:
   - `virtual` in interfaces
   - methods named like their class
   - Boo enumerators
   - joint `spring`/`damper` moved to `SoftJointLimitSpring`
   - `iPhoneSettings` and `TouchScreenKeyboard.autorotate*`

   `tools/api_updater.py` then compiles the scripts against your Unity
   2022.3 engine DLLs (`tools/compilecheck`) and rewrites exactly the tokens
   the compiler flags. This covers everything Unity marks `(UnityUpgradable)`
   (`.renderer`, `.rigidbody`, `isOrthoGraphic`, …), plus `Component.active`,
   `AddComponent("Name")` (now resolved by name at runtime) and a few
   UnityScript-only constructs. It repeats until both assemblies compile with
   zero errors.
6. **Cross-platform file loading.** The Android build read its level, theme
   and language files out of the APK with `jar:file://` URLs and waited on
   each download in a loop. Those URLs only exist on Android, and in a browser
   the loop never finishes. The port packs those files into
   `Assets/Resources/NFPort/GameResources.bytes`, and a codemod points the
   three reads at `NFPortAssetFile`, which reads them synchronously on every
   platform.
7. **Mouse input on desktop and web.** The Android build hard-wired the touch
   controls. The mouse controls from the old Mac version are still in the
   scenes (`KeyboardController` and the mouse FingerGestures prefab). The port
   uses them on Windows, Linux, desktop browsers and in the editor, and keeps
   touch on phones and phone browsers. "Tap to continue" screens also accept a
   mouse click.
8. **Player settings.** `NFPort > Apply Player Settings (all platforms)`
   runs automatically on first open:
   - **Android:**
     - IL2CPP, ARM64 + ARMv7
     - min API 22, target API = highest installed
     - GLES3/Vulkan, landscape auto-rotate
     - application id `com.UBI.A90.WW.arm64`, so it installs alongside the
       original
   - **Windows/Linux:** Mono, a 1280×720 resizable window or borderless
     fullscreen (Alt+Enter toggles)
   - **WebGL:** gzip with a decompression fallback, so it works on any host;
     full exception support, so a stray null reference gets logged instead
     of stopping the game
   - **All platforms:** .NET Standard 2.1, minimal stripping (plus
     `link.xml`)

## Requirements

- Python 3.8+
- [.NET SDK](https://dotnet.microsoft.com/download) 8 or newer (`dotnet` on PATH)
- [AssetRipper](https://github.com/AssetRipper/AssetRipper/releases)
  (verified with 2.0.0, `AssetRipper.GUI.Free`)
- Unity **2022.3 LTS** (verified against 2022.3.62f3 engine assemblies) with
  the Build Support module of each platform you want to build:
  - **Android Build Support**, including the OpenJDK and Android SDK & NDK
    Tools modules
  - **Windows Build Support (Mono)**
  - **Linux Build Support (Mono)**
  - **WebGL Build Support**

## Usage

Windows (PowerShell), adjust the paths:

```powershell
python tools\nfport.py all `
  --apk "C:\Users\you\Downloads\Nutty+Fluffies+Rollercoaster_1.0.5_APKPure.apk" `
  --assetripper "C:\Tools\AssetRipper" `
  --unity "C:\Program Files\Unity\Hub\Editor\2022.3.62f3\Editor\Unity.exe"
```

This writes the upgraded project to `NuttyFluffiesPort/`. Because `--unity`
points at the editor, it then builds `Builds/NuttyFluffies-arm64.apk` in
batch mode. The first build takes a while: Unity imports and upgrades every
asset and IL2CPP compiles the game.

You can also run the steps separately (`extract`, `rip`, `port`, `build`).
Or stop after `port`: open `NuttyFluffiesPort/` in Unity 2022.3, accept the
upgrade prompt, and use the **NFPort** menu.

Other platforms: add `--target` to `build` (repeatable), or use the menu.

| Target | Command | Menu | Output |
|---|---|---|---|
| Android | `--target android` (default) | NFPort > Build Android ARM64 APK | `Builds/NuttyFluffies-arm64.apk` |
| Windows | `--target windows` | NFPort > Build Windows (64-bit) | `Builds/Windows/NuttyFluffies.exe` |
| Linux | `--target linux` | NFPort > Build Linux (64-bit) | `Builds/Linux/NuttyFluffies.x86_64` |
| Web | `--target webgl` | NFPort > Build WebGL | `Builds/WebGL/` |

```powershell
python tools\nfport.py build --unity "C:\Program Files\Unity\Hub\Editor\2022.3.62f3\Editor\Unity.exe" --target windows --target webgl
```

- **Android:** install with `adb install Builds/NuttyFluffies-arm64.apk`.
  The APK is signed with Unity's debug key. Add your own keystore under
  Player Settings if you want to distribute it.
- **Windows/Linux:** ship the whole output folder, not just the executable.
- **Web:** browsers won't run a WebGL build from `file://`, so serve the
  folder over HTTP. Locally, run `python -m http.server 8000` inside
  `Builds/WebGL` and open <http://localhost:8000>. To publish, zip the
  folder's contents and upload them to itch.io as an HTML game.

## Status and known gaps

- **Verified here:** the full pipeline runs from the 1.0.5 APK, and the
  runtime, firstpass and editor assemblies compile with zero errors against
  the Unity 2022.3.62f3 engine and editor assemblies.
- **Not yet verified:** the Unity import and an on-device run. Building
  needs a licensed Unity Editor, which the pipeline was not run with.
  Expect to check these on first play:
  - **Physics feel.** Unity 4 used PhysX 2.8 and 2022.3 uses PhysX 4. The
    coaster carts use ConfigurableJoints, so if the ride feels off, tune
    `Edit > Project Settings > Physics` (solver iterations, default contact
    offset) first.
  - **Particles.** The legacy-particle shims approximate the old emitter,
    animator and renderer behaviour; colours and sizes may differ slightly.
- **Desktop and web input** uses the old Mac version's mouse controls. That
  code shipped in the APK but this Android build never ran it, so it's
  untested. On the web, sound starts only after the first click (browser
  rule), and progress is saved through PlayerPrefs in the browser's storage.
- **Removed features:** in-app purchases (Google/Amazon), Facebook/Twitter
  sharing, leaderboards and achievements (Game Center), push notifications,
  ads/offer walls and analytics. Calls into them are no-ops, so for example
  the coin store can't sell coins for real money.

## Repository layout

```
tools/nfport.py                    pipeline driver (extract / rip / port / build)
tools/api_updater.py               compiler-driven Unity API updater
tools/codemods.py                  decompiler-artifact and removed-API fix-ups
tools/convert_legacy_particles.py  legacy particle YAML -> shim components
tools/stub_sdks.py                 dead SDK bindings -> no-op stubs
tools/compilecheck/                dotnet projects that compile the scripts against Unity's DLLs
port/Assets/                       copied into the project: compat runtime, shims, editor scripts, link.xml
```
