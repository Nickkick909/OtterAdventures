using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{

    public ProgressBar healthBar;
    public int healthVal;

    public TMP_Text levelTMP;
    public TMP_Text expTMP;

    // Light attack text
    [SerializeField] Text lightAttackName;
    [SerializeField] Text lightAttackDescription;
    [SerializeField] Text lightAttackStamina;
    [SerializeField] Text lightAttackPower;

    [SerializeField] Button lightAttackButton;

    // Heavy attack text
    [SerializeField] Text heavyAttackName;
    [SerializeField] Text heavyAttackDescription;
    [SerializeField] Text heavyAttackStamina;
    [SerializeField] Text heavyAttackPower;

    [SerializeField] Button heavyAttackButton;

    // Utility attack text
    [SerializeField] Text utilityAttackName;
    [SerializeField] Text utilityAttackDescription;
    [SerializeField] Text utilityAttackStamina;
    [SerializeField] Text utilityAttackPower;

    [SerializeField] Button utilityAttackButton;

    public Image staminaBar;
    public float staminaVal;


    void Start () {
        
    } 

    // Update is called once per frame
    void Update()
    {
        if (this.healthBar != null)
        {
            this.healthBar.BarValue = healthVal;

            if (staminaBar != null)
                staminaBar.fillAmount = staminaVal;
        }
            

    }

    public void SetLevel(int level) {
        Debug.Log("Set Level: " + level.ToString());
        levelTMP.text = "Lvl: " + level.ToString();
    }

    public void SetExp(int exp, int maxExp) {
        Debug.Log("Set EXP: " + exp.ToString() + "/" + maxExp.ToString());
        expTMP.text = "Exp: " + exp.ToString() + "/" + maxExp.ToString();
    }

    public void SetLightAttack(Attack a)
    {
        lightAttackName.text = a.name;
        lightAttackDescription.text = a.description;
        lightAttackStamina.text = a.staminaCost.ToString();
        lightAttackPower.text = a.attackPower.ToString();

        if (a.name != "-")
        {
            lightAttackButton.interactable = true;
        }
    }

    public void SetHeavyAttack(Attack a)
    {
        heavyAttackName.text = a.name;
        heavyAttackDescription.text = a.description;
        heavyAttackStamina.text = a.staminaCost.ToString();
        heavyAttackPower.text = a.attackPower.ToString();

        if (a.name != "-")
        {
            heavyAttackButton.interactable = true;
        }
    }

    public void SetUtilityAttack(Attack a)
    {
        utilityAttackName.text = a.name;
        utilityAttackDescription.text = a.description;
        utilityAttackStamina.text = a.staminaCost.ToString();
        utilityAttackPower.text = a.attackPower.ToString();

        if (a.name != "-")
        {
            utilityAttackButton.interactable = true;
        }
    }
}
