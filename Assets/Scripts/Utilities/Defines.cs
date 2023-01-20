using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defines
{
    public enum WorldObject
    {
        Unknown,
        Player,
        Monster
    }
    public enum MonsterType
    {
        Mutant = 1000,
        Warrock,
        Boss = 2000,
    }
    public enum UIEvent
    {
        Click,
        Drag,
    }
}
