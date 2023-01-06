using UnityEngine;
using FGL_YaSdk.Ads;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class netxLevelAds : MonoBehaviour, IYaAdsListener
{
    public int placemetID = 6775444;
    public UnityEvent revardSuccess;
    public AudioSource buttonSound;

    public AudioMixer soundMix;
    public AudioMixer musicMix;

    private void Start()
    {
        YaAds.AddListener(this);
    }
    public void ShowRevardnextLevel()
    {
        buttonSound.Play();
        soundMix.SetFloat("Volume", -80);

        musicMix.SetFloat("Volume", -80);
        //YaAds.ShowRewardAds(placemetID);
        if (PlayerPrefs.GetInt("levelsComplete") < SceneManager.GetActiveScene().buildIndex)
            PlayerPrefs.SetInt("levelsComplete", PlayerPrefs.GetInt("levelsComplete") + 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void OnYaAdsDidFinish(int placementId, ShowResult showResult)
    {
        if (PlayerPrefs.GetString("sound") == "on")
        {
            soundMix.SetFloat("Volume", 0);
        }
        if (PlayerPrefs.GetString("music") == "on")
        {
            musicMix.SetFloat("Volume", 0);
        }

        if (placementId == placemetID && showResult == ShowResult.success)
        {

            if (PlayerPrefs.GetInt("levelsComplete") < SceneManager.GetActiveScene().buildIndex)
                PlayerPrefs.SetInt("levelsComplete", PlayerPrefs.GetInt("levelsComplete") + 1);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
