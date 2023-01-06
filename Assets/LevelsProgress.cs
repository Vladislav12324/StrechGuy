using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelsProgress : MonoBehaviour
{
    [SerializeField] private List<Image> _levelIndicators;
    [SerializeField] private Sprite _caseOn;

    public void Set(int level)
    {
        while (level > _levelIndicators.Count)
            level -= _levelIndicators.Count;

        for (int i = 0; i < _levelIndicators.Count; i++)
        {
            if(i == level - 1)
            {
                _levelIndicators[i].transform.localScale *= 1.3f;
                StartCoroutine(Pulsing(_levelIndicators[i].transform));
                return;
            }

            _levelIndicators[i].sprite = _caseOn;
            _levelIndicators[i].color = Color.white;
        }
    }

    private IEnumerator Pulsing(Transform pulse)
    {
        var scl = pulse.localScale.x;

        while (isActiveAndEnabled)
        {
            var scale = Mathf.PingPong(Time.time / 5, scl * 0.25f) + scl;
            pulse.localScale = Vector3.one * scale;

            yield return new WaitForSeconds(0.1f);
        }
    }
}