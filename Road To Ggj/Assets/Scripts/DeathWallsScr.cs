using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathWallsScr : MonoBehaviour
{
    public Score scoreScript;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            scoreScript.cmdPlayerDied(collision.gameObject.GetComponent<Movements>().playerId);
        }
    }
}
