using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FootIK : MonoBehaviour
{
    public Transform ikOrigin;
    public LayerMask layers;
    private RaycastHit _rayHit;
    private Ray _ray;
    public float raycastDist = .3f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PerformIKRaycast() {
        if(Physics.Raycast(_ray, out _rayHit, raycastDist, layers)) {
            Debug.Log("Hit, my son");
        }
    }

}
