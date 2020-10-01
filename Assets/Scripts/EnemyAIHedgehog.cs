using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAIHedgehog : EnemyAI
{
    public override void _direction(Vector2 force, float facing)
    {
        animator.SetFloat("Speed", Mathf.Abs(facing));
        //Debug.Log("hedgespeed:" + facing.ToString());
        Vector3 e = enemySprite.localScale;
        if(facing < 0.01f)
        {
            e.x = Mathf.Abs(e.x) * -1;   
        }
        else if (facing > 0.01f)
        {
            e.x = Mathf.Abs(e.x);
        }
        enemySprite.localScale = e;
    }
}
