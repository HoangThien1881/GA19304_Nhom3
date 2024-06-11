using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevel : MonoBehaviour
{
    public GameObject victory;

    private void Start()
    {
        if (victory == null)
        {
            Debug.LogError("Victory GameObject is not assigned.");
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player exited the trigger zone.");
            Time.timeScale = 0;
            if (victory != null)
            {
                victory.SetActive(true);
            }
            else
            {
                Debug.LogError("Victory GameObject is not assigned.");
            }
        }
    }
}