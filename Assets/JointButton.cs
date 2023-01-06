using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class JointButton : Interactable<JointConnector>
{
    [SerializeField] private UnityEvent _interacted;
    [SerializeField] private UnityEvent _interactEnd;
    [SerializeField] private Transform _position;
    [SerializeField] private Transform _button;
    [SerializeField] private AudioClip _clicked;
    
    public override void OnInteract(JointConnector interact)
    {
        if(!interact.Dragged)
            return;
        
        interact.Connect(true);
        interact.transform.position = _position.position;
        _button.DOMove(-_button.up * 0.05f + transform.position, 1);
        _interacted?.Invoke();
        Audio.Play(_clicked);
    }

    public override void OnInteractEnd(JointConnector interact)
    {
        _button.DOMove(_button.up * 0.05f + transform.position, 1);
        _interactEnd?.Invoke();
    }
}