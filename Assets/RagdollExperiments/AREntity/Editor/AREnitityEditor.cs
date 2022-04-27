using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomEditor(typeof(AREntity))]
public class AREnitityEditor : Editor {
    public VisualTreeAsset m_InspectorXML;
    public override VisualElement CreateInspectorGUI() {
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

        //Enable all joint motion toggles button
        button = myInspector.Q<Button>("enableAllMotionButton");
        button.clickable.clicked += EnableAllJointMotion;

        //disable all joint motion toggles button
        button = myInspector.Q<Button>("disableAllMotionButton");
        button.clickable.clicked += DisableAllJointMotion;

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
       // Undo.RecordObject(target, "Cloned Physics via AREntity"); //TODO - BUG! This Undo funtion fails,
                                                       //   because each physics obj it makes registers an undo thang. We need to group changes after function runs...

        AREntity entity = (AREntity)target;

        if (entity.physicsCloneRoot == null) {
            Debug.LogWarning(entity.gameObject.name + " failed to clone physics because it has no source to clone from");
            return;
        }

        if (entity.copyRigidbods) CopyRigidBodies();
        if (entity.copyConfigJoints) CopyConfigurableJoints();
        if (entity.copyColliders) CopyColliders();// CopyColliders();

    }

    void CloneAREntity() {
        Undo.RecordObject(target, "Cloned AREntity Settings");
        AREntity entity = (AREntity)target;
        if (entity.entityToClone == null) return;//data validation - make sure we've something copy

        AREntity cloning = entity.entityToClone;//what we're cloning

        //copy editor data? Eh, just the main ones...
        entity.motionSourceRoot = cloning.motionSourceRoot;

        //Try to recreate identical joint list/setup
        entity.joints = new List<ARJointData>();
        for (int i = 0; i < cloning.joints.Count; i++) {
            ARJointData aRToAdd = new ARJointData();
            ARJointData toClone = cloning.joints[i];

            //First, try to find the relevant configurable joint!
            if (toClone.joint != null) {
                Transform equivChild = entity.transform.FindDeepChild(toClone.joint.name);
                ConfigurableJoint equivJoint = equivChild.GetComponent<ConfigurableJoint>();
                if (equivJoint != null) aRToAdd.joint = equivJoint;
                else Debug.LogWarning("AREntity " + entity.name + " failed to clone joint ind: " + i + ", name: " +
                    toClone.joint.name + " as it had no identically named child in its heirarchy ");
            }
            aRToAdd.motionSource = toClone.motionSource;
            aRToAdd.enableMotionCopying = toClone.enableMotionCopying;
            entity.joints.Add(aRToAdd);
        }
    }

