using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This upgrade changes the weapon the player has currently equipped.
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObjects/Upgrades/Player/WeaponChange")]
public class ChangeWeaponUpgrade : PlayerUpgrade
{
    // The weapon that gets equipped when picking up the upgrade
    [SerializeField] private GunTypes _gunTypes = GunTypes.Basic;

    public override void ActivateUpgrade()
    {
        base.ActivateUpgrade();
        _player.PlayerShooting.CurrentGun = _gunTypes;
    }

    public override void DeactivateUpgrade()
    {
        _player.PlayerShooting.CurrentGun = GunTypes.Basic;
    }
}
