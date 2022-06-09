using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This upgrade lets all bullets in the game bounce off walls.
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObjects/Upgrades/Bullets/Bouncy")]
public class BouncingBulletUpgrade : BulletUpgrade
{
    public override void ActivateUpgrade()
    {
        base.ActivateUpgrade();

        foreach (BulletAccess bullet in _bullets)
        {
            if (!bullet)
                continue;

            bullet.BulletImpact.DestroyOnWall = false;
            bullet.BulletImpact.AddOnWallImpactListener(BounceOffWall);
        }
    }

    public override void ReapplyUpgrade()
    {
        foreach (BulletAccess bullet in _bullets)
        {
            // We remove the listener in case the listener is already applied
            bullet.BulletImpact.RemoveOnWallImpactListener(BounceOffWall);
            bullet.BulletImpact.DestroyOnWall = false;
            // We apply the listener to all bullets
            bullet.BulletImpact.AddOnWallImpactListener(BounceOffWall);
        }
    }

    public override void DeactivateUpgrade()
    {
        base.DeactivateUpgrade();
        foreach (BulletAccess bullet in _bullets)
        {
            bullet.BulletImpact.RemoveOnWallImpactListener(BounceOffWall);

            // We reset whether a bullet should get destroyed when comming in contact with a wall
            bullet.BulletImpact.ResetDestroyOnWall();
        }
    }

    /// <summary>
    /// This method changes the direction of a bullet to make it look, like it bounced off the wall.
    /// </summary>
    /// <param name="impact">The bullet that is bouncing off the wall</param>
    /// <param name="collision">The collision with the wall</param>
    public void BounceOffWall(BulletImpact impact, Collision collision)
    {
        impact.transform.forward = Vector3.Reflect(impact.transform.forward, collision.contacts[0].normal);
    }
}
