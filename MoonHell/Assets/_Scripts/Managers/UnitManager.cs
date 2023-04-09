using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitManager : StaticInstance<UnitManager>
{
    // Start is called before the first frame update
    [SerializeField] float SpawnDistance;
    private IEnumerator CurrentCoroutine;
    private List<ScriptableEnemy> enemyList;
    Dictionary<EnemyType, (float, float)> timers;
    bool SpawnTroupes = false;
    void Awake()
    {
        base.Awake();
        GameManager.OnAfterStateChanged += OnStateChange;
    }

    private void OnStateChange(GameState gameState)
    {
        StopAllCoroutines();
        switch (gameState)
        {
            case GameState.Menu:
                break;
            case GameState.Preparation:
                break;
            case GameState.PreHunting:
                break;
            case GameState.Jump:
                break;
            case GameState.Hunting:
                //manage hunting phase
                enemyList = ResourceSystem.Instance.GetEnemyByPlanet(GameManager.Instance.currentPlanet);
                timers = enemyList.ToDictionary(r => r.enemyType, r => (0f, r.minGameTimeForSpawn));
                SpawnTroupes = true;
                break;
            case GameState.AssemblingResources:
                break;
            case GameState.StartUp:
                break;
            default:
                break;
        }
        //StartCoroutine(CurrentCoroutine);
    }

    private void Update()
    {
        if (GameManager.Instance.State == GameState.Hunting && SpawnTroupes)
            HandleHuntingPhase();
    }
    /// <summary>
    /// Coroutine che gestisce lo spawn dei mostri
    /// </summary>
    /// <returns></returns>
    private void HandleHuntingPhase()
    {
        foreach (var enemy in enemyList)
        {
            (var spawn, var minTime) = timers[enemy.enemyType];

            if (minTime * 60f > GameManager.Instance.currentGameTime)
                continue;
            if (spawn < GameManager.Instance.currentGameTime)
            {
                Spawn(enemy);
                var newSpawnTime = (enemy.spawnTime - (enemy.spawnTime * StaticFunctions.GetSpawnRate()));
                timers[enemy.enemyType] = (Time.time + newSpawnTime, enemy.minGameTimeForSpawn);
            }
        }

    }

    private void Spawn(ScriptableEnemy enemy)
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        for (int i = 1; i <= (int)Random.Range(1, enemy.maxParallelSpawns); i++)
        {
            Vector3 direction = new();
            while (direction == Vector3.zero)
                direction = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));

            var position = GameManager.Instance.HeroInstance.Position + (direction).normalized * SpawnDistance;
            Instantiate(enemy.unityPrefab, position, Quaternion.identity);
        }
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
