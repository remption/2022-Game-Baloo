using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// //really crappy script that just helps an object keep a specific distance off the ground or whatever is 
/// below the raycast point.  Not great
/// 
/// </summary>
public class Grounder : MonoBehaviour
{
    public Transform raycastOrigin;
    public float raycastDistance = 10;
    public float yOffsetFromHit = 0;
    public LayerMask layersToCast;


    
    // Update is called once per frame
    void LateUpdate()
    {
        Ray downboi = new Ray(raycastOrigin.position,Vector3.down);
        RaycastHit wutWeGot = new RaycastHit();

        if (Physics.Raycast(downboi, out wutWeGot, raycastDistance,layersToCast.value)) {
            Vector3 pos = this.transform.position;
            pos.y = wutWeGot.point.y + yOffsetFromHit;
            this.transform.position = pos;
        }
    }
}
