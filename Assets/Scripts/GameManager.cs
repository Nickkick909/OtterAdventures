using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject activeEnemy;

    public PlayerUIManager playerUI;

    
    public FightUIController fightUIController;

    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Animal>().GetCurrentCondition();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AwardExp(int exp) {
        Animal playerAnimal = this.player.GetComponent<Animal>();

        // Save current attacks (mostly for saving attack uses) after the battle

        foreach (Attack a in playerAnimal.currentAttacks)
        {
            Debug.Log("Attack: " + a.name + " uses: " + a.attackUses);
            if (a.name != "-")
            {
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
            }

        }

        playerAnimal.exp += exp;
        PlayerPrefs.SetInt("PlayerExp", playerAnimal.exp);

        if (playerAnimal.exp >= playerAnimal.expForLevel) {
            playerAnimal.LevelUp();

            if (SceneManager.GetActiveScene().name == "Battle") {
                StartCoroutine(fightUIController.ShowLevelUpUI());
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

    
   
}
