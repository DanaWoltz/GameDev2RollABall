using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;


public class BattleManager : MonoBehaviour
{
    #region Variables

    [Header("Audio Sources")]  
    public AudioSource battleMusic; // Music that plays during battles
    public AudioSource travelMusic; // Music that plays when player is roaming

    [Header("Enemy References")]
    public GameObject[] enemyPrefabs; // Array that holds all enemies in the scene
    public GameObject currentEnemy = null; // Reference to whichever enemy the player is battling
 
    [Header("UI")]
    public GameObject battleUI; // UI that pops up during battles - displays player/enemy health and the attack button
    public TextMeshProUGUI enemyHealthText; // States enemy health during battle mode
    public TextMeshProUGUI playerHealthText; // States player health during battle mode
    public TextMeshProUGUI enemyCount; // States enemy count during entire game
    public TextMeshProUGUI loseText; // Appears if player has died
    public TextMeshProUGUI winText; // Appears if all enemies have been destroyed
    public Button attackButton; // Button that allows player to attack during BattleMode
    public GameObject restartButton; // Pops up if game is over

    [Header("Bools")]
    public bool battleMode; // true: Player is in battle. false: player is not in battle.
    public bool playerCanAttack; // If true, the player can attack. False: Enemy is attacking  
    public bool allEnemiesKilled; // if true, all enemies are dead.

    public ParticleSystem deadParticle; // is instantiated when player or enemy dies

    private GameObject player; // reference to the player
    #endregion

    #region Start and Update
    void Start()
    {
        player = GameObject.Find("Player"); // Finds the player and creates a reference for it
        UpdateUIInfo(); // 
        restartButton.SetActive(false); // Turns restart button off
        //SpawnEnemy();
    }

    #endregion

    #region Battle Coroutines (StartBattle, EndBattle)
    public IEnumerator StartBattle(float waitTime)
    {
        battleMode = true;
        playerCanAttack = true;
        BattleEngaged();
        battleMusic.Play();
        travelMusic.Stop();
        Debug.Log("Battle Started");

        yield return new WaitForSeconds(waitTime);

        BattlePositions();
        UpdateUIInfo();
    }

    public IEnumerator EndBattlePlayerVictory(float waitTime)
    {
        Instantiate(deadParticle, currentEnemy.transform.position, Quaternion.identity);
        battleMode = false;
        DestroyEnemy();
        Debug.Log("Battle Ended");
        battleMusic.Stop();
        travelMusic.Play();
        UpdateUIInfo();
        EndGameState();
        yield return new WaitForSeconds(waitTime);
        
    }

    #endregion

    #region Battle Parameters
    void BattlePositions()
    {
        currentEnemy.transform.position = new Vector3(currentEnemy.transform.position.x - 3, currentEnemy.transform.position.y, currentEnemy.transform.position.z);
        player.transform.position = new Vector3(currentEnemy.transform.position.x + 6, player.transform.position.y, currentEnemy.transform.position.z);
    }

    void BattleEngaged()
    {
        if (battleMode)
        {
            Color playerColor = player.GetComponent<Renderer>().material.color;

            if (playerColor == Color.red)
            {
                RedPlayerLogic();
            }

            if (playerColor == Color.green)
            {
                GreenPlayerLogic();
            }

            if (playerColor == Color.blue)
            {
                BluePlayerLogic();
            }
        }
    }

    void EndGameState()
    {
        
        
            if (player.GetComponent<PlayerController>().playerHealth <= 0)
            {
                Instantiate(deadParticle, player.transform.position, Quaternion.identity);
                player.SetActive(false);
                loseText.text = "Game Over";
                battleMode = false;
                UpdateUIInfo();
                restartButton.SetActive(true);
            }

            if (allEnemiesKilled)
            {
                winText.text = "You Win!";
                battleMode = false;
                UpdateUIInfo();
                restartButton.SetActive(true);
            }
        
    }

    #endregion

