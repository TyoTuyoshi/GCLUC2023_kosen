using System;
using System.Linq;
using Item;
using UniRx;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Actor.Enemy
{
    /// <summary>
    ///     エネミーが死んだときに落とすアイテムの管理
    /// </summary>
    public class EnemyLootTable : MonoBehaviour
    {
        [Header("アイテムと確率を設定")]
        [SerializeField] private Pair<ItemDataScriptable,float>[] dropItems;

        private Enemy _enemy;

        private void Start()
        {
            TryGetComponent(out _enemy);

            _enemy.OnActorEvent
                .Where(e => e is DeathEvent)
                .Select(e => e as DeathEvent)
                .Subscribe(OnDeath)
                .AddTo(this);
        }

        private void OnDeath(DeathEvent e)
        {
            if (dropItems.Length == 0) return;
            
            var total = dropItems.Sum(item => item.Second);
            var rand = Random.Range(0, total);
            
            foreach (var item in dropItems)
            {
                rand -= item.Second;
                if (rand > 0) continue;
                
                DropItem(item.First);
                return;
            }
            
            Debug.LogError("アイテムの抽選ができませんでした。", gameObject);
        }

        private void DropItem(ItemDataScriptable data)
        {
            
        }
    }
}