using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ComponentCopyExtensions
{
    public static void CopyDataFrom(this SphereCollider copyTo, SphereCollider copyFrom)
    {
        copyTo.radius = copyFrom.radius; 
        copyTo.sharedMaterial = copyFrom.sharedMaterial;
        copyTo.material = copyFrom.material;
        copyTo.isTrigger = copyFrom.isTrigger;
        copyTo.center  = copyFrom.center;
    }

    public static void CopyDataFrom(this CapsuleCollider copyTo, CapsuleCollider copyFrom)
    {
        copyTo.isTrigger = copyFrom.isTrigger;
        copyTo.height = copyFrom.height;
        copyTo.radius = copyFrom.radius;
        copyTo.material = copyFrom.material;
        copyTo.sharedMaterial = copyFrom.sharedMaterial;
        copyTo.enabled = copyFrom.enabled;
        copyTo.center  = copyFrom.center;
        copyTo.direction = copyFrom.direction;
    }

    public static void CopyDataFrom(this BoxCollider copyTo, BoxCollider copyFrom)
    {
        copyTo.size = copyFrom.size;
        copyTo.center = copyFrom.center;
        copyTo.material = copyFrom.material;
        copyTo.sharedMaterial =copyFrom.sharedMaterial;
        copyTo.enabled = copyFrom.enabled;
        copyTo.isTrigger = copyFrom.isTrigger;
    }

    public static void CopyDataFrom(this MeshCollider copyTo, MeshCollider copyFrom)
    {
        copyTo.sharedMesh = copyFrom.sharedMesh;
        copyTo.isTrigger = copyFrom.isTrigger;
        copyTo.enabled = copyFrom.enabled;
        copyTo.convex = copyFrom.convex;
        copyTo.cookingOptions= copyFrom.cookingOptions; 
        copyTo.material = copyFrom.material;
        copyTo.sharedMaterial=copyFrom.sharedMaterial;
    }

        /// <summary>
        /// Copies most settings other than connected body/articulation body
        /// </summary>
        /// <param name="copyTo"></param>
        /// <param name="copyFrom"></param>
        public static void CopyMotionDataFrom(this ConfigurableJoint copyTo, ConfigurableJoint copyFrom)
    {
        //copy drives
        copyTo.xDrive = copyFrom.xDrive;
        copyTo.yDrive = copyFrom.yDrive;
        copyTo.zDrive = copyFrom.zDrive;
        copyTo.angularYZDrive = copyFrom.angularYZDrive; ;
        copyTo.angularXDrive = copyFrom.angularXDrive;
        copyTo.slerpDrive = copyFrom.slerpDrive;
        //motion settings
        copyTo.angularXMotion = copyFrom.angularXMotion;
        copyTo.angularYMotion = copyFrom.angularYMotion;
        copyTo.angularZMotion = copyFrom.angularZMotion;
        copyTo.xMotion = copyFrom.xMotion;
        copyTo.yMotion = copyFrom.yMotion;
        copyTo.zMotion = copyFrom.zMotion;
        //Limits (incomplete?)
        copyTo.linearLimit = copyFrom.linearLimit;
        copyTo.linearLimitSpring = copyFrom.linearLimitSpring;
        copyTo.angularYLimit = copyFrom.angularYLimit;
        copyTo.highAngularXLimit = copyFrom.highAngularXLimit;
        copyTo.lowAngularXLimit = copyFrom.lowAngularXLimit;
        copyTo.angularZLimit = copyFrom.angularZLimit;
        copyTo.angularXLimitSpring = copyFrom.angularXLimitSpring;
        copyTo.angularYZLimitSpring = copyFrom.angularYZLimitSpring;
        //Other bits
        copyTo.enableCollision = copyFrom.enableCollision;
        copyTo.configuredInWorldSpace = copyFrom.configuredInWorldSpace;
        //Could still do axes, etc
    }

    public static void CopyDataFrom(this Rigidbody copyTo, Rigidbody copyFrom)
    {
        copyTo.isKinematic = copyFrom.isKinematic;
        copyTo.mass = copyFrom.mass;
        copyTo.constraints = copyFrom.constraints;
        copyTo.drag = copyFrom.drag;
        copyTo.interpolation = copyFrom.interpolation;
        copyTo.useGravity = copyFrom.useGravity;
    }

}
