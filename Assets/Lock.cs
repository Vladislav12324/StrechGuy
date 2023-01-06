using System;
using UnityEngine;
using UnityEngine.Events;

public class Lock : Interactable<Pickable>, IDynamicJointConnection
{
    [SerializeField] private UnityEvent _interacted;
    [SerializeField] private AudioClip _clip;
    [SerializeField] private bool _destroy = true;
    
    public override void OnInteract(Pickable interact)
    {
        if(interact.gameObject.layer != LayerMask.NameToLayer("Items"))
            return;
        
        if(_destroy)
            Destroy(interact.gameObject);
        
        Destroy(gameObject);
        Audio.Play(_clip);
        _interacted?.Invoke();
        Changed?.Invoke(this);
    }

    public event Action<IDynamicJointConnection> Changed;
}