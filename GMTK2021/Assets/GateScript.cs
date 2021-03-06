using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.SceneManagement;


public class GateScript : MonoBehaviour
{
    public string NextLevel;

    // For Observer pattern
    public delegate void PlayerWin();
    public static event PlayerWin Notify;

    public VisualEffect vfx;

    public bool isLight;
    public bool canChange;
    private Animator animator;

    void Start(){
        animator = GetComponent<Animator> ();
        ApplyFx();
    }

    void OnEnable()
    {
        animator = GetComponent<Animator> ();
        hasTriggered = false;
        DeathScript.Notify += SetTriggered;
        PlayerManager.Notify += ChangeFX;
    }

    void OnDisable()
    {
        DeathScript.Notify -= SetTriggered;
        PlayerManager.Notify -= ChangeFX;
    }

    private static bool hasTriggered = false;

    void OnTriggerEnter2D(Collider2D col){
        // TODO: Add proper check for colour
        if(col.gameObject.tag == TagNames.PLAYER_TAG && !hasTriggered){
            Notify();   
            SetTriggered();
            LoadLevel();
            vfx.SendEvent("KillAll");
        }
    }

    ///<summary>
    /// Set triggered which ensure the method isn't ran twice
    ///</summary>
    void SetTriggered(){
        hasTriggered = true;
    }


    ///<summary>
    /// Checks if we can change the colour of the portal
    ///</summary>
    void ChangeFX(){
        if(!canChange){
            return;
        }
        isLight = !isLight;
        ApplyFx();
    }

    ///<summary>
    /// Apply Changes to the Portal
    ///</summary>
    void ApplyFx(){
        animator = GetComponent<Animator> ();
        animator.SetBool("isLight", isLight);
        vfx.SendEvent(isLight? "StartLight": "StartDark");
    }

    void LoadLevel()
    {
        GameManager.Instance.StartLevelLoad(NextLevel);
    }
}
