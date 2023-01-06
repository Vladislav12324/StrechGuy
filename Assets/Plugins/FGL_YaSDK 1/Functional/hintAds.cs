using UnityEngine;
using FGL_YaSdk.Ads;
using UnityEngine.Events;
using UnityEngine.Audio;

public class hintAds : MonoBehaviour, IYaAdsListener
{
    public int placemetID = 6775443;
    public UnityEvent revardSuccess;
    public GameObject hint;
    public AudioSource buttonSound;

    public AudioMixer soundMix;
    public AudioMixer musicMix;

    public static class GameObjectExtension
    {
        public static Object Find(string name, System.Type type)
        {
            Object[] objects = Resources.FindObjectsOfTypeAll(type);
            foreach (var obj in objects)
            {
                if (obj.name == name)
                {
                    return obj;
                }
            }
            return null;
        }

        public static GameObject Find(string name)
        {
            return Find(name, typeof(GameObject)) as GameObject;
        }
    }

    private void Start()
    {
        YaAds.AddListener(this);
        //hint = GameObjectExtension.Find("hint");
    }
    public void ShowRevardHint()
    {
        /*buttonSound.Play();

            soundMix.SetFloat("Volume", -80);

            musicMix.SetFloat("Volume", -80);

        //hint = GameObjectExtension.Find("hint");
#if !UNITY_EDITOR
        YaAds.ShowRewardAds(placemetID);
#endif*/
            hint.SetActive(true);
    }

    public void OnYaAdsDidFinish(int placementId, ShowResult showResult)
    {
        if (placementId == placemetID && showResult == ShowResult.success)
        {
            if (hint)
                hint.SetActive(true);
            else
            {
                hint = GameObjectExtension.Find("hint");
                hint.SetActive(true);
                if (PlayerPrefs.GetString("sound") == "on")
                {
                    soundMix.SetFloat("Volume", 0);
                }
                if (PlayerPrefs.GetString("music") == "on")
                {
                    musicMix.SetFloat("Volume", 0);
                }
            }
        }
    }
}
