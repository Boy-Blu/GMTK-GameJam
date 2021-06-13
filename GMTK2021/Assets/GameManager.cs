using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {
        get
        {
            return _instance;
        }
    }

    private static GameManager _instance;


    /// <summary>
    /// Quit the game to Main
    /// <summary>
    public void QuitToMain(){
        SceneManager.LoadScene("MainMenu");
    }

    // Start is called before the first frame update

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    /// <summary>
    /// Reload the menu to try again
    /// <summary>
    public void RetryMenu(){
        int scene = SceneManager.GetActiveScene().buildIndex;
         SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }
}
