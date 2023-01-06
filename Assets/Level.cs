using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Eiko.YaSDK;
using UnityEngine;
using UnityEngine.Events;

public interface IDynamicJointConnection
{
    public event Action<IDynamicJointConnection> Changed;
}

public class Level : MonoBehaviour
{
    [SerializeField] private SpringMan _man;
    [SerializeField] private List<LevelWall> _walls;
    [SerializeField] private CameraTrigger _trigger;
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _finishZone;
    [SerializeField] private Vector3 _moveAxis;
    [SerializeField] private GameUi _ui;
    [SerializeField] private UnityEvent _onWin;
    [SerializeField] private UnityEvent _onStart;
    [SerializeField] private GameAudio _audio;
    [SerializeField] private GameVibration _vibrate;
    [SerializeField] private SkinsAsset _skins;    
    [SerializeField] private YandexSDK _sdk;
    [SerializeField] private AppMetrica _metrica;

    private JointMover _mover;
    [SerializeField] private AudioClip _lose;
    private BonusSelectionView _selection;

    public void Init(SpringMan man, IEnumerable<LevelWall> walls, Transform finish, CameraTrigger trigger, Color color)
    {
        _man = man;
        _walls = walls.ToList();
        _finishZone = finish;
        
        if (color != Color.black)
            _camera.backgroundColor = color;
        
        if(trigger)
            trigger.Init(_camera);
        
        InitInteractable();
    }

    private void InitInteractable()
    {
        var interactables = FindObjectsOfType<Interactable>();
        
        foreach (var interactable in interactables)
        {
            interactable.Init(_audio, _vibrate);
        }
    }
    
    public void Init(Bonus bonus)
    {
        if (bonus != null)
        {
            _man = bonus.TryGetModifiedSpringMan(_man);
            _man.Apply(bonus);   
        }
        
        #if UNITY_EDITOR
        //_man.Apply(gameObject.AddComponent<InfinityMaxRopeLengthBonus>());
        #endif
        
        _man.JointDragged += JointDragged;

        foreach (var jointConnector in FindObjectsOfType<JointConnector>())
        {
            jointConnector.MouseDragged += JointDragged;
        }
        _man.Activate();
        _mover = new JointMover(_camera, _walls.Select(x => x.Collider), _moveAxis);

        if (_trigger)
        {
            _trigger.Init(_camera);
        }
        
        InitInteractable();
        
        _man.RopeTorn += Lose;
        
        _ui.Init();
        //_ui.SkipClicked.AddListener(() => PlayerData.Level++);
        
        _onStart?.Invoke();
    }

    private void OnDestroy()
    {
        PlayerData.Save();

        _selection.BonusSelected -= SelectionOnBonusSelected;
    }

    private void Awake()
    {
        if(!FindObjectOfType<YandexSDK>())
            Instantiate(_sdk);
        
        if(!FindObjectOfType<AppMetrica>())
            Instantiate(_metrica);

        PlayerData.TryLoad(_skins);
        //YandexSDK.instance.ShowInterstitial();
    }
    
    private void Start()
    {
        _man.Deactivate();
        Application.targetFrameRate = 60;
        _man.Apply(_skins.GetById(PlayerData.Skin));
        
        _ui.ShowMenu();
        
        if (PlayerData.Level != 1)
        {
            _selection = _ui.ShowBonusSelectionView();
            _ui.SetupSound(_audio);
            _ui.SetLevelNumber(PlayerData.Level);
            _selection.BonusSelected += SelectionOnBonusSelected;
            
            return;
        }
        _ui.SetLevelNumber(PlayerData.Level);
        _ui.SetupSound(_audio);

        Init(null);
    }

    private void SelectionOnBonusSelected(Bonus obj)
    {
        _ui.HideMenu();
        Init(obj);
        _selection.BonusSelected -= SelectionOnBonusSelected;
    }

    private void OnEnable()
    {
        AppMetrica.Destroyed += AppMetricaOnDestroyed;
    }

    private void AppMetricaOnDestroyed(IYandexAppMetrica metrica)
    {
        var parameters = new Dictionary<string, object>
        {
            {"Level", PlayerData.Level}
        };
        
        metrica.ReportEvent("ExitGameEvent", parameters);
    }

    private void OnDisable()
    {
        AppMetrica.Destroyed -= AppMetricaOnDestroyed;
        
        _man.JointDragged -= JointDragged;

        foreach (var jointConnector in FindObjectsOfType<JointConnector>())
        {
            jointConnector.MouseDragged -= JointDragged;
        }
    }

