using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgradeManager : UpgradeManager<PlayerUpgrade, PlayerAccess>
{
    protected override void OnAddUpgrade(UpgradeSet newUpgrade)
    {
        newUpgrade._upgrade.ActivateUpgrade();
    }
}
