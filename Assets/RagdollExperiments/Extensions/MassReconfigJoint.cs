using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteAlways]
public class MassReconfigJoint : MonoBehaviour
{
    public enum SetDriverMode
    {
        Overwrite,
        ChooseLesserValue,
        ChooseGreaterValue
    }

    public bool setAxis = false;
    public Vector3 axis = Vector3.right;
    public bool setSecondaryAxis = false;
    public Vector3 secondaryAxis = Vector3.up;

    public bool setAngularDrivers = false;
    public SetDriverMode angularDriverMode = SetDriverMode.ChooseGreaterValue;
    public float angularSpring = 500;

        public bool ReconfigButton = false;
    // Update is called once per frame
    void Update()
    {
        if (ReconfigButton)
        {
            Reconfig();
            ReconfigButton = false;
        }    
    }


    void Reconfig()
    {
        ConfigurableJoint[] joints = this.GetComponentsInChildren<ConfigurableJoint>();
        ConfigurableJoint curJ = null;
        for (int i = 0; i < joints.Length; i++)
        {
            curJ= joints[i];
            if(setAxis) curJ.axis = axis;
            if(setSecondaryAxis) curJ.secondaryAxis = secondaryAxis;
            if (setAngularDrivers) SetAngularDriveValue(curJ);
        }


    }

    void SetAngularDriveValue(ConfigurableJoint joint)
    {
        JointDrive tempX = joint.angularXDrive;//just to get basic settings in. No biggie
        JointDrive tempYZ = joint.angularYZDrive;
        JointDrive tempSlerp = joint.slerpDrive;

        tempX.positionSpring = angularSpring;
        tempYZ.positionSpring = angularSpring;
        tempSlerp.positionSpring = angularSpring;

        if(angularDriverMode == SetDriverMode.ChooseLesserValue)
        {
            //x
            if (tempX.positionSpring < joint.angularXDrive.positionSpring) joint.angularXDrive = tempX;
            //y
            if (tempYZ.positionSpring < joint.angularYZDrive.positionSpring) joint.angularYZDrive = tempYZ;
            //slerp
            if (tempSlerp.positionSpring < joint.slerpDrive.positionSpring) joint.slerpDrive = tempSlerp;
        }
        else if(angularDriverMode == SetDriverMode.ChooseGreaterValue)
        {
            //x
            if (tempX.positionSpring > joint.angularXDrive.positionSpring) joint.angularXDrive = tempX;
            //y
            if(tempYZ.positionSpring > joint.angularYZDrive.positionSpring) joint.angularYZDrive = tempYZ;
            //slerp
            if(tempSlerp.positionSpring > joint.slerpDrive.positionSpring)joint.slerpDrive = tempSlerp;
        }
        else //overwrite
        {
            joint.angularXDrive= tempX;
            joint.angularYZDrive= tempYZ;
            joint.slerpDrive = tempSlerp;
        }


    }

}
