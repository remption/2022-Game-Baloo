using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BearController : MonoBehaviour
{
    private Camera _cam;

    

    private Vector2 _forwardDirection;//calculate in relation to camera! or, should take into account ground plane (aka slope)???
    private float _targetYRotation;
    private float _rotationVelocity;
    private Vector3 _ongoingDesiredVelocity;


    public Rigidbody rigidBody;


    public float RotationSmoothTime = 1;

    public float maxSpeed = 8;
    public float acceleration = 200;
    public float maxAccelForce = 150;

    public float maxSprintSpeed = 16;

    
    private bool isOn2Legs = false;


    public Animator animator;

    private Vector2 input_Move;
    private bool input_sprinting = false;
    private bool input_standToggled = false;


    private float defaultGrassShaderBearStrength = 2.5f;
    private float twoLegGrassShaderBearStrength = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        _cam = Camera.main;
    }

    private void Move()
    {
        if (rigidBody == null)
        {
            Debug.LogWarning(name + "Has no assigned rigidbody to their BearController script - it cannot move");
        }
        if (input_Move.sqrMagnitude > 1) input_Move.Normalize(); // if the moveinput magnitude is > 1 ish, we should limit it? So we get slow ramp into movement, but then lock it off at 1 (magnitude goes up to 1.41~)

        Vector3 desiredMove = MoveInputToWorldVector(input_Move);
        ApplyPhysics(desiredMove);
    }
    private void LateUpdate()
    {
        Move();
        CalculateForwardDirection();
    }

    private void ApplyPhysics(Vector3 movement)
    {
        Vector3 desiredMovement = movement * (input_sprinting? maxSprintSpeed: maxSpeed);//*speedFactor
        Vector3 verticalVeloc = new Vector3(0, rigidBody.velocity.y, 0);//we just need the up down velocity, 'cuz movement is not applied up and down
        _ongoingDesiredVelocity = Vector3.MoveTowards(_ongoingDesiredVelocity,
            desiredMovement + verticalVeloc,
            acceleration * Time.deltaTime);//SHOUDLNT BE DELTA TIME, BUT FUCK IT
        // rigidBody.velocity.magnitude;

        Vector3 neededVelocity = desiredMovement - rigidBody.velocity;
        neededVelocity = Vector3.ClampMagnitude(neededVelocity, maxAccelForce*(input_sprinting?1.5f:1f));
        rigidBody.AddForce(neededVelocity);
        // Debug.Log("added force:" + neededVelocity);
        if (animator != null) animator.SetFloat("Speed", rigidBody.velocity.magnitude);
        if (input_standToggled)
        {
            HandleStandToggleRequest();

        }


        
    }

    void HandleStandToggleRequest()
    {
        input_standToggled = false; //we've used the 'event', so set to false;
        isOn2Legs = !isOn2Legs;
        if (isOn2Legs) {
            Shader.SetGlobalFloat("_bbyBearStrength", twoLegGrassShaderBearStrength);
        }
        else
        {
            Shader.SetGlobalFloat("_bbyBearStrength", defaultGrassShaderBearStrength);
        }
        if (animator != null) animator.SetBool("2LegMode", isOn2Legs);
    }




    private void CalculateForwardDirection()
    {
        Vector3 inputDirection = new Vector3(input_Move.x, 0.0f, input_Move.y);

        if (input_Move != Vector2.zero)
        {
            _targetYRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _cam.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetYRotation, ref _rotationVelocity, RotationSmoothTime);

            // rotate to face input direction relative to camera position
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }

        /* // update animator if using character
         if (_hasAnimator)
         {
             _animator.SetFloat(_animIDSpeed, _animationBlend);
             _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
         }*/
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
        return toRet;
    
    }

    public void OnMove(InputValue value)
    {
        input_Move = value.Get<Vector2>();
    }
    public void OnSprint(InputValue value)
    {
        float sprint = value.Get<float>();
        input_sprinting = sprint > .3f;
        //input_sprinting = value.Get<bool>();
       // Debug.Log("sprint: "+input_sprinting);
    }
    public void OnStandToggle(InputValue value)
    {
         input_standToggled = value.Get<float>() > 0;//if greater than 0, we've had a button press!
    }
}
