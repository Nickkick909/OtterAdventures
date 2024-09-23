using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Animal : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    public float attackStat;
    public float speedStat;
    public float currentStamina;
    public float maxStamina;

    public int level;
    public int exp;
    public int expWorth;
    public int expForLevel;

    public int id;

    public Attack[] currentAttacks;
    public Attack[] learnableAttacks;

    public GameManager gm;

    private bool playerIsDead = false;

    PlayerUIManager playerUIManager;

    // Start is called before the first frame update
    void Start()
    {
        playerUIManager = gameObject.GetComponent<PlayerUIManager>();
        playerIsDead = false;

        if (PlayerPrefs.GetInt("EnemyDead" + id) == 1) {
            Destroy(gameObject);
        }

        if (gameObject.tag == "Player") {
            float x = PlayerPrefs.GetFloat("PlayerX", -13);
            float y = PlayerPrefs.GetFloat("PlayerY", 2);
            float z = PlayerPrefs.GetFloat("PlayerZ", -7);

            gameObject.transform.transform.position = new Vector3(x, y, z);

            maxHealth = PlayerPrefs.GetInt("PlayerMaxHealth", maxHealth);
            currentHealth = PlayerPrefs.GetInt("PlayerCurrentHealth", currentHealth);

            maxStamina = PlayerPrefs.GetFloat("PlayerMaxStamina", maxStamina);
            currentStamina = PlayerPrefs.GetFloat("PlayerCurrentStamina", currentStamina);

            attackStat = PlayerPrefs.GetFloat("PlayerAttackStat", attackStat);
            speedStat = PlayerPrefs.GetFloat("PlayerSpeedStat", speedStat);
            level = PlayerPrefs.GetInt("PlayerLevel", 1);
            exp = PlayerPrefs.GetInt("PlayerExp", 0);
            expForLevel = PlayerPrefs.GetInt("PlayerExpForLevel", expForLevel);

            if (gm)
            {
                gm.AwardExp(0);
            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerUIManager != null)
        {
            playerUIManager.UpdateHealthBar((((float)this.currentHealth / (float)this.maxHealth) * 100));
            playerUIManager.UpdateStaminaBar(this.currentStamina / this.maxStamina);
        }

        if (this.currentHealth < 1) {
            if (playerUIManager.healthBarAtZero)
            {
                if (gameObject.tag == "Enemy") {
                    PlayerPrefs.SetInt("EnemyDead" + id, 1);

                    Destroy(gameObject);
                } else if (gameObject.tag == "Player" && !playerIsDead)
                {
                    playerIsDead = true;
                    StartCoroutine("SendToGameOverScreen");
                }
            }

        }

        if (PlayerPrefs.GetInt("EnemyDead" + id) == 1) {
            Destroy(gameObject);
        }

        if (gameObject.tag == "Player") {
            GameObject gm = GameObject.Find("Game Manager");

            gm.GetComponent<GameManager>().player = gameObject;

            gm.GetComponent<GameManager>().playerUI = gameObject.GetComponent<PlayerUIManager>();
        }

        
    }


    IEnumerator SendToGameOverScreen()
    {
        yield return new WaitForSeconds(1);

        Scene currentScene = SceneManager.GetActiveScene();

        PlayerPrefs.DeleteAll();

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("GameOver");


        // Wait until the last operation fully loads to return anything
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        //yield return new WaitForSeconds(1);

        // Unload the previous Scene
        //SceneManager.UnloadSceneAsync(currentScene);
    }

    public IEnumerator UseAttackOfType(AttackType attackType, Animal target)
    {
        foreach (Attack a in currentAttacks)
        {
            if (a.type == attackType)
            {
                if (attackType == AttackType.Light)
                {
                    yield return StartCoroutine(UseLightAttack(a, target));
                }
                else if (attackType == AttackType.Heavy)
                {
                    yield return StartCoroutine(UseHeavyAttack(a, target));
                }
                else if (attackType == AttackType.Utility)
                {
                    yield return StartCoroutine(UseUtilityAttack(a, target));
                }
            }
        }

        yield return null;

    }

    public IEnumerator UseLightAttack(Attack useAttack, Animal target)
    {
        Animator playerAnimate = gameObject.GetComponent<Animator>();
        playerAnimate.SetTrigger("BasicAttack");

        yield return new WaitUntil(() => playerAnimate.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);
        yield return new WaitWhile(() => playerAnimate.GetCurrentAnimatorStateInfo(0).IsName("BasicAttack"));
        //enemyAnimal.currentHealth -= Mathf.RoundToInt(playerAnimal.attackStat);

        //playerAnimal.currentStamina -= 1;
        // Get info about light attack
        //Attack useAttack = GetAttackInfo("lightAttack");

        if (useAttack != null)
        {
            target.TakeDamage(useAttack.attackPower);
            UseStamina(useAttack.staminaCost);
        }

        yield return null;
        
    }

    public IEnumerator UseHeavyAttack(Attack useAttack, Animal target)
    {
        Animator playerAnimate = gameObject.GetComponent<Animator>();
        playerAnimate.SetTrigger("BasicAttack");

        yield return new WaitUntil(() => playerAnimate.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);
        yield return new WaitWhile(() => playerAnimate.GetCurrentAnimatorStateInfo(0).IsName("BasicAttack"));

        if (useAttack != null)
        {
            target.TakeDamage(useAttack.attackPower);
            UseStamina(useAttack.staminaCost);
        }
    }

    public IEnumerator UseUtilityAttack(Attack useAttack, Animal target)
    {
        Animator playerAnimate = gameObject.GetComponent<Animator>();
        playerAnimate.SetTrigger("UtilityAttack");

        yield return new WaitUntil(() => playerAnimate.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);
        yield return new WaitWhile(() => playerAnimate.GetCurrentAnimatorStateInfo(0).IsName("UtilityAttack"));

        if (useAttack != null)
        {
            if (useAttack.typeHeal == HealType.Stamina)
            {
                GiveStamina(useAttack.healPower);
                UseAttackUse(useAttack);
            } else if (useAttack.typeHeal == HealType.Health)
            {
                UseAttackUse(useAttack);
                Heal(useAttack.healPower);
            } else
            {
                // Heal Type == Both
                UseAttackUse(useAttack);

                GiveStamina(useAttack.healPower);
                Heal(useAttack.healPower);
            }
            
        }
    }

    

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }

    public void UseStamina(int cost)
    {
        currentStamina -= cost;
    }

    public void Heal(int hp)
    {
        currentHealth += hp;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
    public void GiveStamina(float stamina)
    {
        currentStamina += stamina;

        if (currentStamina > maxStamina)
        {
            currentStamina = maxStamina;
        }
    }

    public void UseAttackUse(Attack useAttack)
    {
        foreach (Attack a in currentAttacks)
        {
            if (a.name == useAttack.name)
            {
                Debug.Log("Lower attack uses: " + a.name);
                a.attackUses -= 1;

                playerUIManager.SetUtilityAttack(a);
            }
        }
    }

    public void LevelUp()
    {
        int currHealthChange = 0;
        int maxHealthChange = 0;
        float currStaminaChanged = 0;
        float maxStaminaChanged = 0;

        exp -= expForLevel;
        level += 1;
        expForLevel += 5;

        if (level % 5 == 0)
        {
            maxHealthChange = 5;
            currHealthChange = (maxHealth + maxHealthChange) - currentHealth;
            maxStaminaChanged = 1;
            currStaminaChanged =  (maxStamina + maxStaminaChanged) - currentStamina;
        }
        else if (level % 2 == 0)
        {
            maxHealthChange = 2;
            currHealthChange = (maxHealth + maxHealthChange) - currentHealth;
            maxStaminaChanged = 0.5f;
            currStaminaChanged = 0.5f;
        } else
        {
            maxHealthChange = 2;
            currHealthChange = 2;
            maxStaminaChanged = 0;
            currStaminaChanged = 0.5f;
        }

        maxHealth += maxHealthChange;
        currentHealth += currHealthChange;
        maxStamina += maxStaminaChanged;
        currentStamina += currStaminaChanged;

        if (currentHealth > maxHealth) { currentHealth = maxHealth; }

        // Save current attacks (mostly for saving attack uses) after the battle

        foreach (Attack a in currentAttacks)
        {
            Debug.Log("Attack: " + a.name + " uses: " + a.attackUses);
            if (a.name != "-" && a.staminaCost == 0)
            {
                a.attackUses = a.maxAttackUses;

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


        FightUIController fightUI = GameObject.FindObjectOfType<FightUIController>();

        fightUI?.SetLevelUpStats(currHealthChange, maxHealthChange, currStaminaChanged, maxStaminaChanged);



        PlayerPrefs.SetInt("PlayerMaxHealth", maxHealth);
        PlayerPrefs.SetInt("PlayerCurrentHealth", currentHealth);
        PlayerPrefs.SetFloat("PlayerMaxStamina", maxStamina);
        PlayerPrefs.SetFloat("PlayerCurrentStamina", currentStamina);
        PlayerPrefs.SetFloat("PlayerAttackStat", attackStat);
        PlayerPrefs.SetFloat("PlayerSpeedStat", speedStat);
        PlayerPrefs.SetInt("PlayerLevel", level);
        PlayerPrefs.SetInt("PlayerExp", exp);
        PlayerPrefs.SetInt("PlayerExpForLevel", expForLevel);

        playerUIManager.SetExp(exp, expForLevel);
        playerUIManager.SetLevel(level);
    }

    public void GetCurrentCondition()
    {
        int savedHealth = PlayerPrefs.GetInt("PlayerCurrentHealth");
        float savedStamina = PlayerPrefs.GetFloat("PlayerCurrentStamina");

        if (savedHealth != 0)
        {
            this.currentHealth = savedHealth;
        }

        if (savedStamina != 0)
        {
            this.currentStamina = savedStamina;
        }
        
        
    }
}
