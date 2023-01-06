using UnityEngine;

public class StartBlock : LevelBlock
{
    [SerializeField] private SpringMan _man;

    public SpringMan Man => _man;
}