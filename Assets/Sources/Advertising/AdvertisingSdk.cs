using System;
using InstantGamesBridge;

namespace Sources.Advertising
{
    public static class AdvertisingSdk
    {
        public static event Action<string> RewardedEnded;
        public static event Action<string> RewardedNotSeenToTheEnd;

        public static void ShowInterstitial()
        {
            Bridge.advertisement.ShowInterstitial();
        }
        
        public static void ShowRewarded(string placement)
        {
            Bridge.advertisement.ShowRewarded(isEnded =>
            {
                if (isEnded)
                {
                    RewardedEnded?.Invoke(placement);
                    return;
                }
                RewardedNotSeenToTheEnd?.Invoke(placement);
            });
        }
    }
}