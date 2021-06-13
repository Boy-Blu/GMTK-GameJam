using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField] private GameManager GameManager = null;

    [SerializeField] private PauseMenu PauseMenu = null;
    [SerializeField] private Animator PlayerUIAnimator = null;
    
    public Image BlackOverlayImage = null;

    public Action OnFadeToBlackAnimCompleted = null;
    public Action OnFadeFromBlackAnimCompleted = null;

    private void Awake()
    {
        if (GameManager == null)
        {
            GameManager = GameManager.Instance;
        }

        if (!PauseMenu.Paused)
        {
            PauseMenu.TogglePauseMenu(false);
        }
    }

    public void FadeToBlack()
    {
        PlayerUIAnimator.SetTrigger("FadeToBlack");
    }

    public void FadeFromBlack()
    {
        PlayerUIAnimator.SetTrigger("FadeFromBlack");
    }

    // Called/triggered by Animation Event
    public void FadeToBlackAnimCompleted()
    {
        OnFadeToBlackAnimCompleted?.Invoke();
        OnFadeToBlackAnimCompleted = null;
    }

    // Called/triggered by Animation Event
    public void FadeFromBlackAnimCompleted()
    {
        OnFadeFromBlackAnimCompleted?.Invoke();
        OnFadeFromBlackAnimCompleted = null;
    }

    public void TogglePauseMenu()
    {
        PauseMenu.TogglePauseMenu();
    }

    public void ShowDeathScreen()
    {
        PlayerUIAnimator.SetTrigger("ShowDeathScreen");
    }
}
