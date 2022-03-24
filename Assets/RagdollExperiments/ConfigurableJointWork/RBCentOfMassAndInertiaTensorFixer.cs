using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RBCentOfMassAndInertiaTensorFixer : MonoBehaviour
{

    private void Awake() {
       Rigidbody[] bods =  gameObject.GetComponentsInChildren<Rigidbody>();

        for (int i = 0; i < bods.Length; i++) {
            Rigidbody rb = bods[i];
            rb.centerOfMass = new Vector3(0, 0, 0);
            rb.inertiaTensor = new Vector3(1, 1, 1);

        }
    }
}
