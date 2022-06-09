using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This upgrade increases the fire rate of all weapons.
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObjects/Upgrades/Weapon/FireRate")]
public class IncreaseFireRateUpgrade : WeaponUpgrade
{
    [SerializeField] private float _fireRateChange = 0.5f;
    [SerializeField] private float _minimumFireRate = 0.2f;

    public override void ActivateUpgrade()
    {
        base.ActivateUpgrade();

        // we loop through all weapons and adjust their firerate based on their current firerate
        foreach (Gun gun in _guns)
        {
            gun.CurrentTimeBetweenShots = Mathf.Max(_minimumFireRate, gun.CurrentTimeBetweenShots *= _fireRateChange);
        }
    }

    public override void DeactivateUpgrade()
    {
        // we loop through all weapons and reset their firerate
        foreach (Gun gun in _guns)
        {
            gun.ResetFireRate();
        }
    }
}
