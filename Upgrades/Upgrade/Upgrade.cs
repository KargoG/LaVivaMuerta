using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The base class for all upgrades
/// </summary>
/// <typeparam name="T">The component the upgrade uses to get applied</typeparam>
public abstract class Upgrade<T> : ScriptableObject
{
    [SerializeField] private float _upgradeDuration = 5f;
    public float UpgradeDuration { get { return _upgradeDuration; } protected set { _upgradeDuration = value; } }

    /// <summary>
    /// Override this method to define what happens when an Upgrade gets activated
    /// </summary>
    public abstract void ActivateUpgrade();

    /// <summary>
    /// Override this method to define what happens when an Upgrade gets deactivated
    /// </summary>
    public abstract void DeactivateUpgrade();

}

/// <summary>
/// The base class for all weapon upgrades.
/// This means upgrades FOR a weapon, NOT upgrades that CHANGE OR REPLACE a weapon
/// </summary>
public abstract class WeaponUpgrade : Upgrade<Gun>
{
    // all guns in the scene
    protected List<Gun> _guns = null;

    /// <summary>
    /// When a weapon upgrade is activated this method ensures that we have access to all weapons currently in the scene.
    /// Override this method to define what happens when a Weapon Upgrade gets activated
    /// </summary>
    public override void ActivateUpgrade()
    {
        if (_guns == null)
            _guns = new List<Gun>();
        _guns.Clear();
        _guns.AddRange(FindObjectsOfType<Gun>(true));
    }
}

/// <summary>
/// The base class for all bullet upgrades.
/// </summary>
public abstract class BulletUpgrade : Upgrade<BulletAccess>, IGameEventListener
{
    // This event gets Raised every time a bullet is spawned
    [SerializeField] private GameEvent _event = null;

    // a list of all bullets in the scene
    protected List<BulletAccess> _bullets = null;

    /// <summary>
    /// When a weapon upgrade is activated this method ensures that we have access to all weapons currently in the scene.
    /// Override this method to define what happens when a Bullet Upgrade gets activated
    /// </summary>
    public override void ActivateUpgrade()
    {
        if (_bullets == null)
            _bullets = new List<BulletAccess>();

        _bullets.Clear();
        _bullets.AddRange(FindObjectsOfType<BulletAccess>(true)); 
        _event.RegisterListener(this);
    }

    public override void DeactivateUpgrade()
    { _event.UnregisterListener(this); }

    /// <summary>
    /// This method is called every time a bullet is shot.
    /// It reapplies the upgrade on all bullets.
    /// </summary>
    public void OnEventRaised()
    {
        _bullets.Clear();
        _bullets.AddRange(FindObjectsOfType<BulletAccess>());
        ReapplyUpgrade();
    }

    /// <summary>
    /// Override this method to define how to reapply an upgrade on all bullets in a scene
    /// </summary>
    public abstract void ReapplyUpgrade();
}

/// <summary>
/// The base class for all player upgrades.
/// Upgrades that affect the player are for example weapon changes.
/// </summary>
public abstract class PlayerUpgrade : Upgrade<PlayerAccess>
{
    protected PlayerAccess _player = null;

    public override void ActivateUpgrade()
    {
        _player = FindObjectOfType<PlayerAccess>(true);
    }
}
