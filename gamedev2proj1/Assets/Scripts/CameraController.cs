using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player; // Player reference
    public BattleManager battleManager; // Battle Manager reference
    private Vector3 offset; // Camera Position while not in battle
    private Vector3 battleCamera; // Camera position during battles

    void Start()
    {
        offset = transform.position - player.transform.position; // Defines Offset
        battleCamera = new Vector3(transform.position.x - 3, 0f, 0f); // Defines Battle position
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
            transform.position = player.transform.position + offset; // Follows the player at the offset
        }

        if (battleManager.battleMode)
        {
            StartCoroutine("BattleCameraPosition", 2.5); // Moves Camera to battlePosition after a few seconds
        }
    }

    private IEnumerator BattleCameraPosition(float waitTime) // Updates camera pos to Battle Position
    {
        yield return new WaitForSeconds(waitTime);
        transform.position = player.transform.position + offset + battleCamera; // Moves camera to Battle position

    }
}
