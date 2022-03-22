using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class ActRagChainBuilder : MonoBehaviour
{
    public List<Transform> bones;
   public bool performChain = false;
    public bool addCapsuleColliders;
    public bool sizeCapsuleColliders;
    [Range(0.01f,.5f)]
    public float radiusToHeightRatio = .25f;
    public bool setMotionDefaults = true;
    public ConfigurableJointMotion motionDefaults = ConfigurableJointMotion.Locked;
    public bool initAngularDrives = true;
    public float angularDriveSpring = 500;
    public float maxDriveForce = float.MaxValue;
    public bool clearEverything = false;
    public bool rigidBodCleanup = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (performChain) {
            BuildTheChainBud();
            performChain = false;
        }

        if (clearEverything) {
            ClearEverythinger();
            clearEverything = false;
        }
        if (rigidBodCleanup) {
            RigidbodCleaner();
            rigidBodCleanup = false;
        }
    }

    void ClearEverythinger() {
        if (bones == null) return;
        for (int i = 0; i < bones.Count; i++) {
            Transform curB = bones[i];
            CapsuleCollider col = curB.GetComponent<CapsuleCollider>();
            if (col != null) DestroyImmediate(col);
            ConfigurableJoint joint = curB.GetComponent<ConfigurableJoint>();
            if (joint != null) DestroyImmediate(joint);
            Rigidbody bod = curB.GetComponent<Rigidbody>();
            if (bod != null) DestroyImmediate(bod);
        }
    }

    void RigidbodCleaner() {
        if (bones == null) return;
        for (int i = 0; i < bones.Count; i++) {

            Rigidbody rb = bones[i].GetComponent<Rigidbody>();
            if (rb != null) {
                rb.centerOfMass = new Vector3(0, 0, 0);
                rb.inertiaTensor = new Vector3(1, 1, 1);
            }


        }



    }


    void BuildTheChainBud() {
        if (bones == null || bones.Count <=1) return;
        Rigidbody prevBod = bones[0].GetComponent<Rigidbody>();
        if (prevBod == null) bones[0].gameObject.AddComponent<Rigidbody>();
        ConfigurableJoint curJ = null;
        for (int i = 1; i < bones.Count; i++) {
            curJ = bones[i].GetComponent<ConfigurableJoint>();
            if (curJ == null) curJ = bones[i].gameObject.AddComponent<ConfigurableJoint>();
            curJ.connectedBody = prevBod;
            prevBod = bones[i].GetComponent<Rigidbody>();
            SetupConfigJoint(curJ);
        }

        if (addCapsuleColliders) {
            CapsuleCollider capCol = null;
            for (int i = 0; i < bones.Count-1; i++) {
                 capCol = bones[i].GetComponent<CapsuleCollider>();
                if (capCol == null) capCol = bones[i].gameObject.AddComponent<CapsuleCollider>();
                if (sizeCapsuleColliders) SizeCapsuleCollider(capCol, bones[i + 1]);
                capCol = null;
            }
            //do the last one, which can't get sized.
            capCol = bones[bones.Count - 1].GetComponent<CapsuleCollider>();
            if (capCol == null) capCol = bones[bones.Count-1].gameObject.AddComponent<CapsuleCollider>();
        }

    }

    void SetupConfigJoint(ConfigurableJoint jBoi) {
        if (setMotionDefaults) {
            jBoi.xMotion = motionDefaults;
            jBoi.yMotion = motionDefaults;
            jBoi.zMotion = motionDefaults;
        }
        if (initAngularDrives) {
            JointDrive driveBoi = new JointDrive();
            driveBoi.positionSpring = angularDriveSpring;
            driveBoi.maximumForce = maxDriveForce;
            jBoi.angularXDrive = driveBoi;
            jBoi.angularYZDrive = driveBoi;
            jBoi.slerpDrive = driveBoi;
        }

    }


    void SizeCapsuleCollider(CapsuleCollider toSize, Transform nextBone) {
        Transform cur = toSize.transform;
        float dist = Vector3.Distance(cur.position, nextBone.position);
        toSize.height = dist*.95f;//multiply by .95 to make it a TINY bit smaller! that way, less likely for joints to touch one another?
        toSize.radius = Mathf.Min(toSize.radius, dist*radiusToHeightRatio);
        toSize.center = (Vector3.up * dist*.5f);
    }

}
