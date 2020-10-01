using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAIBird : EnemyAI
{
    public override void _direction(Vector2 force, float facing)
    {
        animator.SetFloat("Speed", Mathf.Abs(force.magnitude));
        //Debug.Log("birdspeed:" + force.magnitude.ToString());
        Vector3 e = enemySprite.localScale;
        if(force.x >= 0.01f)
        {
            e.x = Mathf.Abs(e.x) * -1;
        }
        else if (force.x <= -0.01f)
        {
            e.x = Mathf.Abs(e.x);
        }
        enemySprite.localScale = e;
    }
}
