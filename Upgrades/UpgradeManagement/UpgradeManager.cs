using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is the base class for all upgrade managers.
/// </summary>
/// <typeparam name="T">The Upgrade type the manager is supposed to manage</typeparam>
/// <typeparam name="U">The behaviour the upgrade uses/ needs to apply its upgrade. This is only used to ensure T is valid</typeparam>
public abstract class UpgradeManager<T, U> : MonoBehaviour where T : Upgrade<U>
{
    /// <summary>
    /// A collection of an active upgrade and the time it started.
    /// </summary>
    public struct UpgradeSet
    {
        public T _upgrade;
        public float _startTime;

        public UpgradeSet(T upgrade, float startTime)
        {
            _upgrade = upgrade;
            _startTime = startTime;
        }
    }

    private List<UpgradeSet> _activeUpgrades = new List<UpgradeSet>();

    /// <summary>
    /// This method adds and activates a collected upgrade.
    /// It also starts keeping track of said upgrade to deactivate it later.
    /// </summary>
    /// <param name="upgrade"></param>
    public void AddUpgrade(T upgrade)
    {
        // We start keeping track of the upgrade
        UpgradeSet newUpgrade = new UpgradeSet(upgrade, Time.time);
        _activeUpgrades.Add(newUpgrade);

        // We activate the upgrade
        newUpgrade._upgrade.ActivateUpgrade();
        OnAddUpgrade(newUpgrade);
    }

    /// <summary>
    /// Override this if an upgrade manager should have extra behaviour when activating an Upgrade.
    /// </summary>
    /// <param name="newUpgrade"></param>
    protected virtual void OnAddUpgrade(UpgradeSet newUpgrade) { }

    private void Update()
    {
        // Removes all Upgrades that have been in the List longer than their Upgrade Duration allows
        _activeUpgrades.RemoveAll((UpgradeSet set) => {
            
            bool willBeRemoved = Time.time - set._startTime > set._upgrade.UpgradeDuration;

            // we deactivate the upgrade before removing it
            if (willBeRemoved)
                set._upgrade.DeactivateUpgrade();

            return willBeRemoved;
        });
    }
}

