using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartFight : MonoBehaviour
{
    public GameObject gameManager;

    public bool fightAnimationStarted;

    public bool preventFightTrigger = true;

    void Start()
    {
        this.gameManager = GameObject.Find("Game Manager");
        preventFightTrigger = true;
        StartCoroutine(StartGameImmunity());
    }

    void OnTriggerEnter(Collider objectName)
    {   
        if (objectName.gameObject.tag == "Enemy")
        {
            if (!this.preventFightTrigger)
            {
                preventFightTrigger = true;
                gameObject.GetComponent<PlayerMovement>().blockMovement = true;

                Animator enemyAnimator = objectName.gameObject.GetComponent<Animator>();
                enemyAnimator.SetTrigger("Start Fight");

                StartCoroutine(WaitForAnimation(objectName));
            }
        }
        

    }

    IEnumerator StartGameImmunity()
    {
        yield return new WaitForSeconds(0.5f);
        this.preventFightTrigger = false;
    }

    IEnumerator WaitForAnimation(Collider objectName) {
        Animator enemyAnimator = objectName.gameObject.GetComponent<Animator>();
        yield return new WaitUntil(() => enemyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f );
        yield return new WaitWhile(() => enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Battle Start Jump"));

        Animal enemyAnimal = objectName.gameObject.GetComponent<Animal>();

        Animal playerAnimal = gameObject.GetComponent<Animal>();
        // Save current player stats to transfer to the fight scene
        PlayerPrefs.SetInt("PlayerMaxHealth", playerAnimal.maxHealth);
        PlayerPrefs.SetInt("PlayerCurrentHealth", playerAnimal.currentHealth);
        PlayerPrefs.SetFloat("PlayerMaxStamina", playerAnimal.maxStamina);
        PlayerPrefs.SetFloat("PlayerCurrentStamina", playerAnimal.currentStamina);
        PlayerPrefs.SetFloat("PlayerAttackStat", playerAnimal.attackStat);
        PlayerPrefs.SetFloat("PlayerSpeedStat", playerAnimal.speedStat);
        PlayerPrefs.SetInt("PlayerLevel", playerAnimal.level);
        PlayerPrefs.SetInt("PlayerExp", playerAnimal.exp);
        PlayerPrefs.SetInt("PlayerExpForLevel", playerAnimal.expForLevel);
        


        int enemyId = enemyAnimal.id;

        if (PlayerPrefs.GetInt("EnemyDead"+enemyId) != 1) {
            // Not killed so start a fight
            gameManager.GetComponent<Fight>().StartFight(objectName.gameObject);
        }

        preventFightTrigger = false;

    }


}
