using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : IUpgrade
{
    private int level;
    private static readonly Dictionary<int, float> topspeeds = new Dictionary<int, float>()
    {
        {0, 1.25f},
        {1, 1.75f},
        {2, 2.25f},
        {3, 3.00f}
    };
    public Engine()
    {
        level = 0;
    }
    public void LevelUp()
    {
        // increase level
        level = Mathf.Min(level + 1, 3);

        // apply levelup to player
        Player player = GameObject.FindObjectOfType<Player>();
        player.TopSpeed = topspeeds[level];
        Debug.Log("I got upgraded! my level is now: " + level);
    }
}
