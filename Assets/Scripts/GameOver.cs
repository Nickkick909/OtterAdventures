using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{

    [SerializeField] GameObject otter;

    void Awake()
    {
        Animator anim = otter.GetComponent<Animator>();
        anim.Play("Sit");
        anim.Play("Eyes_Dead");
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("Main");
    }
}
