using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This component is for a charge blaster, which shoots a ray of death after a couple of seconds charging.
/// </summary>
public class ChargeBlaster : Gun
{
    [SerializeField] private PlayerMovement _playerMovement = null;

    // The knockback affecting the player per shot
    [SerializeField] private float _knockbackForce = 10f;

    // This modifier defines how slow the player becomes while charging the weapon
    [SerializeField] private float _movementModifierOnCharge = 0.1f;

    [SerializeField] private GameEvent _onStartCharging = null;
    [SerializeField] private GameEvent _onStopCharging = null;

    // this keeps track if the weapon is currently charging
    private bool _loadingShot = false;
    // this keeps track when the gun started charging
    private float _timeChargingStarted = 0;

    protected override bool HandleShot(Vector3 positionToShoot)
    {
        // Is the gun already charging?
        if (_loadingShot)
        {
            // Did the gun charge long enough?
            if(Time.time - _timeChargingStarted >= CurrentTimeBetweenShots)
            {
                _loadingShot = false;
                Instantiate(_bulletPre, _barrelExit.position, transform.rotation, _bulletContainer);
                return true;
            }
            return false;
        }

        // If we aren't charging the gun currently we start this frame
        StartCharging();
        return false;
    }

    private void OnDisable()
    {
        StopShooting();
    }

    /// <summary>
    /// This method initializes the charging of the charge blaster.
    /// </summary>
    private void StartCharging()
    {
        _onStartCharging.Raise();

        _loadingShot = true;
        _timeChargingStarted = Time.time;

        // We slow the players movement while charging the gun
        _playerMovement.ModifyMovement(_movementModifierOnCharge);
    }

    /// <summary>
    /// This method gets called when the player stops pressing the shooting button (usually LMB)
    /// This method interupts the charging process of the gun.
    /// </summary>
    protected override void OnStopShooting()
    {
        _loadingShot = false;

        // We reset the players movement since they are no longer charging
        _playerMovement.ResetMovement();

        _onStopCharging.Raise();
    }

    /// <summary>
    /// When shooting a charge blaster shot the player gets knocked back.
    /// </summary>
    protected override void OnShoot()
    {
        HandleKnockback();
    }

    private void HandleKnockback()
    {
        // we apply knockback based on -forward since we assume the player looks into the direction they shoot (which is forward)
        _playerMovement.AddKnockback(-transform.forward * _knockbackForce);
    }
}
