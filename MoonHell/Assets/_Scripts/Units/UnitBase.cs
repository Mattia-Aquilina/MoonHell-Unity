using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This will share logic for any unit on the field. Could be friend or foe, controlled or not.
/// Things like taking damage, dying, animation triggers etc
/// </summary>
public abstract class UnitBase : MonoBehaviour {
    ///Switch di lavoro 
    public bool canBeDamaged=true;
    /// ATTRIBUTI UNITY
    [SerializeField]protected SpriteRenderer spriteRenderer;
    [SerializeField]protected BoxCollider boxCollider;
    [SerializeField]protected new Rigidbody rigidbody;
    [SerializeField]protected float ImmunityTime = .125f;
    /// Attibuti di lavoro
    [SerializeField] protected BaseStats stats;


    private List<StatusBase> statusEffects = new List<StatusBase>();
    public virtual void SetStats(BaseStats stats) => this.stats = stats;
    public BaseStats Stats => stats;
    /// <summary>
    /// Identifica la fase di gioco, se false ferma completamente le unità
    /// </summary>
    protected bool _gameTime;
    /// Iscrizione agli eventi del gameManager
    ///
    protected void Awake() => GameManager.OnBeforeStateChanged += OnStateChanged;

    private void OnDestroy() => GameManager.OnBeforeStateChanged -= OnStateChanged;

    protected abstract void OnStateChanged(GameState newState);

    /// <summary>
    /// Metodo update basilare per ogni unità, controllo tutti gli effeti di stato correntemente attivi, e successivamente
    /// controlla gli hp
    /// </summary>
    protected void Update()
    {
        //check sulla posizione degli oggetti
        if (transform.rotation.x != 0) transform.rotation = Quaternion.identity;
        if (transform.position.y > 0) transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        this.tick();
        this.checkHp();
    }
    /// <summary>
    /// Funzione definita su ogni unità, infligge il danno passato come parametro
    /// </summary>
    /// <param name="dmg">Entità del danno da infliggere</param>
    public virtual void TakeDamage(int dmg)
    {
        if (canBeDamaged)
        {
            BaseStats s = this.Stats;
            s.hp -= dmg;
            stats = s;
        }
        OnTakeDmg();
    }

    /// <summary>
    /// Controllo di routine degli hp
    /// </summary>
    public virtual void checkHp()
    {
        if (this.Stats.hp <= 0)
            this.onDie();
    }

    /// <summary>
    /// Controllo degli status effect
    /// </summary>
    public virtual void tick()
    {
        foreach (StatusBase status in statusEffects)
        {
            status.Effect(this);
            if (status.duration <= 0)
                statusEffects.Remove(status);
        }
    }

    /// <summary>
    /// Metodo che gestisce il meccanismo delle animazioni tramite codice
    /// </summary>
    /// <returns>Ritorna il valore identificante lo stato da riprodurre</returns>
    protected abstract int GetState();

    
    ///==========================================///
    ///================EVENTI UNIT===============///
    ///==========================================///
    ///
    /// <summary>
    /// Metodo eseguito quando l'unità riceve danni
    /// </summary>
    protected virtual void OnTakeDmg()
    {
        Debug.Log("UnitBase - on Take Damage");
        canBeDamaged = false;
        Invoke(nameof(ResetImmunity), ImmunityTime);
    }
    private void ResetImmunity() => canBeDamaged = true;
    /// <summary>
    /// Metodo lanciato da checkHp() quando l'unità associato raggiunge hp negativi
    /// </summary>
    public abstract void onDie();
}