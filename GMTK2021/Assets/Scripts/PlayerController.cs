using System.Collections;
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

    [SerializeField] private RigidbodyType2D BodyTypeWhenActive;
    [SerializeField] private RigidbodyType2D BodyTypeWhenInactive;


    // Check if player is active
    private bool isActive = false;

    private bool onClimbable = false;

    private Animator animator;

    public bool isLight;

    #region Observer Pattern
    /// --------------------------------------------------------------------
    /// Observer Pattern Stuff
    /// --------------------------------------------------------------------
    void OnEnable()
    {
        animator = GetComponent<Animator> ();
        DeathScript.Notify += ApplyInactive;
        PlayerManager.Notify += ChangeActive;
    }

    void OnDisable()
    {
        DeathScript.Notify -= ApplyInactive;
        PlayerManager.Notify -= ChangeActive;
    }
    #endregion

    // ---------------------------------------------------------------------
    //Methods Stuff
    // ---------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator> ();
        animator.SetBool("isLight", isLight);

        _rb = gameObject.GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Setup the controller
    /// </summary>
    /// <param name="player"> Rewired Player Object </param>
    /// <param name="active"> If the Player starts Active or not </param>
    public void PlayerSetup(Player player, bool active){
        if (active) 
            StartCoroutine(SetActive());
        else 
            StartCoroutine(SetInactive());
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
        if(isActive && !PauseMenu.Paused){
            // Move the Player
            float del = Time.deltaTime;
            Vector2 movement = new Vector2();
            movement.x = _player.GetAxis (RewiredNames.MOVE_X);
            movement.y = _player.GetAxis (RewiredNames.MOVE_Y);


            animator.SetBool("isMoving", !(Mathf.Abs(movement.x) <= 0.1 && Mathf.Abs(movement.y) <= 0.1) );

            _rb.MovePosition(_rb.position + movement*moveSpeed*del);

            if(isLight) Debug.Log(_rb.mass);
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
    IEnumerator SetActive(){
        _rb.constraints =  RigidbodyConstraints2D.FreezeRotation;
        _rb.gravityScale = 0.0f;
        _rb.SetRotation(0f);
        _rb.bodyType = BodyTypeWhenActive;
        yield return new WaitForSeconds(0.2f);
        isActive = true;
        animator.SetBool("isActive", isActive);
    }

    /// <summary>
    /// Change isActive State
    /// <summary>
    IEnumerator SetInactive(){
        _rb.constraints =  RigidbodyConstraints2D.None;
        _rb.gravityScale = defaultGravityScale;
        _rb.bodyType = BodyTypeWhenInactive;
        yield return new WaitForSeconds(0.2f);
        isActive = false;
        animator.SetBool("isActive", isActive);
    }

    private void ApplyInactive(){
        StartCoroutine(SetInactive());
    }

    public void ChangeActive(){
        if(isActive){
            StartCoroutine(SetInactive());
            return; 
        }
        StartCoroutine(SetActive());
    }
}
