using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Transform transform;
    public Rigidbody rb;
    public float movementSpeed = 1f;
    private Vector3 moveDirection;

    private bool isDashing = false;
    public float dashSpeed = 3f;
    public static float dashDuration = 0.2f;
    private float dashTimeRemaining;

    public Rotate rotateScript;
    //private float rotationAngle = 360f / (dashDuration / Time.deltaTime);

    [SerializeField] Sprite[] playerSprites;
    private Sprite newSprite;

    private bool touchingFloor;
    public float jumpForce = 10f;
    private float backupGravityTimer;

    void Start()
    {
        
    }

    void Update()
    {
        // Zmienia inputy na wartoœci które mo¿na wsadziæ do wektora wskazuj¹cego kierunek ruchu
        
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector3 (moveX, 0, moveZ).normalized;

        if (Input.GetKeyDown(KeyCode.LeftShift) && moveDirection != Vector3.zero && !isDashing)
        {
            StartDash();
        }
        if (isDashing)
        {
            if (touchingFloor)
            {
                Dash();
            }
            else
            {
                Somersault();
            }
        }
    }

    private void FixedUpdate()
    {
        if (!isDashing)
        {
            Move();
        }

        if (touchingFloor && Input.GetKey(KeyCode.Space))
        {
            Jump();
        }

        if (!touchingFloor)
        {
            backupGravityTimer++;

            if (backupGravityTimer > 350)
            {
                BackupGravity();
            }
        }
    }

    void Move()
    {
        Vector3 velocity = moveDirection * movementSpeed;
        rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);

        //if (moveDirection.x > 0)
        //{
        //    if (moveDirection.z > 0 && moveDirection.z > moveDirection.x)
        //    {
        //        newSprite = playerSprites[0];
        //    }
        //    else if (moveDirection.z < 0 && -moveDirection.z > moveDirection.x)
        //    {
        //        newSprite = playerSprites[1];
        //    }
        //    else
        //    {
        //        newSprite = playerSprites[2];
        //    }
        //}
        //else if (moveDirection.x < 0)
        //{
        //    if (moveDirection.z > 0 && -moveDirection.z < moveDirection.x)
        //    {
        //        newSprite = playerSprites[0];
        //    }
        //    else if (moveDirection.z < 0 && moveDirection.z < moveDirection.x)
        //    {
        //        newSprite = playerSprites[1];
        //    }
        //    else
        //    {
        //        newSprite = playerSprites[3];
        //    }
        //}

        //gameObject.GetComponent<SpriteRenderer>().sprite = newSprite;

        // Pozosta³oœci próby zrobienia prostego systemu zmiany sprita, lepiej siê jednak skupiæ na dopracowaniu movementu
        // Je¿eli starczy mi czasu to wrócê to naprawiæ
    }

    void StartDash()
    {
        isDashing = true;
        dashTimeRemaining = dashDuration;
    }

    void Dash()
    {
        if (dashTimeRemaining > 0)
        {
            Debug.Log("I am dashing");
            rb.velocity = moveDirection * dashSpeed;
            dashTimeRemaining -= Time.deltaTime;

            // Mo¿na nastêpnym razem spróbowaæ to zrobiæ loopem
        }
        else
        {
            isDashing = false;
        }
    }

    void Somersault()
    {
        if (dashTimeRemaining > 0)
        {
            //Debug.Log("I am somersaulting");
            rb.velocity = moveDirection * dashSpeed;
            //transform.Rotate (Vector3.forward * rotationAngle);
            //Debug.Log(rotationAngle);

            rotateScript.RotateSprite();

            dashTimeRemaining -= Time.deltaTime;

        }
        else
        {
            rotateScript.FixAngle();
            isDashing = false;
        }
    }

    private void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.collider.tag == "Environment")
        {
            touchingFloor = true;
        }
    }

    void Jump()
    {
        Debug.Log("I'm jumping");
        touchingFloor = false;
        rb.AddForce(0, jumpForce * Time.deltaTime, 0, ForceMode.VelocityChange);
    }

    void BackupGravity()
    {
        Debug.Log("Player has been off the ground for too long. Initializing backup gravity");
        transform.position = new Vector3 (transform.position.x, 0.5f, transform.position.z);    
        backupGravityTimer = 0;
        touchingFloor = true;

        // Obecny jest bug, zak³adam z kolizj¹, który pozostawia gracza w stanie wiecznego pozornego skoku, przez co nie mo¿e skakaæ wcale
        // Niezbyt ³adny fix ale wygl¹da na to, ¿e dzia³a
        // Problemem jest mo¿liwoœæ utkniêcia gracza w przeszkodach w rzadkich sytuacjach
    }
}
