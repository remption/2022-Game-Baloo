using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Active ragdoll manager! It gathers bones and manages them :)
/// </summary>
public class AREntity : MonoBehaviour
{
    public List<ARJointData> joints;

    [Tooltip("If motion is being copied from a rig with identical or mostly identical naming," +
        " assign the source root here and hit auto populate source")]
    public Transform motionSourceRoot;

#if UNITY_EDITOR
    //all cloning using the below 2 objects is name based, so only children in the heirarchies with names matching children in THIS obj's heirarchy
    //will be setup correctly in the scripts that use these variables. Also, name uniquenss is important, as duplicates aren't handled.
    public AREntity entityToClone; //used by editor to copy/duplicate the setup of another AREntity on an identically-ish rigged gameObj
    public Transform physicsCloneRoot; //used by editor to copy/duplicate the physics components/setup of an identically-ish rigged gameObj

    //Physics cloning settings - editor only
    public bool copyRigidbods = true;
    public bool copyRigidBodSettings = true;
    public bool copyConfigJoints = true;
    public bool copyConfigJointSettings = true;
    public bool copyColliders = true;

    //AREntity cloning settings - editor only
    public bool copyJointList = true;
    public bool copyMotionTargets = true;
    public bool copyMotionSettings = true;

#endif

    // Start is called before the first frame update
    void Start()
    {
        InitARJointDatas();
    }
    void InitARJointDatas() {
        for (int i = 0; i < joints.Count; i++)
            joints[i].GatherInitialTransform();
    }


    void ApplyMotionTargetingUpdate(){
        for (int i = 0; i < joints.Count; i++){
            joints[i].ApplyMotionTargeting();
        }
    }

}

[System.Serializable]
public class ARJointData
{
    public ConfigurableJoint joint;
    public Transform motionSource;
    public bool enableMotionCopying;
    private Quaternion initRotation;
    private Vector3 initPosition;

    ///The most basic sort of motion copying... i may in future need to heavily change this.
    ///I did this all in local rotation by default... I may want to change that in the future? 
    ///give options?
    public void ApplyMotionTargeting() {
        if(!enableMotionCopying || joint == null || motionSource == null) return;
       // if (local) {
            /*if (copyRotation)*/ ConfigurableJointExtensions.SetTargetRotationLocal(joint, motionSource.localRotation, initRotation);
            /*if (copyPosition)*/ joint.targetPosition = (motionSource.localPosition - initPosition) * .1f;
       // } 
        //else {
          //  if (copyRotation) ConfigurableJointExtensions.SetTargetRotation(myJoint, toCopy.rotation, toCopyStartRot);
            //if (copyPosition) myJoint.targetPosition = toCopyStartPos - toCopy.position;
        //}

    }

    public void GatherInitialTransform() {
        if(joint == null) return;
        initPosition = joint.transform.localPosition;
        initRotation = joint.transform.localRotation;
    }
}