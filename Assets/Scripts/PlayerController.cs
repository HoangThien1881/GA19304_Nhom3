using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    Vector2 moveInput;
    [SerializeField] float moveSpeed = 15f;
        

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
        Vector2 moveVelocity = new Vector2(x:moveInput.x*moveSpeed,_rigidbody2D.velocity.y);
        _rigidbody2D.velocity = moveVelocity;
    }
    // abs : giá trị tuyệt đối
    // sign : đấu của giá trị
    // epsion : giá trị nhỏ nhất có thể so sánh
    //xoay hướng nhân vật theo chuyển động
    void Flip()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(_rigidbody2D.velocity.x) > Mathf.Epsilon;
    }

}
