using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollower : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private JointConnector[] _joints;
    [SerializeField] private Transform _max;

    private Vector3 _offset;
    private Camera _camera;
    public void SetMax(Transform obj)
    {
        _max = obj;
    }
    public void SetJoints(JointConnector J_1, JointConnector J_2, JointConnector J_3, JointConnector J_4,Transform head)
    {
        _joints[0] = J_1;
        _joints[1] = J_2;
        _joints[2] = J_3;
        _joints[3] = J_4;
        _target = head;
    }
    private void Awake()
    {
        _offset = transform.position - _target.position;
        _camera = GetComponent<Camera>();
    }

    private void Update()
    {
        var screen = _camera.ScreenToWorldPoint(new Vector3(0, 0, 10));
        var minJointPosition = _joints.Select(x => x.transform.position).Min(Vector3.up);
        
        if (screen.y >= minJointPosition.y - 0.5f)
        {
            return;
        }
        
        var result = _target.position + _offset;
        result.y = Mathf.Clamp(result.y, result.y, _max.position.y);
        result.x = transform.position.x;
        result.z = transform.position.z;
        transform.position = result;
        
        
    }
    
    
}

public static class VectorExtensions
{
    public static bool Any(this IEnumerable<Vector3> vectors, Func<Vector3, bool> checker)
    {
        var result = false;
        
        foreach (var vector in vectors)
        {
            result = result || checker(vector);
        }

        return result;
    }
    public static Vector3 Min(this IEnumerable<Vector3> vectors, Vector3 checkingAxis)
    {
        var result = Vector3.zero;
        
        foreach (var vector in vectors)
        {
            if (result == Vector3.zero)
            {
                result = vector;
                continue;
            }
            
            var check = Vector3.Scale(checkingAxis.normalized, vector);

            check.Enumerate((value, index) =>
            {
                result.Enumerate((valueResult, indexResult) =>
                {
                    if (index != indexResult) return;

                    if (value >= valueResult) return;

                    result = vector;
                });
            });
        }

        return result;
    }

    public static void Enumerate(this Vector3 vector, Action<float, int> action)
    {
        for (int i = 0; i < 3; i++)
        {
            action(vector[i], i);
        }
    }
    
    
}