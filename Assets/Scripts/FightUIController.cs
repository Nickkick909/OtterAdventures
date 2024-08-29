using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightUIController : MonoBehaviour
{
    [SerializeField] GameObject fightStarted;
    [SerializeField] GameObject chooseAttack;

    void Start()
    {
        fightStarted.SetActive(true);
        HideChooseAttack();
        StartCoroutine(HideFightStarted());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator HideFightStarted() {
        yield return new WaitForSeconds(1.5f);
        fightStarted.SetActive(false);

        ShowChooseAttack();
    }

    public void HideChooseAttack()
    {
        chooseAttack.SetActive(false);
    }

    public void ShowChooseAttack()
    {
        chooseAttack.SetActive(true);
    }


}
