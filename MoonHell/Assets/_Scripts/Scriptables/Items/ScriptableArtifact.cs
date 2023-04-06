using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "artifact", menuName = "Scriptable/artifact")]
public class ScriptableArtifact : ScriptableItemBase
{
    /// <summary>
    /// Statistiche base che l'oggetto potenzia per il personaggio
    /// In caso di valori tra 0 e 1 (esclusi) il valore verrà inteso in percentuale
    /// </summary>
    [SerializeField] private BaseStats _baseStatsEmp;
    /// <summary>
    /// Statistiche non base che l'oggetto potenzia per il personaggio
    /// In caso di valori tra 0 e 1 (esclusi) il valore verrà inteso in percentuale
    /// </summary>
    [SerializeField] private HeroStats _heroStatsEmp;

    /// <summary>
    /// Lista delle scuole di spirito di appartenenza 
    /// </summary>
    [SerializeField] private List<SpiritSchool> _spiritTypes;

    /// <summary>
    /// Lista di eventuali ingredienti necessari alla creazione
    /// </summary>
    [SerializeField] private List<ScriptableItemBase> _ingredients;

    /// <summary>
    /// Costo necessario per costruire l'oggetto
    /// </summary>
    [SerializeField] private int buildCost;
    public BaseStats BaseStatsEmp() => _baseStatsEmp;
    public HeroStats HeroStatsEmp() => _heroStatsEmp;
    public List<ScriptableItemBase> ingredients() => _ingredients;

    public List<SpiritSchool> spiritTypes() => _spiritTypes;
}
