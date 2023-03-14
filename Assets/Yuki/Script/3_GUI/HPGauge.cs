using UnityEngine;
using UnityEngine.UI;

public class HPGauge : MonoBehaviour
{

    [SerializeField] private GameObject player;
    private Image hp_gauge;

    private int hp_now = 180; //現在のHP（デバッグリテラル）
    private int hp_max = 200;//最大HP
    
    // Start is called before the first frame update
    void Start()
    {
        hp_gauge = this.gameObject.GetComponent<Image>();
    }

    private void Update()
    {
        //Player...
        //hp_now=...
        //hp_max=...
        hp_gauge.fillAmount = (float)hp_now / hp_max;
    }

    //public void ChangeHPGauge(int n,int hp_max)
    //{
    //}
}
