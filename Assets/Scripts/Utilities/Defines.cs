using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defines
{
    public enum WorldObject
    {
        Unknown,
        Player,
        Monster,
        Boss
    }
    public enum MonsterType
    {
        Mutant = 1000,
        Warrock,
        Maw,
        Mutant_Boss = 2000,
        Warrock_Boss,
        Maw_Boss,
    }
    public enum UIEvent
    {
        Click,
        Drag,
        Enter
    }
}
