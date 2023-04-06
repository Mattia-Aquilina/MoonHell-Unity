using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurySchollSpell : SpellBase
{
    //Attributi per effettuare l'attacco
    [SerializeField] float AttackRange = 5;
    [SerializeField] float ComboWindowTime = 0.3f;
    [SerializeField] int DashStrength = 100;
    [SerializeField] float distance = 10;
    [SerializeField] float destinationMultiplier = 0.9f;
    [SerializeField] float DashTime = 0.2f;
    [SerializeField]public Vector3 destination;
    [SerializeField]private float _lockedTill = 0f;
    [SerializeField] LayerMask layerMask;
    private HeroBase heroInstance;
    
    bool dashing = false;
    Coroutine CurrentAttackRoutine;
    IEnumerator _ComboWindow;
    Coroutine _attackSent = null;
    public override void Awake()
    {
        base.Awake();
        heroInstance = GetComponentInParent<HeroBase>();
        //Utiliziamo la classe resourcesSystem per caricare le stats dell'oggetto;

        //Utilizziamo il gameManager per ottenere la reference al player

        //Inizializziamo la velocità di attacco prendendola da player
    }

    /// 
    /// METODI CHE REALIZZANO LA COMBO
    /// 

    private void Update()
    {
        if (dashing)
        {
            //muoviamo il personaggio 
            heroInstance.Rigidbody().position = Vector3.MoveTowards(heroInstance.Position, destination, distance);
        }
    }
    public override void Effect()
    {
        //if (OnGoingCombo)
        //{
        //    Chained = true;
        //    return;
        //}
        //else
        //{
        //    StartCoroutine(Attack1Method());
        //}
        _attackSent = StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        yield return CurrentAttackRoutine;

        switch (heroInstance.playerState)
        {
            case PlayerStates.Idle:
                CurrentAttackRoutine = StartCoroutine(Attack1());
                break;
            case PlayerStates.Attack1:
                StopCoroutine(_ComboWindow);
                CurrentAttackRoutine = StartCoroutine(Attack2());
                break;
            case PlayerStates.Attack2:
                StopCoroutine(_ComboWindow);
                CurrentAttackRoutine = StartCoroutine(Attack3());
                break;
            case PlayerStates.Attack3:
                StopCoroutine(_ComboWindow);
                CurrentAttackRoutine = StartCoroutine(Attack4());
                break;
            default:
                break;
        }
    }

    //Funzioni Combo
    private IEnumerator Attack1()
    {
        heroInstance.Rigidbody().constraints = RigidbodyConstraints.FreezeAll;

        //cambiamo stato del player
        heroInstance.SetState(PlayerStates.Attack1);
        //Avviamo l'animazione
        heroInstance.SetAttacking(true);
        applyDamage();

        yield return new WaitForSeconds(1 / heroInstance.HeroStats.ats);
       
        heroInstance.SetAttacking(false);
        heroInstance.Rigidbody().constraints = RigidbodyConstraints.FreezePositionY;
        heroInstance.Rigidbody().freezeRotation = true;

        _ComboWindow = ComboWindow();
        StartCoroutine(_ComboWindow);
    }

    private IEnumerator Attack2()
    {
        heroInstance.Rigidbody().constraints = RigidbodyConstraints.FreezeAll;
        //cambiamo stato del player
        heroInstance.SetState(PlayerStates.Attack2);
        //Avviamo l'animazione
        heroInstance.SetAttacking(true);
        applyDamage();

        yield return new WaitForSeconds(1 / heroInstance.HeroStats.ats);
        


        heroInstance.SetAttacking(false);
        heroInstance.Rigidbody().constraints = RigidbodyConstraints.FreezePositionY;
        heroInstance.Rigidbody().freezeRotation = true;

        _ComboWindow = ComboWindow();
        StartCoroutine(_ComboWindow);
    }

    //dash attack
    private IEnumerator Attack3()
    {
        heroInstance.Rigidbody().constraints = RigidbodyConstraints.FreezeAll;
        //cambiamo stato del player
        heroInstance.SetState(PlayerStates.DashChannel);
        heroInstance._animator().speed = 1 / DashTime;
        //Avviamo l'animazione
        heroInstance.SetAttacking(true);

        yield return new WaitForSeconds(1 / heroInstance.HeroStats.ats);
        heroInstance.SetState(PlayerStates.Attack3);
        Vector3 dir;
        if (heroInstance.transform.localScale.x == -1)
            dir = new(-1, 0, 0);
        else
            dir = new(1, 0, 0);
        //utilizziamo raycast per ottenere eventuali oggetti nel percorso
        RaycastHit[] hits;
        hits = Physics.RaycastAll(heroInstance.Position, dir * distance, distance , layerMask);

        destination = (heroInstance.Position + dir * distance) * destinationMultiplier;
        heroInstance.canBeDamaged = false;
        dashing = true;
        //Impostiamo il personaggio in modo da eseguire un dash
        heroInstance.Rigidbody().constraints = RigidbodyConstraints.None;
        
        //aspettiamo l'animazione del dash
        yield return new WaitForSeconds(DashTime);
        //riportiamo il pg nello stato di partenza
        heroInstance.canBeDamaged = true;
        dashing = false;

        //Applichiamo il danno in maniera alternativa utilizzando il raycast precedente
        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];

            hit.collider.gameObject.GetComponent<EnemyUnitBase>()?.TakeDamage(GetDamage());
        }

            heroInstance.Rigidbody().constraints = RigidbodyConstraints.FreezePositionY;
        heroInstance.Rigidbody().freezeRotation = true;

        heroInstance.SetAttacking(false);

        _ComboWindow = ComboWindow();
        StartCoroutine(_ComboWindow);
    }

    private IEnumerator Attack4()
    {
        heroInstance.Rigidbody().constraints = RigidbodyConstraints.FreezeAll;
        //cambiamo stato del player
        heroInstance.SetState(PlayerStates.Attack4);
        //Avviamo l'animazione
        heroInstance.SetAttacking(true);
        applyDamage();
        yield return new WaitForSeconds(1 / heroInstance.HeroStats.ats);
        

        heroInstance.Rigidbody().constraints = RigidbodyConstraints.FreezePositionY;
        heroInstance.Rigidbody().freezeRotation = true;

        heroInstance.SetAttacking(false);
        yield return new WaitForSeconds(ComboWindowTime/2);
        heroInstance.SetState(PlayerStates.Idle);
    }

    private IEnumerator ComboWindow()
    {
        yield return new WaitForSeconds(ComboWindowTime);

        heroInstance.SetState(PlayerStates.Idle);
    }


    public void applyDamage()
    {

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, AttackRange);

        foreach (var collider in hitColliders)
        {
            //Siamo sicuri di avere collider solo di oggetti appartenenti alla classe enemyBase
            collider.GetComponent<EnemyUnitBase>()?.TakeDamage(GetDamage());
        }
    }

    /// <summary>
    /// Funzione che calcola i danni senza considerare le difese del nemico. Sarà la funzione takeDamage della
    /// classe unit base a considerare i parametri difensivi di ciascuna unità. Per considerare eventuali tassi
    /// di penetrazione delle difese dovremmo in ogni caso accedere alla reference del player, resa pubblica dal
    /// game manager
    /// </summary>
    /// <param name="defense">Difesa del nemico</param>
    /// <returns>Ritorna il danno calcolato</returns>
    public override int GetDamage()
    {
        int damage = 0;
        //Calcoliamo il danno

        return damage;
    }

    //DEBUG
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, AttackRange);
    }
}
