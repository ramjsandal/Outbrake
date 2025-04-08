using System.Collections.Generic;
using UnityEngine;

public class Chassis : IUpgrade
{
    private int level;
    private static readonly Dictionary<int, float> maxReductions = new Dictionary<int, float>()
    {
        {0, 1.00f},
        {1, 0.90f},
        {2, 0.80f},
        {3, 0.70f}
    };
    public Chassis()
    {
        level = 0;
    }
    public void LevelUp()
    {
        // increase level
        level = Mathf.Min(level + 1, 3);

        // apply levelup to player
        Player player = GameObject.FindObjectOfType<Player>();
        player.DamageReduction = maxReductions[level];
        Debug.Log("I got upgraded! my " + this.GetType() + " level is now: " + level);
    }
}
