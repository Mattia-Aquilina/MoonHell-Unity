using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToothMonster : EnemyUnitBase
{
    private void Update()
    {
        base.Update();
        Movement();
    }
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
        NavMeshAgent.destination = GameManager.Instance.HeroInstance.Position;
    }

    public void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.GetComponentInParent<HeroBase>() == null) return;
            collision.gameObject.GetComponentInParent<HeroBase>().TakeDamage(_enemyStats.damage);
    }
}
