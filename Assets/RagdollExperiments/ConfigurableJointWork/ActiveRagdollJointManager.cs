using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveRagdollJointManager : MonoBehaviour
{
    public List<ConfigurableJoint> _joints;//sort of read only in editor - it should just help us find the joints before tagging or if there are some we dont' want tagged

    /// <summary>
    ///we work from tags - they give us access to the joint, joint movers, etc
    ///Todo - we could work strictly through the tags array and access their subbed joints, instead of
    ///storing this list of references?
    /// </summary>
    public List<ActiveRagdollJointTagInst> _taggedJoints;

    /// <summary>
    ///tags defined for the organization of the editor... need a different structure, tho, that lets us search for multiple snippets (ie, "Left" and "arm" -> left arm
    ///foldout, "Spine" -> spine foldout, or "spine" and "neck" -> spine, "leg1" -> leg1 foldout, etc!
    ///
    /// TODO EVENTUALLY - make a "ActiveRagdollManager" Scriptable object. It can hold all of the tag scriptableObjects as a single asset? Or
    /// figure out asset packing so we don't have folders and folders of wierdly placed SOs
    /// </summary>
    public ActiveRagdollJointTag[] tags; 

    public enum ActiveRagdollDisplayType
    {
        Generic,
        Creature,
        Humanoid
    }












    #region jointsListManagement
    public void CollectJoints(GameObject rootObj) {
        EmptyJointsList();
        CollectJointsHelper(rootObj.transform);
    }

    //Creates depth-first list :)
    void CollectJointsHelper(Transform toCollectFrom) {
        if (toCollectFrom == null) return;

        ConfigurableJoint foundJoint = toCollectFrom.GetComponent<ConfigurableJoint>();
        if(foundJoint != null) {
            if (!_joints.Contains(foundJoint)) _joints.Add(foundJoint);
        }
        for (int i = 0; i < toCollectFrom.childCount; i++) {
            CollectJointsHelper(toCollectFrom.GetChild(i));
        }
    }

    public void EmptyJointsList() {
        _joints = new List<ConfigurableJoint>();
    }
    #endregion

    #region Tagging management

    public void SmartTagJoints()
    {
        //validate that data is setup
        if (tags == null)
        {
            Debug.LogWarning(gameObject.name + " ActiveRagdollManager couldn't smart tag because it has no tags");
            return;
        }
        if(_joints==null || _joints.Count == 0)
        {
            Debug.LogWarning(gameObject.name + " ActiveRagdollManager couldn't smart tag because it has no joints assigned");
            return;
        }

        //for each tag, give it a chance to do it's tagging biz
        for(int i = 0; i< tags.Length; i++)
        {
            tags[i].SmartTag(_joints.ToArray());
        }
    }
    #endregion

    public bool stringContains(string toCheck, params string[] args)
    {
        if (args == null || args.Length == 0) return false;
        
        for (int i = 0; i < args.Length; i++)
        {
            if (!toCheck.Contains(args[i])) return false;
        }
        return true;
    }


}
