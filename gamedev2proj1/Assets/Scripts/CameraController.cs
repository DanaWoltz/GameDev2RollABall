using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public BattleManager battleManager;
    private Vector3 offset;
    private Vector3 battleCamera;
    
    void Start()
    {
        offset = transform.position - player.transform.position;
        battleCamera = new Vector3(transform.position.x - 3, 0f, 0f);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        CameraPositionUpdater();
    }

    void CameraPositionUpdater()
    {
        if (!battleManager.battleMode)
        {
            transform.position = player.transform.position + offset;
        }

        if (battleManager.battleMode)
        {
            StartCoroutine("BattleCameraPosition", 2.5);
        }
    }

    private IEnumerator BattleCameraPosition(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        transform.position = player.transform.position + offset + battleCamera;

    }
}
