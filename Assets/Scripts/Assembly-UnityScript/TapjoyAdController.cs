using System;
using UnityEngine;

// NFPort: drove the Tapjoy banner/full-screen ads and offer wall. The SDK is
// gone and GameData.IS_TAPJOY_ENABLED was never true, so nothing is left to
// do; the class stays only because MenuScene still has the component attached.
[Serializable]
public class TapjoyAdController : MonoBehaviour
{
}
