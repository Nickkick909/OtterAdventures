using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Animal : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    public float attackStat;
    public float defenceStat;
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

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("EnemyDead" + id) == 1) {
            Destroy(gameObject);
        }

        if (gameObject.tag == "Player") {
            float x = PlayerPrefs.GetFloat("PlayerX", 0);
            float y = PlayerPrefs.GetFloat("PlayerY", 4);
            float z = PlayerPrefs.GetFloat("PlayerZ", 0);

            gameObject.transform.transform.position = new Vector3(x, y, z);

            maxHealth = PlayerPrefs.GetInt("PlayerMaxHealth", 10);
            currentHealth = PlayerPrefs.GetInt("PlayerCurrentHealth", 10);

            maxStamina = PlayerPrefs.GetInt("PlayerMaxStamina", 3);
            currentStamina = PlayerPrefs.GetFloat("PlayerCurrentStamina", 3);

            attackStat = PlayerPrefs.GetFloat("PlayerAttackStat", 1);
            defenceStat = PlayerPrefs.GetFloat("PlayerDefenseStat",1);
            speedStat = PlayerPrefs.GetFloat("PlayerSpeedStat", 1);
            level = PlayerPrefs.GetInt("PlayerLevel", 1);
            exp = PlayerPrefs.GetInt("PlayerExp", 0);
            expForLevel = PlayerPrefs.GetInt("PlayerExpForLevel", 20);

            if (gm)
            {
                gm.AwardExp(0);
            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        // TextMesh[] healthbar = gameObject.GetComponentsInChildren<TextMesh>();

        // if (healthbar.Length > 0) {
        //     healthbar[0].text = this.currentHealth + "HP";
        // }
        // Debug.Log(healthbar);

        // Debug.Log(healthbar.GetComponent<TextMesh>());

        

        if (this.currentHealth < 1) {

            if (gameObject.tag == "Enemy") {
                PlayerPrefs.SetInt("EnemyDead" + id, 1);

                Destroy(gameObject);
            } else if (gameObject.tag == "Player")
            {
                PlayerPrefs.DeleteAll();

                SceneManager.LoadScene("GameOver");
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

        if (gameObject.GetComponent<PlayerUIManager>()){
            gameObject.GetComponent<PlayerUIManager>().healthVal = (int)( ((float)this.currentHealth / (float)this.maxHealth) * 100);
            gameObject.GetComponent<PlayerUIManager>().staminaVal = this.currentStamina / this.maxStamina;
        }
    }


  

    public void UseLightAttack(Attack useAttack, Animal target)
    {
        // Get info about light attack
        //Attack useAttack = GetAttackInfo("lightAttack");

        if (useAttack != null)
        {
            target.TakeDamage(useAttack.attackPower);
            UseStamina(useAttack.staminaCost);
        }
        
    }

    public void UseHeavyAttack(Attack useAttack, Animal target)
    {

    }

    public void UseUtilityAttack(Attack useAttack, Animal target)
    {
        // Get info about light attack
        //Attack useAttack = GetAttackInfo("lightAttack");

        if (useAttack != null)
        {
            target.TakeDamage(useAttack.attackPower);
            UseStamina(useAttack.staminaCost);
        }
    }

    public void UseAttackOfType(string attackType, Animal target)
    {
        foreach (Attack a in currentAttacks)
        {
            if (a.type == attackType)
            {
                if (attackType == "lightAttack")
                {
                    UseLightAttack(a, target);
                } else if (attackType == "heavyAttack")
                {
                    UseHeavyAttack(a, target);
                } else if (attackType == "utilityAttack")
                {
                    UseUtilityAttack(a, target);
                }
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
    }
    public void GiveStamina(float stamina)
    {
        currentStamina += stamina;
    }


    public void GetCurrentCondition()
    {
        Debug.Log("Checking current condition");
        int savedHealth = PlayerPrefs.GetInt("PlayerCurrentHealth");
        float savedStamina = PlayerPrefs.GetFloat("PlayerCurrentStamina");

        Debug.Log("Health: " + savedHealth.ToString() + "  Stamina: " + savedStamina.ToString());
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
