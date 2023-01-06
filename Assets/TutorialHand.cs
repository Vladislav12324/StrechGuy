using DG.Tweening;
using UnityEngine;

public class TutorialHand : MonoBehaviour
{
    [SerializeField] private SpringMan _man;
    [SerializeField] private Transform _hand;
    [SerializeField] private Vector3[] _path;
    [SerializeField] private RectTransform _menu;

    private void Start()
    {
        _man.JointDragged += ManOnJointDragged;
        
        for (int i = 0; i < _path.Length; i++)
        {
            _path[i] += _hand.transform.position;
        }
        
        Move(_hand.position);
    }

    private void ManOnJointDragged(JointConnector obj)
    {
        _hand.DOKill();
        Destroy(_hand.gameObject);
        _menu.gameObject.SetActive(false);
        _man.JointDragged -= ManOnJointDragged;
    }


    public void Move(Vector3 startPosition)
    {
        transform.position = startPosition;
        _hand.DOPath(_path, 1, PathType.CatmullRom).OnComplete(() => Move(startPosition));
    }
}