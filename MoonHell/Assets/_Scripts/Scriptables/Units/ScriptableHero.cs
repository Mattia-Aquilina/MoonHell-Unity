using System;
using UnityEngine;

/// <summary>
/// Create a scriptable hero 
/// </summary>
[CreateAssetMenu(fileName = "newHero", menuName = "Scriptable/characters")]
public class ScriptableHero : ScriptableUnitBase
{
    public HeroType HeroType;

    [SerializeField] private HeroStats _heroStats;
    public HeroStats HeroStats => _heroStats;

}

