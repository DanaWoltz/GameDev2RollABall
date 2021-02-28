using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 0;
    //public TextMeshProUGUI colorText;
    
    //public Transform cameraTransform;
    private Rigidbody rb;
    
    private float movementX;
    private float movementY;
    //public float turnSmoothTime;
    //float turnSmoothVelocity;
    
    public bool battleMode = false;
    public int playerHealth = 16;
    public int playerAttackDamage = 0;
    public BattleManager battleManager;
    public ParticleSystem hitParticle;
   
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        

        //SetCountText();
        //winTextObject.SetActive(false);
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;

    }

    

    void FixedUpdate()
    { 
        if(battleManager.battleMode == false)
        {
            GetComponent<Rigidbody>().isKinematic = false;
            Vector3 movement = new Vector3(movementX, 0.0f, movementY).normalized;
            rb.AddForce(movement * speed);

            //if (movement.magnitude > 0.01f)
            //{
            //    float targetAngle = Mathf.Atan2(movementX, movementY) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            //    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            //    transform.rotation = Quaternion.Euler(0f, angle, 0f);
            //    Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            //    rb.AddForce(moveDir.normalized * speed);
            //}
        }
        if(battleManager.battleMode == true)
        {

            GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            Color pickupColor = other.gameObject.GetComponent<Renderer>().material.color;
            Debug.Log(pickupColor);
            GetComponent<Renderer>().material.color = pickupColor;
            other.gameObject.SetActive(false);


            // count = count + 1;
            //SetCountText();
        }
        //if (other.gameObject.CompareTag("EnemyRed"))
        //{
        //    Battle();
        //}
        //if (other.gameObject.CompareTag("EnemyBlue"))
        //{
        //    Battle();
        //}
        //if (other.gameObject.CompareTag("EnemyGreen"))
        //{
        //    Battle();
        //}

    }

    public void PlayerAttack()
    {
        Debug.Log("Player Has Attacked for: " + playerAttackDamage);
        battleManager.currentEnemy.GetComponent<EnemyMovement>().hitParticle.Play();
        battleManager.currentEnemy.GetComponent<EnemyMovement>().enemyHealth -= playerAttackDamage;
        battleManager.UpdateUIInfo();
        if (battleManager.currentEnemy.GetComponent<EnemyMovement>().enemyHealth <= 0)
        {
            battleManager.StartCoroutine("EndBattlePlayerVictory", 3);
        }
        else if (battleManager.currentEnemy.GetComponent<EnemyMovement>().enemyHealth > 0)
        {
            battleManager.StartCoroutine("EnemyBattleLoop", 1.5f);
        }
        
    }
    
}
