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

    // Button Debounce
    private bool justSwapped;

    void Start(){
        _player = ReInput.players.GetPlayer (playerId);
        ActivePlayer.PlayerSetup(_player,true);
        InActivePlayer.PlayerSetup(_player,false);
    }

    // Update is called once per frame
    void Update()
    {
        // button press
        if(_player.GetButtonDown ("Switch") && InActivePlayer.CanConnect()){
            justSwapped = true;
            Debug.Log("Swapped");
            PlayerController p1 = ActivePlayer;
            p1.SetActive(false);
            InActivePlayer.SetActive(true);
            ActivePlayer = InActivePlayer;
            InActivePlayer = p1;

        }  else if (_player.GetButtonUp ("Switch")) {
            justSwapped = false;

        }    
    }
}
