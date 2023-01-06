using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinOpenView : MonoBehaviour
{
    [SerializeField] private Slider _progress;
    [SerializeField] private SkinsAsset _skins;
    [SerializeField] private TextMeshProUGUI _progressPercents;
    public int test;

    public static bool Added;
    private static int _progressValue;
    public void UpProgress()
    {
        if (Added)
        {
            Added = false;
        }
        else
        {
            _progressValue = PlayerPrefs.GetInt("skinprogress");
            _progressValue++;
            if(PlayerPrefs.GetInt("skinprogress")< _progressValue)
                PlayerPrefs.SetInt("skinprogress", _progressValue);
        }

        _progress.DOValue(_progressValue / 10f, 1);
        _progressPercents.text = $"{_progressValue * 10}%";

        if (_progressValue >= 10)
        {
            var skin = _skins.Skins.Random(null);
            _progress.value = 1;

            while (PlayerData.IsOpened(skin))
            {
                skin = _skins.Skins.Random(skin);
            }
            
            SkinPreview.Show(skin);
            _progressValue = 0;
            PlayerPrefs.SetInt("skinprogress", 0);
        }
            
    }
    private void Update()
    {
    }
}