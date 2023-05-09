using UnityEngine;
using System.Runtime.InteropServices;

#if UNITY_IOS && (MEDIATION_MAX || MEDIATION_FairBid || MEDIATION_IronSource)

namespace Ketchapp.Mayo
{
public static class FacebookTrackingBridge
    {
[DllImport("__Internal")]
private static extern void FBAdSettingsBridgeSetAdvertiserTrackingEnabled(bool advertiserTrackingEnabled);

public static void SetAdvertiserTrackingEnabled(bool advertiserTrackingEnabled)
{
FBAdSettingsBridgeSetAdvertiserTrackingEnabled(advertiserTrackingEnabled);
}
}
}

#endif