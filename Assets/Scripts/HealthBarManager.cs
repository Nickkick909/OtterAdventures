using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarManager : MonoBehaviour
{

    public ProgressBar healthBar;
    public int healthVal;

    // Update is called once per frame
    void Update()
    {
        if (this.healthBar != null)
            this.healthBar.BarValue = healthVal;
    }
}
