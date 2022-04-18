using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyBearPositionSetter : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        Shader.SetGlobalVector("_BabyBearPosition", this.transform.position);   
    }
}
