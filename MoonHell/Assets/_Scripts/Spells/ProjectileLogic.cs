using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLogic : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 enemyDirection;
    bool _canStart = false;
    float timeToLive;
    int damage;

    [SerializeField] [Range(0, 20)] float speed;
    [SerializeField] float lastTime;
    [SerializeField]Rigidbody rigidbody;
    
    // Update is called once per frame
    void Update()
    {
        if (!_canStart) return;
        //Muoviamo il proiettile verso enemyDirection con rigidBody

        rigidbody.velocity = enemyDirection.normalized * speed;
        if (Time.time >= timeToLive) Destroy(gameObject);
    }

    /// <summary>
    /// Metodo di inizilizzazione del proiettile. Una volta instanziato, esso non si muoverà finche questo metodo
    /// non verrà lanciato dal creatore. 
    /// </summary>
    /// <param name="enemyPosition">Posizione del nemico a cui sparare il proiettile</param>
    public void InitProjectile(Vector3 enemyDirection,int damage, float speed)
    {
        this.enemyDirection = enemyDirection.normalized;
        this.damage = damage;
        ///si ringrazia il collega tagliaferri giovanni
        var angle = Mathf.Atan2(this.enemyDirection.z, this.enemyDirection.x);


        //impostiamo la rotazione del proiettile
        var rotationVector = transform.rotation.eulerAngles;
        rotationVector.x = 45f;
        rotationVector.y = 0;
        rotationVector.z = angle*Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(rotationVector);

        timeToLive = Time.time + lastTime;
        _canStart = true;
        //this.speed = speed;
        //this.damage = damage;
    }

    public void OnCollisionEnter(Collision collision)
    {

        if (collision.collider.gameObject.CompareTag("Enemy")) Debug.Log("nemico colpito");
            collision.gameObject.GetComponentInParent<EnemyUnitBase>().TakeDamage(damage);
        
        Destroy(gameObject);
    }
}
