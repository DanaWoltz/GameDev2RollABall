using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public TextMeshProUGUI enemyHealthText;
    public TextMeshProUGUI playerHealthText;
    public GameObject enemy;
    public GameObject player;


    private void FixedUpdate()
    {
        enemyHealthText.text = "Remaining HP: " + enemy.GetComponent<EnemyMovement>().enemyHealth;
        playerHealthText.text = "Remaining HP: " + player.GetComponent<PlayerController>().playerHealth;
    }

}
