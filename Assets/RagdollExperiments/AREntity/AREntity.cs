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

    public List<ARJointGroup> jointGroups;

    // Start is called before the first frame update
    void Start()
    {
        InitARJointDatas();
    }
    void InitARJointDatas() {
        for (int i = 0; i < joints.Count; i++)
            joints[i].GatherInitialTransform();
    }


    private void Update() {
        ApplyMotionTargetingUpdate();
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


//TODO - Editor only? only usage I can possible think of is to change vals ConfigJoint values at runtime,
//but this just seems a little bloaty for that usage case.
[System.Serializable]
public class ARJointGroup {
    public enum ColliderType {
        Capsule,
        Sphere,
        Box
    }
    public string groupName;
    public List<ARJointGroupEntry> members;

    /*chain and physics stuff for adding/basic setup of colliders and joints*/
    public bool isChain = false;
    public bool addColliders = false;
    public ColliderType colliderType = ColliderType.Capsule;
    public float capsuleHeightWidthRatio = 2;
    //  public float sphereColliderRadius = 1;
    //public Vector3 boxColliderDimensions = Vector3.one;
    public bool addJoint = false;

    //JOINT AND PHYSICS MATERIAL CONTROLS
    public PhysicMaterial physicsMat;
    public ConfigurableJointMotion motionDefaults = ConfigurableJointMotion.Locked;
    public ConfigurableJointMotion rotationDefaults = ConfigurableJointMotion.Free;
    public RotationDriveMode rotationDriveMode = RotationDriveMode.Slerp;
    //joint drive settings
    public float rotationSpring;
    public float rotationDamping;
    public float maxRotationForce;
    //for use in editor - so we user doesn't have to push updates/overwrites to EVERY single field.
    public bool updatedPhysicsMat = true;
    public bool updateMotionDefaults = true;
    public bool updateRotationDefaults = true;
    public bool updateRotationDriveMode = true;
    public bool updateRotationSpring = true;
    public bool updateRotationDamping = true;
    public bool updateMaxRotationForce = true;
}

[System.Serializable]
public class ARJointGroupEntry {
    public Transform joint;
    public bool updateAllowed;//permission for the editor to update it's settings via the AREntity editor tool.
}