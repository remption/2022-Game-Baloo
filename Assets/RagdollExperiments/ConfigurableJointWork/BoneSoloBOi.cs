using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class BoneSoloBOi : MonoBehaviour
{
    public List<ConfigurableJoint> jointsToAnimate;
    public Transform rootOfObjectToCopy;
    public bool gatherBonesToTag = false;
    public bool addJointCopyScripts = false;
    public bool setupJointCopyScripts = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (gatherBonesToTag)
        {
            gatherBonesToTag = false;
            GatherBones();
        }

        if (addJointCopyScripts)
        {
            addJointCopyScripts = false;
            AddJointMoveScript();
        }

        if (setupJointCopyScripts)
        {
            if (rootOfObjectToCopy == null) Debug.LogWarning(this.gameObject.name + " couldn't setup joint copy scripts because no object to copy was set");
           
            else SetupJointCopyScripts();
            setupJointCopyScripts = false;
        }

    }

    void SetupJointCopyScripts()
    {
        foreach (var joint in jointsToAnimate)
        {
            JointCopyMotion jcm = joint.GetComponent<JointCopyMotion>();
            if (jcm == null)
            {
                Debug.LogWarning(jcm.gameObject.name + " is in jointToAnimate list, but had no jointCopyMotion script applied. Skipping setup");
            }
            else{
                
                Transform toCopyFrom = FindInHeirarchy(rootOfObjectToCopy, joint.gameObject.name);
                if (toCopyFrom != null)
                {
                    jcm.toCopy = toCopyFrom;
                    
                }
                else
                {
                    Debug.LogWarning(joint.gameObject.name + " failed to find a transform with its same name in the root object its trying to copy motion from");
                }
            }
        }
    }



    public Transform FindInHeirarchy(Transform root, string lookFor)
    {
        if(root.name == lookFor) return root;
        Transform toRet = null;
        for (int i = 0; i < root.childCount; i++)
        {
            toRet = FindInHeirarchy(root.GetChild(i), lookFor);
            if (toRet != null) return toRet;

        }
  
        return null;      //didn't find :(
    }


    void AddJointMoveScript()
    {
        if (jointsToAnimate == null || jointsToAnimate.Count == 0) return;
        foreach (ConfigurableJoint joint in jointsToAnimate)
        {
            JointCopyMotion jcm = joint.GetComponent<JointCopyMotion>(); 
            if(jcm == null) jcm = joint.gameObject.AddComponent<JointCopyMotion>();

        }
    }

    void GatherBones()
    {
        jointsToAnimate = new List<ConfigurableJoint>(GetComponentsInChildren<ConfigurableJoint>());
    }
}
