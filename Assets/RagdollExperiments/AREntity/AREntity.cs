using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Active ragdoll manager! It gathers bones and manages them :)
/// </summary>
public class AREntity : MonoBehaviour
{
    public List<ARJointData> joints;

    [Tooltip("If motion is being copied from a rig with identical or mostly identical naming," +
        " assign the source root here and hit auto populate source")]
    public GameObject motionSourceRoot;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }







}

[System.Serializable]
public class ARJointData
{
    public ConfigurableJoint joint;
    public Transform motionSource;
    public bool enableMotionCopying;
}