using DG.Tweening;
using UnityEngine;


public class CameraTrigger : Interactable<JointConnector>
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Vector3 _moveDirection;
    [SerializeField] private bool _interactable = true;
    
    public override void OnInteract(JointConnector interact)
    {
        if(_interactable)
            Move();
        
        _interactable = false;
    }

    public void Move()
    {
        _camera.transform.DOMove(_moveDirection + _camera.transform.position, 2)
            .OnComplete(() => Destroy(this));
    }

    public void Init(Camera camera)
    {
        _camera = camera;
    }
}