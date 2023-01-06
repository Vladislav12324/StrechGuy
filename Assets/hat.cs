using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hat : MonoBehaviour
{
    public  Transform head,hero;
    void Start()
    {
        
    }

    void Update()
    {
        //transform.rotation =Quaternion.Euler(head.rotation.x, head.rotation.y, head.rotation.z);
        transform.position = new Vector3(head.position.x, head.position.y+0.31f , head.position.z );
    }
}
