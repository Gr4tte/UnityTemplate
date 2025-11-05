using UnityEngine;

namespace UnityTemplate
{
    public class DemoEnemyManager : Singleton<DemoEnemyManager>
    {
        [SerializeField] private DemoEnemyDatabase _enemyDatabase;
        [SerializeField] private Vector3 _position;
        
        public void SpawnEnemy(DemoEnemyEnum type)
        {
            if (!_enemyDatabase.TryGet(type, out GameObject enemyPrefab)) return;
            Instantiate(enemyPrefab, _position, Quaternion.identity);

            // is same as this
            // GameObject enemyPrefab2 = _enemyDatabase.Get(type);
            // if (!enemyPrefab2) return;
            // Instantiate(enemyPrefab2, _position, Quaternion.identity);
        }
    }
}