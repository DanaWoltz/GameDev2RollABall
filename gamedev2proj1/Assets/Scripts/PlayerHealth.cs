using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public TextMeshProUGUI enemyHealthText; //text in battle menu UI that displays enemy health
    public TextMeshProUGUI playerHealthText; //text in battle menu UI that displays player health
    public GameObject enemy;
    public GameObject player;


    private void FixedUpdate()
    {
        // function that updates health in the UI after each attack
        enemyHealthText.text = "Remaining HP: " + enemy.GetComponent<EnemyMovement>().enemyHealth; 
        playerHealthText.text = "Remaining HP: " + player.GetComponent<PlayerController>().playerHealth;
    }

}
