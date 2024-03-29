using Game;
using UnityEngine;

namespace Utils
{
    public class ClearArea : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.gameObject.CompareTag("Player")) return;
            GameManager.Instance.GoToResult();
        }
    }
}