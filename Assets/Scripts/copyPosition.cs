using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class copyPosition : MonoBehaviour
{
    public Transform toCopy;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (toCopy != null)
        {
            this.transform.position = toCopy.position;
            this.transform.rotation = toCopy.rotation;  
        }
    }
}
