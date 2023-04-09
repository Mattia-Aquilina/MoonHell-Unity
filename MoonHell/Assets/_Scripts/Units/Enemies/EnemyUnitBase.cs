using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Classe da cui tutti i nemici derivano metodi e attributi
/// </summary>

public abstract class EnemyUnitBase : UnitBase
{
    /// ATTRIBUTI UNITY
    
    [SerializeField] protected EnemyStats _enemyStats;
    [SerializeField] public EnemyType enemyType;
    [SerializeField] protected Animator animator;
    [SerializeField] protected NavMeshAgent NavMeshAgent;
    HeroBase hero;
    ///Elenco di tutti i possibili  stati delle animazioni

    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Walk = Animator.StringToHash("Run");
    private static readonly int Damaged = Animator.StringToHash("Hit");
    private static readonly int Death = Animator.StringToHash("Death");
    private static readonly int Attack = Animator.StringToHash("Attack");
    /// 
    /// Attributi legati allo stato del personaggio
    protected bool _attacking = false;
    protected bool _moving = false;
    protected bool _damaged = false;
    protected float _lockedTill;
    protected int _currentState = Idle;

    //Statistiche complesse
    [SerializeField] float knockBackStrenght = 5;
    protected ScriptableEnemy enemyData;
    public EnemyStats EnemyStats => _enemyStats;
    public void SetEnemyStats(EnemyStats stats) => _enemyStats = stats;
    protected new void Awake()
    {
        base.Awake();
        //hero = GameManager.Instance.HeroInstance;
        LoadStats();
        
        NavMeshAgent.updateRotation = false;
        NavMeshAgent.updateUpAxis = false;
    }
    protected void Update()
    {
        base.Update();
        if (GameManager.Instance.isPaused) return;
        if (GameManager.Instance.State != GameState.Hunting) return;

        if (NavMeshAgent.enabled)
            _moving = true;
        else _moving = false;

        if (GameManager.Instance.HeroInstance.Position.x > transform.position.x)
            transform.localScale = new(1, 1, 1);
        else
            transform.localScale = new(-1, 1, 1);

        //gestione delle animazioni
        HandleAnimations();
    }

    protected abstract void Movement();
    public override void onDie()
    {
        //Aggiungiamo i reward al player tramite l'inventory manager
        NavMeshAgent.enabled = false;
        rigidbody.constraints = RigidbodyConstraints.FreezeAll;


        ForceState(Death, .5f);
        Invoke(nameof(Die), .5f);
    }
    void Die() => Destroy(gameObject);
    protected override void OnTakeDmg(int damage)
    {
        //throw new System.NotImplementedException();
        base.OnTakeDmg(damage);

        NavMeshAgent.enabled = false;
        rigidbody.isKinematic = false;

        var knockback = (transform.position - GameManager.Instance.HeroInstance.Position).normalized;
       

        rigidbody.AddForce(knockback * knockBackStrenght,ForceMode.Impulse);
        _attacking = false;

        animator.CrossFade(Damaged, 0, 0);
        _lockedTill = Time.time + ImmunityTime;

        StopAllCoroutines();
        
        Invoke(nameof(KnockbackReset), ImmunityTime);
    }

    private void KnockbackReset()
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.isKinematic = true;
        NavMeshAgent.enabled = true;
    }
    public override void TakeDamage(int dmg)
    {
        if (!canBeDamaged) return;

        //calcolo del danno
        BaseStats s = Stats;
        var ScaledDamage = StaticFunctions.GetDamage(dmg, _enemyStats.def);
        s.hp -= ScaledDamage;
        stats = s;

        OnTakeDmg(ScaledDamage);
    }
    protected override int GetState()
    {
        animator.speed = 1;
        if (Time.time <= _lockedTill) return _currentState;

        if (_damaged)
        {
            _damaged = false;
            return LockState(Damaged, 0.125f);
        }
        if (_attacking)
        {
            _attacking = false;
            //animator.speed = 1 / _attackAnimDuration;
            return LockState(Attack, 1f);
        }
        if (_moving)
        {
            return Walk;
        }
        return Idle;

        int LockState(int s, float t)
        {
            _lockedTill = Time.time + t;
            return s;
        }
    }
    //Force State
    protected void ForceState(int newState, float lockedTime)
    {
        animator.speed = 1 / lockedTime;
        animator.CrossFade(newState, 0, 0);
        _lockedTill = lockedTime + Time.time;
    }
    protected void HandleAnimations()
    {
        var state = GetState();

        if (state == _currentState) return;
        animator.CrossFade(state, 0, 0);
        _currentState = state;
    }
    protected virtual void LoadStats()
    {
        enemyData = ResourceSystem.Instance.GetEnemy(enemyType);
        SetStats(enemyData.BaseStats);
        SetEnemyStats(enemyData.enemyStats);
        NavMeshAgent.speed = stats.ms;
    }
}
