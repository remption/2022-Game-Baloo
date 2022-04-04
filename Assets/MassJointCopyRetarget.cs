using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MassJointCopyRetarget : MonoBehaviour
{
    public Transform targetAnimationsSource;
    public bool DoUpdate = false;
    // Update is called once per frame
    void Update()
    {
        if (DoUpdate)
        {

            RetargetAnimations();
            DoUpdate = false;
        }



    }

    void RetargetAnimations()
    {
        Transform toCopyFrom = null;
        JointCopyMotion[] jMs = this.transform.transform.GetComponentsInChildren<JointCopyMotion>();
        for (int i = 0; i < jMs.Length; i++)
        {
            JointCopyMotion j = jMs[i];
            if (j.toCopy == null) continue;

            toCopyFrom = targetAnimationsSource.FindDeepChild(j.toCopy.name); //find bone with same name!
            j.toCopy = toCopyFrom;
        }
    }

}
