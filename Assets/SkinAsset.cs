using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Create SkinAsset", fileName = "SkinAsset", order = 0)]
public class SkinAsset : ScriptableObject
{
    [SerializeField] public HeadSkin _skin;
    [SerializeField] private Sprite _image;
    [SerializeField] private bool _alwaysOpened;
    [SerializeField] private Color _ropeColor1;
    [SerializeField] public int _id;
    public HeadSkin Skin => _skin;

    public Sprite Image => _image;

    public bool AlwaysOpened => _alwaysOpened;

    public override int GetHashCode()
    {
        return name.Aggregate(0, (current, t) => current + t);
    }
}