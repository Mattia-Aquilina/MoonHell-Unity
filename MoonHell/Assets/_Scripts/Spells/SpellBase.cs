using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class SpellBase : MonoBehaviour { 
    public List<SpellEffectStats> _spellEffectLevelUp { get; private set; }
    public List<SpellSizesStats> _spellSizesLevelUp { get; private set; }

    public float _currentExp { get; private set; }
    public float _nextExpRequired { get; private set; }

    protected SpellEffectStats _baseStats;
    protected SpellSizesStats _sizesStats;
    public int _spellLevel { get; private set; }

    // Start is called before the first frame update
    public virtual void Awake()
    {
        _spellLevel = 1;
        _currentExp = 0f;
        _nextExpRequired = StaticFunctions.getNextExp(1);

    }

    public abstract void Effect();

    public abstract int GetDamage();
    protected void OnLevelUp()
    {
        _spellLevel++;
        _nextExpRequired = StaticFunctions.getNextExp(_spellLevel);

        UpdateSpellBaseStats();
        UpdateSpellEffectStats();
    }

    /// <summary>
    /// Aggiunge exp alla spell
    /// </summary>
    /// <param name="exp">Valore degli exp da aggiungere</param>
    protected void AddExp(float exp)
    {
        _currentExp += exp;
        if (_currentExp >= _nextExpRequired) OnLevelUp();
    }
    
    /// <summary>
    /// Eseguito quando la spell sale di livello, aggiorna le statistiche di base
    /// </summary>
    protected void UpdateSpellBaseStats()
    {
        _baseStats = _spellEffectLevelUp[_spellLevel];

    }

    /// <summary>
    /// Eseguito quando la spell sale di livello, aggiorna le statistiche legate alle dimensioni delle spell
    /// </summary>
    protected void UpdateSpellEffectStats()
    {
        _sizesStats = _spellSizesLevelUp[_spellLevel];
    }

}
