using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbMageAttack : MonoBehaviour
{
    private static readonly int VFX = Animator.StringToHash("VFX");

    [SerializeField] Animator animator;
    [SerializeField] Vector3 Range;
    [SerializeField] Vector3 RangeCenter;
    [SerializeField] SpriteRenderer spriteRenderer;
    public void Init(float damage)
    {
        animator.CrossFade(VFX, 0, 0);

        Collider[] hits;
        hits = Physics.OverlapBox(RangeCenter + transform.position, Range, Quaternion.identity);

        foreach (Collider hit in hits)
            hit.gameObject.GetComponent<HeroBase>()?.TakeDamage(0);
        Invoke(nameof(RemoveGO), 1f);
    }

    void RemoveGO()
    {
        Destroy(gameObject);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(RangeCenter + transform.position, Range);
    }

}
