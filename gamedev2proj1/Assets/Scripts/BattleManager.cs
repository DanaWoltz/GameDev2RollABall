using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public bool battleMode;

    public AudioSource battleMusic;

    private GameObject enemy;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        enemy = GameObject.Find("Enemy");
        player = GameObject.Find("Player");
    }

    private IEnumerator StartBattle(float waitTime)
    {
        battleMusic.Play();
        enemy.GetComponent<MoveCube>().battleMode = true;
        player.GetComponent<SimplePlayerController>().battleMode = true;
        Debug.Log("Battle Started");

        yield return new WaitForSeconds(waitTime);

        BattlePositions();
        StartCoroutine("EndBattle", 5);
    }

    private IEnumerator EndBattle(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        enemy.GetComponent<MoveCube>().battleMode = false;
        player.GetComponent<SimplePlayerController>().battleMode = false;
        battleMode = false;
        Debug.Log("Battle Ended");
    }

    public void StartBattleMode()
    {
        StartCoroutine("StartBattle", 2.5f);
    }

    void BattlePositions()
    {
        enemy.transform.position = new Vector3(-3, enemy.transform.position.y, 0);
        player.transform.position = new Vector3(3, player.transform.position.y, 0);
    }
}
