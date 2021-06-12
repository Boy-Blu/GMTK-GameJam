using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Quit the game to Main
    /// <summary>
    public void QuitToMain(){
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Reload the menu to try again
    /// <summary>
    public void RetryMenu(){
        int scene = SceneManager.GetActiveScene().buildIndex;
         SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }
}
