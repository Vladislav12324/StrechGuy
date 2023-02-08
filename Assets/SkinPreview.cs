using Eiko.YaSDK;
using Sources.Advertising;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SkinPreview : MonoBehaviour
{
    [SerializeField] private SpringMan _preview;

    private static SkinAsset _skinAsset;
    [SerializeField] private AudioClip _clicked;

    private void Start()
    {
        _preview.Deactivate();
        _preview.Apply(_skinAsset.Skin);
        
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

    public void OpenSkinByViewAd()
    {
        AdvertisingSdk.RewardedEnded += DD;
        AdvertisingSdk.ShowRewarded("SkinOpen");
        //TODO AD
    }

    private void DD(string obj)
    {
        if (obj == "SkinOpen")
        {
            PlayerData.OpenSkin(_skinAsset);
            PlayerData.Skin = _skinAsset.GetHashCode();
            Close();
            AdvertisingSdk.RewardedEnded -= DD;
        }
    }

    public static void Show(SkinAsset skinAsset)
    {
        _skinAsset = skinAsset;
        SceneManager.LoadScene("SkinPreview", LoadSceneMode.Additive);
    }

    private void Close()
    {
        SceneManager.UnloadSceneAsync("SkinPreview", UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
    }
}