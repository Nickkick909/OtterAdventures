using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUIManager : MonoBehaviour
{

    public ProgressBar healthBar;
    public int healthVal;

    public TMP_Text levelTMP;
    public TMP_Text expTMP;



    void Start () {
        
    } 

    // Update is called once per frame
    void Update()
    {
        if (this.healthBar != null)
            this.healthBar.BarValue = healthVal;
    }

    public void SetLevel(int level) {
        Debug.Log("Set Level: " + level.ToString());
        levelTMP.text = "Lvl: " + level.ToString();
    }

    public void SetExp(int exp, int maxExp) {
        Debug.Log("Set EXP: " + exp.ToString() + "/" + maxExp.ToString());
        expTMP.text = "Exp: " + exp.ToString() + "/" + maxExp.ToString();
    }
}
