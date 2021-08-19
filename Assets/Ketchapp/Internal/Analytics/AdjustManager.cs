using System.Collections;
using System.Collections.Generic;
#if Adjust
using com.adjust.sdk;
#endif
using UnityEngine;
namespace Ketchapp.Internal.Analytics
{
    public class AdjustManager : MonoBehaviour, IAnalyticsManager
    {
        public bool IsInitialized { get; set; }

        public void ApplicationInstalled()
        {
            Debug.Log("Event is not used by Adjust");
        }

        public void ApplicationSession()
        {
            Debug.Log("Event is not used by Adjust");
        }

        public void BannerLoaded(string impressionData, string adUnit, string countryCode, string networkName)
        {
            Debug.Log("Event is not used by Adjust");
        }

        public void CustomEvent(string eventName)
        {
            Debug.Log("Event is not used by Adjust");
        }

        public void CustomEvent(string eventName, float eventValue)
        {
            Debug.Log("Event is not used by Adjust");
        }

        public void InAppPurchaseMade(string item, string currency, string value)
        {
            Debug.Log("Event is not used by Adjust");
        }

        public void Initialize()
        {
#if Adjust
            Adjust.start(new AdjustConfig(KetchappInternal.ConfigurationObject.GetConfigurationForCurrentPlatform().AdjustToken, AdjustEnvironment.Production));
            IsInitialized = true;
            Debug.Log($"Initialized adjust : {KetchappInternal.ConfigurationObject.GetConfigurationForCurrentPlatform().AdjustToken}");
#endif
        }

        public void InterstitialShowed(string impressionData, string adUnit, string countryCode, string networkName)
        {
            Debug.Log("Event is not used by Adjust");
        }

        public void LevelFailed(string levelName, int score)
        {
            Debug.Log("Event is not used by Adjust");
        }

        public void LevelPassed(string levelName, int score)
        {
            Debug.Log("Event is not used by Adjust");
        }

        public void LevelStarted(string levelName)
        {
            Debug.Log("Event is not used by Adjust");
        }

        public void RewardVideoValidated(string impressionData, string adUnit, string countryCode, string networkName)
        {
            throw new System.NotImplementedException();
        }

        public void SendAttResult(string eventName)
        {
            Debug.Log("Event is not used by Adjust");
        }
    }
}
