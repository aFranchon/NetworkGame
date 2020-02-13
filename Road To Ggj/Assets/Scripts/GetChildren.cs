using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetChildren : MonoBehaviour
{
    public Vector3 getChildren()
    {
        Debug.Log(transform.GetChild(Random.Range(0, transform.childCount)).transform.position);
        return transform.GetChild(Random.Range(0, transform.childCount)).transform.position;
    }
}
