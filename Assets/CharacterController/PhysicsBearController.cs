using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PhysicsBearController : MonoBehaviour
{

    public PawMonitor rFront;
    public PawMonitor lFront;
    public PawMonitor rBack;
    public PawMonitor lBack;

    public BodySpring butt;
    public BodySpring chest;

    private Camera _cam;


    //Input handling variables
    private Vector2 input_move= Vector2.zero;
    private bool input_sprinting = false;
    private bool input_jump = false;
    private bool input_standToggled = false;

    // Start is called before the first frame update
    void Start()
    {
        _cam = Camera.main;
    }

    public void Move()
    {
        //check input, animate appropriately.
        //paw monitors recieve animation events - they will send force data... this could be real bad idea.
        //paw monitors ground themselves
        //body springs will detect distance from 



    }
    /// <summary>
    /// Uses the active camera to turn an input vector into a world-space movement vector,
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    Vector3 MoveInputToWorldVector(Vector2 v)
    {

        Vector3 toRet = _cam.transform.forward.normalized * v.y + _cam.transform.right.normalized * v.x;
        toRet.y = 0;

        return toRet.normalized;

    }






    #region InputMethods
    public void OnMove(InputValue value)
    {
        input_move = value.Get<Vector2>();
    }
    public void OnSprint(InputValue value)
    {
        float sprint = value.Get<float>();
        input_sprinting = sprint > .3f;
        //input_sprinting = value.Get<bool>();
        // Debug.Log("sprint: "+input_sprinting);
    }

    public void OnJump(InputValue value)
    {
        input_jump = value.Get<float>() > 0;
    }

    public void OnStandToggle(InputValue value)
    {
        input_standToggled = value.Get<float>() > 0;//if greater than 0, we've had a button press!
    }

    #endregion



}
