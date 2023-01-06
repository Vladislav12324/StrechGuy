using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum RopeTornType
{
    Length,
    Break
}

public class SpringMan : MonoBehaviour
{
    public event Action<JointConnector> JointDragged;
    public event Action RopeTorn;
    
    [SerializeField] private int _spring;
    [SerializeField] private int _damper;
    [SerializeField] private float _distance;


    [SerializeField] public List<JointConnector> _joints;
    [SerializeField] public Renderer _head;
    [SerializeField] private List<Texture> _textures;
    [SerializeField] private SpringRope _bodyRope;

    public Transform Head => _head.transform;

    public bool Active => _active;
    public GameObject christmas_hat;
    //public Transform head;
    private int _minCountConnectedJoints = 1;
    private bool _active = true;
    private bool _breakable = true;

    private void Start()
    {
        foreach (var joint in _joints)
        {
            joint.Init(_spring, _damper, _distance);
            joint.MouseDragged += OnJointDragged;
            joint.Rope.RopeTorn += OnRopeTorn;
        }

        ChangeHeadColor();
        christmas_hat.GetComponent<hat>().head = Head;
        christmas_hat.GetComponent<hat>().hero = this.transform;
       // HEAD.Instantiate(christmas_hat, new Vector3(head.position.x, head.position.y + 0.35f, head.position.z), head.rotation);
    }

    private void OnRopeTorn(RopeTornType type)
    {
        if(_breakable == false && type == RopeTornType.Break)
            return;
        
        _head.materials[1].mainTexture = _textures[3];
        
        foreach (var joint in _joints)
        {
            joint.Tear();
        }
        
        RopeTorn?.Invoke();
    }

    private void Update()
    {
        ChangeHeadColor();
    }

    public void Deactivate()
    {
        _head.GetComponent<Rigidbody>().isKinematic = true;
        
        foreach (var joint in _joints)
        {
            joint.Deactivate();
        }

        _active = false;
    }

    public void Activate()
    {
        _head.GetComponent<Rigidbody>().isKinematic = false;
        
        foreach (var joint in _joints)
        {
            joint.Activate();
        }

        _active = true;
    }


    private void OnJointDragged(JointConnector joint)
    {
        if(_active == false)
            return;
        
        JointDragged?.Invoke(joint);
        ChangeHeadColor();

        var count = _joints.Count(jointConnector => jointConnector.Connected);

        if (count == _minCountConnectedJoints && count > 0 && _active)
        {
            _joints.First(x => x.Connected).Deactivate();
        }
        else
        {
            _joints.ForEach(x => x.Activate());
        }
    }

    private void ChangeHeadColor()
    {
        if(_active == false)
            return;
        
        var delta = 0f;

        foreach (var joint in _joints)
            delta = Mathf.Max(joint.Rope.Length / joint.Rope.MaxLength, delta);
        
        _head.materials[1].mainTexture = delta switch
        {
            <= 0.5f => _textures[0],
            <= 0.75f => _textures[1],
            _ => _textures[2]
        };

        _head.material.color = Color.Lerp(Color.green, Color.red, delta) * Mathf.Pow(2, 1.25f);
    }

    public void Apply(Bonus bonus)
    {
        foreach (var joint in _joints)
        {
            joint.Rope.SetMaxLength(bonus.GetModifiedMaxRopeLength(joint.Rope.MaxLength));
            joint.Rope.SetBreakable(bonus.Breakable);
        }
        
        _bodyRope.SetBreakable(bonus.Breakable);
        _bodyRope.SetMaxLength(bonus.GetModifiedMaxRopeLength(_bodyRope.MaxLength));

        _breakable = bonus.Breakable;
        _minCountConnectedJoints = bonus.CountMinConnectedJoints;
    }
    
    public void Apply(HeadSkin skin)
    {
        foreach (var joint in _joints)
        {
            joint.Rope.SetRopeColor(skin.RopeColors.first, skin.RopeColors.second);
        }
        
        _bodyRope.SetRopeColor(skin.RopeColors.first, skin.RopeColors.second);
        
        skin.Instantiate(_head.transform);
    }

    public void HappyFace()
    {
        _head.materials[1].mainTexture = _textures[0];
        _head.material.color = new Color(0, 0.5f, 0.75f);
        _active = false;
    }
}
