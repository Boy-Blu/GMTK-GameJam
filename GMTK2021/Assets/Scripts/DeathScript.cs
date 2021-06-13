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

    private FMOD.Studio.EventInstance _deathAudio;

    private void Start()
    {
        _deathAudio = FMODUnity.RuntimeManager.CreateInstance("event:/sfx/die");
    }

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
            _deathAudio.start();
            Notify();   
            SetTriggered();
            //DeathScreen.SetActive(true);
            GameManager.Instance.TriggerDeathScreen();

            vfx.SendEvent("KillAll");
        }
    }

    ///<summary>
    /// Set triggered which ensure the method isn't ran twice
    ///</summary>
    void SetTriggered(){
        hasTriggered = true;
    }

    private void OnDestroy()
    {
        _deathAudio.release();
    }
}
