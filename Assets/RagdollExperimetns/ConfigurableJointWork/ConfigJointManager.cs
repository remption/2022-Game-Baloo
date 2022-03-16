using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ConfigurableJoint))]
public class ConfigJointManager : MonoBehaviour
{
 
    private ConfigurableJoint _myJoint;
    public ConfigurableJointMotion xMotion;
    public ConfigurableJointMotion yMotion;
    public ConfigurableJointMotion zMotion;
    public ConfigurableJointMotion xAngularMotion;
    public ConfigurableJointMotion yAngularMotion;
    public ConfigurableJointMotion zAngularMotion;



    public void PushJointValues()
    {
      
    }


    private void UpdateRotMotionModes()
    {

    }

    private void UpdatePosMotionModes()
    {

    }

    private void UpdateAngularLimits()
    {

    }

    private void UpdateAngularDrives()
    {
        /*_myJoint.angularXDrive.positionSpring;
        _myJoint.angularXDrive.positionDamper;
        _myJoint.angularXDrive.maximumForce;*/
    }


    /*public List<ConfigurableJoint> joints = new List<ConfigurableJoint>();
    public float positionalDrive = 100;
    public float rotationalDrive = 1000;

    public void GetChildJoints() {
        joints = new List<ConfigurableJoint>();
        GetChildJointsHelper(this.transform);
    }

    void GetChildJointsHelper(Transform parent) {
         if(joints == null) joints = new List<ConfigurableJoint>();
        for (int i = 0; i < parent.childCount; i++) {
            ConfigurableJoint cj = parent.GetChild(i).GetComponent<ConfigurableJoint>();
            if (cj != null) joints.Add(cj);
            //get the children's childrens
            GetChildJointsHelper(parent.GetChild(i));
        }
    }*/
}
