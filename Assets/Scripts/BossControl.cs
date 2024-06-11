using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;




public class BossControl : MonoBehaviour
{
    private float _blood = 500f;
    [SerializeField] private Slider _bloodSlider;
    [SerializeField] private ParticleSystem _explosionPS;
    [SerializeField] private float fspeed = 1f;
    [SerializeField] private float leftBoundary = 70;
    [SerializeField] private float rightBoundary = 92;
    //giả sử quái vật đang di chuyển sang phải
    private bool _isMovingRight = true;

    //private int facingDir = -1;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        _bloodSlider.value = 100f;
    }

    // Update is called once per frame
    void Update()
    {
        var x_enemy = transform.position.x;
        var y_enemy = transform.position.y;
        var x_player = player.transform.position.x;
        var y_player = player.transform.position.y;
        //if ((x_player > leftBoundary && x_player < rightBoundary)) //trong vùng tuần tra
        //{
        //    if (x_player < x_enemy)
        //        _isMovingRight = true;
        //    if (x_player > x_enemy)
        //        _isMovingRight = false;
        //}
        //lấy vị trí của quái vật
        var currentPostion = transform.localPosition;
        if (currentPostion.x > rightBoundary)
        {
            //nếu vị trí hiện tại của quái vật > rightboundary
            //di chuyển sang trái
            _isMovingRight = false;
        }
        else if (currentPostion.x < leftBoundary)
        {
            //nếu vị trí hiện tại của quái vật > leftboundary
            //di chuyển sang phải
            _isMovingRight = true;
        }
        //di chuyển ngang
        // (1,0,0) * 1 * 0.02 = (0.02 , 0 , 0)
        var direction = Vector3.right;
        if (_isMovingRight == false)
        {
            direction = Vector3.left;
            transform.localScale = new Vector3(-10, 10);

        }
        else
            transform.localScale = new Vector3(10, 10);
        //var diretion = _ismovingright ? vector 3.right. : vector.left;

        transform.Translate(direction * fspeed * Time.deltaTime);




    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject); 
            _blood -= 20f;
            _bloodSlider.value = _blood;
        }
        if (_blood <= 0)
        {
            Destroy(gameObject);
            var ps = Instantiate(_explosionPS, gameObject.transform.localPosition, Quaternion.identity);
        }
        
    }

}
