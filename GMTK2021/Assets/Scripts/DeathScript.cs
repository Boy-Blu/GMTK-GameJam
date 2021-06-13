using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public sealed class DeathScript : MonoBehaviour
{

    // For Observer pattern
    public delegate void PlayerDeath();
    public static event PlayerDeath Notify;

    private static bool hasTriggered = false;

    public GameObject DeathScreen;
    public VisualEffect vfx;


    void OnEnable()
    {
        hasTriggered = false;
        GateScript.Notify += SetTriggered;
    }

    void OnDisable()
    {
        GateScript.Notify -= SetTriggered;
    }

    void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.tag == TagNames.PLAYER_TAG && !hasTriggered){
            Notify();   
            SetTriggered();
            DeathScreen.SetActive(true);

            vfx.SendEvent("KillAll");
        }
    }

    ///<summary>
    /// Set triggered which ensure the method isn't ran twice
    ///</summary>
    void SetTriggered(){
        hasTriggered = true;
    }
}
