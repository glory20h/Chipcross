using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPieceHolder : MonoBehaviour
{
    public GameObject pieceHolder;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PieceHolder")
        {
            pieceHolder = collision.gameObject;
        }
    }
}
