using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;
using Rewired.ControllerExtensions;

public class PlayerController : MonoBehaviour
{

    // Movement stuff
    private Player _player;
    private Rigidbody2D _rb;

    public float moveSpeed = 5f;

    


    // Check if player is active
    private bool isActive = false;

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
        isActive = active;
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
            movement.x = _player.GetAxis ("MoveX");
            movement.y = _player.GetAxis ("MoveY");

            
            _rb.MovePosition(_rb.position + movement*moveSpeed*del);
        }
    }

    // ---------------------------------------------------------------------
    // Getters and Setters
    // ---------------------------------------------------------------------

    /// <summary>
    /// Checks if the Player can attach to background
    /// <summary>
    public bool CanConnect(){
        return true;
    }

    /// <summary>
    /// Change isActive State
    /// <summary>
    public void SetActive(bool state){
        isActive = state;
    }
}
