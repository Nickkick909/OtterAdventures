using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fight : MonoBehaviour
{
    public GameObject player;
    public bool fightStarted;
    public GameManager gm;

    public Animator playerAnimate;
    public Animator enemyAnimate;

    public bool attacking = false;
    // Start is called before the first frame update
    void Start()
    {
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
            Debug.Log("**************Start Fight Function Begins***********");

            this.player.transform.position = new Vector3(0, 1, 1);

            DontDestroyOnLoad(enemy.transform.parent.gameObject);
            // DontDestroyOnLoad(player);
            // DontDestroyOnLoad(gameObject);

            Debug.Log("Fight started with enemy: " + enemy.name);

            PlayerPrefs.SetFloat("PlayerX", this.player.transform.transform.position.x);
            PlayerPrefs.SetFloat("PlayerY", this.player.transform.transform.position.y);
            PlayerPrefs.SetFloat("PlayerZ", this.player.transform.transform.position.z);
            
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


        HealthBarManager enemyHB = enemy.GetComponentInChildren<HealthBarManager>();
        Debug.Log("Enemy HB" + enemyHB);
        GameObject enemyUIHealth = GameObject.Find("Fight UI/Enemy Health Bar");
        Debug.Log("Enemy UI Health: " + enemyUIHealth);
        enemyHB.healthBar = enemyUIHealth.GetComponent<ProgressBar>();
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

                        this.gm.awardExp(enemyAnimal.expWorth);

                        // Destroy(this.player);
                        SceneManager.LoadScene("Main");

                        var players = GameObject.FindGameObjectsWithTag("Player");
                        foreach (GameObject player in players) {
                            Debug.Log("Player!!");
                            Debug.Log(player);
                            Debug.Log(player.GetComponent<Animal>().exp);
                        }
                        // Destroy(enemy);

                        
                        break; // Get out of input fight loop
                    }
                }
            // }
            yield return null; //you might want to only do this check once per frame -> yield return new WaitForEndOfFrame();
        }
    }


    public void playerLightAttack() {
        if (!attacking) {
            attacking = true;
            if (!this.playerAnimate) {
                GameObject localPlayer = GameObject.FindGameObjectsWithTag("Player")[0];
                this.playerAnimate = localPlayer.GetComponent<Animator>();
            }
            Debug.Log("Light Attack Selected");

            Debug.Log("Active? "+gameObject.activeInHierarchy);
            gameObject.SetActive(true);
            Debug.Log("Active? "+gameObject.activeInHierarchy);


            StartCoroutine(playAttackAnimations());

            Debug.Log(GameObject.Find("Game Manager"));
        }
        
       
    }

    IEnumerator playAttackAnimations() {
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

            checkEnemyDead(enemyAnimal, playerObj);

            if (this.enemyAnimate){
                this.enemyAnimate.SetTrigger("BasicAttack");
                yield return new WaitWhile(() => enemyAnimate.GetCurrentAnimatorStateInfo(0).IsName("BasicAttack"));
                playerAnimal.currentHealth -= Mathf.RoundToInt(enemyAnimal.attackStat); 
            }

        } else if (playerAnimal.speedStat < enemyAnimal.speedStat) {
            // Enemy moves first
            this.enemyAnimate.SetTrigger("BasicAttack");
            yield return new WaitWhile(() => enemyAnimate.GetCurrentAnimatorStateInfo(0).IsName("BasicAttack"));
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

            checkEnemyDead(enemyAnimal, playerObj);

            if (this.enemyAnimate){
                this.enemyAnimate.SetTrigger("BasicAttack");
                yield return new WaitWhile(() => playerAnimate.GetCurrentAnimatorStateInfo(0).IsName("BasicAttack"));
                playerAnimal.currentHealth -= Mathf.RoundToInt(enemyAnimal.attackStat); 
            }

            
            // yield return new WaitForSeconds(1);
            // }
            
        }

        attacking = false;

        
    }

    void checkEnemyDead (Animal enemyAnimal, GameObject playerObj) {
        if (enemyAnimal.currentHealth < 1) {
            Debug.Log("Enemy Defeated");
            playerObj.GetComponent<PlayerMovement>().blockMovement = false;

            GameObject.Find("Game Manager").GetComponent<GameManager>().awardExp(10);

            PlayerPrefs.SetInt("EnemyDead" + enemyAnimal.id, 1);

            SceneManager.LoadScene("Main");
        }
    }
}
