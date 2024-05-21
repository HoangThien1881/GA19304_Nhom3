using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    Vector2 moveInput;
    [SerializeField] float moveSpeed = 15f;
    private Animator _animator;
    [SerializeField] float jumpSpeed = 15.0f;
     private Rigidbody2D _rigidbody2D;
    [SerializeField] float climbSpeed = 5;
    CapsuleCollider2D _capsuleCollider2D;
    private float gravityScaleAtStart;
    public GameObject _bulletPrefabs;
    public Transform _gunTransform;
    [SerializeField] private bool isMovingRight = true;
    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        gravityScaleAtStart = _rigidbody2D.gravityScale;
    }
    void Update()
    {
        Run();
        FlipSprite();
        climbLadder();
        Fire();
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
        var isTouchingGround = _capsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground"));
        if (!isTouchingGround) return;
        if (value.isPressed)
        {
            _animator.SetBool("isJumping",true);
            _rigidbody2D.velocity = new Vector2(x: 0, y: jumpSpeed);

        }
        _animator.SetBool("isJumping", false);
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
    //leo thang
    void climbLadder()
    {
        var isTouchingLadder = _capsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Climbing"));
        if(!isTouchingLadder)
        {
            _animator.SetBool("isClimbing", false);
            _rigidbody2D.gravityScale = gravityScaleAtStart;
            return;
        }
        var climbVelocity = new Vector2(_rigidbody2D.velocity.x, y:moveInput.y * climbSpeed);
        _rigidbody2D.velocity = climbVelocity;

        //điểu khiển animation cầu thang
        var playerHasVerticalSpeed = Mathf.Abs(moveInput.y) > Mathf.Epsilon;
        _animator.SetBool("isClimbing",true);
        //tắt gravity
        _rigidbody2D.gravityScale = 0;
    }
    private void Fire()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            //tạo viên đạn tại vị trí gun
            var oneBullet = Instantiate(_bulletPrefabs, _gunTransform.position, Quaternion.identity);
            //cho viên đạn bay theo hướng nhân vật
            if (isMovingRight == true)
            {
                oneBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(30f, 0);
                _animator.SetBool("isShooting",true);
            }
            else
            {
                oneBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(-30f, 0);
            
            }
          
            Destroy(oneBullet, 1f);
            _animator.SetBool("isShooting",false);



        }
        
    }
   
}
