using System;
using Item;
using Item.Bullet;
using UnityEngine;
using UnityEngine.Serialization;

namespace Actor.Bullet
{
    /// <summary>
    ///     弾丸の基底クラス
    /// </summary>
    public abstract class BulletBase : MonoBehaviour, IItem
    {
        [SerializeField] protected BulletItemData bulletData;
        [SerializeField] private LayerMask enemyLayer;


        public IItemData ItemData => bulletData;

        public void PickItem()
        {
            throw new NotImplementedException();
        }

        public void Use(ActorBase actor)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     弾丸を発射する
        /// </summary>
        public abstract void Shot(Vector3 pos, Vector3 dir);

        /// <summary>
        ///     弾丸が何かに当たった時の処理
        /// </summary>
        protected abstract void OnHitBullet(Collider2D col);

        private void OnTriggerEnter2D(Collider2D col)
        {
            if ((enemyLayer.value & 1 << col.gameObject.layer) == 0) return;
            
            OnHitBullet(col);
        }
    }
}