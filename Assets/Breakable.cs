using UnityEngine;
using UnityEngine.Events;

public class Breakable : MonoBehaviour
{
    [SerializeField] private UnityEvent _onBreak;

    public void Break()
    {
        Destroy(gameObject);
        _onBreak.Invoke();
    }
}