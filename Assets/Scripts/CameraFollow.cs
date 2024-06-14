using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;

    public float cameraOffsetY;
    public float cameraOffsetZ;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.transform.transform.position.x, cameraOffsetY, player.transform.transform.position.z - cameraOffsetZ);
    }
}
