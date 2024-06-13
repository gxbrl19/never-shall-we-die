using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Demo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("ReturnMenu", 6.1f);
    }

    void ReturnMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Scenes/MainMenu");
        BackgroundMusic.instance.ChangeMusic(BackgroundMusic.instance._nswdTheme);
    }
}
