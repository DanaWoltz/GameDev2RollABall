using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float speed = 5;
    [SerializeField] float xLimit = 3;
    [SerializeField] float zLimit = 3;

    [SerializeField] float coroutineMinLimit = 1; // minimum number for random coroutine timer
    [SerializeField] float coroutineMaxLimit = 3; // maximum number for random coroutine timer

    public bool zAxis;
    private bool targetHit;



    // Start is called before the first frame update
    void Start()
    {
        // checks to see if zAxis bool is true or false
        if (zAxis)
        {
            StartCoroutine("zAxisOff", Random.Range(coroutineMinLimit, coroutineMaxLimit));
        }
        else if (!zAxis)
        {
            StartCoroutine("zAxisOn", Random.Range(coroutineMinLimit, coroutineMaxLimit));
        }
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
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
                if (transform.position.x <= -xLimit)
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
    // Function that Turns the Z-Axis bool on, and starts the coroutine to turn it off
    private IEnumerator zAxisOn(float waitTime)
    {
        zAxis = true;
        yield return new WaitForSeconds(waitTime);

        StartCoroutine("zAxisOff", Random.Range(coroutineMinLimit, coroutineMaxLimit));
    }

    // Function that Turns the Z-Axis bool off, and starts the coroutine to turn on
    private IEnumerator zAxisOff(float waitTime)
    {
        zAxis = false;
        yield return new WaitForSeconds(waitTime);

        StartCoroutine("zAxisOn", Random.Range(coroutineMinLimit, coroutineMaxLimit));
    }

}
