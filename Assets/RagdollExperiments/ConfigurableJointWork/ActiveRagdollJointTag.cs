using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActiveRDJointTag", menuName = "Scriptables/ActiveRag/JointTag")]
public class ActiveRagdollJointTag : ScriptableObject
{
    public string tagName;
    public string[] smartTagSubstrings;
    public List<ActiveRagdollJointTagInst> taggedBois;

    public void Subscribe(ActiveRagdollJointTagInst jointToSub)
    {
        if (taggedBois == null) taggedBois = new List<ActiveRagdollJointTagInst>();
        if (!taggedBois.Contains(jointToSub)) taggedBois.Add(jointToSub);
        Debug.Log("well BOIS: "+taggedBois[0].GetType().ToString());
    }

    public void Unsubscribe(ActiveRagdollJointTagInst jointToUnsub)
    {
        if (taggedBois == null) return;
        taggedBois.Remove(jointToUnsub);
    }

    public void SmartTag(ConfigurableJoint[] jointsToTag)
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
    }


    /// <summary>
    /// Returns true if NameToCheck string contains all the smartTagSubstrings (aka, if we should smart tag this)
    /// IGNORES CASE
    /// </summary>
    /// <param name="nameToCheck"></param>
    /// <returns></returns>
    public bool CheckSmartTags(string nameToCheck)
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

