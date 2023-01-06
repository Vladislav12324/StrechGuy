using UnityEngine;

public class OnDragJointAudio : MonoBehaviour
{
    [SerializeField] private JointConnector _joint;
    [SerializeField] private AudioClip _clip;

    private IGameAudio _audio;
    private AudioSource _source;
    
    private void OnEnable()
    {
        _joint.MouseUp += OnMouseUp;
        _joint.MouseDown += OnMouseDown;
        _audio = FindObjectOfType<GameAudio>();
    }

    private void OnMouseUp(JointConnector joint)
    {
        _source.Stop();
    }

    private void OnMouseDown(JointConnector joint)
    {
        _source = _audio.PlayAndGet(_clip);
        _source.loop = true;
    }

    private void OnDisable()
    {
        _joint.MouseUp -= OnMouseUp;
        _joint.MouseDown -= OnMouseDown;
    }
}