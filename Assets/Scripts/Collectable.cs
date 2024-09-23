using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public int healAmount;
    public HealType healType;
    public int id;

    void Start()
    {
        if (PlayerPrefs.GetInt("CollectableCollected" + id) == 1)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider objectHit)
    {
        Debug.Log(objectHit.gameObject.tag);
        if (objectHit.gameObject.tag == "Player")
        {
            Debug.Log("Trigger enter");

            Destroy(gameObject);

            PlayerPrefs.SetInt("CollectableCollected" + id, 1);

            if (healType == HealType.Health)
            {
                objectHit.gameObject.GetComponent<Animal>().Heal(healAmount);
            }


        }

    }
}