    /// <summary>
    /// Gets all of the ConfigurableJoints in our GameObject's hierarchy.
    /// Then, checks each joint against the entity's existing joints list.
    /// If a joint isn't in the list, it is added via its own new ARJointData.
    /// </summary>
    void GatherJointsIntoARJointDataObjects() {
        Undo.RecordObject(target, "AREntity Gathered all ConfigJoints");
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
        Undo.RecordObject(target, "AREntity Smart Assign Motion Sources");
        AREntity entity = (AREntity)target;
        if (entity.motionSourceRoot == null || entity.joints == null || entity.joints.Count == 0) return;

        ARJointData cur;
        for (int i = 0; i < entity.joints.Count; i++) {
            //get joint and make sure it doesn't have derailing null values
            cur = entity.joints[i];
            if (cur == null || cur.joint == null) continue;
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
    bool ListContainsConfigurableJoint(ConfigurableJoint j, List<ARJointData> _arDatas) {
        if (_arDatas == null) return false;
        for (int i = 0; i < _arDatas.Count; i++) {
            if (_arDatas[i].joint == j) return true;
        }

        return false;
    }

    #region TOGGLE JOINT MOTION COPYING
    void EnableAllJointMotion() {
        Undo.RecordObject(target, "Enabled All Joint Motion");
        SetAllJointMotion(true); 
    }
    void DisableAllJointMotion() {
        Undo.RecordObject(target, "Disabled All Joint Motion");
        SetAllJointMotion(false);
    }
    /// <summary>
    /// Go through all the joints in the joint list and set their "motion copying enabled" bool to given value
    /// </summary>
    /// <param name="setTo"></param>
    void SetAllJointMotion(bool setTo) {
        AREntity entity = (AREntity)target;
        if (entity == null || entity.joints == null) return;
        
        for (int i = 0;i < entity.joints.Count; i++)
            entity.joints[i].enableMotionCopying = setTo;
    }
    #endregion

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

    /// <summary>
    /// Tries to clone the configurable joint setup of the to-clone object + it's heirarchy. Though, settings, such
    /// as connected body, are only copied if the "Copy Config Joint Settings" option is enabled
    /// </summary>
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
                if(entity.copyConfigJointSettings) CopyConfigurableJointSettings(copyFrom, curJoint);
            }
        }
    }

    /// <summary>
    /// This function tries to mimic the connected body settings of the clone-source before 
    /// going off to copy general settings via the extension method I implemented elsewhere
    /// </summary>
    /// <param name="toCopyFrom"></param>
    /// <param name="toCopyTo"></param>
    void CopyConfigurableJointSettings(ConfigurableJoint toCopyFrom, ConfigurableJoint toCopyTo) {
        //first, gotta get the right connectedBody - hardest part
        if (toCopyFrom.connectedBody != null) {
            Transform getBodFrom = ((AREntity)target).transform.FindDeepChild(toCopyFrom.connectedBody.name);
            if (getBodFrom == null)
                Debug.LogWarning("Couldn't setup configurable joint " + toCopyTo.name + " connectedBody because could't find rigidbody on " + toCopyFrom.connectedBody.name);
            else {
                Rigidbody bod = getBodFrom.GetComponent<Rigidbody>();
                toCopyTo.connectedBody = bod;
            }
        }
        //now copy the rest of settings
        toCopyTo.CopyMotionDataFrom(toCopyFrom);
    }
   
    /// <summary>
    /// 
    /// </summary>
    void CopyColliders() {
        AREntity entity = (AREntity)target;
        /**
         collect all colliders.  
         use to get names of objs in "CopyFrom" object that have colliders.
         get one copy from obj.  Get it's colliders/types.
        get equivalent obj in Copy to.  Get it's colliders/types.
         if copyFrom obj has more colliders of that type, add some to the copyTo
         then copy values from each copyFrom to the copyTo
         put the copyFrom we just finished up into a list

        pull the next collider from our list of all colliders. if it's gameobject is already in the finished list, skip and go to next
        repeat until done.
         **/

        //Get all the colliders, so that we know which of the objs in the From heirarchy we need to worry about.
        Collider[] allFromColliders = entity.physicsCloneRoot.GetComponentsInChildren<Collider>();
        //Create list so we can remember which children in the heirarchy we've already copied colliders over for
        List<Transform> alreadyCopied = new List<Transform>();

        Transform curObj = null;
        Transform copyTo = null;
        //for each gameObject (NOT COLLIDER) represented in the allFromColliders list, we need to duplicate it's colliders
        for (int i = 0; i < allFromColliders.Length; i++) {
            curObj = allFromColliders[i].transform;
            //if we've already handled the curObj in a previous iteration of the loop, we can skip it!
            if (alreadyCopied.Contains(curObj)) continue;
            //Find the equivalent child in the "to" heirarchy which we'll be copying to - if fail to find, warn and continue to next object
            copyTo = entity.transform.FindDeepChild(curObj.name);
            if (copyTo == null) {
                Debug.LogWarning("Heirarchy mismatch error - Copy Colliders failed - child '" + curObj.name + "'  was not found in '" + this.name + "' which is being copied to.");
                continue;
            }
            //alrighty, we need to get and copy all colliders of each type from "curObj" to "copyTo"
            CopyIndividColliders<CapsuleCollider>(curObj, copyTo);
            CopyIndividColliders<SphereCollider>(curObj, copyTo);
            CopyIndividColliders<BoxCollider>(curObj, copyTo);
            CopyIndividColliders<MeshCollider>(curObj, copyTo);

            alreadyCopied.Add(curObj);
        }
        //Done-zo
    }

    void CopyIndividColliders<T>(Transform copyFrom, Transform copyTo) where T : Collider {
        //Gather <T> colliders on "copyFrom"
        T[] colsToCopy = copyFrom.GetComponents<T>();
        //if none, return
        if (colsToCopy == null || colsToCopy.Length == 0) return;
        //Gather <T> colliders on "copyTo"
        List<T> colsOnCopyTo = new List<T>(copyTo.GetComponents<T>());
        //figure out the difference in numbers of <T> colliders between the 2
        int difInNumOfCols = colsToCopy.Length - colsOnCopyTo.Count;
        //Add colliders to "copyTo" until it has the same number as "copyFrom"
        for (int i = 0; (i < difInNumOfCols); i++) {
            colsOnCopyTo.Add(copyTo.gameObject.AddComponent<T>());
        }

        T curFrom = null;
        T curTo = null;
        //Now copyTo has at least as many <T> colliders as copyFrom - lets copy the data over
        for (int i = 0; i < colsToCopy.Length; i++) {
            curFrom = colsToCopy[i];
            curTo = colsOnCopyTo[i];

            //use correct data copy method... ye, it's a little dumb to have a generic function and then cast back to a specific class, but I can't be assed rn
            if (typeof(T) == typeof(SphereCollider)) (curTo as SphereCollider).CopyDataFrom(curFrom as SphereCollider);
            else if (typeof(T) == typeof(CapsuleCollider)) (curTo as CapsuleCollider).CopyDataFrom(curFrom as CapsuleCollider);
            else if (typeof(T) == typeof(BoxCollider)) (curTo as BoxCollider).CopyDataFrom(curFrom as BoxCollider);
            else if (typeof(T) == typeof(MeshCollider)) (curTo as MeshCollider).CopyDataFrom(curFrom as MeshCollider);
        }
    }

    #endregion

    #region UI AND EDITOR FUNCTIONS
    void HideShowAssignMotionButton(Button button, ObjectField objF) {
        button.SetEnabled(objF.value != null);
    }

    #endregion

}
