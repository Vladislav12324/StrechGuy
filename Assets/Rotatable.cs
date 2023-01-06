using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Rotatable : MonoBehaviour
{
    [SerializeField] private Vector3 _rotateAxis;
    [SerializeField] private float _eulerRotateValue;
    [SerializeField] private float _speed;
    [SerializeField] private bool _fromStart = true;

    private void Start()
    {
        if(_fromStart)
            StartCoroutine(Rotate());
    }

    public void RotateByValue()
    {
        transform.DORotate(_rotateAxis * _eulerRotateValue, 1);
    }

    private IEnumerator Rotate()
    {
        while (gameObject.activeInHierarchy)
        {
            var d = _rotateAxis * _speed;
            transform.rotation = Quaternion.Euler(d + transform.rotation.eulerAngles);
            yield return new WaitForEndOfFrame();   
        }
    }
}