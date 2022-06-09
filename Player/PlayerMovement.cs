using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 10;
    [SerializeField] private float _rollSpeed = 10;
    [SerializeField] private float _rollDuration = 1f;

    // The physics layer the player is put in when rolling
    [SerializeField] private string _playerRollLayerName = "";

    // The amount the knockback effecting the player is reduced per second (in percent)
    [SerializeField] private float _knockbackRecoverRatePerSec = 0.2f;
    
    private CharacterController _cc = null;
    private Vector3 _knockbackVelocity = Vector3.zero;
    private float _movementSpeedModifier = 1;
    public Vector3 MovementInput { get; private set; } = Vector3.zero;

    public bool IsRolling { get; private set; } = false;
    private float _timeOfRollStart = 0;
    private Vector3 _rollDirection = Vector3.zero;

    // The physics Layer the player is normaly using
    private int _playerDefaultLayer = 0;
    // The physics layer the player is using when rolling
    private int _playerRollLayer = 0;

    private bool _shouldRoll = false;

    private void Awake()
    {
        _cc = GetComponent<CharacterController>();
        _playerDefaultLayer = gameObject.layer;
        _playerRollLayer = LayerMask.NameToLayer(_playerRollLayerName);
    }

    private void Update()
    {
        HandleInput();
    }

    /// <summary>
    /// This method catches all input in a given frame to use it in other methods
    /// </summary>
    private void HandleInput()
    {
        // We dont care about ingame input if the game isn't running
        if (GameManager.Instance.CurrentGameState != GameState.Running)
            return;

        MovementInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (MovementInput.sqrMagnitude > 1)
            MovementInput.Normalize();

        if(!_shouldRoll && !IsRolling)
        {
            _shouldRoll = Input.GetButtonDown("Roll");
        }
    }

    private void FixedUpdate()
    {
        HandleRoll();
        UpdateKnockback();
        HandleMovement();
    }

    /// <summary>
    /// This method updates the knockback every frame, reducing it over time to 0;
    /// </summary>
    private void UpdateKnockback()
    {
        // If the game isn't running we don't want to update anything
        if (GameManager.Instance.CurrentGameState != GameState.Running)
            return;

        _knockbackVelocity -= _knockbackVelocity * _knockbackRecoverRatePerSec * Time.fixedDeltaTime;

        if (_knockbackVelocity.sqrMagnitude < 0.1f)
            _knockbackVelocity = Vector3.zero;
    }

    /// <summary>
    /// This method moves the player based on the input, modifiers and the applied knockback
    /// </summary>
    private void HandleMovement()
    {
        if (GameManager.Instance.CurrentGameState != GameState.Running)
            return;
        if (IsRolling)
            return;

        _cc.SimpleMove(MovementInput * _movementSpeed * _movementSpeedModifier + _knockbackVelocity);
    }

    /// <summary>
    /// This method manages the players rolling, by either trying to start it or updating it.
    /// </summary>
    private void HandleRoll()
    {
        if (GameManager.Instance.CurrentGameState != GameState.Running)
            return;

        if (IsRolling)
            UpdateRoll();
        
        StartRoll();
    }

    /// <summary>
    /// This method initiates the player rolling.
    /// </summary>
    private void StartRoll()
    {
        if (IsRolling) // can't start rolling if already rolling
            return;

        if (!_shouldRoll) // should roll is true if the player pressed the roll button the last update
            return;

        _shouldRoll = false;
        // player layer is updated to let player roll through defined elements
        gameObject.layer = _playerRollLayer;
        IsRolling = true;
        // start time will be needed to define the end of the roll
        _timeOfRollStart = Time.time;

        // direction is set at the start so the player can not change the direction mid roll
        _rollDirection = MovementInput;

        if (_rollDirection.sqrMagnitude > 1)
            _rollDirection.Normalize();
        if (_rollDirection.sqrMagnitude < 0.1f)
            _rollDirection = transform.forward;
    }

    /// <summary>
    /// This method handles the frame to frame elements of the rolling.
    /// This includes moving the player, as well as ending the roll.
    /// </summary>
    private void UpdateRoll()
    {
        // if the player has rolled for the defined time we end the role
        if(Time.time - _timeOfRollStart >= _rollDuration)
        {
            IsRolling = false;
            // The player layer gets reset, to reenable normal collisions
            gameObject.layer = _playerDefaultLayer;
            return;
        }

        _cc.SimpleMove(_rollDirection * _rollSpeed);
    }

    /// <summary>
    /// Call this method if the player should get knocked back.
    /// This usually happens when shooting weapons or when getting hit.
    /// </summary>
    /// <param name="knockbackVelocity">A vector giving the direction and strength of a knockback</param>
    public void AddKnockback(Vector3 knockbackVelocity)
    {
        _knockbackVelocity += knockbackVelocity;
    }

    /// <summary>
    /// This method allows to modify the players movement speed.
    /// This can be needed for upgrades.
    /// This method DOES NOT stack multipliers, but sets one.
    /// </summary>
    /// <param name="modifier">The modifier to set.</param>
    public void ModifyMovement(float modifier)
    {
        _movementSpeedModifier = modifier;
    }

    /// <summary>
    /// This method resets the players movement multiplier back to 1 .
    /// </summary>
    public void ResetMovement()
    {
        _movementSpeedModifier = 1;
    }
}
