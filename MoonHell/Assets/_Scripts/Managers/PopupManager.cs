using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : StaticInstance<PopupManager>
{
    [Header("Color Settings")]
    [SerializeField] Color HealColor;
    [SerializeField] Color DamageColor;
    [SerializeField] Color CritDamageColor;
    [SerializeField] Color EnemyDamageColor;
    [SerializeField] Color EnemyHealColor;

    [Header("PopUp Prefab")]
    [SerializeField] private TextPopUp popUpTextPrefab;


    public void DisplayDamage(UnitBase unit, int damage, bool crit)
    {
        if(unit is HeroBase)
            Instantiate(popUpTextPrefab).Init(EnemyDamageColor, unit.transform,damage);
        else
            Instantiate(popUpTextPrefab).Init(DamageColor, unit.transform, damage);         
    }

    public void DisplayHeal(UnitBase unit, int heal)
    {
        if (unit is HeroBase)
            Instantiate(popUpTextPrefab).Init(HealColor, unit.transform, heal);
        else
            Instantiate(popUpTextPrefab).Init(EnemyHealColor, unit.transform, heal);
    }
}
