using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowSchoolSpell : SpellBase
{
    private bool canDash=true;
    public bool isDashing;
    private Vector3 lastMovement;

    [SerializeField] private float DashingTime;
    [SerializeField] private float DashStrength;

    [SerializeField] private SphereCollider range;
    [SerializeField] private TrailRenderer trailRenderer;

    //Variabili di debug
    [SerializeField]private float cooldown = 5f;
    // Start is called before the first frame update
    void Awake() 
    {
        base.Awake();
        
    }

    private void Update()
    {
        lastMovement = new(Joystick.Instance.Horizontal, 0, Joystick.Instance.Vertical);
    }
    /// <summary>
    /// In questo classe il metodo effect realizza un dash nella direzione del movimento del personaggio.
    /// Il dash deve essere realizzato alla pressione di un tasto. Sarà quindi la classe adibita all'interfaccia
    /// grafica che eseguira il metodo effect
    /// </summary>
    public override void Effect()
    {
        if (canDash)
           StartCoroutine(Dash());
    }

    private IEnumerator Dash()
    {
        Debug.Log("Dashhh");

        canDash = false;
        //Accediamo alla classe de player
        Joystick.Instance.enabled = false;
        var hero = GetComponentInParent<HeroBase>();
        hero.Rigidbody().drag = 0;
        hero.BoxCollider().enabled = false;
        trailRenderer.emitting = true;
        isDashing = true;
        hero.Rigidbody().AddForce(lastMovement.normalized * DashStrength, ForceMode.Impulse);
        
        yield return new WaitForSeconds(DashingTime);
        Joystick.Instance.enabled = true;
        trailRenderer.emitting = false;
        isDashing = false;
        hero.BoxCollider().enabled = true;


        yield return new WaitForSeconds(cooldown);
        canDash = true;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Enemy" && collider.GetType() == typeof(BoxCollider))
            collider.GetComponent<EnemyUnitBase>().TakeDamage(GetDamage());
    }

    public override int GetDamage()
    {
        return 0;
    }
}
