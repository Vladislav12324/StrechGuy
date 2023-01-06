using System;
using UnityEngine;
using WrappingRopeLibrary.Events;
using WrappingRopeLibrary.Scripts;

public interface IGameAudio
{
    public AudioSource PlayAndGet(AudioClip clip);
    public void Play(AudioClip clip);
}

public abstract class Bonus : MonoBehaviour
{
    public virtual bool Breakable => true;
    public virtual int CountMinConnectedJoints => 1;
    public virtual float GetModifiedMaxRopeLength(float maxLength) => maxLength;
    public virtual SpringMan TryGetModifiedSpringMan(SpringMan man) => man;
}

public class SpringRope : MonoBehaviour
{
    [SerializeField] private float _maxLength = 2f;
    
    private Rope _rope;
    private static readonly int TintColorA = Shader.PropertyToID("_TintColorA");
    private static readonly int TintColorB = Shader.PropertyToID("_TintColorB");
    private bool _breakable = true;

    public event Action<RopeTornType> RopeTorn;

    public float Length { get; private set; }

    public float MaxLength => _maxLength;
    public void SetMaxLength(float maxLength) => _maxLength = maxLength;

    private void Awake()
    {
        _rope = GetComponent<Rope>();
        Length = _rope.GetRopeLength();
        _rope.ObjectWrap += RopeOnObjectWrap;
    }

    public void SetRopeColor(Color color1, Color color2)
    {
        _rope.Material.SetColor(TintColorA, color1);
        _rope.Material.SetColor(TintColorB, color2);
        
        _rope.UpdateMaterial();
    }

    private void RopeOnObjectWrap(RopeBase sender, ObjectWrapEventArgs args)
    {
        if(_breakable == false)
            return;
        
        if(args.Target.TryGetComponent<RopeBreaker>(out var breaker))
        {
            RopeTorn?.Invoke(RopeTornType.Break);
        }
    }

    private void FixedUpdate()
    {
        Length = _rope.GetRopeLength();
        if (Length > _maxLength)
        {
            RopeTorn?.Invoke(RopeTornType.Length);
        }

        var d = 1 - (Length - 1.2f) / _maxLength;
        //_ropeRenderer.thicknessScale = 0.8f * d;
    }

    public void SetBreakable(bool breakable)
    {
        _breakable = breakable;
        if(!breakable)
            _rope.IgnoreLayer =LayerMask.GetMask("Obstacle", "Joints");
    }

    public void Tear()
    {
        if(this)
            Destroy(gameObject);
    }

    public void Break()
    {
        RopeTorn?.Invoke(RopeTornType.Break);
    }
}