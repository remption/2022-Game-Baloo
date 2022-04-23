using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class ARJointMotionSetupTool: MonoBehaviour
{
    public List<ConfigurableJoint> jointsToAnimate;
    public Transform rootOfObjectToCopy;
    public bool gatherBonesToTag = false;
    public bool addJointCopyScripts = false;
    public bool setupJointCopyScripts = false;

    // Update is called once per frame
    void Update()
    {
        if (gatherBonesToTag)
        {
            gatherBonesToTag = false;
            //gather all the joints
            jointsToAnimate = new List<ConfigurableJoint>(GetComponentsInChildren<ConfigurableJoint>());
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
                
                Transform toCopyFrom = rootOfObjectToCopy.FindDeepChild(joint.gameObject.name);
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

    void AddJointMoveScript()
    {
        if (jointsToAnimate == null || jointsToAnimate.Count == 0) return;
        foreach (ConfigurableJoint joint in jointsToAnimate)
        {
            JointCopyMotion jcm = joint.GetComponent<JointCopyMotion>(); 
            if(jcm == null) jcm = joint.gameObject.AddComponent<JointCopyMotion>();

        }
    }
}
