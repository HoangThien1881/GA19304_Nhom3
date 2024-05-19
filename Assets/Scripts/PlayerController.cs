using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    Vector2 moveInput;
    [SerializeField] float moveSpeed = 15f;
    private Animator _animator;
    [SerializeField] float jumpSpeed = 15.0f;
     private Rigidbody2D _rigidbody2D;
    CapsuleCollider2D _CapsuleCollider2D;
    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _CapsuleCollider2D = GetComponent<CapsuleCollider2D>();
    }
    void Update()
    {
        Run();
        FlipSprite();
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
    void OnJump(InputValue value)
    {
        var isTouchingGround = _CapsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground"));
        if (!isTouchingGround) return;
        if (value.isPressed)
        {
            _rigidbody2D.velocity = new Vector2(x: 0, y: jumpSpeed);

        }
    }
    void Run()
    {
        Vector2 moveVelocity = new Vector2(x:moveInput.x*moveSpeed,_rigidbody2D.velocity.y);
        _rigidbody2D.velocity = moveVelocity;
        bool playerHasHorizontalSpeed = Mathf.Abs(moveInput.x) > Mathf.Epsilon;
        _animator.SetBool(name:"isRunning", playerHasHorizontalSpeed);
    }
    // abs : giá trị tuyệt đối
    // sign : đấu của giá trị
    // epsion : giá trị nhỏ nhất có thể so sánh
    //xoay hướng nhân vật theo chuyển động
    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(_rigidbody2D.velocity.x) > Mathf.Epsilon;
        if(playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(x: Mathf.Sign(_rigidbody2D.velocity.x), y: 1f);
        }
    }

}
