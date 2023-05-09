using System;
using System.Collections;
using System.Globalization;
using Ketchapp.Internal;
using Ketchapp.Internal.Configuration;
using Ketchapp.MayoSDK;
using UnityEngine;
#if UNITY_IOS
using UnityEngine.iOS;
#endif
#if MEDIATION_MAX
using static MaxSdkBase;
#endif

namespace Ketchapp.Internal.Advertisement
{
#if MEDIATION_MAX
    internal class MAXManager : AdvertisementBase, IDisposable
    {
        private string BannerId => ConfigurationType == ConfigType.Android ? KetchappInternal.ConfigurationObject.AndroidConfiguration.BannerId : KetchappInternal.ConfigurationObject.IosConfiguration.BannerId;
        private string SdkKey => ConfigurationType == ConfigType.Android ? KetchappInternal.ConfigurationObject.AndroidConfiguration.MediationAppId : KetchappInternal.ConfigurationObject.IosConfiguration.MediationAppId;
        private string InterstitialId => ConfigurationType == ConfigType.Android ? KetchappInternal.ConfigurationObject.AndroidConfiguration.InterstitialId : KetchappInternal.ConfigurationObject.IosConfiguration.InterstitialId;
        private string RewardedId => ConfigurationType == ConfigType.Android ? KetchappInternal.ConfigurationObject.AndroidConfiguration.RewardedVideoId : KetchappInternal.ConfigurationObject.IosConfiguration.RewardedVideoId;

        public override void HideBanner(Action onHidden = null)
        {
            MaxSdk.HideBanner(BannerId);
        }

        public override void Initialize()
        {
            MaxSdkCallbacks.OnSdkInitializedEvent += OnSDKInitialized;
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += FetchInterstitial;
            MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialAdFailedToDisplayEvent;
            MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialLoadFailedEvent;

            MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += FetchRewardedVideo;
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdLoadFailedEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
            MaxSdkCallbacks.Banner.OnAdLoadedEvent += MaxBannerLoaded;
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += FetchRewardedVideo;
            MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += MaxBannerLoadedFailed;
            MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayedEvent;

            MaxSdk.SetSdkKey(SdkKey);
            MaxSdk.InitializeSdk();
            MaxSdk.SetHasUserConsent(KetchappInternal.CrossPromo.GetGdprValue());
#if UNITY_IOS && Appsflyer
            MaxSdk.SetUserId(AppsFlyerSDK.AppsFlyeriOS.getAppsFlyerId());
#endif
        }

        private void MaxBannerLoadedFailed(string arg1, ErrorInfo err)
        {
            Debug.LogError($"[KetchappMayo MAXManager] MaxBannerLoadedFailed");
        }

        public void MaxBannerLoaded(string placement, AdInfo adInfo)
        {
            var revenue = adInfo.Revenue;
            KetchappInternal.Analytics.BannerLoaded(
               revenue.ToString(CultureInfo.InvariantCulture),
               adInfo.AdUnitIdentifier,
               MaxSdk.GetSdkConfiguration().CountryCode,
               adInfo.NetworkName);
        }

        public void Dispose()
        {
            MaxSdkCallbacks.OnSdkInitializedEvent -= OnSDKInitialized;
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent -= FetchInterstitial;
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent -= FetchRewardedVideo;
            MaxSdkCallbacks.Banner.OnAdLoadedEvent -= MaxBannerLoaded;
            MaxSdkCallbacks.Banner.OnAdLoadFailedEvent -= MaxBannerLoadedFailed;
            MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent -= OnInterstitialDisplayedEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent -= OnRewardedDisplayedEvent;

            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent -= OnRewardedAdLoadFailedEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent -= OnRewardedAdFailedToDisplayEvent;

            MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent -= OnInterstitialAdFailedToDisplayEvent;
            MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent -= OnInterstitialLoadFailedEvent;

            GC.SuppressFinalize(this);
        }

        private void OnInterstitialDisplayedEvent(string adUnitId, AdInfo adInfo)
        {
            double revenue = adInfo.Revenue;
            KetchappInternal.Analytics.InterstitialShowed(
               revenue.ToString(CultureInfo.InvariantCulture),
               adInfo.AdUnitIdentifier,
               MaxSdk.GetSdkConfiguration().CountryCode,
               adInfo.NetworkName);
        }

        private void OnRewardedDisplayedEvent(string adUnitId, AdInfo adInfo)
        {
            double revenue = adInfo.Revenue;
            KetchappInternal.Analytics.RewardVideoValidated(
               revenue.ToString(CultureInfo.InvariantCulture),
               adInfo.AdUnitIdentifier,
               MaxSdk.GetSdkConfiguration().CountryCode,
               adInfo.NetworkName);
        }

        public override bool IsRewardedVideoAvailable()
        {
            return MaxSdk.IsRewardedAdReady(RewardedId);
        }

