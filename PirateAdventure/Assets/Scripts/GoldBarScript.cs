using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoldBarScript : MonoBehaviour
{
    public static GoldBarScript instance;

    public TMP_Text goldText;
    public static int currentGold = 0;


    private void Awake()
    {
        instance = this;
        currentGold = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        goldText.text = "GOLD " + currentGold.ToString();
    }

    public void IncreaseGold(int v)
    {
        currentGold += v;
        goldText.text = "GOLD " + currentGold.ToString();
    }

    public void DecreaseGold(int v)
    {
        currentGold -= v;
        goldText.text = "GOLD " + currentGold.ToString();
    }
}
