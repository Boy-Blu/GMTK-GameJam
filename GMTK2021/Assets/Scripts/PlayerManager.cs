using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;
using Rewired.ControllerExtensions;

public class PlayerManager : MonoBehaviour
{
    [Tooltip("Id of player in Rewired")]
    public int playerId;
    private Player _player;

    [Tooltip("Player Who Starts Active")]
    [SerializeField] private PlayerController ActivePlayer;
    [Tooltip("Player Who Starts inactive")]
    [SerializeField] private PlayerController InActivePlayer; 

    private GameObject[] AirColliders; 

    // Button Debounce
    private bool justSwapped;

    public delegate void PlayerSwap();
    public static event PlayerSwap Notify;

    private FMOD.Studio.EventInstance _darkSwapAudio;
    private FMOD.Studio.EventInstance _lightSwapAudio;
    private FMOD.Studio.EventInstance _darkCollideAudio;
    private FMOD.Studio.EventInstance _lightCollideAudio;
    // ------------------------------------------------------------
    // Methods Start here
    // ------------------------------------------------------------

    void Start(){
        _darkSwapAudio = FMODUnity.RuntimeManager.CreateInstance("event:/sfx/swapDark");
        _lightSwapAudio = FMODUnity.RuntimeManager.CreateInstance("event:/sfx/swapLight");
        _darkCollideAudio = FMODUnity.RuntimeManager.CreateInstance("event:/sfx/darkCollide");
        _lightCollideAudio = FMODUnity.RuntimeManager.CreateInstance("event:/sfx/lightCollide");

        _player = ReInput.players.GetPlayer (playerId);
        ActivePlayer.PlayerSetup(_player,true);
        InActivePlayer.PlayerSetup(_player,false);

        AirColliders = GameObject.FindGameObjectsWithTag (TagNames.AIR_TAG);

        foreach (GameObject collider in AirColliders) {
            if (collider.GetComponent<Collider2D> ()) {
                Physics2D.IgnoreCollision (collider.GetComponent<Collider2D> (), InActivePlayer.GetComponent<Collider2D> (), true);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (_player.GetButtonDown(RewiredNames.PAUSE) && !GameManager.Instance.DeathScreenActive)
        {
            GameManager.Instance.TogglePauseMenu();
        }

        if (PauseMenu.Paused)
            return;

        // button press
        if(_player.GetButtonDown (RewiredNames.SWITCH) && InActivePlayer.CanConnect()){
            justSwapped = true;
            StartCoroutine(SwapPlayers());
        }  else if (_player.GetButtonUp (RewiredNames.SWITCH)) {
            justSwapped = false;
        }
    }

    /// <summary>
    /// Swaps the Players
    /// <summary>
    IEnumerator  SwapPlayers(){
        Debug.Log("Swapped");
        
        PlayerController p1 = ActivePlayer;

        if (p1.isLight)
            _lightSwapAudio.start();
        else
            _darkSwapAudio.start();

        Notify();
        // Add delay
        foreach (GameObject collider in AirColliders) {
            if (collider.GetComponent<Collider2D> ()) {
                Physics2D.IgnoreCollision (collider.GetComponent<Collider2D> (), p1.GetComponent<Collider2D> (), true);
                Physics2D.IgnoreCollision (collider.GetComponent<Collider2D> (), InActivePlayer.GetComponent<Collider2D> (), false);
            }
        }
        ActivePlayer = InActivePlayer;
        InActivePlayer = p1;
        yield return new WaitForSeconds(0.2f);

    }

    private void OnDestroy()
    {
        _darkCollideAudio.release();
        _darkSwapAudio.release();
        _lightCollideAudio.release();
        _lightSwapAudio.release();
    }
    // ------------------------------------------------------------
    // Getters + Setters
    // ------------------------------------------------------------
}
