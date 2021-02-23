using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public AudioSource battleMusic;
    public AudioSource travelMusic;
    public GameObject[] enemyPrefabs;
    public GameObject currentEnemy = null;
    public GameObject battleUI;


    private GameObject player;

    public bool battleMode;
    public bool playerCanAttack;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");

        //SpawnEnemy();
    }

    private void Update()
    {
        //BattleEngaged();
        if (battleMode)
        {
            battleUI.SetActive(true);

            if (player.GetComponent<PlayerController>().playerHealth <= 0)
            {
                // Game Over
                Debug.Log("Game Over, Player is Dead");
            }

            if (currentEnemy.GetComponent<EnemyMovement>().enemyHealth <= 0)
            {
                StartCoroutine("EndBattlePlayerVictory", 3);
                battleUI.SetActive(false);
            }
        }

    }



    public IEnumerator StartBattle(float waitTime)
    {
        battleMode = true;
        BattleEngaged();
        battleMusic.Play();
        travelMusic.Stop();
        Debug.Log("Battle Started");

        yield return new WaitForSeconds(waitTime);

        BattlePositions();
        //StartCoroutine("EndBattle", 5);
    }

    private IEnumerator EndBattlePlayerVictory(float waitTime)
    {

        DestroyEnemy();
        battleMode = false;
        Debug.Log("Battle Ended");
        battleMusic.Stop();
        travelMusic.Play();
        yield return new WaitForSeconds(waitTime);
    }

    void BattlePositions()
    {
        currentEnemy.transform.position = new Vector3(-3, currentEnemy.transform.position.y, currentEnemy.transform.position.z);
        player.transform.position = new Vector3(3, player.transform.position.y, currentEnemy.transform.position.z);
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



    // *************************************Enemy Scripts Below*************************************

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
            enemyPrefabs = null;
        }
    }

    public IEnumerator EnemyBattleLoop(float waitTime)
    {
        playerCanAttack = false;
        yield return new WaitForSeconds(waitTime);
        int enemyAttackDamage = currentEnemy.GetComponent<EnemyMovement>().enemyAttackDamage;
        int playerHealth = player.GetComponent<PlayerController>().playerHealth;

        Debug.Log("Enemy Has Attacked! " + enemyAttackDamage);
        player.GetComponent<PlayerController>().playerHealth -= currentEnemy.GetComponent<EnemyMovement>().enemyAttackDamage;
        playerCanAttack = true;
    }

    // *************************************Color Scripts Below*************************************
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


}
