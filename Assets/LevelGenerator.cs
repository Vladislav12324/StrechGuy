using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private StartBlock _start;
    [SerializeField] private List<LevelBlock> _blocks;
    [SerializeField] private List<LongBlock> _longBlocks;
    [SerializeField] private FinishBlock _finishBlock;
    [SerializeField] private Level _level;
    [SerializeField] private GameUi _ui;
    [SerializeField] private CameraFollower _camera;

    private static LevelBlock _previous;
    private static List<LevelBlock> _unusedBlocks = new List<LevelBlock>();

    private void Awake()
    {
        if(_unusedBlocks.Count == 0)
            _unusedBlocks.AddRange(_blocks);
        
        Transform finishZone = null;
        CameraTrigger trigger = null;
        SpringMan man = null;
        Color background = Color.black;
        
        if (PlayerData.Level % 7 == 0)
        {
            _camera.enabled = true;
            var longBlock = GenerateBlock(_longBlocks.Random(null), null);
            
            finishZone = longBlock.Finish;
            man = longBlock.Man;
            trigger = longBlock.Trigger;
            background = longBlock.BackgroundColor;
        }
        else
        {
            _camera.enabled = false;
            StartBlock start = GenerateBlock(_start, null);
            _previous = _unusedBlocks.Random(_previous);
            _unusedBlocks.Remove(_previous);
            LevelBlock block = GenerateBlock(_previous, start);
            FinishBlock finish = GenerateBlock(_finishBlock, block);

            finishZone = finish.FinishZone;
            trigger = block.Trigger;
            man = start.Man;
        }
        _camera.SetMax(finishZone);
        _camera.SetJoints(man._joints[0], man._joints[1], man._joints[2], man._joints[3],man._head.transform);
        _level.Init(man, FindObjectsOfType<LevelWall>(), finishZone, trigger, background);
        _ui.SetActions(LoadNext, Reload);
    }

    private void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    private void LoadNext()
    {
        var sceneIndex = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(sceneIndex);
        PlayerData.Level++;
    }

    private T GenerateBlock<T>(T prefab, [AllowNull] LevelBlock previous) where T : LevelBlock
    {
        var block = Instantiate(prefab);

        if (previous is null)
            return block;
        
        var offset = block.Enter.position - block.transform.position;
        block.transform.position = previous.Exit.position - offset;

        return block;
    }
}