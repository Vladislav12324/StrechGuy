﻿using System;
using System.Collections;
using Eiko.YaSDK;
using UnityEngine;
using Eiko.YaSDK.Data;
using Sources.Advertising;
using Sources.Data;

public class BonusSelectionView : MonoBehaviour
{
    public event Action<Bonus> BonusSelected;
    
    [SerializeField] private RectTransform _left, _right,_mid;
    [SerializeField] private BonusView _exit;
    [SerializeField] private BuyBonus buy_left,buy_right;
    [SerializeField] private BonusView[] _bonusPrefabs;
    [SerializeField] private float _waitTime;
    [SerializeField] private GameObject[] _bords;
    public void Enable() => gameObject.SetActive(true);
    public void Disable() => gameObject.SetActive(false);
    public static string buy_id;

    private Bonus _bonus;
    private Action onDestroy;
    public static int bonus_i;
    private void Start()
    {
        var leftPrefab = _bonusPrefabs.Random(null);

        YandexSDK.instance.InitializePurchases();
        var left = Instantiate(leftPrefab, _left);
        var right = Instantiate(_bonusPrefabs.Random(leftPrefab), _right);
        left.buy = true;
        AdvertisingSdk.RewardedEnded += InstanceOnonRewardedAdReward;
        AdvertisingSdk.RewardedNotSeenToTheEnd += OnRewardedNotSeenToTheEnd;
        
        left.Clicked += OnBonusClicked;
        right.Clicked += OnBonusClicked;
        _exit.Clicked += OnBonusClicked;
        buy_left._bonus = left._bonus;
        buy_right._bonus = right._bonus;
        buy_right.id = right.buy_id;
        buy_left.id = left.buy_id;
        buy_left.Clicked += BuyBonus;
        buy_right.Clicked += BuyBonus;
        onDestroy = () =>
        {
            left.Clicked -= OnBonusClicked;
            right.Clicked -= OnBonusClicked;
            _exit.Clicked -= OnBonusClicked;
        };

        StartCoroutine(ShowExit());
    }
    private void OnEnable()
    {
        YandexSDK.instance.onPurchaseSuccess += InstanceOnPurchaseSuccess;
    }

    private void OnDisable()
    {
        YandexSDK.instance.onPurchaseSuccess -= InstanceOnPurchaseSuccess;
    }
    private IEnumerator ShowExit()
    {
        yield return new WaitForSeconds(_waitTime);
        
        _exit.gameObject.SetActive(true);
    }
    private void BuyBonus(Bonus bonus)
    {
        _bonus = bonus;
        YandexSDK.instance.ProcessPurchase(buy_id);
    }
    private void InstanceOnPurchaseSuccess(Purchase id)
    {
        if (id.productID == buy_id)
        {
            BonusSelected?.Invoke(_bonus);
            BonusBord();
            Disable();
        }
    }
    private void OnBonusClicked(Bonus bonus)
    {
        _bonus = bonus;

        if (bonus is NoBonus || bonus == null)
        {
            BonusSelected?.Invoke(bonus);
            Disable();
            return;
        }
        AdvertisingSdk.ShowRewarded("BonusSelect");
        
    }

    private void InstanceOnonRewardedAdReward(string obj)
    {
        if (obj == "BonusSelect")
        {
            BonusSelected?.Invoke(_bonus);
            BonusBord();
            Disable();
        }
    }

    private void OnDestroy()
    {
        AdvertisingSdk.RewardedEnded -= InstanceOnonRewardedAdReward;
        AdvertisingSdk.RewardedNotSeenToTheEnd -= OnRewardedNotSeenToTheEnd;
        
        onDestroy?.Invoke();
    }
    private void BonusBord()
    {
        //Instantiate(_bords[bonus_i],_mid);
        if (bonus_i == 0 && Prefs.GetInt("double") !=1)
        {
            Prefs.SetInt("double", 1);
            _bords[bonus_i].SetActive(true);
        }
        if (bonus_i == 1 && Prefs.GetInt("fly") != 1)
        {
            Prefs.SetInt("fly", 1);
            _bords[bonus_i].SetActive(true);
        }
        if (bonus_i == 2 && Prefs.GetInt("infinity") != 1)
        {
            Prefs.SetInt("infinity", 1);
            _bords[bonus_i].SetActive(true);
        }
        if (bonus_i == 3 && Prefs.GetInt("iron") != 1)
        {
            Prefs.SetInt("iron", 1);
            _bords[bonus_i].SetActive(true);
        }
    }
    private void OnRewardedNotSeenToTheEnd(string placement)
    {
        OnBonusClicked(null);
    }
}