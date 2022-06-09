using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This component should be added to an object representing an Upgrade in the scene.
/// </summary>
/// <typeparam name="T">The Upgrade type this collectable object is supposed to represent</typeparam>
/// <typeparam name="U">The behaviour the upgrade uses/ needs to apply its upgrade. This is only used to ensure T is valid</typeparam>
public class CollectableUpgrade<T, U> : MonoBehaviour where T : Upgrade<U>
{
    // The scriptable object in the project defining the upgrade
    [SerializeField] private T _upgrade = null;

    // The upgrade manager responsible for the upgrade
    private UpgradeManager<T, U> _upgradeManager = null;

    void Start()
    {
        _upgradeManager = FindObjectOfType<UpgradeManager<T, U>>();
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerAccess player = other.GetComponent<PlayerAccess>();

        // only the player can pick up upgrades
        if (player == null)
            return;

        _upgradeManager.AddUpgrade(_upgrade);
        Destroy(gameObject);
    }
}
