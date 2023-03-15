using Item;
using UnityEngine;

namespace Actor.Bullet
{
    /// <summary>
    /// 弾丸の基底クラス
    /// </summary>
    public abstract class BulletBase: MonoBehaviour
    {
        /// <summary>
        /// 弾丸を発射する
        /// </summary>
        public abstract void Shot(Vector3 pos, Vector3 dir);
        
        
    }
}