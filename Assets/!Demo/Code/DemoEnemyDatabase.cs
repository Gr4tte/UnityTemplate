using UnityEngine;

namespace UnityTemplate
{
    [CreateAssetMenu (fileName = "DemoEnemyDatabase", menuName = "Demo/Enemy Database", order = 1)]
    public class DemoEnemyDatabase : Database<DemoEnemyEnum, GameObject> { }
    
    public enum DemoEnemyEnum
    {
        Slime,
        Shroom 
    }
}