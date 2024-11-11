using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdmobAdsScript : MonoBehaviour
{
#if UNITY_ANDROID
    private string adUnitId = "ca-app-pub-3940256099942544/5224354917";//testId
   // private string adUnitId = "ca-app-pub-8213297186156293/4980299947";//appId
#elif UNITY_IPHONE
            private string adUnitId = "ca-app-pub-3940256099942544/1712485313";//testId
            //private string adUnitId = "ca-app-pub-8213297186156293/3098637400";//appId
#else
            private string adUnitId = "unexpected_platform";
#endif

    private RewardedAd _rewardedAd;
    public event Action<int> collectRewards;

    private void Start()
    {
        MobileAds.Initialize((InitializationStatus initStatus) => { });
        RequestRewarded();
    }

    private void RequestRewarded()
    {
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }

        Debug.Log("Loading the rewarded ad.");
       
        var adRequest = new AdRequest();
       
        RewardedAd.Load(adUnitId, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());

                _rewardedAd = ad;
            });
    }
 
    public void LoadRewardedAd()
    {
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }

        Debug.Log("Loading the rewarded ad.");
       
        var adRequest = new AdRequest();
      
        RewardedAd.Load(adUnitId, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());

                _rewardedAd = ad;
            });
    }

    public void ShowRewardedAd(int _id)
    {
        if (_rewardedAd == null)
        {
            LoadRewardedAd();
        }
        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            _rewardedAd.Show((Reward reward) =>
            {
                collectRewards?.Invoke(_id);
                LoadRewardedAd();
            });
        }
    }
}