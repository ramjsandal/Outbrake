using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUpgrade
{
    // Levels up the upgrade and returns the new level
    public int LevelUp();
}
