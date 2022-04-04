using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BearController : MonoBehaviour
{
    private Camera cam;
    
    private Vector2 input_Move;

    private Vector2 moveDirection;//calculate in relation to camera! or, should take into account ground plane (aka slope)???
    

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }


    private void Move()
    {
        
    }


    public void OnMove(InputValue value)
    {
        input_Move= value.Get<Vector2>();
    }
}
