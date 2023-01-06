using UnityEngine;

public class PlaySoundWhenMove : MonoBehaviour
{
    [SerializeField] private AudioClip _clip;

    private IGameAudio _audio;
    private Vector3 _previousFramePosition;
    private AudioSource _source;

    private void Start()
    {
        _audio = FindObjectOfType<GameAudio>();
        _previousFramePosition = transform.position;
    }

    private void Update()
    {
        if ((transform.position - _previousFramePosition).magnitude < 0.01f && _source != null)
        {
            _source.Stop();
            return;
        }
        
        if(_source == null || !_source.isPlaying)
        {
            _source = _audio.PlayAndGet(_clip);
            _source.loop = true;
        }

        _previousFramePosition = transform.position;
    }
}