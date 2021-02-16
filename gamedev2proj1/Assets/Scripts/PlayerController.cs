﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 0;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    private Rigidbody rb;
    private int count;
    private float movementX;
    private float movementY;
    public GameObject battleUI;
    public bool battleMode = false;
   
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;

        SetCountText();
        winTextObject.SetActive(false);
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;

    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if(count >= 11)
        {
            winTextObject.SetActive(true);

        }

    }

    void FixedUpdate()
    { 
        if(battleMode == false)
        {
            GetComponent<Rigidbody>().isKinematic = false;
            Vector3 movement = new Vector3(movementX, 0.0f, movementY);
            rb.AddForce(movement * speed);
        }
        if(battleMode == true)
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count++;
            // count = count + 1;

            SetCountText();
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
    void Battle()
    {
        battleUI.SetActive(true);
    }
}