    private void Lose()
    {
        _man.Deactivate();
        _ui.ShowLoseScreen();
        _audio.Play(_lose);
    }

    private void Win()
    {
        if(!_man.Active)
            return;
        
        _onWin?.Invoke();
        _man.Deactivate();
        _man.HappyFace();
        
        _ui.ShowWinScreen();
        _man.Head.transform.position = _finishZone.transform.position;
    }

    private void JointDragged(JointConnector joint)
    {
        if(!_man.Active)
            _man.Deactivate();
        
        _mover.Move(joint);
        if ((_man.Head.transform.position - _finishZone.transform.position).magnitude <= 0.35f)
            Win();
    }
}

public class JointMover2
{
    private readonly Camera _camera;
    private Plane _plane;
    private readonly IEnumerable<Collider> _bounds;

    public JointMover2(Camera camera, IEnumerable<Collider> bounds, Vector3 moveAxis)
    {
        _camera = camera;
        _bounds = bounds;

        _plane = new Plane(-camera.transform.forward, moveAxis);
    }

    public void Move(JointConnector joint)
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        _plane.Raycast(ray, out var distance);

        var movePoint = ray.GetPoint(distance);
    }
}

public class JointMover
{
    private readonly Camera _camera;
    private Plane _plane;
    private readonly IEnumerable<Collider> _bounds;

    public JointMover(Camera camera, IEnumerable<Collider> bounds, Vector3 moveAxis)
    {
        _camera = camera;
        _bounds = bounds;

        _plane = new Plane(-camera.transform.forward, moveAxis);
    }

    public void Move(JointConnector joint)
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        _plane.Raycast(ray, out var distance);

        var movePoint = ray.GetPoint(distance);

        var position = joint.transform.position;
        var direction = movePoint - position;

        var d = new Ray(position - direction.normalized * 0.25f, direction.normalized);
        
        Debug.DrawRay(d.origin, d.direction, Color.red);

        int boundedCount = 0;

        var moveDistance = Mathf.Clamp(direction.magnitude, 0.5f, direction.magnitude);

        Collider collided = null;

        foreach (var bound in _bounds)
        {
            if(d.direction == Vector3.zero)
                break;
            
            if(bound == null)
                continue;
            

            if (bound.Raycast(d, out var hitInfo, moveDistance))
            {
                var bounds = bound.bounds;
                
                if (FloatEqualAny(hitInfo.point.x, bounds.max.x, bounds.min.x, 0.05f,out var value))
                    movePoint.x = value;

                if (FloatEqualAny(hitInfo.point.y, bounds.max.y, bounds.min.y,0.05f,out value))
                    movePoint.y = value;
                
                //var f = Equality(hitInfo.point, bounds.max, bounds.min, 0.25f, out var mask);
                //movePoint = Vector3.Scale(movePoint, mask) + f;
                
                d = new Ray(position - direction.normalized * 0.25f, movePoint - joint.transform.position);
                Debug.DrawRay(d.origin, d.direction, Color.yellow);
                
                foreach (var collider in _bounds)
                {
                    if(d.direction == Vector3.zero)
                        break;
                    
                    if(collider == bound)
                        continue;
                    
                    if(collider == null)
                        continue;
                    
                    if (collider.Raycast(d, out hitInfo, 0.5f))
                    {
                        bounds = collider.bounds;
                        if (FloatEqualAny(hitInfo.point.x, bounds.max.x, bounds.min.x, 0.05f,out value))
                            movePoint.x = value;

                        if (FloatEqualAny(hitInfo.point.y, bounds.max.y, bounds.min.y,0.05f,out value))
                            movePoint.y = value;
                    }
                }

                collided = bound;
                boundedCount++;
            }
        }
        
        if (boundedCount > 0)
        {
            if (!joint.Connected)
            {
                if(collided != null && collided.TryGetComponent<IDynamicJointConnection>(out var dynamicJointConnection))
                    joint.Connect(dynamicJointConnection);
                else
                    joint.Connect(true);   
            }
        }
        else
        {
            if(joint.Connected)
                joint.Disconnect();
        }
    
        if(joint.MoveExecutor != null) 
            movePoint = joint.MoveExecutor(joint, movePoint);

