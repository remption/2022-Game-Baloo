using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ConfigurableJoint))]
public class JointCopyMotion : MonoBehaviour
{
    public Transform toCopy;
    protected ConfigurableJoint myJoint;

    private Vector3 targetAnchorStartPos;
    private Quaternion targetAnchorStartRot;

    // Start is called before the first frame update
    void Start()
    {
        myJoint = GetComponent<ConfigurableJoint>();
        if (toCopy)
        {
            targetAnchorStartPos = toCopy.position;//todo - better to do this in local space and stuff?
            targetAnchorStartRot = toCopy.rotation;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (toCopy && myJoint)
        {
            CalculateOffsetAndStuff();
        }
    }

    void CalculateOffsetAndStuff()
    {
        Vector3 targetPos = targetAnchorStartPos - toCopy.position; //current offset in position from where it started at the v beginning
        Quaternion targetRot = Quaternion.Inverse( toCopy.rotation) * targetAnchorStartRot; //same,but with rotation

        myJoint.targetRotation = targetRot;
        myJoint.targetPosition = targetPos;
    }
}
