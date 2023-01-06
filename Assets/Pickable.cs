using UnityEngine;

[DisallowMultipleComponent]
public class Pickable : Interactable<JointConnector>
{
    [SerializeField] private bool _destroyable = true;
    [SerializeField] private GameObject _onDestroySpawnedItem;
    
    private bool _picked;

    public void PickUp(GameObject picker)
    {
        if(_picked)
            return;
        
        if(picker.transform.childCount > 1)
            return;
        
        transform.SetParent(picker.transform);
        
        transform.localPosition = Vector3.zero;
        _picked = true;

        if (gameObject.TryGetComponent<Rigidbody>(out var rigidbody))
            Destroy(rigidbody);
    }

    public void UnPick()
    {
        transform.SetParent(null);
        _picked = false;
    }

    public override void OnInteract(JointConnector interact)
    {
        if(interact.Dragged)
            PickUp(interact.gameObject);
    }

    public void Destroy()
    {
        if (_destroyable && enabled)
        {
            Destroy(gameObject);
            enabled = false;
        }
        
        if(_onDestroySpawnedItem)
            Instantiate(_onDestroySpawnedItem, transform.position, Quaternion.identity);
    }
}