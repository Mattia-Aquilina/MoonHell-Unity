using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(fileName = "newEnemy", menuName = "Scriptable/Enemies")]
public class ScriptableEnemy : ScriptableUnitBase
{
    [Header("Stats")]
    [SerializeField] private EnemyType _enemyType;
    [SerializeField] private EnemyStats _enemyStats;
    [SerializeField] List<ChiaveValore> SkillStatsList = new();

    [Header("Unit Manager infos")]
    [SerializeField] public float spawnTime;
    [SerializeField] public float minGameTimeForSpawn;
    [SerializeField] public float maxParallelSpawns;
    [SerializeField] public List<PlanetType> Planet;
    public EnemyStats enemyStats => _enemyStats;

    public EnemyType enemyType => _enemyType;
    

    //Continer parametri custom per determinate skill
   

    public Dictionary<EnemySkillStats,float> GetSkillStats() => SkillStatsList.ToDictionary(r => r.key, r => r.val);
}
