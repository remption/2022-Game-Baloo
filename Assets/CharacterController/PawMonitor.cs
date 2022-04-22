using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Legs ha
public class PawMonitor : MonoBehaviour
{
    public bool canGrabObjects;//true for front legs only
    public LayerMask groundSensorLayerMask;
    public float rayCastDistance = .05f;

    private RaycastHit _rayHit;
    private Ray _ray;

    private bool shouldBeGrounded = false; //according to animation data, this paw should be grounded/try to be.
    private bool isGroundedATM = false; //is the paw currently locked onto anything?

    private Rigidbody _rigidBod;
    // Start is called before the first frame update
    void Awake()
    {
        _rayHit = new RaycastHit(); 
        _rigidBod = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   public void Ground()
    {
        Vector3 desiredGroundPoint = this.transform.position;
        _rigidBod.isKinematic = true;
        if (RaycastGround(out desiredGroundPoint))
        {
          // this.transform.position = desiredGroundPoint;
          // _rigidBod.isKinematic = true;
            Debug.Log("grounded,bitch");
        }
    }
    public void Release()
    {
        _rigidBod.isKinematic=false;


    }


    /// <summary>
    /// Checks if there's anything the paw can "stick" to
    /// </summary>
    /// <param name="desiredGroundPoint"></param>
    /// <returns></returns>
    bool RaycastGround(out Vector3 desiredGroundPoint)
    {
        _ray = new Ray(this.transform.position, Vector3.down);
        bool toRet = Physics.Raycast(_ray, out _rayHit, rayCastDistance, groundSensorLayerMask);
        if (toRet) desiredGroundPoint = _rayHit.point;
        else desiredGroundPoint = Vector3.zero;
        desiredGroundPoint += transform.forward * .1f;
        return toRet;

    }
   
}
