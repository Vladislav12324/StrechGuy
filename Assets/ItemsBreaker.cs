using UnityEngine;

[DisallowMultipleComponent]
public class ItemsBreaker : Interactable<Pickable>
{
    [SerializeField] private AudioClip _clip;
    public override void OnInteract(Pickable interact)
    {
        interact.Destroy();
        
        if(_clip)
            Audio.Play(_clip);   
    }
}