using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FightUIController : MonoBehaviour
{
    [SerializeField] GameObject fightStarted;
    [SerializeField] GameObject chooseAttack;
    [SerializeField] GameObject levelUpUI;
    [SerializeField] GameManager gameManager;

    [SerializeField] TMP_Text attackLearnedName;
    [SerializeField] TMP_Text attackLearnedDescription;

    [SerializeField] TMP_Text currentHealthGained;
    [SerializeField] TMP_Text maxHealthGained;
    [SerializeField] TMP_Text currentStaminaGained;
    [SerializeField] TMP_Text maxStaminaGained;

    [SerializeField] GameObject attacksLearnedUI;
    [SerializeField] GameObject noAttacksLearnedUI;


    void Start()
    {
        fightStarted.SetActive(true);
        HideChooseAttack();
        StartCoroutine(HideFightStarted());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator HideFightStarted() {
        yield return new WaitForSeconds(1.5f);
        fightStarted.SetActive(false);

        ShowChooseAttack();
    }

    public void HideChooseAttack()
    {
        chooseAttack.SetActive(false);
    }

    public void ShowChooseAttack()
    {
        chooseAttack.SetActive(true);
    }

    public IEnumerator ShowLevelUpUI()
    {
        yield return StartCoroutine(CheckLevelUpMoves());
        levelUpUI.SetActive(true);

        yield return new WaitForSeconds(5);

        levelUpUI.SetActive(false);

        SceneManager.LoadScene("Main");

    }

    public IEnumerator CheckLevelUpMoves()
    {
        bool attackLearned = false;
        Animal playerAnimal = GameObject.FindGameObjectWithTag("Player").GetComponent<Animal>();

        int currentLevel = playerAnimal.level;

        // Save current attacks (mostly for saving attack uses) after the battle

        foreach (Attack a in playerAnimal.learnableAttacks)
        {
            if (a.levelUnlocked == currentLevel)
            {
                attackLearned = true;
                switch (a.type)
                {
                    case AttackType.Light:
                        PlayerPrefs.SetString("PlayerLightAttack", JsonUtility.ToJson(a));
                        break;
                    case AttackType.Heavy:
                        PlayerPrefs.SetString("PlayerHeavyAttack", JsonUtility.ToJson(a));
                        break;
                    case AttackType.Utility:
                        PlayerPrefs.SetString("PlayerUtilityAttack", JsonUtility.ToJson(a));
                        break;
                    default:
                        break;
                }


                attackLearnedName.text = a.name;
                attackLearnedDescription.text = a.type + " Attack: " + a.description;

            }
        }

        if (attackLearned)
        {
            attacksLearnedUI.SetActive(true);
            noAttacksLearnedUI.SetActive(false);
        } else
        {
            attacksLearnedUI.SetActive(false);
            noAttacksLearnedUI.SetActive(true);
        }

        

        yield return null;

        
    }

    public void SetLevelUpStats(int currHealthChange, int maxHealthChange, float currStaminaChanged, float maxStaminaChanged)
    {
        currentHealthGained.text = "+ " + currHealthChange.ToString();
        maxHealthGained.text = "+ " + maxHealthChange.ToString();
        currentStaminaGained.text = "+ " + currStaminaChanged.ToString();
        maxStaminaGained.text = "+ " + maxStaminaChanged.ToString();
    }
}
