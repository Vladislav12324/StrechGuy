using System;
using System.Collections;
using Eiko.YaSDK;
using UnityEngine;

public class BonusSelectionView : MonoBehaviour
{
    public event Action<Bonus> BonusSelected;
    
    [SerializeField] private RectTransform _left, _right,_mid;
    [SerializeField] private BonusView _exit;
    [SerializeField] private BonusView[] _bonusPrefabs;
    [SerializeField] private float _waitTime;
    [SerializeField] private GameObject[] _bords;
    public void Enable() => gameObject.SetActive(true);
    public void Disable() => gameObject.SetActive(false);

    private Bonus _bonus;
    private Action onDestroy;
    public static int bonus_i;
    private void Start()
    {
        var leftPrefab = _bonusPrefabs.Random(null);
        
        var left = Instantiate(leftPrefab, _left);
        var right = Instantiate(_bonusPrefabs.Random(leftPrefab), _right);
        
        YandexSDK.instance.onRewardedAdReward += InstanceOnonRewardedAdReward;
        YandexSDK.instance.onRewardedAdClosed += OnBonusClicked;
        
        left.Clicked += OnBonusClicked;
        right.Clicked += OnBonusClicked;
        _exit.Clicked += OnBonusClicked;

        onDestroy = () =>
        {
            left.Clicked -= OnBonusClicked;
            right.Clicked -= OnBonusClicked;
            _exit.Clicked -= OnBonusClicked;
        };

        StartCoroutine(ShowExit());
    }

    private IEnumerator ShowExit()
    {
        yield return new WaitForSeconds(_waitTime);
        
        _exit.gameObject.SetActive(true);
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
        
        YandexSDK.instance.ShowRewarded("BonusSelect");
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
        YandexSDK.instance.onRewardedAdReward -= InstanceOnonRewardedAdReward;
        YandexSDK.instance.onRewardedAdClosed -= OnBonusClicked;
        
        onDestroy?.Invoke();
    }
    private void BonusBord()
    {
        //Instantiate(_bords[bonus_i],_mid);
        if (bonus_i == 0 && PlayerPrefs.GetInt("double") !=1)
        {
            PlayerPrefs.SetInt("double", 1);
            _bords[bonus_i].SetActive(true);
        }
        if (bonus_i == 1 && PlayerPrefs.GetInt("fly") != 1)
        {
            PlayerPrefs.SetInt("fly", 1);
            _bords[bonus_i].SetActive(true);
        }
        if (bonus_i == 2 && PlayerPrefs.GetInt("infinity") != 1)
        {
            PlayerPrefs.SetInt("infinity", 1);
            _bords[bonus_i].SetActive(true);
        }
        if (bonus_i == 3 && PlayerPrefs.GetInt("iron") != 1)
        {
            PlayerPrefs.SetInt("iron", 1);
            _bords[bonus_i].SetActive(true);
        }
    }
    private void OnBonusClicked(int obj)
    {
        OnBonusClicked(null);
    }
}