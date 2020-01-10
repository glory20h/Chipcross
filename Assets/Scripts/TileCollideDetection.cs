using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCollideDetection : MonoBehaviour
{
    public GameObject overlappedObject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "EmptyTile")
        {
            overlappedObject = collision.gameObject;
        }
    }
}
