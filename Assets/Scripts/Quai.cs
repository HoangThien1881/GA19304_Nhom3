using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Quai : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float positionLeft;
    [SerializeField] private float positionRight;
    private int moveDiretion = 5;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var tag = other.gameObject.tag;
        Debug.Log(tag);
        if (tag == "dan")
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * moveDiretion * moveSpeed * Time.deltaTime);
        Vector2 scale = transform.localScale;
        if (transform.position.x <= positionLeft)
        {
            moveDiretion = 1;
            scale.x = -5;
        }
        else if (transform.position.x >= positionRight)
        {
            moveDiretion = -1;
            scale.x = 5;
        }
        transform.localScale = scale;
    }
    public GameObject gameoverObiect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.SetActive(false);
            gameoverObiect.SetActive(true);
        }
        
    }


}
