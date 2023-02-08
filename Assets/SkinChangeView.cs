using System.Linq;
using Eiko.YaSDK;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class SkinChangeView : MonoBehaviour
{
    [SerializeField] private SpringMan _skinViewMan;
    [SerializeField] private SkinView _template;
    [SerializeField] private SkinsAsset _skins;
    [SerializeField] private RectTransform _content;
    [SerializeField] private Sprite _locked;
    [SerializeField] private Sprite _rewarded;
    [SerializeField] private AudioClip _clicked;
    [SerializeField] private string[] skins_id= {"11", "12", "13", "14" };
    [SerializeField] private List<int> OpenedSkins = new();
    public static string buy_id;

    public bool IsOpened(SkinAsset skin) => OpenedSkins.Contains(skin.GetHashCode()) || skin.AlwaysOpened;
    public void OpenSkin(SkinAsset skin)
    {
        if (IsOpened(skin))
            return;

        OpenedSkins.Add(skin.GetHashCode());
    }
    private void Start()
    {
        _skinViewMan.Deactivate();
        _skinViewMan.Apply(gameObject.AddComponent<InfinityMaxRopeLengthBonus>());
        _skinViewMan.Apply(_skins.GetById(PlayerData.Skin));
        YandexSDK.instance.InitializePurchases();
        for(int i = 0; i < 4; i++)
        {
            if (PurchaseProcess.Has(skins_id[i]) == true || PurchaseProcess.Has("22"))
            {
                OpenedSkins.Add((_skins.Skins.First(x => x._id == i+1)).GetHashCode());
                PlayerData.OpenSkin(_skins.Skins.First(x => x._id == i + 1));
                
            }
        }
        var rewarded = _skins.Skins.Random(null, x => IsOpened(x) == false);

        foreach (var skinAsset in _skins.Skins)
        {
            if (skinAsset == rewarded)
            {
                var rewardedView = _template.Instantiate(_content, _rewarded, rewarded.Skin, rewarded.GetHashCode(),true);
                rewardedView.Clicked += OnClickedSkinView; 
                rewardedView.AddOnClickedAction(OnRewardedClicked);
                continue;
            }
            
            var opened = IsOpened(skinAsset);
            var sprite = opened ? skinAsset.Image : _locked;
            
            var skinView = _template.Instantiate(_content, sprite, skinAsset.Skin, skinAsset.GetHashCode(),opened);
            skinView.Clicked += OnClickedSkinView;
            skinView.buy_button.GetComponent<Button>().onClick.AddListener(() =>BuySkin());
        }
        
        SetupClicks();
    }
    private void InstanceOnPurchaseSuccess(Purchase obj)
    {
        var skinAsset = _skins.Skins.First(x => x._id == int.Parse(buy_id) - 10);
        OpenSkin(skinAsset);
        _skinViewMan.Apply(skinAsset._skin);
        YandexSDK.instance.onPurchaseSuccess -= InstanceOnPurchaseSuccess;
        SceneManager.LoadScene("SkinChange", LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync("SkinChange", UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);

    }
    private void InstanceOnPurchaseNoAddsSuccess(Purchase obj)
    {
        for (int i = 11; i < 15; i++)
        {
            var skinAsset = _skins.Skins.First(x => x._id == i - 10);
            OpenSkin(skinAsset);
            _skinViewMan.Apply(skinAsset._skin);
        }
        //PlayerPrefs.SetInt("SkinsNoAdds", 1);
        //PlayerPrefs.SetInt("NoAdds", 1);
        YandexSDK.instance.onPurchaseSuccess -= InstanceOnPurchaseNoAddsSuccess;
        SceneManager.LoadScene("SkinChange", LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync("SkinChange", UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);

    }
    public void BuySkinsNoAdds()
    {
        YandexSDK.instance.onPurchaseSuccess += InstanceOnPurchaseNoAddsSuccess;
        YandexSDK.instance.ProcessPurchase("22");
    }
    public void BuySkin()
    {
        YandexSDK.instance.onPurchaseSuccess += InstanceOnPurchaseSuccess;
        YandexSDK.instance.ProcessPurchase(buy_id);
    }
    private void SetupClicks()
    {
        var audio = FindObjectOfType<GameAudio>();

        var roots = SceneManager.GetSceneByName("SkinChange").GetRootGameObjects();
        foreach (var root in roots)
        {
            foreach (var button in root.GetComponentsInChildren<Button>())
            {
                button.onClick.AddListener(() => audio.Play(_clicked));
            }
        }
    }

    private void OnRewardedClicked(SkinView view, HeadSkin skin)
    {
        //TODO AD
        YandexSDK.instance.onRewardedAdReward += (obj) => OnRewardedAdReward(obj, view, skin);
        YandexSDK.instance.ShowRewarded("SkinOpenChange");
    }

    private void OnRewardedAdReward(string obj, SkinView view, HeadSkin skin)
    {
        if (obj == "SkinOpenChange")
        {
            var skinAsset = _skins.Skins.First(x => x.Skin == skin);
            OpenSkin(skinAsset);
            view.RemoveOnClickedAction();

            YandexSDK.instance.onRewardedAdReward -= (obj) => OnRewardedAdReward(obj, view, skin);
        }
    }

    private void OnClickedSkinView(HeadSkin headSkin, int id)
    {
        _skinViewMan.Apply(headSkin);
        PlayerData.Skin = id;
    }
    
    public void Disable()
    {
        SceneManager.UnloadSceneAsync("SkinChange", UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}