using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightUIController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("FightStartedPanel").SetActive(true);
        StartCoroutine(HideFightStarted());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator HideFightStarted() {
         yield return new WaitForSeconds(1.5f);
        GameObject.Find("FightStartedPanel").SetActive(false);;
    }
}
