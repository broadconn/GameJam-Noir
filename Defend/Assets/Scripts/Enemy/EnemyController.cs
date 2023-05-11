using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    [SerializeField] private GameObject enemy;

    private GameController _gameController;

    private void Awake() {
        _gameController = GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            var newEnemy = Instantiate(enemy, _gameController.EnemySpawnPos, Quaternion.identity);
            var e = newEnemy.GetComponent<Enemy>();
            e.Spawn(_gameController.EnemySpawnPos, _gameController.EnemyGoalPos);
        }
    }
}
