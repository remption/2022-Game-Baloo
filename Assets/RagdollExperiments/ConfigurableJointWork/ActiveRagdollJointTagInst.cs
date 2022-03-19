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
public class ActiveRagdollJointTagInst : MonoBehaviour
{
    [HideInInspector]  public ConfigurableJoint _myJoint;
    [HideInInspector] public JointCopyMotion _jointMotion;

    public ActiveRagdollJointTag myTagSO;
    public ActiveRagdollJointManager myManager;

    private void OnEnable()
    {
        _myJoint = GetComponent<ConfigurableJoint>();
        _jointMotion = GetComponent<JointCopyMotion>();
    }

    public void SubscribeMe(ActiveRagdollJointTag toSubTo, ActiveRagdollJointManager manager)
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
