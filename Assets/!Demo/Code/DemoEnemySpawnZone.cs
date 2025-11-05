using System;
using UnityEngine;

namespace UnityTemplate
{
    public class DemoEnemySpawnZone : MonoBehaviour
    {
        [SerializeField] private DemoEnemyEnum _enemyType = DemoEnemyEnum.Slime;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            DemoEnemyManager.Instance.SpawnEnemy(_enemyType);
            gameObject.SetActive(false);
        }
    }
}