using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool Paused => _paused;
    
    [SerializeField] private Animator animator = null;
    private static bool _paused = false;
    private bool _isBusy = false;

    private void Start()
    {
        DeathScript.Notify += () =>
        {
            Debug.Log("Notified?");
            TogglePauseMenu(false);
        };
    }


    public void TogglePauseMenu(bool openMenu)
    {
        if (_isBusy)
            return;

        this.gameObject.SetActive(openMenu);
        _paused = openMenu;

        Time.timeScale = _paused ? 0 : 1;
    }

    public void TogglePauseMenu()
    {
        TogglePauseMenu(!_paused);
    }

    // Called via Unity Action
    public void RestartLevel()
    {
        TogglePauseMenu(false);
        _isBusy = true;
        GameManager.Instance.PlayerUIController.OnFadeToBlackAnimCompleted += () => _isBusy = false;    // Disables pause menu while fading to black/restarting level
        GameManager.Instance.RestartLevel();
    }

    // Called via Unity At
    public void ExitToMainMenu()
    {
        TogglePauseMenu(false);
        _isBusy = true;
        GameManager.Instance.StartReturnToMenu();
    }
}