        public override void ShowBanner(Action onShown)
        {
            MaxSdk.ShowBanner(BannerId);
            KetchappSDK.Advertisement.BannerDisplayed = true;
            void OnBannerShown(string id, AdInfo adInfo)
            {
                onShown?.Invoke();
                MaxSdkCallbacks.Banner.OnAdExpandedEvent -= OnBannerShown;
            }

            MaxSdkCallbacks.Banner.OnAdExpandedEvent += OnBannerShown;
        }

        public override bool IsInterstitialAvailable()
        {
            return MaxSdk.IsInterstitialReady(InterstitialId);
        }

        public override void ShowInterstitial(Action<bool> onShown)
        {
            if (MaxSdk.IsInterstitialReady(InterstitialId))
            {
                MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialFail;
                MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialClosed;
                MaxSdk.ShowInterstitial(InterstitialId);

                void OnInterstitialClosed(string id, AdInfo adInfo)
                {
                    onShown?.Invoke(true);
                    MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent -= OnInterstitialFail;
                    MaxSdkCallbacks.Interstitial.OnAdHiddenEvent -= OnInterstitialClosed;
                }

                void OnInterstitialFail(string placement, ErrorInfo error, AdInfo adInfo)
                {
                    onShown?.Invoke(false);
                    MaxSdkCallbacks.Interstitial.OnAdHiddenEvent -= OnInterstitialClosed;
                    MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent -= OnInterstitialFail;
                }
            }
            else
            {
                Debug.Log("Interstitial is not ready");
                onShown?.Invoke(false);
            }
        }

        public override void ShowRewardedVideo(Action<bool> rewardedFinished)
        {
            if (MaxSdk.IsRewardedAdReady(RewardedId))
            {
                MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += RewardedFinished;
                MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += RewardedFailed;
                MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += RewardedFailedToDisplay;

                MaxSdk.ShowRewardedAd(RewardedId);
                void ClearCallback()
                {
                    MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent -= RewardedFinished;
                    MaxSdkCallbacks.Rewarded.OnAdHiddenEvent -= RewardedFailed;
                    MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent -= RewardedFailedToDisplay;
                }

                void RewardedFinished(string id, MaxSdkBase.Reward reward, AdInfo adInfo)
                {
                    rewardedFinished?.Invoke(true);
                    ClearCallback();
                }

                void RewardedFailedToDisplay(string id, ErrorInfo error, AdInfo adInfo)
                {
                    Debug.LogError(error.Message);
                    rewardedFinished?.Invoke(false);
                    ClearCallback();
                }

                void RewardedFailed(string id, AdInfo adInfo)
                {
                    rewardedFinished?.Invoke(false);
                    ClearCallback();
                }
            }
        }

        public override void ShowTestSuite()
        {
            MaxSdk.ShowMediationDebugger();
        }

        private void FetchInterstitial(string id, AdInfo adInfo)
        {
            MaxSdk.LoadInterstitial(InterstitialId);
        }

        private void FetchRewardedVideo(string id, AdInfo adInfo)
        {
            MaxSdk.LoadRewardedAd(RewardedId);
        }

        private void OnSDKInitialized(MaxSdkBase.SdkConfiguration sdkConfiguration)
        {
            IsInitialized = true;
            MaxSdk.CreateBanner(BannerId, MaxSdkBase.BannerPosition.BottomCenter);
            FetchInterstitial(InterstitialId, null);
            FetchRewardedVideo(RewardedId, null);
        }

        private void HideBannerBackground(string banner, int id)
        {
            KetchappSDK.Advertisement.BannerDisplayed = false;
        }

        public override void RefreshUserConsent(bool result)
        {
            MaxSdk.SetHasUserConsent(result);
            MaxSdk.CreateBanner(BannerId, MaxSdkBase.BannerPosition.BottomCenter);
            FetchInterstitial(InterstitialId, null);
            FetchRewardedVideo(RewardedId, null);

            void OnBannerLoadedAfterRefresh(string id, AdInfo adInfo)
            {
                ShowBanner(null);
                MaxSdkCallbacks.Banner.OnAdLoadedEvent -= OnBannerLoadedAfterRefresh;
            }

            MaxSdkCallbacks.Banner.OnAdLoadedEvent += OnBannerLoadedAfterRefresh;
        }

        #region RewardedCallbacks
        private void OnRewardedAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        {
            FetchRewardedVideo(RewardedId, null);
        }

        private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
        {
            FetchRewardedVideo(RewardedId, null);
        }
        #endregion

        #region InterstitialCallbacks

        private void OnInterstitialLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        {
            FetchInterstitial(InterstitialId, null);
        }

        private void OnInterstitialAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
        {
            FetchInterstitial(InterstitialId, null);
        }
        #endregion
    }
#endif
}
