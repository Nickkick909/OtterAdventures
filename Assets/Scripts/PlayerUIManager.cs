using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{

    public ProgressBar healthBar;
    public float healthVal;

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

    private float slowHealthVal;
    private float healthTimer = 0;

    private float slowStaminaVal;
    private float staminaTimer = 0;

    private float healthBarAnimationTime= 100f;
    private float staminaBarAnimationTime = 1f;


    public bool healthBarAtZero = false;

    void Start()
    {
        healthBarAtZero = false;
    }
    void Update()
    {
        if (this.healthBar != null)
        {
            //this.healthBar.BarValue = healthVal;
            //Debug.Log("Slow health: " + slowHealthVal + " -- Health val: " + healthVal + " --- bar value: " + healthBar.BarValue + " -- health timer: " + healthTimer);
            if (slowHealthVal != healthVal)
            {
                slowHealthVal = Mathf.MoveTowards(slowHealthVal, healthVal, healthBarAnimationTime * Time.deltaTime);
                healthTimer += Time.deltaTime;
                //Debug.Log("Not there");

                healthBarAtZero = false;
            }
            else
            {
                healthTimer = 0;
                //resetting interpolator
                //Debug.Log("Here?");

                if (healthBar.BarValue == 0)
                {
                    //Time.timeScale = 0;
                    healthBarAtZero = true;
                } else
                {
                    healthBarAtZero = false;
                }
            }

            healthBar.BarValue = slowHealthVal;

            

            if (staminaBar != null)
            {
                if (slowStaminaVal != staminaVal)
                {
                    slowStaminaVal = Mathf.MoveTowards(slowStaminaVal, staminaVal, staminaBarAnimationTime * Time.deltaTime);
                    staminaTimer += Time.deltaTime;
                }
                else
                {
                    staminaTimer = 0;
                    //resetting interpolator
                }

                staminaBar.fillAmount = slowStaminaVal;
            }
                //staminaBar.fillAmount = slowHealthVal;
        }


        
    }
    public void UpdateHealthBar(float newVal)
    {
        
        if ( healthVal != newVal)
        {
            healthVal = newVal;
        }
    }

    public void UpdateStaminaBar(float newVal)
    {

        if (staminaVal != newVal)
        {
            staminaVal = newVal;
        }
    }
    IEnumerator MoveSlowly()
    {
        yield return null;
    }

    public void SetLevel(int level) {
        levelTMP.text = "Lvl: " + level.ToString();
    }

    public void SetExp(int exp, int maxExp) {
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
        utilityAttackStamina.text = a.attackUses.ToString();
        utilityAttackPower.text = a.healPower.ToString();

        if (a.name != "-")
        {
            utilityAttackButton.interactable = true;
        }
    }

    public void DisableAttacks(AttackType attackToDisable)
    {
        if (attackToDisable == AttackType.Light)
        {
            lightAttackButton.interactable = false;
        } else if (attackToDisable == AttackType.Heavy)
        {
            heavyAttackButton.interactable = false;
        } else if (attackToDisable == AttackType.Utility)
        {
            utilityAttackButton.interactable = false;
        }
    }

    public void EnableAttacks(AttackType attackToDisable)
    {
        if (attackToDisable == AttackType.Light)
        {
            lightAttackButton.interactable = true;
        }
        else if (attackToDisable == AttackType.Heavy && heavyAttackName.text != "-")
        {
            heavyAttackButton.interactable = true;
        }
        else if (attackToDisable == AttackType.Utility && utilityAttackName.text != "-")
        {
            utilityAttackButton.interactable = true;
        }
    }

   
}
