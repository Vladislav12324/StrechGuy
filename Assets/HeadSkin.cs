using UnityEngine;

public class HeadSkin : MonoBehaviour
{
    [SerializeField] private Vector3 _positionOffset;
    [SerializeField] private Color _ropeColor1, _ropeColor2;
    
    public (Color first, Color second) RopeColors => (_ropeColor1, _ropeColor2);

    public void Instantiate(Transform head)
    {
        for (int i = 0; i < head.childCount; i++)
        {
            Destroy(head.GetChild(i).gameObject);
        }
        
        var skin = Instantiate(gameObject, head);
        skin.transform.position += _positionOffset;
    }
}