using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public ScriptableFloat speedController;
    public float speedMultiplier = 1;
    public bool physicsBased = false;

    private Rigidbody myBod;

    private void Start() {
        myBod = this.GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        if(!physicsBased && speedController != null) {
            Vector3 pos = this.transform.position;
            pos.z += speedController.value * speedMultiplier * Time.deltaTime;
            this.transform.position = pos;
        }
    }

    private void FixedUpdate() {
        if (physicsBased) {
            if (myBod == null) myBod = this.GetComponent<Rigidbody>();
            if (myBod != null && speedController != null) {
                Vector3 velocity = myBod.velocity;
                velocity.z = speedController.value * speedMultiplier;
                myBod.velocity = velocity;
            } else if (myBod == null) Debug.Log("No rigidbod on " + gameObject.name + ". MoveForward script needs it for physics based motion!");
        }
    }
}
