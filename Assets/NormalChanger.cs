using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class NormalChanger : MonoBehaviour
{
    public Mesh mesh;
    public Vector3 desiredNormal = Vector3.up;
    public float weight = .5f;

    public bool perform = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (perform)
        {
            if (mesh != null) PerformNormalBend();
            perform = false;
        }
    }

    void PerformNormalBend()
    {
        Vector3[] norms = mesh.normals;
        for (int i = 0; i < norms.Length; i++)
        {
            norms[i] = Vector3.Lerp(norms[i],desiredNormal , weight);
        }
        mesh.normals = norms;
    }
}
