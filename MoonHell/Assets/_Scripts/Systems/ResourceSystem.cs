using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// One repository for all scriptable objects. Create your query methods here to keep your business logic clean.
/// I make this a MonoBehaviour as sometimes I add some debug/development references in the editor.
/// If you don't feel free to make this a standard class
/// </summary>
public class ResourceSystem : StaticInstance<ResourceSystem> {
    public List<ScriptableArtifact> _ArtifactList { get; private set; }
    private Dictionary<ItemType, ScriptableArtifact> _ArtifactDict;

    public List<ScriptableHero> _HeroList { get; private set; }
    public List<ScriptableSpellBase> _SpellList { get; private set; }
    private Dictionary<SpiritSchool, ScriptableSpellBase> _SpellDict;

    public List<ScriptableItemBase> _ItemList { get; private set; }
    private Dictionary<ItemType, ScriptableItemBase> _ItemDict;

    public List<ScriptableEnemy> _EnemyList { get; private set; }
    private Dictionary<EnemyType, ScriptableEnemy> _EnemyDict;

    //public List<SpellBase> SpiritSchoolPrefab { get; private set; }
    protected override void Awake() {
        base.Awake();
    }

    public void AssembleResources() {
        _ItemList = Resources.LoadAll<ScriptableItemBase>("Item").ToList();
        _ItemDict = _ItemList.ToDictionary(r => r.ItemType, r => r);

        _ArtifactList = Resources.LoadAll<ScriptableArtifact>("Artifacts").ToList();
        _ArtifactDict = _ArtifactList.ToDictionary(r => r.ItemType, r => r);

        _SpellList = Resources.LoadAll<ScriptableSpellBase>("Spells").ToList();
        _SpellDict = _SpellList.ToDictionary(r => r.SpiritSchool, r => r);

        _HeroList = Resources.LoadAll<ScriptableHero>("HeroStats").ToList();

        
        _EnemyList = Resources.LoadAll<ScriptableEnemy>("Units").ToList();

        _EnemyDict = _EnemyList.ToDictionary(r => r.enemyType, r => r);

       //Una volta caricate tutte le risorse necessarie, si cambia lo stato di gioco
        GameManager.Instance.ChangeState(GameState.Preparation);
    }
   
    public ScriptableArtifact GetRandomArtifact() => Instantiate(_ArtifactList[Random.Range(0, _ArtifactList.Count)]);

    public ScriptableHero GetRandomHero() => _HeroList[Random.Range(0, _HeroList.Count)];
    public ScriptableArtifact GetArtifact(ItemType name) => _ArtifactDict[name];
    public ScriptableItemBase GetItem(ItemType name) => _ItemDict[name];

    public ScriptableEnemy GetEnemy(EnemyType type) => _EnemyDict[type];

    public List<ScriptableEnemy> GetEnemyByPlanet(PlanetType planet)
    {
        List<ScriptableEnemy> enemyList = new();
        foreach(var enemy in _EnemyList)
        {
            if (enemy.Planet.Contains(planet)) enemyList.Add(enemy);
        }
        return enemyList;
    }
    public ScriptableSpellBase GetSpell(SpiritSchool spell) => _SpellDict[spell];

}