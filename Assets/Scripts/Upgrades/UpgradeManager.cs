using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{

    public enum Upgrade
    {
        ENGINE,
        SEATBELT,
        TIRES,
        CHASSIS
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
            upgrades.Add(Upgrade.SEATBELT, new Seatbelt());
            upgrades.Add(Upgrade.TIRES, new Tires());
            upgrades.Add(Upgrade.CHASSIS, new Chassis());
        }
    }

    public void LevelUpUpgrade(Upgrade upgrade)
    {
        OnLevelUp(new LevelUpEventArgs(upgrade, upgrades[upgrade].LevelUp()));
    }

    public event EventHandler<LevelUpEventArgs> LeveledUp;

    public class LevelUpEventArgs : EventArgs
    {
        public Upgrade upgrade;
        public int level;

        public LevelUpEventArgs(Upgrade u, int l)
        {
            this.upgrade = u;
            this.level = l;
        }
    }
    public void OnLevelUp(LevelUpEventArgs e)
    {
        if (LeveledUp != null)
        {
            LeveledUp(this, e);
        }
    }

}
