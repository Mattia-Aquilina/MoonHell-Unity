using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrecisionSchoolSpell : SpellBase
{
    [SerializeField] private SphereCollider range;
    [SerializeField] private float _lockedTill = 0f;

    /// <summary>
    /// Per ora creiamo il riferimento tramite l'inspector. Successivamente potrebbe essere necessario differenziare
    /// o la creazione del riferimento, tramite ResourcesSystem, o modificando direttamente la classe del
    /// proiettile
    /// </summary>
    [SerializeField] private ProjectileLogic projectile;
    private Vector3 nearestEnemy;
    private bool _canShoot;
    private List<Collider> enemyColliders = new();
    void Awake()
    {
        base.Awake();

    }

    private void Update()
    {
        //controlliamo se il player è fermo
        _canShoot = !GameManager.Instance.HeroInstance._moving;

        if (_canShoot)
            Effect();
    }

    /// <summary>
    /// In questa classe il metodo effect deve essere lanciato solamente se lo switch canFire è true
    /// </summary>
    public override void Effect()
    {
        if (Time.time <= _lockedTill || enemyColliders.Count == 0)
            return;
        //funzia devo solo aggiungere l'attacco
        Debug.Log("fire");
        GetNearestEnemy();
        _lockedTill = Time.time + (1 / GameManager.Instance.HeroInstance.HeroStats.ats);

        var p = Instantiate(projectile,transform.position, Quaternion.identity);
        //Calcoliamo la direzione del nemico, notiamo come quest'ultima sia normalizzata per evitare che la
        //velocità del proiettile scali con la distanza
        var dir = (nearestEnemy - transform.position).normalized;

        p.InitProjectile(dir, GetDamage() , 0);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Enemy" && collider.GetType() == typeof(BoxCollider))
            enemyColliders.Add(collider);
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Enemy" && collider.GetType() == typeof(BoxCollider))
            enemyColliders.Remove(collider);
    }

    /// <summary>
    /// Calcoliamo il nemico più vicino al momento dello sparo
    /// </summary>
    private void GetNearestEnemy()
    {
        Vector3 enemyDirection = Vector3.positiveInfinity;
        float currentMinDistance = Mathf.Infinity;
        foreach(var collider in enemyColliders)
        {
            //enemy distance
            float enemyDistance = (collider.transform.position - this.transform.position).magnitude;

            if (enemyDistance < currentMinDistance)
            {
                enemyDirection = collider.gameObject.transform.position + collider.bounds.size/2;
                currentMinDistance = enemyDistance;
            }
        }
         
        //Aggiorniamo il campo nearestEnemy
        nearestEnemy = enemyDirection;
    }
    public override int GetDamage()
    {
        int damage = 0;
        //Calcoliamo il danno

        return damage;
    }
    //===============================DEBUG===============================

    void OnDrawGizmosSelected()
    {
        //if(enemyColliders.Count == 0) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, nearestEnemy);
    }
}
