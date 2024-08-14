using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreenAttack : MonoBehaviour
{
    [SerializeField] float attackDelay;
    [SerializeField] float repeatDelay;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitForStartDelay());
    }

    IEnumerator WaitForStartDelay()
    {

        yield return new WaitForSeconds(attackDelay);

        gameObject.GetComponent<Animator>().SetTrigger("AttackTime");

        StartCoroutine(WaitForAttackDelay());
    }

    IEnumerator WaitForAttackDelay()
    {

        yield return new WaitForSeconds(repeatDelay);

        gameObject.GetComponent<Animator>().SetTrigger("AttackTime");

        StartCoroutine(WaitForAttackDelay());
    } 
}
