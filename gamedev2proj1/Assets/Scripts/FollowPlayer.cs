using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;

    // Script is for the Player's particle effects
    void Update()
    {
        transform.position = new Vector3(player.transform.position.x, 1.5f, player.transform.position.z);
    }
}
