using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Viapix_PlayerParams;

namespace Viapix_HealingItem
{
    public class Viapix_HealingItem : MonoBehaviour
    {
        [SerializeField]
        float rotationSpeedX, rotationSpeedY, rotationSpeedZ;


        void Update()
        {
            transform.Rotate(rotationSpeedX, rotationSpeedY, rotationSpeedZ);
        }

    }
}

