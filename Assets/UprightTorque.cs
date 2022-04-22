using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ RequireComponent(typeof(Rigidbody))]
public class UprightTorque : MonoBehaviour
{
    public float uprightSpringStrength = 100;
    public float uprightSpringDamping = 5;
    public Transform rotationTarget;
        
    private Rigidbody _myBod;
    // Start is called before the first frame update
    void Start()
    {
        _myBod = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateUprightForce(Time.fixedDeltaTime);
    }

    private void OnAnimatorIK(int layerIndex)
    {
        
    }

    void UpdateUprightForce(float timeElapsed)
    {
        Quaternion currentRot = transform.rotation;
        Quaternion desiredRot = ShortestRotation(rotationTarget.rotation, currentRot);

        Vector3 rotAxis;
        float rotDegrees;

        desiredRot.ToAngleAxis( out rotDegrees, out rotAxis);
        rotAxis.Normalize();

        float rotRads = rotDegrees * Mathf.Deg2Rad;

        _myBod.AddTorque((rotAxis * (rotRads * uprightSpringStrength)) - (_myBod.angularVelocity * uprightSpringDamping));
    }

    public static Quaternion ShortestRotation(Quaternion a, Quaternion b)    {
        if (Quaternion.Dot(a, b) < 0) return a * Quaternion.Inverse(Multiply(b, -1));
        else return a * Quaternion.Inverse(b);
    }
    public static Quaternion Multiply(Quaternion input, float scalar)    {
        return new Quaternion(input.x * scalar, input.y * scalar, input.z * scalar, input.w * scalar);
    }
}
