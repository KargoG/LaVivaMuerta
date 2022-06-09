using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Gun
{
    [SerializeField] protected Transform _secondBarrelExit = null;
    [SerializeField] private int _bulletsPerBarrel = 2;

    // The range bullets can be spread randomly per barrel in angles
    [SerializeField] private int _barrelSpread = 10;

    // The knockback affecting the player per shot
    [SerializeField] private float _knockbackForce = 10f;

    [SerializeField] private PlayerMovement _playerMovement = null;

    protected override bool HandleShot(Vector3 positionToShoot)
    {
        // If the time since the last shot isn't longer than the cooldown we return early
        if (Time.time - _timeOfLastShot < CurrentTimeBetweenShots)
            return false;

        _timeOfLastShot = Time.time; // Setting the time of the last shot for the cooldown to work

        positionToShoot.y = (_barrelExit.position.y + _secondBarrelExit.position.y)/2;

        Vector3 barrelOneDirection = positionToShoot - _barrelExit.position;
        Vector3 barrelTwoDirection = positionToShoot - _secondBarrelExit.position;

        // We call handle barrel Shots for every barrel
        HandleBarrelShots(_barrelExit.position, barrelOneDirection.normalized);
        HandleBarrelShots(_secondBarrelExit.position, barrelTwoDirection.normalized);

        return true;
    }

    /// <summary>
    /// This method handles shooting the bullets for a given barrel.
    /// The method slightly randomizes the direction of shot bullets based on the
    /// shot direction and the barrel spread
    /// </summary>
    /// <param name="barrelExit">The position to shoot bullets out of</param>
    /// <param name="shotDirection">The normal direction in which to shoot the bullets.</param>
    private void HandleBarrelShots(Vector3 barrelExit, Vector3 shotDirection)
    {
        for (int i = 0; i < _bulletsPerBarrel; i++)
        {
            // We randomize the bullet direction based on the given direction and the barrel spread
            Vector3 usedDirection = Quaternion.Euler(0, Random.Range(-_barrelSpread, _barrelSpread), 0) * shotDirection;

            GameObject newBullet = Instantiate(_bulletPre, barrelExit, Quaternion.identity, _bulletContainer);

            // We set the bullet direction by setting its forward vector
            newBullet.transform.forward = usedDirection;
        }
    }

    /// <summary>
    /// When shooting a shotgun shot the player gets knocked back.
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
