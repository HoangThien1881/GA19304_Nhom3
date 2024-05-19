using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    Vector2 moveInput;

  
    void Update()
    {
        
    }
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        Debug.Log("Move input: " + moveInput);
        //moveInput 
        //1,0 -> move phải
        //-1,0 -> move trái
        // 0,1 -> lên
        //0,-1 -> xuống
    }
    void OnJump()
    {
        Debug.Log("Jump");
    }

}
