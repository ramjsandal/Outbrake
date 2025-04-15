using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UpgradeManager;

public class UpgradeUI : MonoBehaviour
{

    public TMP_Text engineLevel;
    public TMP_Text chassisLevel;
    public TMP_Text seatbeltLevel;
    public TMP_Text tiresLevel;


    private UpgradeManager manager;
    void Start()
    {
        manager = UpgradeManager.Instance;
        manager.LeveledUp += UpdateUI;
    }


    private void UpdateUI(object sender, LevelUpEventArgs e)
    {
        int level = e.level;
        Upgrade upgrade = e.upgrade;

        switch (upgrade)
        {
            case Upgrade.ENGINE:
                engineLevel.text = level.ToString();
                break;
            case Upgrade.CHASSIS:
                chassisLevel.text = level.ToString();
                break;
            case Upgrade.SEATBELT:
                seatbeltLevel.text = level.ToString();
                break;
            case Upgrade.TIRES:
                tiresLevel.text = level.ToString();
                break;
            default:
                throw new System.Exception("WAAHHAHAHAHAHA INVALID UPGRADE");
        }
    }
}
