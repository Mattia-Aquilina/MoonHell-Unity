using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroBase : UnitBase {

    // ATTRIBUTI UNITY
    // Definiamo gli attributi comuni a tutti gli eroi
    
    /// <summary>
    /// Riferimento allo scriptable del personaggio, in questo modo possiamo sempre accedere alle statistiche
    /// di base
    /// </summary>
    [SerializeField] protected ScriptableHero scriptableHero;
    /// <summary>
    /// Struttura dati che contiene le statistiche attuali del giocatore, tenendo conto di effetti o oggetti
    /// </summary>
    [SerializeField] protected HeroStats _heroStats;
    [SerializeField] private Animator animator;
    public Animator _animator() => animator;
    [SerializeField] public Dictionary<SpiritSchool, SpellBase> _schoolSpiritList  { get; private set; }


    //metodi get
    public HeroStats HeroStats => _heroStats;
    public void SetHeroStats(HeroStats s) => _heroStats = s;
    public Vector3 Position => transform.position;

    //Attributi di lavoro

    // Eventi

    /// <summary>
    /// Evento lanciato quanto il giocatore subisce danno, utile sia per implentare funzionalità nelle classi
    /// sia per realizzare il sistema di display dei danni
    /// </summary>
    public event Action OnAfterTakeDamage;

    /// <summary>
    /// WIP -> potrebbe essere usato per visualizzare i danni inflitti dal player
    /// </summary>
    public event Action OnDamageDealt;

    //Elenco di tutti i possibili  stati delle animazioni

    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Run = Animator.StringToHash("Run");
    //Le quattro animazioni della combo di base
    private static readonly int Attack1 = Animator.StringToHash("Attack-1");
    private static readonly int Attack2 = Animator.StringToHash("Attack-2");
    private static readonly int Attack3 = Animator.StringToHash("Attack-3");
    private static readonly int Attack4 = Animator.StringToHash("Attack-4");
    private static readonly int DashChannel = Animator.StringToHash("DashChannel");
    private static readonly int Death = Animator.StringToHash("Death");
    private static readonly int Hit = Animator.StringToHash("Hit");
    //private static readonly int Spell = Animator.StringToHash("new-spell");
    /// Attributi legati allo stato del personaggio

    //Variabile nel quale codifichiamo lo stato del player, Implenta il sistema di combo
    [SerializeField]public PlayerStates playerState=PlayerStates.Idle;

    public void SetState(PlayerStates state) => playerState = state;

    public bool _attacking { private set; get; }
    public void SetAttacking(bool flag) => _attacking = flag;
    public bool _moving { private set; get; }

    public bool _dashing = false;

    private float _lockedTill = 0;
    /// <summary>
    /// Animation State
    /// </summary>
    protected int _currentState = Idle;
    //Da inserire nelle statistiche del personaggio

    //metodi GET
    public Rigidbody Rigidbody() => rigidbody;
    public BoxCollider BoxCollider() => boxCollider;

    public SpriteRenderer SpriteRender() => spriteRenderer;
    protected override void OnStateChanged(GameState newState)
    {
        //controllo del new State
        switch (newState)
        {
            case GameState.Menu:
                break;
            case GameState.Preparation:
                break;
            case GameState.PreHunting:
                break;
            case GameState.Jump:
                break;
            default:
                break;  
        }

    }
    new void Awake()
    {
        base.Awake();

        //Inizializzazione delle statistiche, deve essere fatta in un altro modo
        _schoolSpiritList = new();
        scriptableHero = ResourceSystem.Instance.GetRandomHero();   

        SetStats(scriptableHero.BaseStats);
        SetHeroStats(scriptableHero.HeroStats);


        _attacking = false;
        _moving = false;
        //Spawn dei prefab di tutte le spell
        SpawnSpellSystems();
        animator.CrossFade(_currentState, 0, 0);

        //iscrizione agli eventi dell'inventory manager
        InventoryManager.Instance.InventoryChanged += OnInventoryChanged;
    }

    //METODO UPDATE

    new void Update()
    {
        if (GameManager.Instance.isPaused) return;
        base.Update();

        ///Gestione di tutti gli input
        ManageInputs();

        ///Gestione delle scuole di spirito
        //Attack();

        ///Gestione delle animazioni
        var state = GetState();

        if (state == _currentState) return;
        animator.CrossFade(state, 0, 0);
        _currentState = state;
    }
    ///GESTIONE DELL'INPUT
    protected void ManageInputs()
    {
        movement();

    }

    /// <summary>
    /// Metodo che parsa la lista di tutte le scuole attive esegue le relative abilità tenendo conto dei loro cooldown;
    /// </summary>
    //private void Attack()
    //{
    //    foreach (var school in _schoolSpiritList)
    //    {
    //        //school.Effect();
    //    }
    //}

    /// <summary>
    /// Metodo che gestisce il movimento del personaggio
    /// </summary>
    protected void movement()
    { 
        //solitamente non dipende dal classe, ma in caso ne effettuiamo l'override
        Vector3 dir = new Vector3(Joystick.Instance.Horizontal, 0, Joystick.Instance.Vertical);
        if (Mathf.Abs(dir.x) > 0 || Mathf.Abs(dir.z) > 0)
            _moving = true;
        else
            _moving = false;

        //rotation
        if (dir.x > 0)
            transform.localScale = new(1,1,1);
        if (dir.x < 0)
            transform.localScale = new(-1, 1, 1);

        rigidbody.velocity = dir.normalized * this.Stats.ms;
    }

    //Funzione di attacco, chiamata dal menu manager ogni volta che viene premuto il tasto attacca
   

    //Funzioni per animare il personaggio
    protected override int GetState()
    {
        if (Time.time <= _lockedTill)
        {
            return _currentState;
        }
        //da corregere
        if (_attacking)
        {
            animator.speed = this.HeroStats.ats;

            switch (playerState)
            {
                case PlayerStates.Attack1:
                    return Attack1;
                case PlayerStates.Attack2:
                    return Attack2;
                case PlayerStates.DashChannel:   
                    return DashChannel;
                case PlayerStates.Attack3:
                    return Attack3;
                case PlayerStates.Attack4:
                    return Attack4;
            }
        }

        if (_moving)
        {
            animator.speed = 1;
            return Run;
        }
        
        return Idle;

        int LockState(int s, float t)
        {
            _lockedTill = Time.time + t;
            return s;
        }
    }

    protected void SpawnSpellSystems()
    {
        foreach(var spell in ResourceSystem.Instance._SpellList)
        {
            _schoolSpiritList.Add(spell.SpiritSchool, Instantiate(spell.SpellPrefab, transform));
        }
    }
    public void setAttacking(bool status) => this._attacking = status;

    protected override void OnTakeDmg()
    {
        base.OnTakeDmg();
        Debug.Log("player took damage");
       
        OnAfterTakeDamage?.Invoke();
    }

    public override void onDie()
    {
        //throw new System.NotImplementedException();
    }

    //METODI PER GESTIONE DEGLI EVENTI
    /// <summary>
    /// Metodo eseguito ogni qualvolta l'inventario del giocatore cambia
    /// </summary>
    private void OnInventoryChanged()
    {
        //Attiviamo le scuole di spirit in base agli oggetti equipaggiati 
        foreach(SpellBase spell in _schoolSpiritList.Values)
        {
             spell.gameObject.SetActive(false);
        }

        _schoolSpiritList[SpiritSchool.fury].gameObject.SetActive(true);
        foreach (SpiritSchool type in InventoryManager.Instance.ActiveSchools)
        {
            try
            {
                _schoolSpiritList[type].gameObject.SetActive(true);
            }catch(Exception e)
            {

            }
        }
        //Calcoliamo le statistiche
        var hpPercentage = stats.hp / stats.MaxHP;
        var newBaseStats = scriptableHero.BaseStats;
        var newHeroStats = scriptableHero.HeroStats;

        foreach (ScriptableArtifact item in InventoryManager.Instance.ArtifactList)
        {
            newBaseStats.AddStats(scriptableHero.BaseStats.GetStatsIncrement(item.BaseStatsEmp()));
            newHeroStats.AddStats(scriptableHero.HeroStats.GetStatsIncrement(item.HeroStatsEmp()));
        }

        newBaseStats.hp = newBaseStats.MaxHP * hpPercentage;
        //Possiamo aggiornare le statistiche
        stats = newBaseStats;
        _heroStats = newHeroStats;
    }
}
public enum PlayerStates
{
    Attack1 = 0,
    Attack2 = 1,
    Attack3 = 2,
    Attack4 = 3,
    Idle = 4,
    DashChannel = 5
}