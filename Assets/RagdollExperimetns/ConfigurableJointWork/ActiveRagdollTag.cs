using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveRagdollTag : MonoBehaviour
{
    public ActiveJointTag _tag;
}
public enum ActiveJointTag
{
    Generic,
    Hips,
    Spine,
    Neck,
    Head,
    UpperArm,
    LowerArm,
    UpperLeg,
    LowerLeg,
    Feet,
    Hands
}