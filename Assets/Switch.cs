using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class Switch : Interactable<JointConnector>
{
    [SerializeField] private UnityEvent _switched;
    [SerializeField] private Renderer _colorQuad;
    [SerializeField] private Transform _switch;
    [SerializeField] private AudioClip _switchClip;

    private bool _activated;

    public bool Activated => _activated;

    public UnityEvent Switched => _switched;

    public override void OnInteract(JointConnector interact)
    {
        if(_activated)
            return;
        
        _colorQuad.material.color = Color.green;
        _switch.DOComplete();
        _switch.DORotate(new Vector3(0, 0, _switch.rotation.eulerAngles.z + 70), 0.5f);
        _activated = true;
        Audio.Play(_switchClip);
        _switched.Invoke();
    }

    public void Deactivate()
    {
        if(_activated == false)
            return;
        
        _colorQuad.material.color = Color.red;
        _switch.DOComplete();
        _switch.DORotate(new Vector3(0, 0, _switch.rotation.eulerAngles.z - 70), 0.5f);
        _activated = false;
    }
}