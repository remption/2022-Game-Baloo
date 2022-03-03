using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ConfigurableJoint))]
public class JointCopyMotion : MonoBehaviour
{
    public Transform toCopy;
    public bool local = true;
    protected ConfigurableJoint myJoint;

    private Vector3 toCopyStartPos;
    private Quaternion toCopyStartRot;
    // Start is called before the first frame update
    void Start()
    {
        myJoint = GetComponent<ConfigurableJoint>();
        if (toCopy)
        {
            toCopyStartPos = toCopy.position;//todo - better to do this in local space and stuff?
            if (local) {
                toCopyStartRot = toCopy.localRotation;
                myJoint.configuredInWorldSpace = false;
            } else {
                toCopyStartRot = toCopy.rotation;
                myJoint.configuredInWorldSpace = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (toCopy && myJoint)
        {
            // CalculateOffsetAndStuff();
            //   CalculateOffsetAndStuffv2();
            if (local) ConfigurableJointExtensions.SetTargetRotationLocal(myJoint, toCopy.localRotation, toCopyStartRot);
           else  ConfigurableJointExtensions.SetTargetRotation(myJoint, toCopy.rotation, toCopyStartRot);
        }
    }

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
    }
}
