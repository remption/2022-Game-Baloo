using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActiveRDJointTag", menuName = "Scriptables/ActiveRag/JointTag")]
public class ARJointTag : ScriptableObject
{
    public string tagName;
    public string[] smartTagSubstrings;



   /* public void SmartTag(ConfigurableJoint[] jointsToTag)
    {
        //Data validation - make sure those joints are legit, bro!
        if(jointsToTag == null || jointsToTag.Length == 0)
        {
            Debug.LogWarning(this.name + " couldn't Smart tag - no joints were given to it");
            return;
        }

        for (int i = 0; i < jointsToTag.Length; i++)
        {
            //if this doesn't meet smart tag criteria, continue to next joint in list
            if (!CheckSmartTags(jointsToTag[i].gameObject.name)) continue;

            ActiveRagdollJointTagInst tagInst = jointsToTag[i].GetComponent<ActiveRagdollJointTagInst>();
            if(tagInst == null) tagInst = jointsToTag[i].gameObject.AddComponent<ActiveRagdollJointTagInst>();

            tagInst.SubscribeMe(this);
        }
    }*/


    /// <summary>
    /// Returns true if NameToCheck string contains all the smartTagSubstrings (aka, if we should smart tag this)
    /// IGNORES CASE
    /// </summary>
    /// <param name="nameToCheck"></param>
    /// <returns></returns>
    public bool StringContainTags(string nameToCheck)
    {
        if (nameToCheck == null) return false;

        bool toReturn = true;
        for(int i = 0; i < smartTagSubstrings.Length; i++)
        {
            string checkLower = nameToCheck.ToLower();
            if (!checkLower.Contains(smartTagSubstrings[i].ToLower())) return false;
        }
        return toReturn;
    }

}

