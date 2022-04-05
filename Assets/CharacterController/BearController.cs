using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BearController : MonoBehaviour
{
    private Camera _cam;
    
    private Vector2 input_Move;

    private Vector2 _forwardDirection;//calculate in relation to camera! or, should take into account ground plane (aka slope)???
    private float _targetYRotation;
    private float _rotationVelocity;
    public float RotationSmoothTime = 1;

    public float _speed = 3;
    // Start is called before the first frame update
    void Start()
    {
        _cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }


    private void Move()
    {
        CalculateForwardDirection();
    }

    private void CalculateForwardDirection()
    {
        Vector3 inputDirection = new Vector3(input_Move.x, 0.0f, input_Move.y).normalized;

        if (input_Move != Vector2.zero)
        {
            _targetYRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _cam.transform.eulerAngles.y;
            float rotation  = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetYRotation, ref _rotationVelocity, RotationSmoothTime);

            // rotate to face input direction relative to camera position
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }


        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetYRotation, 0.0f) * Vector3.forward;
        Vector3 yo = transform.rotation * Vector3.forward;



        // move the player
        this.transform.position = this.transform.position+(targetDirection.normalized * (_speed * Time.deltaTime));// + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

        /* // update animator if using character
         if (_hasAnimator)
         {
             _animator.SetFloat(_animIDSpeed, _animationBlend);
             _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
         }*/
    }


    public void OnMove(InputValue value)
    {
        input_Move= value.Get<Vector2>();
    }
}
