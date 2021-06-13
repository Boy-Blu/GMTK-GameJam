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

    public bool DeathScreenActive => DeathScreen.activeSelf;

    public PlayerUIController PlayerUIController = null;
    [SerializeField] private string MainMenuScene = null;
    [SerializeField] private GameObject DeathScreen = null;
    private static GameManager _instance;
    private string _levelToLoad = null;
    private string _currentLevel = null;
    private bool _isTransitioningLevels = false;

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

        DontDestroyOnLoad(this);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    /// <summary>
    /// Reload the menu to try again
    /// <summary>
    public void RetryMenu(){
        int scene = SceneManager.GetActiveScene().buildIndex;
         SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }

    public void TriggerDeathScreen()
    {
        DeathScreen.SetActive(true);
        PlayerUIController.ShowDeathScreen();
    }

    // Called via Unity Action
    public void RetryFromDeathScreen()
    {
        PlayerUIController.BlackOverlayImage.raycastTarget = true;
        PlayerUIController.OnFadeToBlackAnimCompleted += () => PlayerUIController.BlackOverlayImage.raycastTarget = false;
        PlayerUIController.OnFadeToBlackAnimCompleted += () => DeathScreen.SetActive(false);
        RestartLevel();
    }

    public void StartReturnToMenuFromDeathScreen()
    {
        PlayerUIController.OnFadeToBlackAnimCompleted += () => DeathScreen.SetActive(false);
        StartReturnToMenu();
    }

    public void StartLevelLoad(string sceneName)
    {
        _levelToLoad = sceneName;
        PlayerUIController.OnFadeToBlackAnimCompleted += LoadNextLevel;
        PlayerUIController.FadeToBlack();
    }

    public void LoadNextLevel()
    {
        _isTransitioningLevels = true;
        SceneManager.LoadScene(_levelToLoad);
    }

    public void RestartLevel()
    {
        StartLevelLoad(_currentLevel);
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Loaded {scene.name}");
        _currentLevel = scene.name;
        if (_isTransitioningLevels)
        {
            PlayerUIController.FadeFromBlack();
        }
    }

    public void TogglePauseMenu()
    {
        PlayerUIController.TogglePauseMenu();
    }

    public void StartReturnToMenu()
    {
        PlayerUIController.OnFadeToBlackAnimCompleted += ExitToMenu;
        PlayerUIController.FadeToBlack();
    }

    private void ExitToMenu()
    {
        _isTransitioningLevels = true;
        SceneManager.LoadScene(MainMenuScene);
    }
}
