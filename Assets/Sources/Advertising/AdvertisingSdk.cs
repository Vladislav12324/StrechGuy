using System;

namespace Sources.Advertising
{
    public static class AdvertisingSdk
    {
        
        public static event Action<string> RewardedEnded;
        public static event Action<string> RewardedNotSeenToTheEnd;

        public static void ShowInterstitial()
        {
            
        }
        
        public static void ShowRewarded(string skiplevel)
        {
            throw new System.NotImplementedException();
        }
    }
}