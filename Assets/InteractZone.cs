using UnityEngine;
using UnityEngine.Events;

public class InteractZone : Interactable<JointConnector>
{
    [SerializeField] private UnityEvent _interacted;
    [SerializeField] private UnityEvent _interactedEnd;
    
    public override void OnInteract(JointConnector interact)
    {
        _interacted?.Invoke();
    }

    public override void OnInteractEnd(JointConnector interact)
    {
        _interactedEnd?.Invoke();
    }
}