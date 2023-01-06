using System;
using UnityEngine;


[RequireComponent(typeof(Collider))]
public class JointConnector : MonoBehaviour
{
    public event Action<JointConnector> MouseDragged; 
    public event Action<JointConnector> MouseUp; 
    public event Action<JointConnector> MouseDown; 
    
    public Rigidbody _rigidbody;
    public SpringJoint _springJoint;

    [SerializeField] private bool _connected = true;
    [SerializeField] private SpringRope _rope;
    private float _connectedValue;
    private bool _active;
    private Vector3 _connectedAxis;
    private Bounds _connectedCollider;

    public bool Connected => _connected;
    public Collider Collider { get; private set; }
    public bool Dragged { get; private set; }

    public SpringRope Rope => _rope;
    
    public Func<JointConnector, Vector3, Vector3> MoveExecutor;
    private IGameAudio _audio;
    
    [SerializeField] private AudioClip _connectedClip;
    [SerializeField] private AudioClip _disconnectedClip;
    private AudioSource _connectedAudioSource;
    private AudioSource _disconnectAudioSource;

    public void Init(int spring, int damper, float distance)
    {
        _springJoint.damper = damper;
        _springJoint.spring = spring;
        _springJoint.maxDistance = distance;
        _springJoint.minDistance = 0;
    }

    private void Start()
    {
        Collider = GetComponent<Collider>();

        _audio = FindObjectOfType<GameAudio>();
        
        if(_connected)
            Connect(false);
    }

    public void Activate()
    {
        _active = true;
        GetComponent<Renderer>().material.color = Color.white;
    }

    private void OnMouseUp()
    {
        if (_active)
        {
            transform.localScale = Vector3.one * 0.5f;
            MouseUp?.Invoke(this);
        }

        Dragged = false;
    }

    private void OnMouseDown()
    {
        if (_active)
        {
            transform.localScale = Vector3.one * 0.6f;
            MouseDown?.Invoke(this);
        }

        Dragged = true;
    }

    public void Connect(bool withSound)
    {
        _connected = true;

        _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        
        if(ConnectionSoundsNotPlaying && withSound)
            _connectedAudioSource = _audio.PlayAndGet(_connectedClip);
    }
    
    public void Connect(IDynamicJointConnection connection)
    {
        Connect(true);

        connection.Changed += Disconnect;
    }

    public bool ConnectionSoundsNotPlaying
        => (!_connectedAudioSource || !_disconnectAudioSource)
           || (!_connectedAudioSource.isPlaying && !_disconnectAudioSource.isPlaying);
    
    public void Tear()
    {
        if(!_springJoint) return;
        
        _springJoint.breakForce = 0;
        _rope.Tear();
    }

    public void Disconnect()
    {
        _connected = false;
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
        
        if(ConnectionSoundsNotPlaying)
            _disconnectAudioSource = _audio.PlayAndGet(_disconnectedClip);
    }
    
    public void Disconnect(IDynamicJointConnection connection)
    {
        Disconnect();

        connection.Changed -= Disconnect;
        
    }

    private void OnMouseDrag()
    {
        if (!_active) 
            return;
        
        MouseDragged?.Invoke(this);
        
        _rigidbody.velocity = Vector3.zero;
    }

    public void Deactivate()
    {
        _active = false;
        GetComponent<Renderer>().sharedMaterial.color = new Color(0.25f, 0.25f, 0.25f, 1);
        //Debug.Log(_active);
    }
}
