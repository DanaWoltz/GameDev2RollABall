using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleChoices : MonoBehaviour
{
    public GameObject playerObject;

    void Fight()
    {
        if(playerObject.GetComponent<Material> == "ballRed")
        {
            if(Enemy.CompareTag("EnemyRed"))
            {

            }
            
        }
    }

}
