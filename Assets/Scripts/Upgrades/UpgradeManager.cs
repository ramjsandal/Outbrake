using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{

    public enum Upgrade
    {
        ENGINE
    };
    private static UpgradeManager _instance;
    public static UpgradeManager Instance { get { return _instance; } }

    private Dictionary<Upgrade, IUpgrade> upgrades;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        } else
        {
            _instance = this;
            upgrades = new Dictionary<Upgrade, IUpgrade>();
            upgrades.Add(Upgrade.ENGINE, new Engine());
        }
    }

    public void LevelUpUpgrade(Upgrade upgrade)
    {
        upgrades[upgrade].LevelUp();
    }

}
