using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyMovement : MonoBehaviour
{
    public int enemyHealth = 16;
    public int enemyAttackDamage = 16; // Defined in BattleManager depending on Player/Enemy Color

    [SerializeField] float speed = 5; //gameObject Movement Speed
    [SerializeField] float xLimit = 3; // limit for x-axis movement
    [SerializeField] float xLimitNeg = 1;
    [SerializeField] float zLimit = 3; // limit for z-axis movement
    [SerializeField] float zLimitNeg = 1;

    [SerializeField] float coroutineMinLimit = 1; // minimum number for random coroutine timer
    [SerializeField] float coroutineMaxLimit = 3; // maximum number for random coroutine timer

    public bool zAxis; // if true, gameObject will move along z-axis. If false, gameObject will move along x-axis.
    private bool targetHit; // if false, gameObject will move towards the positive axis limit. If true, gameObject will move towards negative axis Limit.

    private BattleManager battleManager; // Reference to BattleManager. Declared in Start

    public ParticleSystem hitParticle; // Particle that plays when enemy is hit


    void Start()
    {
        battleManager = GameObject.Find("BattleManager").GetComponent<BattleManager>(); // Find BattleManager

    }

    void Update()
    {
        Movement();
        BystanderEnemyController();
    }

    public void BystanderEnemyController() // Turns enemies that aren't currently in battle off for the duration of the battle
    {
        // Enemy MeshRenderers and Colliders are turned off if they arent the current enemy to make them invisible during battle

        if (!battleManager.battleMode)
        {
            if (gameObject != battleManager.currentEnemy)
            {
                GetComponent<Collider>().enabled = true;
                GetComponent<MeshRenderer>().enabled = true;
            }
        }

        if (battleManager.battleMode)
        {
            if (gameObject != battleManager.currentEnemy)
            {
                GetComponent<Collider>().enabled = false;
                GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }

    void Movement()
    {
        // Moves players back and fourth along x axis and z axis. 
        // If bool zAxis is false, enemies move on x axis. If true, they move on z axis
        // Once they hit their target, they move towards their negative target

        if (!battleManager.battleMode)
        {
            if (!zAxis)
            {
                // positive movement along x-axis
                if (!targetHit)
                {
                    transform.Translate(Vector3.right * speed * Time.deltaTime);
                    if (transform.position.x >= xLimit)
                    {
                        targetHit = true;
                    }
                }

                // negative movement along x-axis
                if (targetHit)
                {
                    transform.Translate(Vector3.left * speed * Time.deltaTime);
                    if (transform.position.x <= xLimitNeg)
                    {
                        targetHit = false;
                    }
                }
            }

            else if (zAxis)
            {
                // positive movement along z-axis
                if (!targetHit)
                {
                    transform.Translate(Vector3.forward * speed * Time.deltaTime);
                    if (transform.position.z >= zLimit)
                    {
                        targetHit = true;
                    }
                }

                // negative movement along z-axis
                if (targetHit)
                {
                    transform.Translate(Vector3.back * speed * Time.deltaTime);
                    if (transform.position.z <= -zLimit)
                    {
                        targetHit = false;
                    }
                }
            }
        }
    }


    private void OnTriggerEnter(Collider other) // Starts the battle if player collides into them
    {
        if (other.gameObject.CompareTag("Player"))
        {
            battleManager.currentEnemy = gameObject; // Makes this enemy the currentEnemy on BattleManager
            battleManager.StartCoroutine("StartBattle", 2.5f); // Defined in BattleManager

            Debug.Log("Collided with enemy");
        }
    }
}