using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomEditor(typeof(AREntity))]
public class AREnitityEditor : Editor
{
    public VisualTreeAsset m_InspectorXML;
    public override VisualElement CreateInspectorGUI()
    {
        VisualElement myInspector = new VisualElement();
        m_InspectorXML.CloneTree(myInspector);

        //Gather joints button
        Button button = myInspector.Q<Button>("GatherJointsButton");
        button.clickable.clicked += GatherJointsIntoARJointDataObjects;
        
        //Physics Clone Button
        button = myInspector.Q<Button>("physicsCloneButton");
        button.clickable.clicked += ClonePhysicsFromObj;

        //AREntity Clone Button
        button = myInspector.Q<Button>("AREntityCloneButton");
        button.clickable.clicked += CloneAREntity;

        //Auto Assign motion source button
        button = myInspector.Q<Button>("SmartAssignMotionSources");
        button.clickable.clicked += SmartAssignMotionSources;
        ObjectField obF = myInspector.Q<ObjectField>("MotionSourceField");
        obF.RegisterValueChangedCallback(evt => {
            HideShowAssignMotionButton(button, obF);
        });

        button.SetEnabled(((AREntity)target).motionSourceRoot != null); // only show the "smartAssign" button if there is a source object given to draw from

        return myInspector;
    }
    


    #region Setup and Initialization Tools
    void ClonePhysicsFromObj() {
        AREntity entity = (AREntity)target;

        if (entity.physicsCloneRoot == null) {
            Debug.LogWarning(entity.gameObject.name + " failed to clone physics because it has no source to clone from");
            return;
        }

        if (entity.copyRigidbods) CopyRigidBodies();
        if (entity.copyConfigJoints) CopyConfigurableJoints();
        if (entity.copyColliders) ;// CopyColliders();
        
    }

    void CloneAREntity() { 
    
    }

    /// <summary>
    /// Gets all of the ConfigurableJoints in our GameObject's hierarchy.
    /// Then, checks each joint against the entity's existing joints list.
    /// If a joint isn't in the list, it is added via its own new ARJointData.
    /// </summary>
    void GatherJointsIntoARJointDataObjects() {
        AREntity entity = (AREntity)target;
        Transform t = entity.transform;
        if (entity.joints == null) entity.joints = new List<ARJointData>();

        //Gather joints, and then check each. For each one that is not in the entity.joints list, make a new ARJointData and add it! 
        ConfigurableJoint[] potentialJoints = entity.GetComponentsInChildren<ConfigurableJoint>();
        for (int i = 0; i < potentialJoints.Length; i++) {
            if (!ListContainsConfigurableJoint(potentialJoints[i], entity.joints)) {
                ARJointData ajd = new ARJointData();
                ajd.joint = potentialJoints[i];
                entity.joints.Add(ajd);
            }
        }
    }
    
    /// <summary>
    /// For each joint in the AREntity's "joint" list, search and see if there is an identically named 
    /// tranform in the "motionSourceRoot" heirarchy. If so, assign that identically named transform 
    /// to be the joint's motion source.  
    /// 
    /// This method may assign incorrect objects if there are identicaly named objects in the motion source heirarchy,
    /// as it doesn't take into consideration an object's position in the hierarchy. So, unique naming is important.
    /// </summary>
    void SmartAssignMotionSources() {
        AREntity entity = (AREntity)target;
        if (entity.motionSourceRoot == null || entity.joints == null || entity.joints.Count == 0) return;

        ARJointData cur;
        for (int i = 0; i < entity.joints.Count; i++) {
            //get joint and make sure it doesn't have derailing null values
            cur = entity.joints[i];
            if(cur == null || cur.joint == null) continue;
            // if there is a child in the motion source with a matching name, make it the source!
            cur.motionSource = entity.motionSourceRoot.transform.FindDeepChild(cur.joint.name);
        }
    }
    #endregion

    /// <summary>
    /// True if the ARJointData list contains an entry for the given joint already
    /// </summary>
    /// <param name="j"></param>
    /// <param name="_arDatas"></param>
    /// <returns></returns>
    bool ListContainsConfigurableJoint(ConfigurableJoint j, List<ARJointData> _arDatas)
    {
        if (_arDatas == null) return false;
        for (int i = 0;i < _arDatas.Count; i++)
        {
            if(_arDatas[i].joint == j)return true;
        }

        return false;
    }

    #region CLONING SUBROUTINES
    void CopyRigidBodies() {
        AREntity entity = (AREntity)target;
        if (entity == null || entity.physicsCloneRoot == null) return;

        //get all the rigidbodies from the clone source && verify
        Rigidbody[] toCopyBods = entity.physicsCloneRoot.GetComponentsInChildren<Rigidbody>();
        if (toCopyBods == null) return;

        //for each body in the source, see if the destination has an identically named child. if so, add a bod to it! if not, warning issued
        for (int i = 0; i < toCopyBods.Length; i++) {
            Rigidbody copyFrom = toCopyBods[i];
            Transform transformTocopyTo = entity.transform.FindDeepChild(copyFrom.name);

            if (transformTocopyTo == null)
                Debug.LogWarning("Heirarchy mismatch error - Copy RigidBodies failed - child '" + copyFrom.name + "'  was not found in '" + this.name + "' which is being copied to.");
            else {
                Rigidbody curBod = null;
                curBod = transformTocopyTo.GetComponent<Rigidbody>();

                if (curBod == null) curBod = transformTocopyTo.gameObject.AddComponent<Rigidbody>();
                if (curBod == null) Debug.LogWarning("ARCopyARComponents failed to create rigidBody on " + copyFrom.name);
                else if (entity.copyRigidBodSettings) curBod.CopyDataFrom(copyFrom);
            }
        }
    }

    void CopyConfigurableJoints() {
        AREntity entity = (AREntity)target;
        if (entity.physicsCloneRoot == null) return;

        if (!entity.copyRigidbods) {  // because the config joints are going to add them anyway
            bool copySettingsVal = entity.copyRigidBodSettings; //we don't want to copy settings, so disable, but then renable after bods added if needed.
            entity.copyRigidBodSettings = false;
            CopyRigidBodies();
            entity.copyRigidBodSettings = copySettingsVal;
        }
        ConfigurableJoint[] toCopyJoints = entity.physicsCloneRoot.GetComponentsInChildren<ConfigurableJoint>();
        if (toCopyJoints == null) return;

        for (int i = 0; i < toCopyJoints.Length; i++) {
            ConfigurableJoint copyFrom = toCopyJoints[i];
            Transform transToCopyTo = entity.transform.FindDeepChild(copyFrom.name);

            if (transToCopyTo == null)
                Debug.LogWarning("Heirarchy mismatch error - Copy Configurable Joints failed - child '" + copyFrom.name + "'  was not found in '" + this.name + "' which is being copied to.");
            else {
                ConfigurableJoint curJoint = null;
                curJoint = transToCopyTo.GetComponent<ConfigurableJoint>();

                if (curJoint == null) curJoint = transToCopyTo.gameObject.AddComponent<ConfigurableJoint>();
                if (curJoint == null) Debug.LogWarning("ARCopyARComponents failed to create ConfigurableJoint on " + copyFrom.name);
                if(entity.copyConfigJointSettings) CopyConfigJointSettings(copyFrom, curJoint);
            }
        }
    }

    #endregion

    #region UI AND EDITOR FUNCTIONS
    void HideShowAssignMotionButton(Button button, ObjectField objF) {
        button.SetEnabled(objF.value != null);
    }

    #endregion

}
