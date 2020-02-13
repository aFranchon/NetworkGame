using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    public Vector3 DirectionF;
    public Vector3 DirectionR;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Collision with Player");
            collision.gameObject.GetComponent<Movements>().PlayerHit(DirectionF, DirectionR);
            Destroy(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
