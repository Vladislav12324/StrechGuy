using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Movable : MonoBehaviour, IDynamicJointConnection
{
    [SerializeField] private Vector3 _direction;
    [SerializeField] private float _distance;
    [SerializeField] private float _speed;
    [SerializeField] private float _moveDelay;
    [SerializeField] private bool _loop;
    [SerializeField] private bool _onStart;
    [SerializeField] private bool _localDirection;
    [SerializeField] private AudioClip _onMoveClip;

    private IGameAudio _audio;
    private Vector3 _startPosition;

    private void Start()
    {
        _startPosition = transform.position;
        _audio = FindObjectOfType<GameAudio>();
        
        if(_onStart)
            Activate();
    }

    private IEnumerator Move(Vector3 destination, float delay)
    {
        yield return new WaitForSeconds(delay);
        
        var point = transform.position;
        transform.DOKill();
        transform.DOMove(destination, (destination - point).magnitude / _speed).OnComplete(() =>
        {
            if(_loop)
                StartCoroutine(Move(point, _moveDelay));
        });
        
        Changed?.Invoke(this);
    }

    public void Activate()
    {
        if(_onMoveClip)
            _audio.Play(_onMoveClip);
        
        StartCoroutine(Move(GetDestination(), 0));
    }

    public void MoveByClamped(float value)
    {
        value = Mathf.Clamp01(value);

        transform.position = Vector3.Lerp(_startPosition, GetDestination(), value);
    }

    private Vector3 GetDestination()
    {
        var direction = _direction;

        if (_localDirection)
            direction = transform.InverseTransformDirection(direction);

        return _startPosition + direction * _distance;
    }

    public void MoveToStart()
    {
        transform.DOKill();
        StartCoroutine(Move(_startPosition, 0));
    }

    public event Action<IDynamicJointConnection> Changed;
}

public interface IActivate
{
    public bool Status { get; }
    public void Activate();
    public void Deactivate();
}