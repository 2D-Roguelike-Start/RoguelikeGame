using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum Layer
    { 
       Enemy = 3,
       Floor =  6,
       Wall = 7,
       Player = 8,
       Platform = 9,
       AttackCollider = 10,
       NPC = 11,
       Projectile = 12,
    }

    public enum CreatureState
    {
        Idle,
        Moving,
        Attack,
        Skill,
        Die,
        //InjuredFront,
    }

    public enum Scene
    {
        Title,
        Tutorial,
    }


    public enum MouseEvent
    {
        Press,
        Click,
    }

    public enum UIEvent
    {
        Click,
        Drag,
    }

    public enum NPC
    {
        Tutorial,
    }

}


