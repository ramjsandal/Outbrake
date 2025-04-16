using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : IUpgrade
{
    private int level;
    private static readonly Dictionary<int, float> topspeeds = new Dictionary<int, float>()
    {
        {0, 1.75f},
        {1, 2.25f},
        {2, 2.75f},
        {3, 3.50f}
    };
    public Engine()
    {
        level = 0;
    }
    public int LevelUp()
    {
        // increase level
        level = Mathf.Min(level + 1, 3);

        // apply levelup to player
        Player player = GameObject.FindObjectOfType<Player>();
        player.TopSpeed = topspeeds[level];
        Debug.Log("I got upgraded! my " + this.GetType() + " level is now: " + level);
        return level;
    }
}
