using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GunTypes
{
    Basic,
    Shotgun,
    Charge,
    End
}

/// <summary>
/// This component handles the player shooting with the different weapon at their disposal.
/// </summary>
public class PlayerShooting : MonoBehaviour
{
    // This list needs the guns from the player prefab to be in the right order.
    // The order is based on the order in the gun types enum.
    [SerializeField] private List<Gun> _guns = null;

    [SerializeField] private GunTypes _currentGun = GunTypes.Basic;
    public GunTypes CurrentGun { get { return _currentGun; }
        set {
            _guns[(int)_currentGun].gameObject.SetActive(false);
            _currentGun = value;
            _guns[(int)_currentGun].gameObject.SetActive(true);
        } }

    private PlayerAccess _playerAccess = null;

    private void Awake()
    {
        _playerAccess = GetComponent<PlayerAccess>();
    }

    private void Start()
    {
        InitializeGuns();
    }

    void Update()
    {
        HandleShooting();
    }

    private void InitializeGuns()
    {
        // The equiped gun is reset
        _currentGun = GunTypes.Basic;

        // All guns are set inactive, except for the basic gun of the player
        for (GunTypes i = 0; i < GunTypes.End; i++)
        {
            if (i == GunTypes.Basic)
            {
                _guns[(int)i].gameObject.SetActive(true);
                continue;
            }

            _guns[(int)i].gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// This method takes care of making the currently equiped gun shoot.
    /// The method also takes into account the current game state and player state.
    /// </summary>
    private void HandleShooting()
    {
        // We dont want to be shooting if we the game isn't running (for example if its paused)
        if (GameManager.Instance.CurrentGameState != GameState.Running)
        {
            if (_guns[(int)_currentGun].IsShooting)
                _guns[(int)_currentGun].StopShooting();
            return;
        }

        // When rolling the player can't shoot
        if (_playerAccess.PlayerMovement.IsRolling)
        {
            if (_guns[(int)_currentGun].IsShooting)
                _guns[(int)_currentGun].StopShooting();
            return;
        }

        // When the left mouse button is pressed we tell the equipped gun to start shooting
        if (Input.GetMouseButton(0))
            _guns[(int)_currentGun].Shoot(_playerAccess.PlayerAiming.PositionToAimAt);

        // When the left mouse button is released we tell the equipped gun to stop shooting
        if (Input.GetMouseButtonUp(0))
            _guns[(int)_currentGun].StopShooting();
    }
}
