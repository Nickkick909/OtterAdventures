using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartFight : MonoBehaviour
{
    public GameObject gameManager;

    public bool fightAnimationStarted;
    public

    // Start is called before the first frame update
    void Start()
    {
        this.gameManager = GameObject.Find("Game Manager");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     // Gets called when the object enters the collider area 
    void OnTriggerEnter(Collider objectName)
    {   
        gameObject.GetComponent<PlayerMovement>().blockMovement = true;

        Animator enemyAnimator = objectName.gameObject.GetComponent<Animator>();
        enemyAnimator.SetTrigger("Start Fight");

        StartCoroutine(WaitForAnimation(objectName));

        

    }

    IEnumerator WaitForAnimation(Collider objectName) {
        Animator enemyAnimator = objectName.gameObject.GetComponent<Animator>();
        yield return new WaitUntil(() => enemyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f );
        yield return new WaitWhile(() => enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Battle Start Jump"));

Debug.Log("Start fight 123");
        Animal enemyAnimal = objectName.gameObject.GetComponent<Animal>();

        Animal playerAnimal = gameObject.GetComponent<Animal>();
        // Save current player stats to transfer to the fight scene
        PlayerPrefs.SetInt("PlayerMaxHealth", playerAnimal.maxHealth);
        PlayerPrefs.SetInt("PlayerCurrentHealth", playerAnimal.currentHealth);
        PlayerPrefs.SetFloat("PlayerAttackStat", playerAnimal.attackStat);
        PlayerPrefs.SetFloat("PlayerDefenceStat", playerAnimal.defenceStat);
        PlayerPrefs.SetFloat("PlayerSpeedStat", playerAnimal.speedStat);
        PlayerPrefs.SetInt("PlayerLevel", playerAnimal.level);
        PlayerPrefs.SetInt("PlayerExp", playerAnimal.exp);
        PlayerPrefs.SetInt("PlayerExpForLevel", playerAnimal.expForLevel);
        

        Debug.Log("Entered collision with " + objectName.gameObject.name);

        int enemyId = enemyAnimal.id;

        if (PlayerPrefs.GetInt("EnemyDead"+enemyId) == 1) {
            Debug.Log("Enemy already killed");
        } else {
            // Not killed so start a fight
            gameManager.GetComponent<Fight>().startFight(objectName.gameObject);
        }

    }

    public void goToFightAfterAnimation ()  {

    }


}
