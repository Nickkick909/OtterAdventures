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
        var playerAnimal = this.player.GetComponent<Animal>();

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

        SceneManager.LoadScene("Main");
    }
}
