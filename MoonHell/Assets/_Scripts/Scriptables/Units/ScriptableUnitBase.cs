using System;
using UnityEngine;

/// <summary>
/// Scriptable object contente tutte le informazioni relative ad un unità di base. La classe è astratta deve essere instanziata 
/// utilizzando enemyBase o heroBase.
/// </summary>
public abstract class ScriptableUnitBase : ScriptableObject {

    [SerializeField] private BaseStats _baseStats;
    public BaseStats BaseStats => _baseStats;

    public UnitBase unityPrefab;
    // Used in menus
    public string Descrizione;
    public Sprite MenuSprite;
}

