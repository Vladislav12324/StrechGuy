using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    private IGameAudio _audio;
    private IVibration _vibration;

    public IGameAudio Audio => _audio;

    public IVibration Vibration => _vibration;

    public void Init(IGameAudio audio, IVibration vibration)
    {
        _audio = audio;
        _vibration = vibration;
    }   
} 

[RequireComponent(typeof(Collider))]
public abstract class Interactable<T> : Interactable where T : Component
{
    [SerializeField] private bool _trigger = true;
    private Collider _collider;
    private void Start()
    {
        _collider = GetComponent<Collider>();
        _collider.isTrigger = _trigger;
    }

    public bool Intersects(Bounds bounds)
    {
        return _collider.bounds.Intersects(bounds);
    }
    
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<T>(out var interact))
            OnInteract(interact);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<T>(out var interact))
            OnInteractEnd(interact);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<T>(out var interact))
            OnInteracted(interact);
    }

    public abstract void OnInteract(T interact);
    public virtual void OnInteractEnd(T interact) {}
    public virtual void OnInteracted(T interact) {}
}