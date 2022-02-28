using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Attempting to make a smarter script (in terms of physics/velocity) to
/// copy the animations of a character and emulate via physics.
/// 
/// v0 - lets try to use target velocity instead of target position/rot
/// </summary>
/// 

[RequireComponent(typeof(ConfigurableJoint))]
public class SmartJointCopy : MonoBehaviour
{

    public Transform transToCopy;
    private ConfigurableJoint _myJoint;

    private Vector3 prevPosition;
    private Vector3 currentPosition;

    private Quaternion prevRot;
    private Quaternion currentRot;

    
    // Start is called before the first frame update
    void Start()
    {
        _myJoint = this.GetComponent<ConfigurableJoint>();
        currentRot = transToCopy.rotation;
        prevRot = transToCopy.rotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       GetTargetVelocities();

       //Vector3 targetAngular  = GetAngularVelocity(prevRot, currentRot);
      //  _myJoint.targetAngularVelocity = targetAngular;
        //Debug.Log("target angular velocity: " + targetAngular);
        prevRot = currentRot;
        currentRot = transToCopy.rotation;
    }

    void GetTargetVelocities() {
        Vector3 targetRot = clampRotTo180s(getRotationalDifference());
        float x = targetRot.z;

        _myJoint.targetAngularVelocity = targetRot / Time.fixedDeltaTime;
        _myJoint.targetRotation = Quaternion.Inverse(currentRot) * prevRot;

       
    }

    Vector3 getRotationalDifference() {
        //Quaternion targetRot = Quaternion.Inverse(currentRot) * prevRot;
        Quaternion targetRot = currentRot * Quaternion.Inverse(prevRot);
        return targetRot.eulerAngles;
    }

    static Vector3 GetAngularVelocity(Quaternion foreLastFrameRotation, Quaternion lastFrameRotation) {
        var q = lastFrameRotation * Quaternion.Inverse(foreLastFrameRotation);
        // no rotation?
        // You may want to increase this closer to 1 if you want to handle very small rotations.
        // Beware, if it is too close to one your answer will be Nan
        if (Mathf.Abs(q.w) > 1023.5f / 1024.0f)
            return new Vector3(0, 0, 0);
        float gain;
        // handle negatives, we could just flip it but this is faster
        if (q.w < 0.0f) {
            var angle = Mathf.Acos(-q.w);
            gain = -2.0f * angle / (Mathf.Sin(angle) * Time.fixedDeltaTime);
        } else {
            var angle = Mathf.Acos(q.w);
            gain = 2.0f * angle / (Mathf.Sin(angle) * Time.fixedDeltaTime);
        }
        return new Vector3(q.x * gain, q.y * gain, q.z * gain);
    }

    /// <summary>
    /// this is bad, but ok
    /// </summary>
    /// <param name="toClamp"></param>
    /// <returns></returns>
    Vector3 clampRotTo180s(Vector3 toClamp) {
        while (toClamp.x < -180) toClamp.x += 180;
        while (toClamp.x > 180) toClamp.x -= 180;

        while (toClamp.y < -180) toClamp.y += 180;
        while (toClamp.y > 180) toClamp.y -= 180;

        while (toClamp.z < -180) toClamp.z += 180;
        while (toClamp.z > 180) toClamp.z -= 180;

        return toClamp;
    }
}
