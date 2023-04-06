using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepulsionSpellSchool : SpellBase
{

    [SerializeField] private SphereCollider range;
    [SerializeField] MeshRenderer ShieldGraphic;
    private float DisableTime=0f;

    HeroBase hero;
    private List<Collider> enemyColliders = new List<Collider>();
    float cooldown = 10f;
    private float _lockedTill = 0f;
    bool shield = false;
    // Start is called before the first frame update
    void Awake()
    {
        base.Awake();
        //caricare le statistiche usando la classe ResourcesSystem
        hero = GetComponentInParent<HeroBase>();
    }
    /// <summary>
    /// Quando si attiva la classe viene immediatamente innalzato lo scudo
    /// </summary>

    // Update is called once per frame
    void Update()
    {
        if (_lockedTill <= Time.time && !shield)
            ShieldUp();
    }

    /// <summary>
    /// Applichiamo la repulsione e mettiamo lo scudo in cooldown
    /// </summary>
    public override void Effect()
    {
        _lockedTill = Time.time + cooldown;
        Debug.Log("Effect");
        shield = false;
        hero.canBeDamaged = true;
        ShieldGraphic.enabled = false; 
        hero.OnAfterTakeDamage -= Effect;

        foreach (Collider collider in enemyColliders)
        {
            var enemy = collider.GetComponentInParent<EnemyUnitBase>();
            //forse è necessario implementare il knockback
            enemy.TakeDamage(GetDamage());
        }
        
    }

    private void ShieldUp()
    {
        shield = true;
        hero.canBeDamaged = false;
        ShieldGraphic.enabled = true;
        hero.OnAfterTakeDamage += Effect;
    }
    public override int GetDamage()
    {
        return 0;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Enemy" && collider.GetType() == typeof(BoxCollider))
            enemyColliders.Add(collider);
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.gameObject.CompareTag("Enemy") && collider.GetType() == typeof(BoxCollider))
            enemyColliders.Remove(collider);
    }

    private void OnEnable()
    {
        if (DisableTime == 0) return;

        _lockedTill += (Time.time - DisableTime);
    }

    private void OnDisable()
    {
        hero.canBeDamaged = true;

        if (_lockedTill <= Time.time) DisableTime = 0;
        else DisableTime = Time.time;
    }
}
