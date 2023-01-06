using System.Linq;
using Eiko.YaSDK;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SkinChangeView : MonoBehaviour
{
    [SerializeField] private SpringMan _skinViewMan;
    [SerializeField] private SkinView _template;
    [SerializeField] private SkinsAsset _skins;
    [SerializeField] private RectTransform _content;
    [SerializeField] private Sprite _locked;
    [SerializeField] private Sprite _rewarded;
    [SerializeField] private AudioClip _clicked;
    
    private void Start()
    {
        _skinViewMan.Deactivate();
        _skinViewMan.Apply(gameObject.AddComponent<InfinityMaxRopeLengthBonus>());
        _skinViewMan.Apply(_skins.GetById(PlayerData.Skin));

        var rewarded = _skins.Skins.Random(null, x => PlayerData.IsOpened(x) == false);

        foreach (var skinAsset in _skins.Skins)
        {
            if (skinAsset == rewarded)
            {
                var rewardedView = _template.Instantiate(_content, _rewarded, rewarded.Skin, rewarded.GetHashCode(),true);
                rewardedView.Clicked += OnClickedSkinView;
                rewardedView.AddOnClickedAction(OnRewardedClicked);
                continue;
            }
            
            var opened = PlayerData.IsOpened(skinAsset);
            var sprite = opened ? skinAsset.Image : _locked;
            
            var skinView = _template.Instantiate(_content, sprite, skinAsset.Skin, skinAsset.GetHashCode(),opened);
            skinView.Clicked += OnClickedSkinView;
        }
        
        SetupClicks();
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
            PlayerData.OpenSkin(skinAsset);
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