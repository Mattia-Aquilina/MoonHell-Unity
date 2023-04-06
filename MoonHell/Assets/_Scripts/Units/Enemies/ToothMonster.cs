using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToothMonster : EnemyUnitBase
{
   
    protected override void OnStateChanged(GameState newState)
    {
        //throw new System.NotImplementedException();
    }

    new void Awake()
    {
        base.Awake();
        NavMeshAgent.speed = stats.ms;
        NavMeshAgent.updateRotation = false;
        NavMeshAgent.updateUpAxis = false;
    }


    // Update is called once per frame

    protected override void Movement()
    {
        //var dir = GameManager.Instance.HeroInstance.Position - this.transform.position;

        //if (dir.magnitude > 0) this._moving = true;
        //else
        //{
        //    this._moving = false;
        //    rigidbody.velocity = Vector3.zero;
        //}
        //if (dir.x > 0)
        //    spriteRenderer.flipX = false;
        //if (dir.x < 0)
        //    spriteRenderer.flipX = true; 

        //rigidbody.velocity = dir.normalized * this.Stats.ms;

        //movimento utilizzando il pathfinding

        NavMeshAgent.destination = GameManager.Instance.HeroInstance.Position;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponentInParent<HeroBase>() == null) return;
            collision.gameObject.GetComponentInParent<HeroBase>().TakeDamage(0);
    }
}
