using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defines
{
    public enum Scene
    {
        Unknown,
        Login,
        Lobby,
        Loading,
        Game01,
    }

    public enum WorldObject
    {
        Unknown,
        Player,
        Monster,
        Boss
    }
    
    public enum Sound
    {
        Bgm,
        Bgm01,
        Bgm02,
        Effect,
        MaxCount,
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
        BeginDrag,
        Drag,
        EndDrag,
        Drop,
        Enter,
        Exit
    }

    public enum ItemType
    {
        Helmat,
        Shoulder,
        Top,
        Bottom,
        Glove,
        Weapon,
    }

    public enum ItemClass
    {
        Rair,
        Epic,
        Legandary,
        Relics,
    }

    public enum NpcType
    {
        Enforce,
        Smith,
        Shop,
        Alchemy,
        Normal,
    }
}