        joint.transform.position = movePoint;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool FloatEqual(float first, float second, float tolerance) => Math.Abs(first - second) < tolerance;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool FloatEqualAny(float value, float first, float second, float tolerance, out float equalValue)
    {
        if (FloatEqual(value, first, tolerance))
        {
            equalValue = first;
            return true;
        }

        if (FloatEqual(value, second, tolerance))
        {
            equalValue = second;
            return true;
        }

        equalValue = 0;
        return false;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Vector3 Equality(Vector3 vector, Vector3 first, Vector3 second, float tolerance, out Vector3 resultMask)
    {
        var result = Vector3.zero;
        resultMask = Vector3.one;

        for (int i = 0; i < 3; i++)
        {
            if (FloatEqualAny(vector[i], first[i], second[i], tolerance, out var value))
            {
                result[i] = value;
                resultMask[i] = 0;
            }
        }

        return result;
    }
}

public class JointByPlaneMover
{
    private readonly Camera _camera;
    private Plane _plane;
    private readonly IEnumerable<Collider> _bounds;

    public JointByPlaneMover(Camera camera, IEnumerable<Collider> bounds, Vector3 moveAxis)
    {
        _camera = camera;
        _bounds = bounds;

        _plane = new Plane(-camera.transform.forward, moveAxis);
    }

    /*public void Move(JointConnector joint)
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        _plane.Raycast(ray, out var distance);

        var movePoint = ray.GetPoint(distance);
        

        if (joint.Connected && !joint.ConnectedCollider.Contains(movePoint))
        {
            var position = joint.transform.position;
            var direction = movePoint - position;
            var jointRay = new Ray(position + direction.normalized * 0.25f, direction);

            if (joint.ConnectedCollider.IntersectRay(jointRay))
                movePoint = Vector3.Scale(Xor(joint.ConnectedAxis), movePoint) + (joint.ConnectedAxis * joint.ConnectedValue);
            else
                joint.Disconnect();
        }
        else
        {
            foreach (var bound in _bounds)
            {
                if (!bound.Contains(movePoint)) 
                    continue;
                
                if (!joint.Connected)
                {
                    if(FloatEqualAny(movePoint.x, bound.max.x, bound.min.x, out var value))
                        joint.Connect(Vector3.right, value, bound);
                    
                    if(FloatEqualAny(movePoint.y, bound.max.y, bound.min.y, out value))
                        joint.Connect(Vector3.up, value, bound);   
                }

                if(joint.Connected)
                    movePoint = Vector3.Scale(Xor(joint.ConnectedAxis), movePoint) + (joint.ConnectedAxis * joint.ConnectedValue);
            }   
        }
        
        if (joint.Connected)
        {
            var c = _bounds.Count(x => x.Contains(ray.GetPoint(distance)));

            if (c >= 2)
                movePoint = joint.transform.position;

            if (c == 1)
            {
                var b = _bounds.First(x => x.Contains(ray.GetPoint(distance)));
                
                if(b.Equals(joint.ConnectedCollider))
                    return;

                Vector2 d = Xor(joint.ConnectedAxis);
                joint.Connect(d, b.max.y, b);
            }
        }

        //movePoint.z = -7f;
        joint.transform.position = movePoint;
    }*/

    public void Move(JointConnector joint)
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        _plane.Raycast(ray, out var distance);

        var movePoint = ray.GetPoint(distance);

        // if (joint.Connected && !joint.ConnectedBound.Contains(movePoint))
        // {
        //     var position = joint.transform.position;
        //     var direction = movePoint - position;
        //     var jointRay = new Ray(position + direction.normalized * 0.25f, direction);
        //     Debug.Log("F");
        //     if (joint.ConnectedBound.IntersectRay(jointRay))
        //         movePoint = movePoint;
        //     else
        //         joint.Disconnect();
        // }

        if (joint.Connected)
        {
            var position = joint.transform.position;
            var direction2 = movePoint - position;
            var jointRay = new Ray(position + direction2.normalized * 0.25f, direction2);

            joint.Disconnect();
        }
        
        //if(joint.Connected)

