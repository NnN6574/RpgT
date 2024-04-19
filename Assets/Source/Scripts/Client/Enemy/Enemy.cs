using Mistave.Client.Data;
using UnityEngine;

namespace Mistave.Client.Enemy
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private EnemyConfig _enemyConfig;
        private EnemyData _enemyData;
    }
}