using UnityEngine;
using TMPro;

public class CoinCounter : MonoBehaviour
{

    public static CoinCounter instance;

    public TMP_Text coinText;
    public int currentCoins = 0;

    void Awake(){

        instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        coinText.text="COINS: "+currentCoins.ToString();
    }

    // Update is called once per frame
    public void IncreaseCoins(int v){

        currentCoins +=v;
        coinText.text="COINS: "+currentCoins.ToString();
    }
}
