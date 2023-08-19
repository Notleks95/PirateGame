using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoldScore : MonoBehaviour
{
    public TMP_Text goldScore;
    GoldBarScript goldBarScript;

    // Start is called before the first frame update
    void Start()
    {
        goldScore.text = "You left the island with " + GoldBarScript.currentGold.ToString() + " gold! Can you do better?";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
