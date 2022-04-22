using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]           
public class CopyVelocity: MonoBehaviour
{

    public Rigidbody toCopy;
    public bool copyAngularToo = true;
    private Rigidbody _myBod;
    // Start is called before the first frame update
    void Start()
    {
        _myBod = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(toCopy != null)
        {
            _myBod.velocity = toCopy.velocity;
            if(copyAngularToo)_myBod.angularVelocity = toCopy.angularVelocity;  
        }
       
    }
}
