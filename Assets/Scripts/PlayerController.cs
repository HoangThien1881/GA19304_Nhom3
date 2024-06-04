using Cinemachine;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

// tên file: viết hoa chữ cái đầu của từng từ
// tên file == tên class
// tên file ko có dấu cách, ko có ký tự đặc biệt, ko có số
public class PlayerControls : MonoBehaviour
{
    // vận tốc chuyển động 
    // public, private, protected, internal, protected internal
    [SerializeField]
    private float moveSpeed = 5f; // 5m/s
                                  // Start is called before the first frame update
                                  // hàm chạy 1 lần duy nhất khi game bắt đầu
                                  // dùng để khởi tạo giá trị

    // biến kiểm tra hướng di chuyển
    [SerializeField]
    private bool _isMovingRight = true;

    // tham chiếu đến rigidbody2D
    private Rigidbody2D _rigidbody2D;
    // giá trị của lực nhảy
    [SerializeField]
    private float _jumpForce = 20f;

    // tham chiếu đến collider2D
    private CapsuleCollider2D _capsuleCollider2D;
    // tham chiếu đến animator
    private Animator _animator;
    //tham chiếu đến file âm thanh
    [SerializeField]
    private AudioClip _coinCollectSXF; // file âm thanh
    private AudioSource _audioSource; // nguồn phát âm thanh
    // tham chiếu text mesh pro để hiển thị điểm
    [SerializeField]
    private TextMeshProUGUI _ScoreText;
    private static int _score = 0;

    // tham chieu den panel game over       
    [SerializeField] private GameObject _gameOverPanel;
    private static int _lives = 3;
    [SerializeField]
    private TextMeshProUGUI _LivesText;
    [SerializeField] private GameObject[] _liveImages;
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

        // hiển thị điểm
        _ScoreText.text = _score.ToString();
        // hien thi mang
        _LivesText.text = _lives.ToString();
        // hien thi hinh cua mang
        for (int i = 0; i < 3; i++)
        {
            if (i < _lives)
            {
                _liveImages[i].SetActive(true);
            }
            else
            {
                _liveImages[i].SetActive(false);
            }
        }
    }

    //Tao tham hieu den vien dan va sung
    [SerializeField]
    private GameObject _bulletPrefab;
    [SerializeField]
    private Transform _gun;

    // Update is called once per frame
    // cố gắng chạy max frame rate
    void Update()
    {
        Move();
        Jump();
        Fire();
    }

    //Ham xu li ban dan
    private void Fire()
    {
        //neu nguoi choi nhan phim F
        if (Input.GetKeyDown(KeyCode.F))
        {
            //tao ra vien dan tai vi tri sung
            var bullet = Instantiate(_bulletPrefab, _gun.position, Quaternion.identity);
            //cho vien dan bay theo huong mat cua nhan vat
            var velocity = new Vector3(30f, 0);
            if (_isMovingRight == false)
            {
                velocity.x *= -1;
            }
            bullet.GetComponent<Rigidbody2D>().velocity = velocity;
            //huy vien dan sau ?s
            Destroy(bullet, 0.5f);
        }
    }

    // xử lý điều khiển chuyển động ngang của nhân vật
    // Time.deltaTime: thời gian giữa 2 frame liên tiếp
    // 60fps: 1/60s = 0.0167s
    private void Move()
    {
        // left, right, a, d
        var horizontalInput = Input.GetAxis("Horizontal");
        // 0: không nhấn, âm: trái, dương: phải
        // điều khiển phải trái
        // x=1.5 ----> x=1.5+1=2.5
        transform.localPosition += new Vector3(horizontalInput, 0, 0)
            * moveSpeed * Time.deltaTime;
        // localPosition: vị trí tương đối so với cha
        // position: vị trí tuyệt đối so với thế giới
        if (horizontalInput > 0)
        {
            // qua phải
            _isMovingRight = true;
            _animator.SetBool("isRunning", true);
        }
        else if (horizontalInput < 0)
        {
            // qua trái
            _isMovingRight = false;
            _animator.SetBool("isRunning", true);
        }
        else
        {
            // đứng yên
            _animator.SetBool("isRunning", false);
        }
        // xoay nhân vật
        transform.localScale = _isMovingRight ?
            new Vector2(3f, 3f)
            : new Vector2(-3f, 3f);
    }

    private void Jump()
    {
        // kiểm tra nhân vật còn đang ở trên nền đất không
        var check = _capsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Platfrorm"));
        if (check == false)
        {
            return;
        }
        // var verticalInput = Input.GetAxis("Jump");
        var verticalInput = Input.GetKeyDown(KeyCode.Space) ? 1 : 0;
        if (verticalInput > 0)
        {
            // cach 1: cung cấp 1 lực đẩy lên trên
            // _rigidbody2D.AddForce(new Vector2(0, _jumpForce));
            // cach 2: dùng velocity
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _jumpForce);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Nếu chạm với xu
        if (other.gameObject.CompareTag("Coins"))
        {
            // biến mất xu
            Destroy(other.gameObject);
            // phát ra tiếng nhạc
            _audioSource.PlayOneShot(_coinCollectSXF);
            // tăng điểm
            _score += 1;
            // hiển thị điểm
            _ScoreText.text = _score.ToString();
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            // neu va cham voi quai
            _lives -= 1;
            // hien thi hinh cua mang
            for (int i = 0; i < 3; i++)
            {
                if (i < _lives)
                {
                    _liveImages[i].SetActive(true);
                }
                else
                {
                    _liveImages[i].SetActive(false);
                }
            }
            if (_lives > 0)
            {
                // reload game tai man choi hien tai
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                _LivesText.text = _lives.ToString();
            }
            else
            {
                // hien panel game over
                _gameOverPanel.SetActive(true);
                // dung game
                Time.timeScale = 0;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            // cho khối item di chuyển lên trên một đoạn
            // sau đó đi xuống vị trí ban đầu
            // lấy vị trí hiện tại
            StartCoroutine(GoUp(other.gameObject));
            // lên trên một đoạn

        }
    }
    // đi lên 1 đoạn
    // List [1, 2, 3, 4, 5]
    IEnumerator GoUp(GameObject _gameObject)
    {
        // lấy vị trí hiện tại
        var currentPosition = _gameObject.transform.localPosition;
        // vị trí ban đầu
        var originalPosition = currentPosition;
        while (true)
        {
            currentPosition.y += 0.1f;
            _gameObject.transform.localPosition = currentPosition;
            if (currentPosition.y > originalPosition.y + 1.5f)
            {
                break;
            }
            yield return null;
        }
        StartCoroutine(GoDown(_gameObject));
    }
    IEnumerator GoDown(GameObject _gameObject)
    {
        // lấy vị trí hiện tại
        var currentPosition = _gameObject.transform.localPosition;
        // vị trí ban đầu
        var originalPosition = currentPosition;
        while (true)
        {
            currentPosition.y -= 0.1f;
            _gameObject.transform.localPosition = currentPosition;
            if (currentPosition.y < originalPosition.y - 1.5f)
            {
                break;
            }
            yield return null;
        }
        // hiện ra item secret
        _gameObject.transform.GetChild(0).gameObject.SetActive(true);
        // ẩn item hiện tại
        _gameObject.GetComponent<SpriteRenderer>().enabled = false;

    }
    // lấy điểm số
    public int GetScore()
    {
        return _score;
    }
}

