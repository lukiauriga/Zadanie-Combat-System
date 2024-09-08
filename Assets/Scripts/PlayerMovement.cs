using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
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

    void Start()
    {
        
    }

    void Update()
    {
        // Zmienia inputy na warto�ci kt�re mo�na wsadzi� do wektora wskazuj�cego kierunek ruchu
        
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

        // Pozosta�o�ci pr�by zrobienia prostego systemu zmiany sprita, lepiej si� jednak skupi� na dopracowaniu movementu
        // Je�eli starczy mi czasu to wr�c� to naprawi�
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

            // Mo�na nast�pnym razem spr�bowa� to zrobi� loopem
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

        // Jest bug, kt�ry powoduje �e czasami gracz zupe�nie traci umiej�tno�� skakania
        // Zak�adam, �e jest to b��d wynikaj�cy z wykrywania kolizji mi�dzy graczem a pod�og�
        // Wymaga naprawy
    }
}
