using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Create SkinsAsset", fileName = "SkinsAsset", order = 0)]
public class SkinsAsset : ScriptableObject
{
    [SerializeField] private List<SkinAsset> _skins;


    public IReadOnlyCollection<SkinAsset> Skins => _skins;

    public HeadSkin GetById(int id)
    {
        return _skins.First(x => x.GetHashCode() == id).Skin;
    }
}