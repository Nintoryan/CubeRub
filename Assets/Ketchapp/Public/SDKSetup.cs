﻿using Ketchapp.Internal.Analytics;
#if CrossPromotion
using KetchappCrossPromotion.NativeSDK;
#endif
#if GameAnalytics
using GameAnalyticsSDK;
#endif
#if Adjust
using com.adjust.sdk;
#endif
using UnityEngine;

namespace Ketchapp.MayoSDK
{
    public class SDKSetup : MonoBehaviour
    {
        private static string MainObjectName => "KetchappSDK";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void SetupSDK()
        {
            if (!GameObject.Find(MainObjectName))
            {
#if AppsFlyer
            var appsflyerObject = new GameObject
            {
                name = "Appsflyer"
            };
            appsflyerObject.AddComponent(typeof(AppsFlyerManager));
            DontDestroyOnLoad(appsflyerObject);
#endif
            var sDKObject = new GameObject
            {
                name = MainObjectName
            };
            sDKObject.AddComponent(typeof(KetchappSDK));
            DontDestroyOnLoad(sDKObject);
#if GameAnalytics
            var gaObject = new GameObject
            {
                name = "GameAnalytics"
            };
            gaObject.AddComponent(typeof(GameAnalytics));
            DontDestroyOnLoad(gaObject);
#endif
#if Adjust
            var adjustObject = new GameObject("Adjust");
            var adjustComp = (Adjust)adjustObject.AddComponent(typeof(Adjust));
            adjustComp.startManually = true;
            DontDestroyOnLoad(adjustObject);
#endif
            }
        }
    }
}
