using System;
using UnityEngine;

public class LevelBlock : MonoBehaviour
{
    [SerializeField] private Transform _enter;
    [SerializeField] private Transform _exit;
    [SerializeField] private CameraTrigger _trigger;

    public Transform Enter => _enter;

    public Transform Exit => _exit;

    public CameraTrigger Trigger => _trigger;

    public override bool Equals(object other)
    {
        if (other is LevelBlock block)
            return Equals(block);
        
        return base.Equals(other);
    }

    private bool Equals(LevelBlock other)
    {
        return Equals(_enter, other._enter) && Equals(_exit, other._exit) && Equals(_trigger, other._trigger);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_enter, _exit, _trigger);
    }
}