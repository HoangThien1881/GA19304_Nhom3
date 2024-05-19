using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    Vector2 moveInput;
    [SerializeField] float moveSpeed = 5f;

     private Rigidbody2D _rigidbody2D;
    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        Run();
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
    void Run()
    {
        _rigidbody2D.velocity = new Vector2(moveInput.x, 0f);
    }

}