    #region UI Functions
    public void UpdateUIInfo()
    {
        if (!battleMode)
        {
            battleUI.SetActive(false);
            enemyHealthText.text = "";
            playerHealthText.text = "";
        }
        if (battleMode)
        {
            battleUI.SetActive(true);
            enemyHealthText.text = "Enemy HP: " + currentEnemy.GetComponent<EnemyMovement>().enemyHealth;
            playerHealthText.text = "Player HP: " + player.GetComponent<PlayerController>().playerHealth;

            if (playerCanAttack)
            {
                attackButton.interactable = true;
            }
            if (!playerCanAttack)
            {
                attackButton.interactable = false;
            }
        }

        if (!allEnemiesKilled)
        {
            enemyCount.text = "Enemies Remaining: " + enemyPrefabs.Length;
        }
        if (allEnemiesKilled)
        {
            enemyCount.text = "Enemies Remaining: 0";
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    #endregion

    #region Enemy Scripts

    /*void SpawnEnemy()
    {
        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            Vector3 spawnPos = new Vector3(UnityEngine.Random.Range(-3, 3), enemyPrefabs[i].transform.position.y, UnityEngine.Random.Range(-3, 3));
            Instantiate(enemyPrefabs[i], spawnPos, Quaternion.identity);
        }
    }*/


    void DestroyEnemy()
    {
        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            if (enemyPrefabs[i] == currentEnemy)
            {
                enemyPrefabs[i] = enemyPrefabs[enemyPrefabs.Length - 1];
                break;
            }
        }
        Array.Resize(ref enemyPrefabs, enemyPrefabs.Length - 1);
        Destroy(currentEnemy);
        currentEnemy = null;

        if (enemyPrefabs.Length == 0)
        {
            allEnemiesKilled = true;
            enemyPrefabs = null;
        }
    }

    public IEnumerator EnemyBattleLoop(float waitTime)
    {
        playerCanAttack = false;
        UpdateUIInfo();
        
        yield return new WaitForSeconds(waitTime);

        player.GetComponent<PlayerController>().playerHealth -= currentEnemy.GetComponent<EnemyMovement>().enemyAttackDamage;
        player.GetComponent<PlayerController>().hitParticle.Play();
        playerCanAttack = true;
        UpdateUIInfo();
        EndGameState();
    }

    #endregion

    #region Color Logic
    void RedPlayerLogic()
    {
        Color enemyColor = currentEnemy.GetComponent<Renderer>().material.color;
        if (enemyColor == Color.green)
        {
            player.GetComponent<PlayerController>().playerAttackDamage = 8;
            currentEnemy.GetComponent<EnemyMovement>().enemyAttackDamage = 4;
        }
        if (enemyColor == Color.blue)
        {
            player.GetComponent<PlayerController>().playerAttackDamage = 4;
            currentEnemy.GetComponent<EnemyMovement>().enemyAttackDamage = 8;
        }
        if (enemyColor == Color.red)
        {
            player.GetComponent<PlayerController>().playerAttackDamage = 4;
            currentEnemy.GetComponent<EnemyMovement>().enemyAttackDamage = 4;
        }
    }

    void GreenPlayerLogic()
    {
        Color enemyColor = currentEnemy.GetComponent<Renderer>().material.color;
        if (enemyColor == Color.green)
        {
            player.GetComponent<PlayerController>().playerAttackDamage = 4;
            currentEnemy.GetComponent<EnemyMovement>().enemyAttackDamage = 4;
        }
        if (enemyColor == Color.blue)
        {
            player.GetComponent<PlayerController>().playerAttackDamage = 8;
            currentEnemy.GetComponent<EnemyMovement>().enemyAttackDamage = 4;
        }
        if (enemyColor == Color.red)
        {
            player.GetComponent<PlayerController>().playerAttackDamage = 4;
            currentEnemy.GetComponent<EnemyMovement>().enemyAttackDamage = 8;
        }
    }

    void BluePlayerLogic()
    {
        Color enemyColor = currentEnemy.GetComponent<Renderer>().material.color;
        if (enemyColor == Color.green)
        {
            player.GetComponent<PlayerController>().playerAttackDamage = 4;
            currentEnemy.GetComponent<EnemyMovement>().enemyAttackDamage = 8;
        }
        if (enemyColor == Color.blue)
        {
            player.GetComponent<PlayerController>().playerAttackDamage = 4;
            currentEnemy.GetComponent<EnemyMovement>().enemyAttackDamage = 4;
        }
        if (enemyColor == Color.red)
        {
            player.GetComponent<PlayerController>().playerAttackDamage = 8;
            currentEnemy.GetComponent<EnemyMovement>().enemyAttackDamage = 4;
        }
    }

    #endregion
}
