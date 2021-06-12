using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class DeathScript : MonoBehaviour
{

    // For Observer pattern
    public delegate void PlayerDeath();
    public static event PlayerDeath Notify;

    private static bool hasTriggered = false;

    public GameObject DeathScreen;

    void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.tag == TagNames.PLAYER_TAG && !hasTriggered){
            Notify();   
            SetTriggered();
            DeathScreen.SetActive(true);
        }
    }

    ///<summary>
    /// Set triggered which ensure the method isn't ran twice
    ///</summary>
    void SetTriggered(){
        hasTriggered = true;
    }
}