        foreach (var collider in _bounds)
        {
            if(collider.isTrigger)
                continue;
            
            var d = Physics.ComputePenetration(collider, collider.transform.position, collider.transform.rotation,
                joint.Collider, movePoint, joint.transform.rotation,
                out var direction, out var distanceMove);

            if (d)
            {
                var newPosition = movePoint - direction * (distanceMove - 0.25f);
                var oldPosition = movePoint - joint.transform.position;
                var distanceBetweenOldAndNew = newPosition - oldPosition;


                //Debug.Log("New " + newPosition);
                //Debug.Log("Old " + oldPosition);
                //Debug.Log("Distance " + distanceBetweenOldAndNew);
                //Debug.Log(" " + direction);

                //Debug.DrawRay(joint.transform.position, newPosition, Color.green);
                //Debug.DrawRay(joint.transform.position, oldPosition, Color.red);
                //Debug.DrawRay(joint.transform.position, distanceBetweenOldAndNew, Color.yellow);

                var newdir = movePoint - newPosition;
                var olddir = movePoint - joint.transform.position;
                var dir = newdir - olddir;

                //Debug.Log("New " + newdir.magnitude);
                //Debug.Log("Old " + olddir.magnitude);
                //Debug.Log("Distance " + dir.magnitude);
                //Debug.Log(" " + direction);

                //Debug.Log(Vector3.Angle(olddir, newdir));

                Debug.DrawRay(joint.transform.position, movePoint - newPosition, Color.red);
                Debug.DrawRay(joint.transform.position, movePoint - joint.transform.position, Color.yellow);
                Debug.DrawRay(joint.transform.position, dir, Color.green);


                if (newPosition.magnitude >= distanceBetweenOldAndNew.magnitude &&
                    newPosition.magnitude >= oldPosition.magnitude)
                {
                    //Debug.Log("Normal");
                }
                else
                {
                    //Debug.Log("Error");
                }

                //joint.Connect();


                var position = joint.transform.position;
                var direction2 = movePoint - position;
                var jointRay = new Ray(position - direction * 0.5f, direction2);


                if (dir.magnitude < newdir.magnitude && dir.magnitude < olddir.magnitude)
                    movePoint = newPosition;
                else
                {
                    var scaled = Clamp011(newdir * 2);
                    var positiond = Vector3.Scale(joint.transform.position, Xor(scaled));
                    var positiondt = Vector3.Scale(movePoint, scaled);

                    movePoint = positiond + positiondt;
                }

                movePoint = newPosition;


                //Debug.DrawRay(jointRay.origin, jointRay.direction, Color.red);
            }



            /*if (d)
            {
                movePoint -= direction * (distanceMove - 0.25f);
                movePoint.z = -3.5f;
                joint.Connect(direction, distanceMove, collider);

                var position = joint.transform.position;
                var direction1 = movePoint - position;
                var jointRay = new Ray(position + direction1.normalized * 0.25f, direction1);
                Debug.DrawRay(jointRay.origin, jointRay.direction, Color.red);
                //Debug.Log(direction1);
                //Debug.Log(Clamp011(direction1));
                
                Debug.Log(Abs(direction));
                
                Vector3 scaled = Clamp011(direction1 * 3);
                Debug.Log(scaled);

                var dtt = Vector3.Scale(position, scaled);
                var dff = Vector3.Scale(movePoint, Xor(scaled));

                movePoint = dff + dtt;
            }
        }

        /*if (joint.Connected)
        {
            var position = joint.transform.position;
            var direction2 = movePoint - position;
            var jointRay = new Ray(position + direction2.normalized * 0.25f, direction2);

            if (joint.ConnectedCollider is not null && joint.ConnectedCollider.bounds.IntersectRay(jointRay))
            {
                Debug.Log("D");
            }
            
            joint.Disconnect();
        }*/
        }

        joint.transform.position = movePoint;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool FloatEqual(float first, float second) => Math.Abs(first - second) < 0.25f;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool FloatEqualAny(float first, float second, float third, out float equalValue)
    {
        if (FloatEqual(first, second))
        {
            equalValue = second;
            return true;
        }

        if (FloatEqual(first, third))
        {
            equalValue = third;
            return true;
        }

        equalValue = 0;
        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Vector3 Xor(Vector3 value)
    {
        var result = new Vector3();
        for (int i = 0; i < 3; i++)
        {
            result[i] = -1 * Clamp01(value[i]) + 1;
        }

        return result;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Vector3 Abs(Vector3 value)
    {
        var result = new Vector3();
        for (int i = 0; i < 3; i++)
        {
            result[i] = Mathf.Abs(value[i]);
        }

        return result;
    }

    private Vector3 Clamp01(Vector3 value)
    {
        var result = new Vector3();
        int most = 0;
        float val = Mathf.Abs(value[0]);
        for (int i = 0; i < 3; i++)
        {
            if (Mathf.Abs(value[i]) > val)
            {
                most = i;
                val = Mathf.Abs(value[i]);
            }
            //result[i] = Clamp01(Mathf.Abs(value[i]));
        }

        result[most] = 1;

        return result;
    }
    
    private Vector3 Clamp011(Vector3 value)
    {
        var result = new Vector3();
        for (int i = 0; i < 3; i++)
        {
            result[i] = Clamp01(Mathf.Abs(value[i]));
        }


        return result;
    }

    private float Clamp01(float value)
    {
        return value < 1 ? 0 : 1;
    }
}