using System.Collections;
using UnityEngine;

public class ItemsSpawner : MonoBehaviour
{
    [SerializeField] private Pickable[] _items;
    [SerializeField] private float _delay;

    private void Start()
    {
        StartCoroutine(SpawnRoutine(_delay));
    }

    private IEnumerator SpawnRoutine(float delay)
    {
        while (isActiveAndEnabled)
        {
            yield return new WaitForSeconds(delay);

            Instantiate(_items.Random(null), transform.position, Quaternion.identity);
        }
    }

}