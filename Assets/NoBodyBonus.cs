using UnityEngine;

public sealed class NoBodyBonus : Bonus
{
    [SerializeField] private SpringMan _noBodyPrefabSpringMan;
    
    public override SpringMan TryGetModifiedSpringMan(SpringMan man)
    {
        var position = man.transform.position;
        var rotation = man.transform.rotation;
        
        Destroy(man);
        return Instantiate(_noBodyPrefabSpringMan, position, rotation);
    }
}