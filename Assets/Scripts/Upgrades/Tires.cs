using System.Collections.Generic;
using UnityEngine;

public class Tires : IUpgrade
{
    private int level;
    private static readonly Dictionary<int, float> maxRotations = new Dictionary<int, float>()
    {
        {0, 2.25f},
        {1, 2.75f},
        {2, 3.25f},
        {3, 4.00f}
    };
    public Tires()
    {
        level = 0;
    }
    public int LevelUp()
    {
        // increase level
        level = Mathf.Min(level + 1, 3);

        // apply levelup to player
        Player player = GameObject.FindObjectOfType<Player>();
        player.RotationalConstant = maxRotations[level];
        Debug.Log("I got upgraded! my " + this.GetType() + " level is now: " + level);
        return level;
    }
}
