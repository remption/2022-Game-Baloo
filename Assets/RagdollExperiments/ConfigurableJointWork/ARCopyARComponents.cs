using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ARCopyARComponents : MonoBehaviour
{
    public Transform toCopyFrom;
    public bool copyRigidBodies;
    public bool copyRBodSettings = true;
    public bool copyConfigJoints;
    public bool copyColliders;
    public bool copyJointMotionScripts;
    public bool CopyButton = false;

    // Update is called once per frame
    void Update()
    { 
        if (CopyButton)
        {
            CopyButton = false;
            if (toCopyFrom == null) { 
                Debug.LogWarning(this.gameObject.name + " failed to copy AR components because it has to object to copy from assigned");
                return;
            }

            if (copyRigidBodies) CopyRigidbods(copyRBodSettings);
            if(copyColliders) CopyColliders();
            if (copyConfigJoints) CopyConfigJoints();
            if (copyJointMotionScripts) CopyJointMotionScripts();
        }
    }

    void CopyRigidbods(bool copyKeySettings = false)
    {
        Rigidbody[] toCopyBods = toCopyFrom.GetComponentsInChildren<Rigidbody>();
        if (toCopyBods == null) return;

        for (int i = 0; i < toCopyBods.Length; i++)
        {
            Rigidbody copyFrom = toCopyBods[i];
            Transform transformTocopyTo = this.transform.FindDeepChild(copyFrom.name);
            
            if (transformTocopyTo == null)
                Debug.LogWarning("Heirarchy mismatch error - Copy RigidBodies failed - child '" + copyFrom.name + "'  was not found in '" + this.name + "' which is being copied to.");
            else
            {
                Rigidbody curBod = null;
                curBod = transformTocopyTo.GetComponent<Rigidbody>();

                if (curBod == null) curBod = transformTocopyTo.gameObject.AddComponent<Rigidbody>();
                if (curBod == null) Debug.LogWarning("ARCopyARComponents failed to create rigidBody on " + copyFrom.name);
                else if (copyKeySettings) curBod.CopyDataFrom(copyFrom);
            }
        }
    }

    void CopyConfigJoints()
    {
        if (!copyRigidBodies) CopyRigidbods(true); // because the config joints are going to add them anyway
        ConfigurableJoint[] toCopyJoints = toCopyFrom.GetComponentsInChildren<ConfigurableJoint>();
        if (toCopyJoints == null) return;

        for (int i = 0; i < toCopyJoints.Length; i++)
        {
            ConfigurableJoint copyFrom = toCopyJoints[i];
            Transform transToCopyTo = this.transform.FindDeepChild(copyFrom.name);

            if (transToCopyTo == null)
                Debug.LogWarning("Heirarchy mismatch error - Copy Configurable Joints failed - child '" + copyFrom.name + "'  was not found in '" + this.name + "' which is being copied to.");
            else
            {
                ConfigurableJoint curJoint = null;
                curJoint = transToCopyTo.GetComponent<ConfigurableJoint>();

                if (curJoint == null) curJoint = transToCopyTo.gameObject.AddComponent<ConfigurableJoint>();
                if (curJoint == null) Debug.LogWarning("ARCopyARComponents failed to create ConfigurableJoint on " + copyFrom.name);
                CopyConfigJointSettings(copyFrom, curJoint);
            }
        }
    }

    void CopyConfigJointSettings(ConfigurableJoint fromJ, ConfigurableJoint toJ)
    {
        //first, gotta get the right connectedBody - hardest part
        if (fromJ.connectedBody != null)
        {
            Transform getBodFrom = this.transform.FindDeepChild(fromJ.connectedBody.name);
            if(getBodFrom == null)
                Debug.LogWarning("Couldn't setup configurable joint " + toJ.name + " connectedBody because could't find rigidbody on " + fromJ.connectedBody.name);
            else
            {
                Rigidbody bod = getBodFrom.GetComponent<Rigidbody>();
                toJ.connectedBody =bod;
            }
        }
        //now copy the rest of settings
        toJ.CopyMotionDataFrom(fromJ);
    }

    /// <summary>
    /// Collects all the colliders in the "from" object heirarchy, duplicates on the "to" heirarchy, and copies info (size, isTrigger, etc)
    /// </summary>
    void CopyColliders()
    {
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
        Collider[] allFromColliders = toCopyFrom.GetComponentsInChildren<Collider>();
        //Create list so we can remember which children in the heirarchy we've already copied colliders over for
        List<Transform> alreadyCopied  = new List<Transform>();

        Transform curObj = null;
        Transform copyTo = null;
        //for each gameObject (NOT COLLIDER) represented in the allFromColliders list, we need to duplicate it's colliders
        for (int i = 0; i < allFromColliders.Length; i++)
        {
            curObj = allFromColliders[i].transform;
            //if we've already handled the curObj in a previous iteration of the loop, we can skip it!
            if (alreadyCopied.Contains(curObj)) continue;
            //Find the equivalent child in the "to" heirarchy which we'll be copying to - if fail to find, warn and continue to next object
            copyTo = this.transform.FindDeepChild(curObj.name);
            if (copyTo == null) { 
                Debug.LogWarning("Heirarchy mismatch error - Copy Colliders failed - child '" + curObj.name + "'  was not found in '" + this.name + "' which is being copied to.");
                continue;
            }
            //alrighty, we need to get and copy all colliders of each type from "curObj" to "copyTo"
            CopyIndividColliders<CapsuleCollider>(curObj, copyTo);
            CopyIndividColliders<SphereCollider>(curObj, copyTo);
            CopyIndividColliders<BoxCollider>(curObj, copyTo);
            CopyIndividColliders<MeshCollider>(curObj,copyTo);

            alreadyCopied.Add(curObj);
        }
        //Done-zo
    }

    void CopyIndividColliders<T>(Transform copyFrom, Transform copyTo) where T : Collider
    {
        //Gather <T> colliders on "copyFrom"
        T[] colsToCopy = copyFrom.GetComponents<T>();
        //if none, return
        if (colsToCopy == null || colsToCopy.Length == 0) return;
        //Gather <T> colliders on "copyTo"
        List<T> colsOnCopyTo = new List<T>( copyTo.GetComponents<T>());
        //figure out the difference in numbers of <T> colliders between the 2
        int difInNumOfCols = colsToCopy.Length - colsOnCopyTo.Count;
        //Add colliders to "copyTo" until it has the same number as "copyFrom"
        for (int i = 0; (i < difInNumOfCols); i++)
        {
            colsOnCopyTo.Add(copyTo.gameObject.AddComponent<T>());
        }

        T curFrom = null;
        T curTo = null;
        //Now copyTo has at least as many <T> colliders as copyFrom - lets copy the data over
        for (int i = 0; i < colsToCopy.Length; i++)
        {
            curFrom = colsToCopy[i];
            curTo = colsOnCopyTo[i];

            //use correct data copy method... ye, it's a little dumb to have a generic function and then cast back to a specific class, but I can't be assed rn
            if(typeof(T) == typeof(SphereCollider))  (curTo as SphereCollider).CopyDataFrom(curFrom as SphereCollider);
            else if(typeof(T) == typeof(CapsuleCollider)) (curTo as CapsuleCollider).CopyDataFrom(curFrom as CapsuleCollider);
            else if(typeof(T) == typeof(BoxCollider)) (curTo as BoxCollider).CopyDataFrom(curFrom as BoxCollider);
            else if (typeof(T) == typeof(MeshCollider)) (curTo as MeshCollider).CopyDataFrom(curFrom as MeshCollider);
        }
    }

    void CopyJointMotionScripts()
    {
        JointCopyMotion[] toCopyJoints = toCopyFrom.GetComponentsInChildren<JointCopyMotion>();
        if (toCopyJoints == null) return;

        for (int i = 0; i < toCopyJoints.Length; i++)
        {
            JointCopyMotion copyFrom = toCopyJoints[i];
            Transform transToCopyTo = this.transform.FindDeepChild(copyFrom.name);

            if (transToCopyTo == null)
                Debug.LogWarning("Heirarchy mismatch error - Copy JointCopyMotion.cs failed - child '" + copyFrom.name + "'  was not found in '" + this.name + "' which is being copied to.");
            else
            {
                JointCopyMotion curJoint = null;
                curJoint = transToCopyTo.GetComponent<JointCopyMotion>();

                if (curJoint == null) curJoint = transToCopyTo.gameObject.AddComponent<JointCopyMotion>();
                if (curJoint == null) Debug.LogWarning("ARCopyARComponents failed to create JointCopyMotion on " + copyFrom.name);
                curJoint.CopySettings(copyFrom);
            }
        }
    }
}
