using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "spell", menuName = "Scriptable/spell")]

public class ScriptableSpellBase : ScriptableObject
{
    /// <summary>
    /// Stastiche di base della generica spell. Il fatto che sia un'array permette di tenere conto dei livelli successivi. 
    /// Se la spell è a livello i, le sue statistiche si ottengono sommando tutte le struct da j=0 fino a j=i;
    /// </summary>

    [SerializeField] private SpellBase _SpellPrefab;

    [SerializeField] private List<SpellEffectStats> _spellBaseStats;
    /// <summary>
    /// Array delle statistiche relative a dimensione, durata, ecc della spell. È un array per tenere conto dei livelli.
    /// Se la spell è a livello i, le sue statistiche si ottengono sommando tutte le struct da j=0 fino a j=i;
    /// </summary>
    [SerializeField] private List<SpellSizesStats> _spellSizeStats;

    /// <summary>
    /// Scuola di spirito
    /// </summary>
    [SerializeField] private SpiritSchool _spiritSchool;
    ///Menu
    [SerializeField] private  Sprite _menuIcon;
    [SerializeField] private  string _spellName;

    public SpellBase SpellPrefab => _SpellPrefab;
    public List<SpellSizesStats> SpellSizeStats => _spellSizeStats;
    public List<SpellEffectStats> SpellBaseStats => _spellBaseStats;
    public Sprite MenuIcon => _menuIcon;
    public string SpellName => _spellName;
    public SpiritSchool SpiritSchool => _spiritSchool;

   
}
