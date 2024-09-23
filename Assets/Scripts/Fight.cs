using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class Fight : MonoBehaviour
{
    public GameObject player;
    public GameObject enemy;
    public bool fightStarted;
    public GameManager gm;

    public Animator playerAnimate;
    public Animator enemyAnimate;

    public bool attacking = false;

    [SerializeField] float staminaRegen = 0.5f;
    [SerializeField] FightUIController fightUI;

    [SerializeField] float lowestStaminaCost = 999;
    [SerializeField] int avaiableAttackUses = 0;
    bool attacksHaveBeenSet = false;

    [SerializeField] GameObject outOfStaminaUI;


    void Start()
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;

        SceneManager.sceneUnloaded += OnSceneUnloaded;

        fightStarted = false;
        attacking = false;
        this.gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
        this.player = GameObject.FindGameObjectsWithTag("Player")[0];

        //this.playerAnimate = this.player.GetComponentInChildren<Animator>();

        // if (this.fightStarted) {
        //     this.player.GetComponent<PlayerMovement>().blockMovement = true;
        //     StartCoroutine(WaitForKeyDown(enemy));
        // }
    }

    // Update is called once per frame
    void Update()
    {
        var playerArray = GameObject.FindGameObjectsWithTag("Player");
        if (playerArray.Length > 0)
        {
            player = playerArray[0];
        }
        // this.playerAnimate = this.player.GetComponent<Animator>();

        //if (attacksHaveBeenSet && (lowestStaminaCost < player.GetComponent<Animal>().currentStamina) && !attacking)
        //{
        //    StartCoroutine(PlayerOutOfStamina());
        //}
    }

    public void StartFight(GameObject enemy) {
        if (!fightStarted) {
            fightStarted = true;
             enemy.GetComponent<Collider>().enabled = false;

            // Save player's current postion
            PlayerPrefs.SetFloat("PlayerX", this.player.transform.transform.position.x);
            PlayerPrefs.SetFloat("PlayerY", this.player.transform.transform.position.y);
            PlayerPrefs.SetFloat("PlayerZ", this.player.transform.transform.position.z);

            DontDestroyOnLoad(enemy.transform.parent.gameObject);
            
            StartCoroutine(LoadYourAsyncScene(enemy.transform.parent.gameObject, this.player));
        }
       
    }

    IEnumerator LoadYourAsyncScene(GameObject enemy, GameObject player)
    {
        enemy.transform.SetParent(null);

        // Set the current Scene to be able to unload it later
        Scene currentScene = SceneManager.GetActiveScene();

        // The Application loads the Scene in the background at the same time as the current Scene.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Battle", LoadSceneMode.Additive);


        // Wait until the last operation fully loads to return anything
        while (!asyncLoad.isDone)
        {
            yield return null;
        }


        player.transform.position = new Vector3(0, 1, 1);

        
        // Move the GameObject (you attach this in the Inspector) to the newly loaded Scene
        SceneManager.MoveGameObjectToScene(enemy, SceneManager.GetSceneByName("Battle"));


        // Unload the previous Scene
        SceneManager.UnloadSceneAsync(currentScene);


        PlayerUIManager enemyHB = enemy.GetComponentInChildren<PlayerUIManager>();
        GameObject enemyUIHealth = GameObject.Find("Fight UI/Enemy Health Bar");
        enemyHB.healthBar = enemyUIHealth.GetComponent<ProgressBar>();

        GameObject enemyStaminaBar = GameObject.Find("Fight UI/Enemy Health Bar/Stamina");
        Image enemyStaminaBarImg = enemyStaminaBar.GetComponent<Image>();
        enemy.GetComponentInChildren<PlayerUIManager>().staminaBar = enemyStaminaBarImg;

        enemy.transform.position = new Vector3(this.player.transform.position.x, 0, this.player.transform.position.z + 10);
        enemy.transform.eulerAngles = new Vector3(0,180,0);

        
    }

    IEnumerator PlayerOutOfStamina()
    {
        attacking = true;
        fightUI.HideChooseAttack();

        // show out of stamina ui
        outOfStaminaUI.SetActive(true);

        yield return new WaitForSeconds(2);
        outOfStaminaUI.SetActive(false);
        yield return new WaitForSeconds(0.5f);


        Animal playerAnimal = player.GetComponent<Animal>();
        Animal enemyAnimal = enemy.GetComponent<Animal>();

        yield return StartCoroutine(TriggerEnemyAttack(playerAnimal, enemyAnimal));

        

        yield return null;

        playerAnimal.GiveStamina(staminaRegen);
        enemyAnimal.GiveStamina(staminaRegen);

        fightUI.ShowChooseAttack();
        CheckStaminaForAttacks();

        attacking = false;

        if (playerAnimal.currentStamina < lowestStaminaCost)
        {
            StartCoroutine(PlayerOutOfStamina());
        }
    }

    public void StartTurnAttacks(string playerAttackType)
    {
        fightUI.HideChooseAttack();
        // Check if player is currently attacking to prevent multiple attacks
        if (!attacking)
        {
            attacking = true;


            if (!this.playerAnimate)
            {
                player = GameObject.FindGameObjectsWithTag("Player")[0];
                this.playerAnimate = player.GetComponentInChildren<Animator>();
            }

            enemy = GameObject.FindGameObjectsWithTag("Enemy")[0];

            // Check each animal's speed
            Animal playerAnimal = player.GetComponent<Animal>();
            Animal enemyAnimal = enemy.GetComponent<Animal>();

            AttackType attackTypeE = (AttackType)Enum.Parse(typeof(AttackType), playerAttackType);

            // Player wins in speed tie
            if (playerAnimal.speedStat >= enemyAnimal.speedStat)
            {
                StartCoroutine(PlayAttacksAndWait("player", attackTypeE, playerAnimal, enemyAnimal));
            }
            else
            {
                StartCoroutine(PlayAttacksAndWait("enemy", attackTypeE, playerAnimal, enemyAnimal));
            }
           
        }

        
    }

    IEnumerator PlayAttacksAndWait(string fasterAnimal, AttackType playerAttackType, Animal playerAnimal, Animal enemyAnimal)
    {
        if (fasterAnimal == "player")
        {
            yield return StartCoroutine(TriggerPlayerAttack(playerAttackType, playerAnimal, enemyAnimal));
            yield return StartCoroutine(CheckEnemyDead(enemyAnimal, playerAnimal.gameObject));

            if (enemyAnimal != null)
            {
                yield return StartCoroutine(TriggerEnemyAttack(playerAnimal, enemyAnimal));
            }
        } else
        {
            yield return StartCoroutine(TriggerEnemyAttack(playerAnimal, enemyAnimal));
            
            if (playerAnimal != null)
            {
                yield return StartCoroutine(TriggerPlayerAttack(playerAttackType, playerAnimal, enemyAnimal));
            }

            yield return StartCoroutine(CheckEnemyDead(enemyAnimal, playerAnimal.gameObject));


        }

        

        

        playerAnimal.GiveStamina(staminaRegen);
        enemyAnimal.GiveStamina(staminaRegen);

        fightUI.ShowChooseAttack();
        CheckStaminaForAttacks();

        attacking = false;

        if (playerAnimal.currentStamina < lowestStaminaCost && avaiableAttackUses == 0)
        {
            StartCoroutine(PlayerOutOfStamina());
        }

        yield return null;
    }

    IEnumerator TriggerPlayerAttack(AttackType playerAttackType, Animal playerAnimal, Animal enemyAnimal)
    {
        yield return StartCoroutine(playerAnimal.UseAttackOfType(playerAttackType, enemyAnimal));
        
    }

    IEnumerator TriggerEnemyAttack(Animal playerAnimal, Animal enemyAnimal)
    {

        // TODO: add in AI for picking move type
        yield return StartCoroutine(enemyAnimal.UseAttackOfType(AttackType.Light, playerAnimal));

    }

    IEnumerator CheckEnemyDead (Animal enemyAnimal, GameObject playerObj) {
        if (enemyAnimal.currentHealth < 1) {
            PlayerUIManager healthBar = enemyAnimal.gameObject.GetComponent<PlayerUIManager>();
            yield return new WaitUntil(() => healthBar.healthBarAtZero == true);

            playerObj.GetComponent<PlayerMovement>().blockMovement = false;

            GameObject.Find("Game Manager").GetComponent<GameManager>().AwardExp(5);

            PlayerPrefs.SetInt("EnemyDead" + enemyAnimal.id, 1);
            Animal playerAnimal = player.GetComponent<Animal>();

            playerAnimal.GiveStamina(1);

            PlayerPrefs.SetInt("PlayerCurrentHealth", playerAnimal.currentHealth);
            PlayerPrefs.SetFloat("PlayerCurrentStamina", playerAnimal.currentStamina);

            

        }
        else
        {
            yield return null;
        }
    }


    private void OnSceneUnloaded(Scene scene)
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (scene.name == "Main")
        {
            SetPlayerAttacks();
        }

    }
    void SetPlayerAttacks()
    {
        lowestStaminaCost = 999;

        Attack savedLightAttack = JsonUtility.FromJson<Attack>(PlayerPrefs.GetString("PlayerLightAttack"));
        Attack savedHeavytack = JsonUtility.FromJson<Attack>(PlayerPrefs.GetString("PlayerHeavyAttack"));
        Attack savedUtilitytAttack = JsonUtility.FromJson<Attack>(PlayerPrefs.GetString("PlayerUtilityAttack"));


        GameObject localPlayer = GameObject.FindGameObjectsWithTag("Player")[0];
        Animal playerAnimal = localPlayer.GetComponent<Animal>();

        //playerAnimal.currentAttacks;
        if (savedLightAttack != null)
        {
            playerAnimal.currentAttacks[0] = savedLightAttack;
        }

        if (savedHeavytack != null)
        {
            playerAnimal.currentAttacks[1] = savedHeavytack;
        }

        if (savedUtilitytAttack != null)
        {
            playerAnimal.currentAttacks[2] = savedUtilitytAttack;
        }


        PlayerUIManager playerUI = localPlayer.GetComponent<PlayerUIManager>();

        foreach (Attack a in playerAnimal.currentAttacks)
        {
            if (a.type == AttackType.Light)
            {
                playerUI.SetLightAttack(a);
            } else if (a.type == AttackType.Heavy)
            {
                playerUI.SetHeavyAttack(a);
            } else if (a.type == AttackType.Utility)
            {
                playerUI.SetUtilityAttack(a);
            }

            if (a.name != "-" && (a.staminaCost < lowestStaminaCost) && a.staminaCost != 0)
            {
                lowestStaminaCost = a.staminaCost;
            }
        }


        CheckStaminaForAttacks();
    }

    void CheckStaminaForAttacks()
    {
        avaiableAttackUses = 0;
        GameObject localPlayer = GameObject.FindGameObjectsWithTag("Player")[0];
        Animal playerAnimal = localPlayer.GetComponent<Animal>();

        float currentStamina = playerAnimal.currentStamina;
        Attack[] playerAttacks = playerAnimal.currentAttacks;
        PlayerUIManager playerUI = localPlayer.GetComponent<PlayerUIManager>();


        foreach (Attack a in playerAttacks)
        {
            if (a.staminaCost > 0)
            {
                if (currentStamina < a.staminaCost)
                {
                    playerUI.DisableAttacks(a.type);
                }
                else
                {
                    playerUI.EnableAttacks(a.type);
                }
            } else
            {
                avaiableAttackUses += a.attackUses;

                if (a.attackUses > 0)
                {
                    playerUI.EnableAttacks(a.type);

                } else
                {
                    
                    playerUI.DisableAttacks(a.type);
                }
            }
            
        }

    }
}
