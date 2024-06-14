using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject activeEnemy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void awardExp(int exp) {
        var playerAnimal = this.player.GetComponent<Animal>();

        playerAnimal.exp += exp;
        PlayerPrefs.SetInt("PlayerExp", playerAnimal.exp);
        Debug.Log("Experince awarded to player: " + exp);
        Debug.Log(playerAnimal.exp);
    }
}
