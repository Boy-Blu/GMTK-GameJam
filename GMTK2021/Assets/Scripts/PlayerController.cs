﻿using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;
using Rewired.ControllerExtensions;

public class PlayerController : MonoBehaviour
{

    // Movement stuff
    private Player _player;
    [Tooltip("Player Controller Rigid Body")]
    [SerializeField] private Rigidbody2D _rb;

    [Tooltip("Player Controller Collider")]
    [SerializeField] private Collider2D _collider;

    [Tooltip("Player Move Speed")]
    [SerializeField] private float moveSpeed = 5f;

    [Tooltip("Player Default Gravity (should be 1)")]
    [SerializeField] private float defaultGravityScale = 1f;

    // Check if player is active
    private bool isActive = false;

    private bool onClimbable = false;

    // ---------------------------------------------------------------------
    //Methods Stuff
    // ---------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Setup the controller
    /// </summary>
    /// <param name="player"> Rewired Player Object </param>
    /// <param name="active"> If the Player starts Active or not </param>
    public void PlayerSetup(Player player, bool active){
        if (active) 
            SetActive();
        else 
            SetInactive();
        _player = player;
    }

    // Update is called once per frame
    void Update()
    {
        // Only do stuff when active
        if(isActive){
            // Move the Player

        }
    }

    void FixedUpdate(){
        // Only do stuff when active
        if(isActive){
            // Move the Player
            float del = Time.deltaTime;
            Vector2 movement = new Vector2();
            movement.x = _player.GetAxis (RewiredNames.MOVE_X);
            movement.y = _player.GetAxis (RewiredNames.MOVE_Y);

            _rb.MovePosition(_rb.position + movement*moveSpeed*del);
        }
    }

    void OnTriggerEnter2D(Collider2D col){
        switch(col.gameObject.tag){
            case TagNames.CLIMBABLE_TAG:
                onClimbable = true;
                break;
        }
    }

    void OnTriggerExit2D(Collider2D col){
        switch(col.gameObject.tag){
            case TagNames.CLIMBABLE_TAG:
                onClimbable = false;
                break;
        }
    }


    // ---------------------------------------------------------------------
    // Getters and Setters
    // ---------------------------------------------------------------------

    /// <summary>
    /// Checks if the Player can attach to background
    /// <summary>
    public bool CanConnect(){
        return onClimbable;
    }

    /// <summary>
    /// Change isActive State
    /// <summary>
    public void SetActive(){
        isActive = true;
        _rb.gravityScale = 0.0f;
    }

    /// <summary>
    /// Change isActive State
    /// <summary>
    public void SetInactive(){
        _rb.gravityScale = defaultGravityScale;
        isActive = false;
    }
}
