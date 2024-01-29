using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece_ID : MonoBehaviour
{
    public string color;
    public Tile_ID currentTile;

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, -1);
    }
}
