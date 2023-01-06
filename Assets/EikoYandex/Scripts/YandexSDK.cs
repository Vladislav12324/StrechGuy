﻿#if UNITY_EDITOR
using Eiko.YaSDK.Editor;
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Eiko.YaSDK
{
    public class YandexSDK : MonoBehaviour {

    #if UNITY_EDITOR
        [HideInInspector]
        public CanvasAddEditor editorCanvas;
    #endif
        public const int ReloadAdsSeconds = 3;

    
        public static YandexSDK instance;
        [DllImport("__Internal")]
        private static extern void GetUserData();
        [DllImport("__Internal")]
        private static extern void ShowFullscreenAd();
        /// <summary>
        /// Returns an int value which is sent to index.html
        /// </summary>
        /// <param name="placement"></param>
        /// <returns></returns>
        [DllImport("__Internal")]
        private static extern int ShowRewardedAd(string placement);
        //[DllImport("__Internal")]
        //private static extern void GerReward();
        [DllImport("__Internal")]
        private static extern void AuthenticateUser();
        [DllImport("__Internal")]
        private static extern void InitPurchases();
        [DllImport("__Internal")]
        private static extern void Purchase(string id);

    

        public event Action addsOnReloaded;
        public event Action onUserDataReceived;

        public event Action onInterstitialShown;
        public event Action<string> onInterstitialFailed;
        /// <summary>
        /// Пользователь открыл рекламу
        /// </summary>
        public event Action<int> onRewardedAdOpened;
        /// <summary>
        /// Пользователь должен получить награду за просмотр рекламы
        /// </summary>
        public event Action<string> onRewardedAdReward;
        /// <summary>
        /// Пользователь закрыл рекламу
        /// </summary>
        public event Action<int> onRewardedAdClosed;
        /// <summary>
        /// Вызов/просмотр рекламы повлёк за собой ошибку
        /// </summary>
        public event Action<string> onRewardedAdError;
        /// <summary>
        /// Покупка успешно совершена
        /// </summary>
        public event Action<string> onPurchaseSuccess;
        /// <summary>
        /// Покупка не удалась: в консоли разработчика не добавлен товар с таким id,
        /// пользователь не авторизовался, передумал и закрыл окно оплаты,
        /// истекло отведенное на покупку время, не хватило денег и т. д.
        /// </summary>
        public event Action<string> onPurchaseFailed;

        public event Action onClose;

        public Queue<int> rewardedAdPlacementsAsInt = new Queue<int>();
        public Queue<string> rewardedAdsPlacements = new Queue<string>();
   
        public bool addsAvailable;

        public UserData user;
        public int skip_chek;

        private void Awake() {
            if (instance == null) {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else {
                Destroy(gameObject);
            }
            StartCoroutine(WaitAddReload());
    #if UNITY_EDITOR
            editorCanvas =  Instantiate(editorCanvas);
    #endif
        }

        /// <summary>
        /// Call this to ask user to authenticate
        /// </summary>
        public void Authenticate() {
    #if !UNITY_EDITOR && UNITY_WEBGL
            AuthenticateUser();
    #endif
        }

        /// <summary>
        /// Call this to show interstitial ad. Don't call frequently. There is a 3 minute delay after each show.
        /// </summary>
        public void ShowInterstitial() {
            if (addsAvailable)
            {
                StartCoroutine(WaitAddReload());
        #if !UNITY_EDITOR && UNITY_WEBGL
                ShowFullscreenAd();
        #else
                editorCanvas.OpenFullScreen();
        #endif          
                if(skip_chek != SceneManager.GetActiveScene().buildIndex)
                    Time.timeScale = 0;
            }
            else
            {
                Debug.LogWarning("Ad not ready!");
            }
        }

        /// <summary>
        /// Call this to show rewarded ad
        /// </summary>
        /// <param name="placement"></param>
        public void ShowRewarded(string placement) {
            if (placement == "SkipLevel")
                skip_chek = SceneManager.GetActiveScene().buildIndex + 1;
            else
                skip_chek = -1;
#if !UNITY_EDITOR && UNITY_WEBGL
            int placemantId = ShowRewardedAd(placement);
#else
            int placemantId = 0;
    #endif
            rewardedAdPlacementsAsInt.Enqueue(placemantId);
            rewardedAdsPlacements.Enqueue(placement);
            Time.timeScale = 0;
    #if UNITY_EDITOR
            editorCanvas.OpenReward(placemantId);
    #endif
        }

        /// <summary>
        /// Call this to receive user data
        /// </summary>
        public void RequestUserData() {
    #if !UNITY_EDITOR && UNITY_WEBGL
            GetUserData();
    #endif
        }
    
        public void InitializePurchases() {
    #if !UNITY_EDITOR && UNITY_WEBGL
            InitPurchases();
    #endif
        }

        public void ProcessPurchase(string id) {
    #if !UNITY_EDITOR && UNITY_WEBGL
            Purchase(id);
    #endif
        }
    
        public void StoreUserData(string data) {
            user = JsonUtility.FromJson<UserData>(data);
            onUserDataReceived?.Invoke();
        }

        /// <summary>
        /// Callback from index.html
        /// </summary>
        public void OnInterstitialShown() {
            onInterstitialShown?.Invoke();
            Time.timeScale = 1;
        }

        /// <summary>
        /// Callback from index.html
        /// </summary>
        /// <param name="error"></param>
        public void OnInterstitialError(string error) {
            onInterstitialFailed?.Invoke(error);
            Time.timeScale = 1;
        }

        /// <summary>
        /// Callback from index.html
        /// </summary>
        /// <param name="placement"></param>
        public void OnRewardedOpen(int placement) {
            onRewardedAdOpened?.Invoke(placement);
        }

        /// <summary>
        /// Callback from index.html
        /// </summary>
        /// <param name="placement"></param>
        public void OnRewarded(int placement) {
            if (placement == rewardedAdPlacementsAsInt.Dequeue()) {
                onRewardedAdReward?.Invoke(rewardedAdsPlacements.Dequeue());
            }
            Time.timeScale = 1;
        }

        /// <summary>
        /// Callback from index.html
        /// </summary>
        /// <param name="placement"></param>
        public void OnRewardedClose(int placement) {
            onRewardedAdClosed?.Invoke(placement);
            Time.timeScale = 1;
        }

        /// <summary>
        /// Callback from index.html
        /// </summary>
        /// <param name="placement"></param>
        public void OnRewardedError(string placement) {
            onRewardedAdError?.Invoke(placement);
            rewardedAdsPlacements.Clear();
            rewardedAdPlacementsAsInt.Clear();
            Time.timeScale = 1;
        }

        /// <summary>
        /// Callback from index.html
        /// </summary>
        /// <param name="id"></param>
        public void OnPurchaseSuccess(string id) {
            onPurchaseSuccess?.Invoke(id);
        }

        /// <summary>
        /// Callback from index.html
        /// </summary>
        /// <param name="error"></param>
        public void OnPurchaseFailed(string error) {
            onPurchaseFailed?.Invoke(error);
        }
    
        /// <summary>
        /// Browser tab has been closed
        /// </summary>
        /// <param name="error"></param>
        public void OnClose() {
            onClose?.Invoke();
        }
    
        public IEnumerator WaitAddReload()
        {
            addsAvailable = false;
            yield return new WaitForSecondsRealtime(ReloadAdsSeconds);
            addsAvailable = true;
            addsOnReloaded?.Invoke();
        }
        public void skip()
        {
            skip_chek = SceneManager.GetActiveScene().buildIndex + 1;
        }
    }

    public struct UserData {
        public string id;
        public string name;
        public string avatarUrlSmall;
        public string avatarUrlMedium;
        public string avatarUrlLarge;
    }
}