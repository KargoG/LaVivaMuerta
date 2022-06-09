using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This component is for a simple gun, shooting one bullet every couple of seconds.
/// </summary>
public class BasicGun : Gun
{
    protected override bool HandleShot(Vector3 positionToShoot)
    {
        // If the time since the last shot isn't longer than the cooldown we return early
        if (Time.time - _timeOfLastShot < CurrentTimeBetweenShots)
            return false;

        _timeOfLastShot = Time.time; // Setting the time of the last shot for the cooldown to work

        GameObject newBullet = Instantiate(_bulletPre, _barrelExit.position, Quaternion.identity, _bulletContainer);

        // we set the y coordinate of our aimed at position to our barrel height to keep the shot on the xz-plane
        positionToShoot.y = _barrelExit.position.y;

        // We set the bullet direction by setting its forward vector
        newBullet.transform.forward = positionToShoot - _barrelExit.position;

        return true;
    }

    protected override void OnShoot() { }

}
