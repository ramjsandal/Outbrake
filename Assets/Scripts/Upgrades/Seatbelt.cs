using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seatbelt : IUpgrade
{
    private int level;
    private static readonly Dictionary<int, int> maxHealths = new Dictionary<int, int>()
    {
        {0, 100},
        {1, 125},
        {2, 150},
        {3, 200}
    };
    public Seatbelt()
    {
        level = 0;
    }
    public void LevelUp()
    {
        // increase level
        level = Mathf.Min(level + 1, 3);

        // apply levelup to player
        Player player = GameObject.FindObjectOfType<Player>();
        player.health = maxHealths[level];
        Debug.Log("I got upgraded! my " + this.GetType() + " level is now: " + level);
    }
}
