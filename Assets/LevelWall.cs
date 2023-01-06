using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class LevelWall : MonoBehaviour
{
    [SerializeField] private Vector3 _constantSide;
    [SerializeField] private bool _change = true;
    
    private BoxCollider _collider;

    public BoxCollider Collider => _collider;

    public Vector3 ConstantSide => _constantSide;

    private void Start()
    {
        _collider = GetComponent<BoxCollider>();
        var size = Vector3.one * 10;
        var center = (size * 0.5f) - Vector3.one * 0.5f;
    }
}