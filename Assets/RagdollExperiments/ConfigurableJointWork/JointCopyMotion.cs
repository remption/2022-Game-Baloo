using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ConfigurableJoint))]
public class JointCopyMotion : MonoBehaviour
{
    public bool copyRotation = true;
    public bool copyPosition = true;
    public Transform toCopy;
    public bool local = true; //todo - could have separate position and rotation "local" bools, so you can pick for each!
    protected ConfigurableJoint myJoint;

    private Vector3 toCopyStartPos;
    private Quaternion toCopyStartRot;
    // Start is called before the first frame update
    void Start()
    {
        myJoint = GetComponent<ConfigurableJoint>();
        if (toCopy)
        {
            
            if (local) {
                toCopyStartRot = toCopy.localRotation;
                myJoint.configuredInWorldSpace = false;
                toCopyStartPos = toCopy.localPosition;
            } else {
                toCopyStartRot = toCopy.rotation;
                myJoint.configuredInWorldSpace = true;
                toCopyStartPos = toCopy.position;
            }
        }
        //rigid body auto calculates center of mass and inertia tensor based on any non-trigger colliders attached.
        //these 2 variables can cause some big jittering issues when you try to match a target rotation (i didn't test, but maybe for position too?)
        //so, we need to reset them manually.  It is pretty possible I don't need to do this at run time, just in editor when I mess with colliders/set it up.
        //but... this is fine.
        Rigidbody rb = this.GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, 0, 0); 
        rb.inertiaTensor = new Vector3(1, 1, 1);

    }

    // Update is called once per frame
    void Update()
    {
        if (toCopy && myJoint)
        {
            if (local) {
               if(copyRotation) ConfigurableJointExtensions.SetTargetRotationLocal(myJoint, toCopy.localRotation, toCopyStartRot);
                if (copyPosition) myJoint.targetPosition = (toCopy.localPosition - toCopyStartPos)*.1f;
            }
            else {
                if(copyRotation)ConfigurableJointExtensions.SetTargetRotation(myJoint, toCopy.rotation, toCopyStartRot);
                if (copyPosition) myJoint.targetPosition = toCopyStartPos - toCopy.position;
            }
        }
    }





    /*
    void CalculateOffsetAndStuff()
    {
        Vector3 targetPos = toCopyStartPos - toCopy.position; //current offset in position from where it started at the v beginning
        Quaternion targetRot = Quaternion.Inverse( toCopy.rotation) * toCopyStartRot; //same,but with rotation

        myJoint.targetRotation = targetRot.normalized;
        myJoint.targetPosition = targetPos;

       // Quaternion nooo = new Quaternion();
        //nooo.eulerAngles = Vector3.zero;
    }

    void CalculateOffsetAndStuffv2() {
    }

    Vector3 clampRotTo180s(Vector3 toClamp) {
        while (toClamp.x < -180) toClamp.x += 180;
        while (toClamp.x > 180) toClamp.x -= 180;

        while (toClamp.y < -180) toClamp.y += 180;
        while (toClamp.y > 180) toClamp.y -= 180;

        while (toClamp.z < -180) toClamp.z += 180;
        while (toClamp.z > 180) toClamp.z -= 180;

        return toClamp;
    } */
}

public static class JointCopyMotionExtensions
{
    public static void CopySettings(this JointCopyMotion copyTo, JointCopyMotion copyFrom)
    {
        copyTo.toCopy = copyFrom.toCopy;
        copyTo.copyPosition = copyFrom.copyPosition;
        copyTo.copyRotation = copyFrom.copyRotation;
        copyTo.local = copyFrom.local;
    }
}