using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject activeEnemy;

    public PlayerUIManager playerUI;

    public GameObject levelUpUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AwardExp(int exp) {
        Animal playerAnimal = this.player.GetComponent<Animal>();

        playerAnimal.exp += exp;
        PlayerPrefs.SetInt("PlayerExp", playerAnimal.exp);
        Debug.Log("Experince awarded to player: " + exp);
        Debug.Log(playerAnimal.exp);

        if (playerAnimal.exp >= playerAnimal.expForLevel) {
            playerAnimal.exp -= playerAnimal.expForLevel;
            playerAnimal.level += 1;

            PlayerPrefs.SetInt("PlayerExp", playerAnimal.exp);
            PlayerPrefs.SetInt("PlayerLevel", playerAnimal.level);

            playerUI.SetExp(playerAnimal.exp, playerAnimal.expForLevel);
            playerUI.SetLevel(playerAnimal.level);

            if (SceneManager.GetActiveScene().name == "Battle") {
                StartCoroutine(ShowLevelUpUI());
            }
        } else if (SceneManager.GetActiveScene().name == "Battle" ) {
            playerUI.SetExp(playerAnimal.exp, playerAnimal.expForLevel);
            playerUI.SetLevel(playerAnimal.level);

            SceneManager.LoadScene("Main");
        } else {
            playerUI.SetExp(playerAnimal.exp, playerAnimal.expForLevel);
            playerUI.SetLevel(playerAnimal.level);
        }

        

        
    }

    IEnumerator ShowLevelUpUI()
    {
        Debug.Log("Show level up start");
        levelUpUI.SetActive(true);

        yield return new WaitForSeconds(2);

        levelUpUI.SetActive(false);
        Debug.Log("Show level up end");

        StartCoroutine("CheckLevelUpMoves");
    }

    IEnumerator CheckLevelUpMoves()
    {
        Animal playerAnimal = this.player.GetComponent<Animal>();

        int currentLevel = playerAnimal.level;

        foreach (Attack a in playerAnimal.learnableAttacks)
        {
            if (a.levelUnlocked == currentLevel)
            {
                Debug.Log("level up!!: " + a.type);
                switch (a.type)
                {
                    case "lightAttack":
                        PlayerPrefs.SetString("PlayerLightAttack", JsonUtility.ToJson(a));
                        break;
                    case "heavyAttack":
                        PlayerPrefs.SetString("PlayerHeavyAttack", JsonUtility.ToJson(a));
                        break;
                    case "utilityAttack":
                        PlayerPrefs.SetString("PlayerUtilityAttack", JsonUtility.ToJson(a));
                        break;
                    default:
                        break;
                }
                    
            }
        }

        yield return null;

        SceneManager.LoadScene("Main");
    }
}
