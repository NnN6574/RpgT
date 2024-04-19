using Mistave.Client.Enemy;
using UnityEngine;

namespace Mistave.Client.Data
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "Data/Configs/EnemyConfig")]
    public class EnemyConfig : ScriptableObject
    {
        [SerializeField] private EnemyData _enemyData;
    }
}