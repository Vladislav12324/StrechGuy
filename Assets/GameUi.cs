using Eiko.YaSDK;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using Sources.Advertising;

public class GameUi : MonoBehaviour
{
    [SerializeField] private Button _retry;
    [SerializeField] private Button _next;
    [SerializeField] private Button _skip;
    [SerializeField] private Button _retry2, _back;
    [SerializeField] private TextMeshProUGUI _level;
    [SerializeField] private LevelsProgress _progress;
    [SerializeField] private RectTransform _menu;
    [SerializeField] private BonusSelectionView _bonusSelection;
    [SerializeField] private SkinOpenView _skinOpen;
    [SerializeField] private AudioClip _click;

    private UnityAction _loadNextLevel = LevelLoader.LoadNextLevel;
    private UnityAction _retryLevel = () => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    public void SetActions(UnityAction loadNextLevel, UnityAction retryLevel)
    {
        _loadNextLevel = loadNextLevel;
        _retryLevel = retryLevel;
    }

    private void OnEnable()
    {
        AdvertisingSdk.RewardedEnded += InstanceOnonRewardedAdReward;
    }

    private void OnDisable()
    {
        AdvertisingSdk.RewardedEnded -= InstanceOnonRewardedAdReward;
    }

    public void Init()
    {
        _retry.onClick.AddListener(() =>
        {
            if (_next.isActiveAndEnabled)
                SkinOpenView.Added = true;

        });

        _retry.onClick.AddListener(() =>
        {
            _retryLevel();
            AdvertisingSdk.ShowInterstitial();
        });

        _next.onClick.AddListener(() =>
        {
            //YandexSDK.instance.ShowInterstitial();
            _loadNextLevel();
        });
        
        _skip.onClick.AddListener(() =>
        {
            AdvertisingSdk.ShowRewarded("SkipLevel");
            // _loadNextLevel();
        });
        
        _retry2.onClick = _retry.onClick;
        _back.onClick = _retry.onClick;
    }

    private void InstanceOnonRewardedAdReward(string obj)
    {
        if(obj == "SkipLevel")
            _loadNextLevel();
    }

    public void SetupSound(IGameAudio audio)
    {
        foreach (var button in FindObjectsOfType<Button>(true))
        {
            button.onClick.AddListener(() =>
            {
                if(audio == null) return;
                
                audio.Play(_click);
            });
        }
    }

    public void SetLevelNumber(int number)
    {
        _progress.Set(number);
    }

    public void ShowLoseScreen()
    {
        _retry.gameObject.SetActive(true);
    }

    public void ShowWinScreen()
    {
        if(_next.isActiveAndEnabled)
            return;
        StartCoroutine(WinScreen());
    }
    public IEnumerator WinScreen() 
    {
        yield return new WaitForSeconds(2f);
        AdvertisingSdk.ShowInterstitial();

        _next.gameObject.SetActive(true);

        _skinOpen.gameObject.SetActive(true);

        _skinOpen.UpProgress();
    }
    public BonusSelectionView ShowBonusSelectionView()
    {
        _bonusSelection.Enable();

        return _bonusSelection;
    }
    
    public void ShowMenu()
    {
        _menu.gameObject.SetActive(true);
    }

    public void HideMenu()
    {
        _menu.gameObject.SetActive(false);
    }
}