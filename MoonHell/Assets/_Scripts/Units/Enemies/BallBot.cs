using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBot : EnemyUnitBase
{
    // Start is called before the first frame update
    [SerializeField] Vector3 AttackRangeCenter;
    [SerializeField] Vector3 AttackRangeSize;
    private Vector3 AttackRangeCenterOpp; 
    [SerializeField] Vector3 playerDistance;
    private bool canAttack=true;

    private static readonly int Charge = Animator.StringToHash("Charge");
    [SerializeField] private float ChargeTime = .5f;
    //Ovveride delle ananimazioni

    new void Awake()
    {
        base.Awake();
        LoadStats();
        AttackRangeCenterOpp = Vector3.Scale(AttackRangeCenter, new(-1, 1, -1));
    }
    protected override void LoadStats()
    {
        base.LoadStats();
        ChargeTime = enemyData.GetSkillStats()[EnemySkillStats.ChannelTime];
    }
    // Update is called once per frame
    private void Update()
    {
        if (GameManager.Instance.isPaused) return;
        if (GameManager.Instance.State != GameState.Hunting) return;
        
        base.Update();

        playerDistance = (GameManager.Instance.HeroInstance.Position - spriteRenderer.gameObject.transform.position);

        if (Mathf.Abs(playerDistance.z) < AttackRangeSize.z && playerDistance.x < AttackRangeSize.x && canAttack)
            StartCoroutine(Attack());
        else 
            Movement();
    }

    private IEnumerator Attack()
    {
        canAttack = false;
        NavMeshAgent.enabled = false;
        ForceState(Charge, ChargeTime);
        yield return new WaitForSeconds(ChargeTime);

        _attacking = true;

        Collider[] hits;
        //primo swing dell'arma
        if (transform.localScale.x == 1)
            hits = Physics.OverlapBox(AttackRangeCenter + transform.position, AttackRangeSize, Quaternion.identity);
        else
            hits = Physics.OverlapBox(AttackRangeCenterOpp + transform.position, AttackRangeSize);

        foreach (Collider hit in hits)
            hit.gameObject.GetComponent<HeroBase>()?.TakeDamage(EnemyStats.damage);


        //secondo swing dell'arma
        yield return new WaitForSeconds(.500f);

        hits = null;
        if (transform.localScale.x == 1)
            hits = Physics.OverlapBox(AttackRangeCenter + transform.position, AttackRangeSize);
        else
            hits = Physics.OverlapBox(AttackRangeCenterOpp + transform.position, AttackRangeSize);

        foreach (Collider hit in hits)
            hit.gameObject.GetComponent<HeroBase>()?.TakeDamage(EnemyStats.damage);
        //apply damage
        yield return new WaitForSeconds(.500f);
        canAttack = true;
        NavMeshAgent.enabled = true;
    }
    protected override void Movement()
    {
        if (!NavMeshAgent.enabled) return;
        NavMeshAgent.speed = stats.ms;

        if(transform.localScale.x ==1)
            NavMeshAgent.destination = GameManager.Instance.HeroInstance.Position + AttackRangeCenterOpp;
        else
            NavMeshAgent.destination = GameManager.Instance.HeroInstance.Position + AttackRangeCenter;
    }
    protected override void OnStateChanged(GameState newState)
    {

    }

    //DEBUG
    protected override void OnTakeDmg(int damage)
    {
        if (!canBeDamaged) return;
        base.OnTakeDmg(damage);
        Invoke(nameof(ResetAttack), .5f);
    }

    void ResetAttack() => canAttack = true;
    private void OnDrawGizmos()
    {

            if(transform.localScale.x == 1)
                Gizmos.DrawWireCube(AttackRangeCenter + transform.position, AttackRangeSize);
            else
                Gizmos.DrawWireCube(AttackRangeCenterOpp + transform.position, AttackRangeSize);
  
    }
}
