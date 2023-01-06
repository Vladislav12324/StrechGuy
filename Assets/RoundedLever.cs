using UnityEngine;
using UnityEngine.Events;

public class RoundedLever : Interactable<JointConnector>
{
    [SerializeField] private UnityEvent<float> _rotated;
    [SerializeField] private AudioClip _rotatedClip;
    
    [SerializeField] private Transform _lever;

    private float _rotatedAngles;
    private AudioSource _source;
    
    public override void OnInteract(JointConnector interact)
    {
        if(!interact.Dragged)
            return;
        
        interact.MoveExecutor = MoveExecutor;
        
        _source = Audio.PlayAndGet(_rotatedClip);
        _source.loop = true;
    }

    private Vector3 MoveExecutor(JointConnector joint, Vector3 movePoint)
    {
        var bounds = joint.Collider.bounds;
        bounds.center = movePoint;
        
        if(!Intersects(bounds))
            OnInteractEnd(joint);
        
        var direction = movePoint - transform.position;
        var newAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        var rotation = Quaternion.AngleAxis(newAngle, Vector3.forward);

        var oldAngle = transform.rotation.eulerAngles.z;
        
        var angle = Quaternion.Angle(rotation, transform.rotation);
        
        if (angle > 0)
        {
            if (newAngle <= 0)
                newAngle = 360 + newAngle;

            if (newAngle > oldAngle)
                _rotatedAngles -= angle;
            else if (newAngle < oldAngle)
                _rotatedAngles += angle;

            if (_rotatedAngles > 360)
                _rotatedAngles = 360f;
            else if (_rotatedAngles < -360)
                _rotatedAngles = -360f;
            
            var value = Mathf.Abs(_rotatedAngles / 360);

            _rotated?.Invoke(value);
        }

        transform.rotation = rotation;
        
        return _lever.position;
    }

    public override void OnInteractEnd(JointConnector interact)
    {
        interact.MoveExecutor = null;
        
        _source.Stop();
    }
}