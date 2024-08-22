using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Fight : MonoBehaviour
{
    public GameObject player;
    public GameObject enemy;
    public bool fightStarted;
    public GameManager gm;

    public Animator playerAnimate;
    public Animator enemyAnimate;

    public bool attacking = false;


    void Start()
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;

        SceneManager.sceneUnloaded += OnSceneUnloaded;

        fightStarted = false;
        attacking = false;
        this.gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
        this.player = GameObject.FindGameObjectsWithTag("Player")[0];

        this.playerAnimate = this.player.GetComponent<Animator>();

        // if (this.fightStarted) {
        //     this.player.GetComponent<PlayerMovement>().blockMovement = true;
        //     StartCoroutine(WaitForKeyDown(enemy));
        // }
    }

    // Update is called once per frame
    void Update()
    {
        this.player = GameObject.FindGameObjectsWithTag("Player")[0];
        // this.playerAnimate = this.player.GetComponent<Animator>();
    }

    public void startFight(GameObject enemy) {
        if (!fightStarted) {
            fightStarted = true;
             enemy.GetComponent<Collider>().enabled = false;

            // Save player's current postion
            PlayerPrefs.SetFloat("PlayerX", this.player.transform.transform.position.x);
            PlayerPrefs.SetFloat("PlayerY", this.player.transform.transform.position.y);
            PlayerPrefs.SetFloat("PlayerZ", this.player.transform.transform.position.z);
            Debug.Log("**************Start Fight Function Begins***********");

            this.player.transform.position = new Vector3(0, 1, 1);

            DontDestroyOnLoad(enemy.transform.parent.gameObject);
            // DontDestroyOnLoad(player);
            // DontDestroyOnLoad(gameObject);

            Debug.Log("Fight started with enemy: " + enemy.name);

            
            
            StartCoroutine(LoadYourAsyncScene(enemy.transform.parent.gameObject));

            // SceneManager.LoadScene("Battle");

            // SceneManger
        
            
            
            // StartCoroutine(WaitForKeyDown(enemy, this.player));
        }
       
    }

    IEnumerator LoadYourAsyncScene(GameObject enemy)
    {
        // Set the current Scene to be able to unload it later
        Scene currentScene = SceneManager.GetActiveScene();

        // The Application loads the Scene in the background at the same time as the current Scene.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Battle", LoadSceneMode.Additive);

        // Wait until the last operation fully loads to return anything
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Move the GameObject (you attach this in the Inspector) to the newly loaded Scene
        SceneManager.MoveGameObjectToScene(enemy, SceneManager.GetSceneByName("Battle"));

        


        // Unload the previous Scene
        SceneManager.UnloadSceneAsync(currentScene);


        PlayerUIManager enemyHB = enemy.GetComponentInChildren<PlayerUIManager>();
        Debug.Log("Enemy HB" + enemyHB);
        GameObject enemyUIHealth = GameObject.Find("Fight UI/Enemy Health Bar");
        Debug.Log("Enemy UI Health: " + enemyUIHealth);
        enemyHB.healthBar = enemyUIHealth.GetComponent<ProgressBar>();

        GameObject enemyStaminaBar = GameObject.Find("Fight UI/Enemy Health Bar/Stamina");
        Image enemyStaminaBarImg = enemyStaminaBar.GetComponent<Image>();
        enemy.GetComponentInChildren<PlayerUIManager>().staminaBar = enemyStaminaBarImg;

        enemy.transform.position = new Vector3(this.player.transform.position.x, 0, this.player.transform.position.z + 10);
        enemy.transform.eulerAngles = new Vector3(0,180,0);

        
    }

    IEnumerator WaitForKeyDown(GameObject enemy, GameObject playerObj) {
        
        bool pressed = false;

        bool playerWon = false;
        int expAward = 0;

        Debug.Log("Enemy found:" + enemy.name);

        Debug.Log("Player found:" + playerObj.name);

        Debug.Log("*********************");
        while (!pressed) {
            if (enemy == null) {
                enemy = GameObject.FindGameObjectsWithTag("Enemy")[0];
            }
            if (playerObj == null) {
                playerObj = GameObject.FindGameObjectsWithTag("Player")[0];
                player = playerObj;
            }
            
            // foreach (KeyCode k in codes) {
                if (Input.GetKey("space")) {
                    Debug.Log("Enemy found:");
                    Debug.Log(enemy);
                    Debug.Log("Player found:");
                    Debug.Log(playerObj);
                    pressed = true;

                    Animal enemyAnimal = enemy.GetComponent<Animal>();
                    Animal playerAnimal = playerObj.GetComponent<Animal>();

                    if (playerAnimal.speedStat > enemyAnimal.speedStat) {
                        // Player moves first

                        enemyAnimal.currentHealth -= Mathf.RoundToInt(playerAnimal.attackStat);
                        yield return new WaitForSeconds(1);

                        playerAnimal.currentHealth -= Mathf.RoundToInt(enemyAnimal.attackStat);
                        yield return new WaitForSeconds(1);

                    } else if (playerAnimal.speedStat < enemyAnimal.speedStat) {
                        // Enemy moves first
                        playerAnimal.currentHealth -= Mathf.RoundToInt(enemyAnimal.attackStat);
                        yield return new WaitForSeconds(1);

                        enemyAnimal.currentHealth -= Mathf.RoundToInt(playerAnimal.attackStat);
                        yield return new WaitForSeconds(1);
                    } else {
                        // Speed tie

                        // Player moves first

                        enemyAnimal.currentHealth -= Mathf.RoundToInt(playerAnimal.attackStat);
                        yield return new WaitForSeconds(1);

                        playerAnimal.currentHealth -= Mathf.RoundToInt(enemyAnimal.attackStat);
                        yield return new WaitForSeconds(1);
                    }
                    
                    
                    pressed = false;

                    if (enemy == null) {
                        Debug.Log("Enemy Defeated");
                        playerObj.GetComponent<PlayerMovement>().blockMovement = false;

                        this.gm.AwardExp(enemyAnimal.expWorth);

                        // Destroy(this.player);
                        // SceneManager.LoadScene("Main");

                        // var players = GameObject.FindGameObjectsWithTag("Player");
                        // foreach (GameObject player in players) {
                        //     Debug.Log("Player!!");
                        //     Debug.Log(player);
                        //     Debug.Log(player.GetComponent<Animal>().exp);
                        // }
                        // // Destroy(enemy);

                        
                        break; // Get out of input fight loop
                    }
                }
            // }
            yield return null; //you might want to only do this check once per frame -> yield return new WaitForEndOfFrame();
        }
    }


    public void playerAttack(int power) {
        Debug.Log("Attack power: " + power);
    }
    public void PlayerLightAttack() {
        if (!attacking) {
            attacking = true;
            if (!this.playerAnimate) {
                GameObject localPlayer = GameObject.FindGameObjectsWithTag("Player")[0];
                this.playerAnimate = localPlayer.GetComponent<Animator>();
            }


            //gameObject.SetActive(true);


            StartCoroutine(PlayAttackAnimations());

        }
    }

    public void PlayerUtilityAttack(int power, int stamina)
    {

    }

    public void EnemyLightAttack()
    {

    }

    public void StartTurnAttacks(string playerAttackType)
    {
        // Check if player is currently attacking to prevent multiple attacks
        if (!attacking)
        {
            attacking = true;

            if (!this.playerAnimate)
            {
                player = GameObject.FindGameObjectsWithTag("Player")[0];
                this.playerAnimate = player.GetComponent<Animator>();
            }

            enemy = GameObject.FindGameObjectsWithTag("Enemy")[0];

            // Check each animal's speed
            Animal playerAnimal = player.GetComponent<Animal>();
            Animal enemyAnimal = enemy.GetComponent<Animal>();

            // Player wins in speed tie
            if (playerAnimal.speedStat >= enemyAnimal.speedStat)
            {
                StartCoroutine(PlayAttacksAndWait("player", playerAttackType, playerAnimal, enemyAnimal));
            }
            else
            {
                StartCoroutine(PlayAttacksAndWait("player", playerAttackType, playerAnimal, enemyAnimal));
            }
        }


    }

    IEnumerator PlayAttacksAndWait(string fasterAnimal, string playerAttackType, Animal playerAnimal, Animal enemyAnimal)
    {
        Debug.Log("Starting attack turns");
        if (fasterAnimal == "player")
        {
            yield return StartCoroutine(TriggerPlayerAttack(playerAttackType, playerAnimal, enemyAnimal));

            yield return StartCoroutine(TriggerEnemyAttack(playerAnimal, enemyAnimal));
        } else
        {
            yield return StartCoroutine(TriggerEnemyAttack(playerAnimal, enemyAnimal));
            
            yield return StartCoroutine(TriggerPlayerAttack(playerAttackType, playerAnimal, enemyAnimal));

            
        }

        attacking = false;

        yield return null;
    }

    IEnumerator TriggerPlayerAttack(string playerAttackType, Animal playerAnimal, Animal enemyAnimal)
    {
        this.playerAnimate.SetTrigger("DropKick");
        yield return new WaitUntil(() => playerAnimate.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);
        yield return new WaitWhile(() => playerAnimate.GetCurrentAnimatorStateInfo(0).IsName("DropKick"));
        enemyAnimal.currentHealth -= Mathf.RoundToInt(playerAnimal.attackStat);
        playerAnimal.currentStamina -= 1;

        CheckEnemyDead(enemyAnimal, playerAnimal.gameObject);
    }

    IEnumerator TriggerEnemyAttack(Animal playerAnimal, Animal enemyAnimal)
    {
        this.enemyAnimate = enemyAnimal.gameObject.GetComponent<Animator>();
        this.enemyAnimate.SetTrigger("BasicAttack");
        yield return new WaitUntil(() => enemyAnimate.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);
        yield return new WaitWhile(() => enemyAnimate.GetCurrentAnimatorStateInfo(0).IsName("Enemy Basic Attack"));
        playerAnimal.currentHealth -= Mathf.RoundToInt(enemyAnimal.attackStat);
        enemyAnimal.currentStamina -= 1;
    }


    IEnumerator PlayAttackAnimations() {
        GameObject enemy = GameObject.FindGameObjectsWithTag("Enemy")[0];
        GameObject playerObj = GameObject.FindGameObjectsWithTag("Player")[0];

        this.enemyAnimate = enemy.GetComponent<Animator>();

        Animal enemyAnimal = enemy.GetComponent<Animal>();
        Animal playerAnimal = playerObj.GetComponent<Animal>();

        if (playerAnimal.speedStat > enemyAnimal.speedStat) {
            // Player moves first
            this.playerAnimate.SetTrigger("DropKick");
            yield return new WaitUntil(() => playerAnimate.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f );
            yield return new WaitWhile(() => playerAnimate.GetCurrentAnimatorStateInfo(0).IsName("DropKick"));
            enemyAnimal.currentHealth -= Mathf.RoundToInt(playerAnimal.attackStat);

            CheckEnemyDead(enemyAnimal, playerObj);

            if (this.enemyAnimate){
                this.enemyAnimate.SetTrigger("BasicAttack");
                yield return new WaitUntil(() => enemyAnimate.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f );
                yield return new WaitWhile(() => enemyAnimate.GetCurrentAnimatorStateInfo(0).IsName("Enemy Basic Attack"));
                playerAnimal.currentHealth -= Mathf.RoundToInt(enemyAnimal.attackStat); 
            }

        } else if (playerAnimal.speedStat < enemyAnimal.speedStat) {
            // Enemy moves first
            this.enemyAnimate.SetTrigger("BasicAttack");
            yield return new WaitWhile(() => enemyAnimate.GetCurrentAnimatorStateInfo(0).IsName("Enemy Basic Attack"));
            playerAnimal.currentHealth -= Mathf.RoundToInt(enemyAnimal.attackStat);

            this.playerAnimate.SetTrigger("DropKick");
            yield return new WaitWhile(() => playerAnimate.GetCurrentAnimatorStateInfo(0).IsName("DropKick"));
            enemyAnimal.currentHealth -= Mathf.RoundToInt(playerAnimal.attackStat);
            // yield return new WaitForSeconds(1);
        } else {
            // Speed tie

            // Player moves first
            this.playerAnimate.SetTrigger("DropKick");
            // yield return new WaitWhile(() => playerAnimate.GetCurrentAnimatorStateInfo(0).IsName("DropKick"));

            yield return new WaitUntil(() => playerAnimate.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f );
            yield return new WaitWhile(() => playerAnimate.GetCurrentAnimatorStateInfo(0).IsName("DropKick"));
            enemyAnimal.currentHealth -= Mathf.RoundToInt(playerAnimal.attackStat);
            // yield return new WaitForSeconds(1);

            CheckEnemyDead(enemyAnimal, playerObj);

            if (this.enemyAnimate){
                this.enemyAnimate.SetTrigger("BasicAttack");
                yield return new WaitUntil(() => enemyAnimate.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f );
                yield return new WaitWhile(() => enemyAnimate.GetCurrentAnimatorStateInfo(0).IsName("Enemy Basic Attack"));
                playerAnimal.currentHealth -= Mathf.RoundToInt(enemyAnimal.attackStat); 
            }

            
            // yield return new WaitForSeconds(1);
            // }
            
        }

        attacking = false;

        
    }

    void CheckEnemyDead (Animal enemyAnimal, GameObject playerObj) {
        if (enemyAnimal.currentHealth < 1) {
            Debug.Log("Enemy Defeated");
            playerObj.GetComponent<PlayerMovement>().blockMovement = false;

            GameObject.Find("Game Manager").GetComponent<GameManager>().AwardExp(5);

            PlayerPrefs.SetInt("EnemyDead" + enemyAnimal.id, 1);
            Animal playerAnimal = player.GetComponent<Animal>();

            PlayerPrefs.SetFloat("PlayerCurrentHealth", playerAnimal.currentHealth);
            PlayerPrefs.SetFloat("PlayerCurrentStamina", playerAnimal.currentStamina);

        }
    }

    private void OnSceneUnloaded(Scene scene)
    {
        Debug.Log("Scene loaded: " + scene.name);
        Scene currentScene = SceneManager.GetActiveScene();
        if (scene.name == "Main")
        {
            Debug.Log("Tiggering");
            SetPlayerAttacks();
        }

    }
    void SetPlayerAttacks()
    {
        Debug.Log("Setting player attacks...");

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

        Debug.Log("current attacks: " + playerAnimal.currentAttacks);
        //playerAnimal.currentAttacks.push(savedLightAttack);

        Debug.Log("player: " + localPlayer.name);

        PlayerUIManager playerUI = localPlayer.GetComponent<PlayerUIManager>();

        foreach (Attack a in playerAnimal.currentAttacks)
        {
            if (a.type == "lightAttack")
            {
                playerUI.SetLightAttack(a);
            } else if (a.type == "heavyAttack")
            {
                playerUI.SetHeavyAttack(a);
            } else if (a.type == "utilityAttack")
            {
                playerUI.SetUtilityAttack(a);
            }
        }
        
    }
}
