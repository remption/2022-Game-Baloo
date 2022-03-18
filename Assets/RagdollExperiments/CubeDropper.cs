using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeDropper : MonoBehaviour
{

    public GameObject toClone;
    public bool DROP = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(DROP && toClone != null)
        {
            GameObject obj = GameObject.Instantiate(toClone);
            obj.transform.position= transform.position; 
            DROP=false;
        }
    }
}
