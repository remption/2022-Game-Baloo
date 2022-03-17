using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveRagdollJointManager : MonoBehaviour
{
    public List<ConfigurableJoint> _joints;
   
    public enum ActiveRagdollDisplayType
    {
        Generic,
        Creature,
        Humanoid
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CollectJoints(GameObject rootObj) {
        if (_joints == null) _joints = new List<ConfigurableJoint>();
        CollectJointsHelper(rootObj.transform);
    }

    //Creates depth-first list :)
    void CollectJointsHelper(Transform toCollectFrom) {
        if (toCollectFrom == null) return;
        ConfigurableJoint foundJoint = toCollectFrom.GetComponent<ConfigurableJoint>();
        if(toCollectFrom != null) {
            if (!_joints.Contains(foundJoint)) _joints.Add(foundJoint);
        }
        Transform t = toCollectFrom.transform;
        for (int i = 0; i < t.childCount; i++) {
            CollectJointsHelper(t.GetChild(i));
        }
    }

    public void EmptyJointsList() {
        _joints.Clear();
    }



    public void TagJoint(ConfigurableJoint toTag, bool smartTag = false) {
        ActiveRagdollTag tag = toTag.GetComponent<ActiveRagdollTag>();
        if (tag == null) {

        }

    }


}
