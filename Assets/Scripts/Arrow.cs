using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Arrow : MonoBehaviour
{
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            Destroy(collision.gameObject); // Tiêu diệt quái vật
    
        }
        Destroy(gameObject); // Tiêu diệt mũi tên
    }
}