using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed;
    public bool blockMovement = false;
    Rigidbody rb;

    public Animator playerAnimator;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        playerAnimator = gameObject.GetComponentsInChildren<Animator>()[1];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!this.blockMovement) {
            var gamepad = Gamepad.current;
            if (gamepad == null)
                return; // No gamepad connected.

            if (gamepad.rightTrigger.wasPressedThisFrame)
            {
                // 'Use' code here
            }

            Vector2 move = gamepad.leftStick.ReadValue();

            bool isWalking = Mathf.Abs(move.x) > 0.1 || Mathf.Abs(move.y) > 0.1;
            bool isRunning = Mathf.Abs(move.x) > 0.5 || Mathf.Abs(move.y) > 0.5;
            
            playerAnimator.SetBool("isRunning", isRunning);
            playerAnimator.SetBool("isWalking", isWalking);

            

            // transform.position = new Vector3(transform.position.x + (move.x * this.movementSpeed), transform.position.y, transform.position.z + (move.y * this.movementSpeed));
            if (move.x != 0 || move.y != 0) {
                
            
                Vector3 m_Input = new Vector3(move.x, 0, move.y);

                if (move.y >= 0) {
                    gameObject.transform.eulerAngles = new Vector3(
                        gameObject.transform.eulerAngles.x,
                        90f * move.x,
                        gameObject.transform.eulerAngles.z
                    );
                } else {
                    gameObject.transform.eulerAngles = new Vector3(
                        gameObject.transform.eulerAngles.x,
                        180 - (90f * move.x),
                        gameObject.transform.eulerAngles.z
                    );
                    
                }

            
                

                rb.MovePosition(transform.position + m_Input * Time.deltaTime * movementSpeed);
            }
        }
        

        // Using KeyControl property directly.
        // Keyboard.current.spaceKey.isPressed
        // Keyboard.current.aKey.isPressed // etc.
    }
}
