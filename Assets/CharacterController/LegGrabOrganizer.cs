using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//gets events from animation engine, which legs when to grab/release
public class LegGrabOrganizer : MonoBehaviour
{
    public PawMonitor lFront;
    public PawMonitor lBack;
    public PawMonitor rFront;
    public PawMonitor rBack;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FrontLegGrab(int i)
    {
        if (lFront != null) lFront.Ground();
        if (rFront != null) rFront.Ground();
    }
    public void RearLegGrab(int i)
    {
        if (rBack != null) rBack.Ground();
        if (lBack != null) lBack.Ground();
    }

    public void FrontLegRelease(int i)
    {
        if(lFront != null) lFront.Release();
        if(rFront != null) rFront.Release();
    }

    public void RearLegRelease(int i)
    {
        if (lBack != null) lBack.Release();
        if (rBack != null) rBack.Release();
    }

}
