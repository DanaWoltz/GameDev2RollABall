using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;


public class PlayerController : MonoBehaviour
{
    private Rigidbody rb; // Reference to rigidbody. Declared at Start

    public float speed = 0;
    private float movementX; // Calculated in OnMove()
    private float movementY; // Calculated in OnMove()

    public int playerHealth = 16;
    public int playerAttackDamage = 0; // Set in Battle Manager depending on Enemy/Player Color

    public ParticleSystem hitParticle; // Particle that plays when player is attacked by enemy

    public BattleManager battleManager; // reference to BattleManager




    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Finds player rigidbody
    }

    void FixedUpdate()
    {
        PlayerMovement();
    }

    void OnMove(InputValue movementValue) // Stores input value. Used in PlayerMovement()
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void PlayerMovement() // Controls player movement
    {
        if (battleManager.battleMode == false) // if battle mode = false, allow movement
        {
            GetComponent<Rigidbody>().isKinematic = false;

            // Movement Calculations
            Vector3 movement = new Vector3(movementX, 0.0f, movementY).normalized;
            rb.AddForce(movement * speed);
        }

        if (battleManager.battleMode == true) // if battle mode is true, make player rigidbody kinematic which immediately halts their movement
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }



    private void OnTriggerEnter(Collider other) // used for pickup items. Sets pickup item as false and changes player color
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            Color pickupColor = other.gameObject.GetComponent<Renderer>().material.color; // Stores pickup color
            Debug.Log(pickupColor);
            GetComponent<Renderer>().material.color = pickupColor; // Switches player color to stored pickup color
            other.gameObject.SetActive(false);
        }

    }

    public void PlayerAttack() // Player attack loop. if Enemy health is greater than 0, run EnemyBattleLoop Coroutine. Else, Run the end battle coroutine
    {
        Debug.Log("Player Has Attacked for: " + playerAttackDamage);
        battleManager.currentEnemy.GetComponent<EnemyMovement>().hitParticle.Play();
        battleManager.currentEnemy.GetComponent<EnemyMovement>().enemyHealth -= playerAttackDamage;
        battleManager.UpdateUIInfo();
        if (battleManager.currentEnemy.GetComponent<EnemyMovement>().enemyHealth <= 0) // if Enemy is dead
        {
            battleManager.StartCoroutine("EndBattlePlayerVictory", 3); // Defined in BattleManager
        }
        else if (battleManager.currentEnemy.GetComponent<EnemyMovement>().enemyHealth > 0) // If Enemy is still alive
        {
            battleManager.StartCoroutine("EnemyBattleLoop", 1.5f); // Defined in BattleManager
        }
    }
}
