using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    private Rigidbody2D _rigBody2D;
    void Start()
    {
        _rigBody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _rigBody2D.velocity = new Vector2(x: moveSpeed, y: 0);
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        moveSpeed *= -1;
        //xoay hướng
        transform.localScale = new Vector2(-(Mathf.Sign(_rigBody2D.velocity.x)), 1f);
    }
    
}
