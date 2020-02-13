using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TpScr : MonoBehaviour
{
    public GameObject OtherTp;
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && collision.gameObject.GetComponent<Movements>().isAbleToTp)
        {
            Debug.Log(OtherTp.transform.position);
            collision.gameObject.transform.position = OtherTp.transform.position;
            collision.gameObject.GetComponent<Movements>().Tped();
        }
    }
}
