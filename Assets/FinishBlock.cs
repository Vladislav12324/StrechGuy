using UnityEngine;

public class FinishBlock : LevelBlock
{
    [SerializeField] private Transform _finishZone;

    public Transform FinishZone => _finishZone;
}