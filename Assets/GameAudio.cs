using UnityEngine;
using UnityEngine.Audio;


public interface IVibration
{
    public void Vibrate();
}

public class GameAudio : MonoBehaviour, IGameAudio, IActivate
{
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private AudioMixerGroup _mixerGroup;

    private GameObject _gameAudio;

    public bool Status { get; private set; }

    private void Awake()
    {
        _gameAudio = new GameObject("GameAudio");
        _gameAudio.transform.parent = transform;

        Status = PlayerData.SoundStatus;
        
        if(PlayerData.SoundStatus)
            Activate();
        else
            Deactivate();
    }

    public AudioSource PlayAndGet(AudioClip clip)
    {
        if (_gameAudio == null)
            return null;
        
        var source = _gameAudio.AddComponent<AudioSource>();
        source.outputAudioMixerGroup = _mixerGroup;
        source.playOnAwake = false;
        source.clip = clip;
        source.Play();

        return source;
    }

    public void Play(AudioClip clip)
    {
        PlayAndGet(clip);
    }


    public void Activate()
    {
        _mixer.SetFloat("MasterVolume", 0);
        Status = true;
        PlayerData.SoundStatus = Status;
    }

    public void Deactivate()
    {
        _mixer.SetFloat("MasterVolume", -80);
        Status = false;
        PlayerData.SoundStatus = Status;
    }
}