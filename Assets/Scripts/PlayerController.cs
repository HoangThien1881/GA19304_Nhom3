using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
public class PlayerController : MonoBehaviour
{
    Vector2 moveInput;
    [SerializeField] float moveSpeed = 15f;
    private Animator _animator;
    [SerializeField] float jumpSpeed = 15.0f;
    private Rigidbody2D _rigidbody2D;
    [SerializeField] float climbSpeed = 5;
    CapsuleCollider2D _capsuleCollider2D;

    BoxCollider2D _boxCollider2D;
    private float gravityScaleAtStart;
    //public GameObject _bulletPrefabs;
    //public Transform _gunTransform;



    public float dodgeSpeed = 4f; // Tốc độ khi dodge (lộn)
    public float dodgeDirection = 0.5f; // Thời gian dodge
    private float dodgeTime; // Thời gian còn lại của dodge
    private bool isDodging = false; // Trạng thái dodge (đang dodge hay không)
    [SerializeField] private bool isMovingRight = true;

    public GameObject arrowPrefab; // Prefab của mũi tên
    public Transform bowPosition; // Vị trí cung để sinh ra mũi tên
    public float arrowSpeed = 10f; // Tốc độ của mũi tên
    public float arrowLifetime = 1f; // Thời gian tồn tại của mũi tên


    [SerializeField] TextMeshProUGUI _scoreText;
    public int v = 0;
    private static int _score = 0;
    [SerializeField] private AudioClip _coinCollectSFX;






    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        gravityScaleAtStart = _rigidbody2D.gravityScale;
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _scoreText.text = _score.ToString();
    }
    void Update()
    {
        Run();
        FlipSprite();
        climbLadder();
        //Fire();
        Shoot();
        Dodge();

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

    void Dodge()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && dodgeTime <= 0) // Khi nhấn phím Shift và thời gian dodge đã hết
        {
            _animator.SetBool("isDodging", true); // Kích hoạt animation dodge
            moveSpeed += dodgeSpeed; // Tăng tốc độ di chuyển
            dodgeTime = dodgeDirection; // Đặt lại thời gian dodge
            isDodging = true; // Đánh dấu trạng thái đang dodge
        }

        if (dodgeTime <= 0 && isDodging == true) // Khi thời gian dodge hết và đang trong trạng thái dodge
        {
            _animator.SetBool("isDodging", false); // Tắt animation dodge
            moveSpeed -= dodgeSpeed; // Trả lại tốc độ di chuyển ban đầu
            isDodging = false; // Đặt lại trạng thái dodge
        }
        else
        {
            dodgeTime -= Time.deltaTime; // Giảm thời gian dodge theo thời gian thực
        }
    }
    void OnJump(InputValue value)
    {
        var isTouchingGround = _capsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground"));
        if (!isTouchingGround) return;
        if (value.isPressed)
        {
            _animator.SetBool("isJumping", true);
            _rigidbody2D.velocity = new Vector2(x: 0, y: jumpSpeed);

        }
        _animator.SetBool("isJumping", false);
    }
    void Run()
    {
        Vector2 moveVelocity = new Vector2(x: moveInput.x * moveSpeed, _rigidbody2D.velocity.y);
        _rigidbody2D.velocity = moveVelocity;
        bool playerHasHorizontalSpeed = Mathf.Abs(moveInput.x) > Mathf.Epsilon;
        _animator.SetBool(name: "isRunning", playerHasHorizontalSpeed);
    }
    // abs : giá trị tuyệt đối
    // sign : đấu của giá trị
    // epsion : giá trị nhỏ nhất có thể so sánh
    //xoay hướng nhân vật theo chuyển động
    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(_rigidbody2D.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(x: Mathf.Sign(_rigidbody2D.velocity.x), y: 1f);
        }
    }
    //leo thang
    void climbLadder()
    {
        var isTouchingLadder = _capsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Climbing"));
        if (!isTouchingLadder)
        {
            _animator.SetBool("isClimbing", false);
            _rigidbody2D.gravityScale = gravityScaleAtStart;
            return;
        }
        var climbVelocity = new Vector2(_rigidbody2D.velocity.x, y: moveInput.y * climbSpeed);
        _rigidbody2D.velocity = climbVelocity;

        //điểu khiển animation cầu thang
        var playerHasVerticalSpeed = Mathf.Abs(moveInput.y) > Mathf.Epsilon;
        _animator.SetBool("isClimbing", true);
        //tắt gravity
        _rigidbody2D.gravityScale = 0;
    }
    //private void Fire()
    //{
    //    if (Input.GetKeyDown(KeyCode.F))
    //    {
    //        //tạo viên đạn tại vị trí gun
    //        var oneBullet = Instantiate(_bulletPrefabs, _gunTransform.position, Quaternion.identity);
    //        //cho viên đạn bay theo hướng nhân vật
    //        if (isMovingRight == true)
    //        {
    //            oneBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(30f, 0);
    //            _animator.SetBool("isShooting", true);
    //        }
    //        else
    //        {
    //            oneBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(-30f, 0);

    //        }

    //        Destroy(oneBullet, 1f);
    //        _animator.SetBool("isShooting", false);



    //    }

    //}
    void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.F)) // Khi nhấn phím F
        {
            _animator.SetTrigger("isShooting"); // Kích hoạt animation bắn cung
            Invoke(nameof(FireArrow), 0.1f); // Gọi hàm FireArrow sau một khoảng thời gian ngắn để khớp với animation
        }
    }


    void FireArrow()
    {
        if (bowPosition == null) // Kiểm tra bowPosition đã được gán chưa
        {
            Debug.LogError("Bow Position chưa được gán trong Inspector.");
            return;
        }

        GameObject arrow = Instantiate(arrowPrefab, bowPosition.position, Quaternion.identity); // Tạo mũi tên từ prefab

        Rigidbody2D arrowRb = arrow.GetComponent<Rigidbody2D>(); // Lấy thành phần Rigidbody2D của mũi tên

        arrowRb.velocity = new Vector2(transform.localScale.x * arrowSpeed, 0); // Điều chỉnh hướng bay của mũi tên dựa trên hướng của nhân vật

        // Kiểm tra hướng của nhân vật và flip mũi tên nếu cần
        Vector3 arrowScale = arrow.transform.localScale;
        if (transform.localScale.x < 0)
        {
            arrowScale.x = -Mathf.Abs(arrowScale.x); // Flip mũi tên nếu nhân vật đang quay về bên trái
        }
        else
        {
            arrowScale.x = Mathf.Abs(arrowScale.x); // Đảm bảo mũi tên không bị flip nếu nhân vật đang quay về bên phải
        }
        arrow.transform.localScale = arrowScale;

        _animator.SetTrigger("isStaying"); // Đảm bảo nhân vật trở về trạng thái đứng im

        Destroy(arrow, arrowLifetime); // Hủy mũi tên sau khoảng thời gian
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
            //phát ra tiếng ăn đồng xu 
            AudioSource.PlayClipAtPoint(_coinCollectSFX, Vector3.zero);
            // cộng điểm 
            _score += v;
            _scoreText.text = _score.ToString();

        }


    }
}
