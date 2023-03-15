using Actor.Player;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Yuki.Script._3_GUI
{
    public class HpGauge : MonoBehaviour
    {
        [SerializeField] private Player player;
        private Image _hpGaugeImage;

        private void Start()
        {
            TryGetComponent(out _hpGaugeImage);

            player
                .ObserveEveryValueChanged(p => p.CurrentHp)
                .Subscribe(OnChangeHp)
                .AddTo(player);
        }

        private void OnChangeHp(float current)
        {
            _hpGaugeImage.fillAmount = current / player.MaxHp;
        }
    }
}