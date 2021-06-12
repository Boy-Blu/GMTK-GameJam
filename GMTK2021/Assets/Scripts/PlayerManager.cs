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
        
    // ------------------------------------------------------------
    // Methods Start here
    // ------------------------------------------------------------

    void Start(){
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
        p1.SetInactive();
        // Add delay
        foreach (GameObject collider in AirColliders) {
            if (collider.GetComponent<Collider2D> ()) {
                Physics2D.IgnoreCollision (collider.GetComponent<Collider2D> (), p1.GetComponent<Collider2D> (), true);
                Physics2D.IgnoreCollision (collider.GetComponent<Collider2D> (), InActivePlayer.GetComponent<Collider2D> (), false);
            }
        }
        yield return new WaitForSeconds(0.2f);

        InActivePlayer.SetActive();
        
        ActivePlayer = InActivePlayer;
        InActivePlayer = p1;
    }

    // ------------------------------------------------------------
    // Getters + Setters
    // ------------------------------------------------------------
}
