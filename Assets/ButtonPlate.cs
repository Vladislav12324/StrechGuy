using UnityEngine;
using UnityEngine.Events;

public class ButtonPlate : Interactable<Pickable>
{
    [SerializeField] private Transform _placePoint;
    [SerializeField] private UnityEvent _interacted;

    private bool _isInteracted;
    
    public override void OnInteract(Pickable interact)
    {
        if(_isInteracted)
            return;
        
        interact.UnPick();
        Destroy(interact);
        interact.transform.position = _placePoint.position;
        _interacted?.Invoke();
        _isInteracted = true;
    }
}