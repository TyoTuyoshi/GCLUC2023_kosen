using DG.Tweening;
using UnityEngine;

namespace Actor.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private Enemy enemyPrefab;
        [SerializeField] private float delay;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.CompareTag("Player")) return;

            DOVirtual.DelayedCall(delay, () =>
            {
                Instantiate(enemyPrefab, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }).SetLink(gameObject);
        }
    }
}