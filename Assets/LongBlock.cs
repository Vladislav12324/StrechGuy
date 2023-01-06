using UnityEngine;

public class LongBlock : LevelBlock
{
    [SerializeField] private SpringMan _man;
    [SerializeField] private Transform _finish;
    [SerializeField] private Color _backgroundColor;

    public SpringMan Man => _man;

    public Transform Finish => _finish;

    public Color BackgroundColor => _backgroundColor;
}