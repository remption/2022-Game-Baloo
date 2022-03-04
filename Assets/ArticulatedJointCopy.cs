using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(ArticulationBody))]
public class ArticulatedJointCopy : MonoBehaviour
{
    public Transform jointToCopy;
    private ArticulationBody _myBody;
    private List<float> _driveTargets = new List<float>(3);
    // Start is called before the first frame update
    void Start()
    {
        _myBody=GetComponent<ArticulationBody>();   

    }

    // Update is called once per frame
    void Update()
    {
        if (jointToCopy && _myBody.jointType == ArticulationJointType.SphericalJoint)
        {
            Vector3 targetRots = jointToCopy.localEulerAngles;
            ArticulationDrive setterBoi = _myBody.xDrive;
            setterBoi.target = targetRots.x;
            _myBody.xDrive = setterBoi;

         //   setterBoi = _myBody.zDrive;
          //  setterBoi.target = targetRots.z;
          //  _myBody.zDrive = setterBoi;

           // setterBoi = _myBody.yDrive;
          //  setterBoi.target = targetRots.y;
          //  _myBody.yDrive = setterBoi;
        }
    }
}
