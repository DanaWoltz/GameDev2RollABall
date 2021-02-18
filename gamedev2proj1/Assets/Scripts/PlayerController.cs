using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 0;
    //public TextMeshProUGUI colorText;
    public GameObject winTextObject;
    private Rigidbody rb;
    private int count;
    private float movementX;
    private float movementY;
    public GameObject battleUI;
    public bool battleMode = false;
    public int playerHealth = 16;
    public int playerAttackDamage = 0;
    public BattleManager battleManager;
    Color pickupColor;
   
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;

        //SetCountText();
        //winTextObject.SetActive(false);
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;

    }

    void SetCountText()
    {
       // colorText.text = "Color: " + count.ToString();
        if(count >= 11)
        {
            winTextObject.SetActive(true);

        }

    }

    void FixedUpdate()
    { 
        if(battleManager.battleMode == false)
        {
            GetComponent<Rigidbody>().isKinematic = false;
            Vector3 movement = new Vector3(movementX, 0.0f, movementY);
            rb.AddForce(movement * speed);
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
            
            count++;  
            Color pickupColor = other.gameObject.GetComponent<Renderer>().material.color;
            Debug.Log(pickupColor);
            GetComponent<Renderer>().material.color = pickupColor;
            other.gameObject.SetActive(false);


            // count = count + 1;
            //SetCountText();
        }
        if (other.gameObject.CompareTag("EnemyRed"))
        {
            Battle();
        }
        if (other.gameObject.CompareTag("EnemyBlue"))
        {
            Battle();
        }
        if (other.gameObject.CompareTag("EnemyGreen"))
        {
            Battle();
        }

    }

    public void PlayerAttack()
    {
        Debug.Log("Player Has Attacked for: " + playerAttackDamage);
        battleManager.currentEnemy.GetComponent<EnemyMovement>().enemyHealth -= playerAttackDamage;
        battleManager.StartCoroutine("EnemyBattleLoop", 3);
    }
    void Battle()
    {
        battleUI.SetActive(true);
    }
}
