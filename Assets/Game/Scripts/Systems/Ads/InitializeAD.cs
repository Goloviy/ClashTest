using UnityEngine;
using GoogleMobileAds.Api;

public class InitializeAD : MonoBehaviour
{
    private void Awake()
    {
        MobileAds.RaiseAdEventsOnUnityMainThread = true;
        MobileAds.Initialize(initStatus => {

            print("Ads Initialised !!");
        
        });
    }
}
