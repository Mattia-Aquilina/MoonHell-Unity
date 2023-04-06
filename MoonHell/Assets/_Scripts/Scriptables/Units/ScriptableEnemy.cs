using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(fileName = "newEnemy", menuName = "Scriptable/Enemies")]
public class ScriptableEnemy : ScriptableUnitBase
{
    [SerializeField] private EnemyType _enemyType;
    [SerializeField] private EnemyStats _enemyStats;
    public EnemyStats enemyStats => _enemyStats;

    public EnemyType enemyType => _enemyType;
    [SerializeField] public List<PlanetType> Planet;

    //Continer parametri custom per determinate skill
    [SerializeField] List<ChiaveValore> SkillStatsList = new();

    public Dictionary<EnemySkillStats,float> GetSkillStats() => SkillStatsList.ToDictionary(r => r.key, r => r.val);
}
