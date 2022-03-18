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

    private void OnEnable()
    {
        _myJoint = GetComponent<ConfigurableJoint>();
        _jointMotion = GetComponent<JointCopyMotion>();
        if (myTagSO != null) myTagSO.Subscribe(this);
    }

    private void OnDisable()
    {
        if (myTagSO != null) UnsubscribeMe();
    }

    public void SubscribeMe(ActiveRagdollJointTag toSubTo)
    {
        if (myTagSO != null) myTagSO.Unsubscribe(this);

        myTagSO = toSubTo;
        myTagSO.Subscribe(this);
    }

    public void UnsubscribeMe()
    {
        if(myTagSO != null) myTagSO.Unsubscribe(this);
        myTagSO = null;
    }

}
