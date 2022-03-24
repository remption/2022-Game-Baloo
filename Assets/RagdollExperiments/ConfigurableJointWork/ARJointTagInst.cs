using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// A monobehavior that gets "tagged" on to configurableJoint gameobjects.
/// It helps our Active Ragdoll system keep track of all the joints and
/// what sort of joints they are!
/// </summary>
[ExecuteAlways]
[RequireComponent(typeof(ConfigurableJoint))]
public class ARJointTagInst : MonoBehaviour
{
    [HideInInspector]  public ConfigurableJoint _myJoint;
    [HideInInspector] public JointCopyMotion _jointMotion;

    public ARJointTag myTagSO;
    public ARJointManager myManager;

    private void OnEnable()
    {
        _myJoint = GetComponent<ConfigurableJoint>();
        _jointMotion = GetComponent<JointCopyMotion>();
    }

    public void SubscribeMe(ARJointTag toSubTo, ARJointManager manager)
    {
        if (myTagSO != null && myManager!= null)
        {
            myManager.Unsubscribe(this, myTagSO);
        }

        myTagSO = toSubTo;
        myManager = manager;

        myManager.Subscribe(this, myTagSO);
    }

    public void UnsubscribeMe()
    {
        if (myTagSO != null && myManager != null) myManager.Unsubscribe(this, myTagSO);
        myTagSO = null;
        myManager = null;
    }

}
