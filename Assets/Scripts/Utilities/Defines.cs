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
        Warrock
    }
    public enum UIEvent
    {
        Click,
        Drag,
    }
}
