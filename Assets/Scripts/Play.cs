using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Play : MonoBehaviour
{
    // Start is called before the first frame update
  

    // Update is called once per frame
    

    public void PlayGame()
    {
        SceneManager.LoadScene("Thien_Scene1");
        Time.timeScale = 1;
    }

}
