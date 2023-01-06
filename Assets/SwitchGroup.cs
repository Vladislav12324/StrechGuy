using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Events;

public class SwitchGroup : MonoBehaviour
{
    [SerializeField] private UnityEvent _activated;
    [SerializeField] private List<Switch> _switches;

    private int _index1, _index2;
    private void Awake()
    {
        if (_switches.Count != 3)
            throw new SerializationException("Need 3 Switch");

        _index1 = Random.Range(0, 3);

        _index2 = _index1;

        while (_index2 == _index1)
            _index2 = Random.Range(0, 3);
        
        foreach (var @switch in _switches)
        {
            @switch.Switched.AddListener(CheckSwitches);
        }
    }

    private void CheckSwitches()
    {
        var countActivated = _switches.Count(x => x.Activated);

        if (countActivated == 2)
        {
            if(_switches[_index1].Activated && _switches[_index2].Activated)
                return;
            
            _switches.ForEach(x => x.Deactivate());
        }
        
        if(countActivated == 3)
            _activated?.Invoke();
    }
}