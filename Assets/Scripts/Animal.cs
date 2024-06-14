using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    public float attackStat;
    public float defenceStat;
    public float speedStat;

    public int level;
    public int exp;
    public int expWorth;
    public int expForLevel;

    public int id;

    public Attack[] currentAttacks;
    public Attack[] learnableAttacks;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("EnemyDead" + id) == 1) {
            Destroy(gameObject);
        }

        if (gameObject.tag == "Player") {
            float x = PlayerPrefs.GetFloat("PlayerX", 0);
            float y = PlayerPrefs.GetFloat("PlayerY", 1);
            float z = PlayerPrefs.GetFloat("PlayerZ", 0);

            gameObject.transform.transform.position = new Vector3(x, y, z);

            maxHealth = PlayerPrefs.GetInt("PlayerMaxHealth", 10);
            currentHealth = PlayerPrefs.GetInt("PlayerCurrentHealth", 10);

            attackStat = PlayerPrefs.GetFloat("PlayerAttackStat", 1);
            defenceStat = PlayerPrefs.GetFloat("PlayerDefenseStat",1);
            speedStat = PlayerPrefs.GetFloat("PlayerSpeedStat", 1);
            level = PlayerPrefs.GetInt("PlayerLevel", 1);
            exp = PlayerPrefs.GetInt("PlayerExp", 0);
            expForLevel = PlayerPrefs.GetInt("PlayerExpForLevel", 10);
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
            }

            Destroy(gameObject);
        }

        if (PlayerPrefs.GetInt("EnemyDead" + id) == 1) {
            Destroy(gameObject);
        }

        if (gameObject.tag == "Player") {
            GameObject gm = GameObject.Find("Game Manager");

            gm.GetComponent<GameManager>().player = gameObject;
        }

        if (gameObject.GetComponent<HealthBarManager>()){
            gameObject.GetComponent<HealthBarManager>().healthVal = (int)( ((float)this.currentHealth / (float)this.maxHealth) * 100);
        }
    }

    public void PrintFloat (float theValue) {
        Debug.Log ("PrintFloat is called with a value of " + theValue);
    }
    
}
