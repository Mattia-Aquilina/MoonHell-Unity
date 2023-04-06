using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbMage : EnemyUnitBase
{
    //attributi epr animare
    private static readonly int Healing = Animator.StringToHash("Healing");

    //Informazioni per attaccare
    [SerializeField]private bool canAttack = true;
    [SerializeField] private float StartingHp;
    [SerializeField] private float channelTime = .5f;
    [SerializeField] OrbMageAttack attackPrefab;
    //statistiche particolari
    int NumberOfHealing = 3;
    float AttackRange =3.5f;
    float healAmount;
    // Start is called before the first frame update
    void Awake()
    {
        base.Awake();
        LoadStats();
        StartingHp = stats.hp;
    }

    protected override void LoadStats()
    {
        base.LoadStats();
        var statsDic = enemyData.GetSkillStats();

        healAmount = statsDic[EnemySkillStats.HealAmmount];
        NumberOfHealing =Mathf.RoundToInt(statsDic[EnemySkillStats.NumberOfHealing]);
        channelTime = statsDic[EnemySkillStats.ChannelTime];
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.isPaused) return;
        if (GameManager.Instance.State != GameState.Hunting) return;

        base.Update();
        var playerDistance = (GameManager.Instance.HeroInstance.Position - spriteRenderer.gameObject.transform.position);

        

        if (stats.hp < StartingHp * .5 && NumberOfHealing > 0 && canAttack)
        {
            StartCoroutine(Heal());
        }
        if (playerDistance.magnitude < AttackRange && canAttack)
        {
            StartCoroutine(Attack());
        }
        else Movement();
            
    }
    protected override void Movement()
    {
        if (!NavMeshAgent.enabled) return;

        NavMeshAgent.speed = stats.ms;
        NavMeshAgent.destination = GameManager.Instance.HeroInstance.Position;
    }

    protected override void OnStateChanged(GameState newState)
    {
        
    }

    protected IEnumerator Attack()
    {
        
        canAttack = false;
        _attacking = true;
        NavMeshAgent.enabled = false;
        yield return new WaitForSeconds(channelTime/2);
        var playerPosition = GameManager.Instance.HeroInstance.Position;
        yield return new WaitForSeconds(channelTime/2);

        var attack = Instantiate(attackPrefab, playerPosition, Quaternion.identity);
        attack.Init(0);

        yield return new WaitForSeconds(channelTime);
        canAttack = true;
        NavMeshAgent.enabled = true;
        _attacking = false;
    }
    protected IEnumerator Heal()
    {
        canAttack = false;
        NavMeshAgent.enabled = false;
        ForceState(Healing, 2 * channelTime);
        yield return new WaitForSeconds(channelTime);

        
        //perfrom heal
        yield return new WaitForSeconds(channelTime);
        canAttack = true;
        NavMeshAgent.enabled = true;
    }
}
