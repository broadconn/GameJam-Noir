using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    [SerializeField] private List<EnemyToPrefab> enemiesToPrefabs;
    [SerializeField] private List<EnemyWave> waves;
    [SerializeField] private int debugStartWave = 0;

    private GameController _gameController;
    private int _curWave = 0;

    private void Awake() {
        _gameController = GetComponent<GameController>();
    }

    private void Start()
    {
        _curWave = debugStartWave;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnWave();
        }
    }

    private void SpawnWave()
    {
        var wave = waves[_curWave];
        var enemyPrefab = enemiesToPrefabs.First(ep => ep.EnemyType == wave.EnemyType).Prefab;
        for (var i = 0; i < wave.NumToSpawn; i++)
        {
            var newEnemy = Instantiate(enemyPrefab, _gameController.EnemySpawnPos, Quaternion.identity);
            var e = newEnemy.GetComponent<Enemy>();
            e.Spawn(_gameController.EnemySpawnPos, _gameController.EnemyGoalPos);
        }
    }
}

[Serializable]
public class EnemyToPrefab
{
    public EnemyType EnemyType;
    public GameObject Prefab;
}

[Serializable]
public class EnemyWave
{
    public EnemyType EnemyType;
    public int NumToSpawn = 1;
}

public enum EnemyType
{
    Grunt
}
