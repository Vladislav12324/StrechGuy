using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Sources.Data;

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
            Prefs.Increment("skinprogress");
            _progressValue = Prefs.GetInt("skinprogress");
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
            Prefs.SetInt("skinprogress", 0);
        }
            
    }
}