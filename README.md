# Nutty Fluffies Rollercoaster: ARM64 port

Tooling that turns the original Android release of **Nutty Fluffies
Rollercoaster 1.0.5** (RedLynx/Ubisoft, 2013, `com.UBI.A90.WW`) into a
**Unity 2022.3 LTS** project that builds a **64-bit (arm64-v8a)** APK.

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
6. **Android settings.** `NFPort > Apply Android ARM64 Player Settings`
   runs automatically on first open and sets:
   - IL2CPP, ARM64 + ARMv7
   - min API 22, target API = highest installed
   - .NET Standard 2.1, minimal stripping (plus `link.xml`)
   - GLES3/Vulkan, landscape auto-rotate
   - application id `com.UBI.A90.WW.arm64`, so it installs alongside the
     original

## Requirements

- Python 3.8+
- [.NET SDK](https://dotnet.microsoft.com/download) 8 or newer (`dotnet` on PATH)
- [AssetRipper](https://github.com/AssetRipper/AssetRipper/releases)
  (verified with 2.0.0, `AssetRipper.GUI.Free`)
- Unity **2022.3 LTS** (verified against 2022.3.62f3 engine assemblies) with
  **Android Build Support**, including the OpenJDK and Android SDK & NDK
  Tools modules

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
upgrade prompt, and use **NFPort > Build Android ARM64 APK**.

Install with `adb install Builds/NuttyFluffies-arm64.apk`. The APK is
signed with Unity's debug key. Add your own keystore under Player Settings
if you want to distribute it.

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
