using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardState : MonoBehaviour
{
    public List<GameObject> tiles = new List<GameObject>();
    public List<GameObject> pieces = new List<GameObject>();

    public List<GameObject> whitePieces = new List<GameObject>();
    public List<GameObject> blackPieces = new List<GameObject>();

    /*
    public void Wipe()
    {
        List<GameObject> whitePiecesToRemouve = new List<GameObject>();
        List<GameObject> blackPiecesToRemouve = new List<GameObject>();
        List<GameObject> tilesToRemouve = new List<GameObject>();

        foreach (var piece in pieces)
        {
            if (whitePieces.Contains(piece))
                whitePiecesToRemouve.Add(piece);
            else
                blackPiecesToRemouve.Add(piece);
        }
        foreach (var tile in tiles)
        {
            tilesToRemouve.Add(tile);
        }
        foreach (var piece in whitePiecesToRemouve)
        {
            whitePieces.Remove(piece);
            pieces.Remove(piece);
            Destroy(piece);
        }
        foreach (var piece in blackPiecesToRemouve)
        {
            blackPieces.Remove(piece);
            pieces.Remove(piece);
            Destroy(piece);
        }
        foreach (var tile in tilesToRemouve)
        {
            tiles.Remove(tile);
            Destroy(tile);
        }
    }
    */
    
    public void Wipe(){
        foreach (GameObject piece in pieces){
            Destroy(piece);
        }
        foreach (GameObject tile in tiles)
        {
            Destroy(tile);
        }

        pieces.Clear();
        whitePieces.Clear();
        blackPieces.Clear();
        tiles.Clear();
    }
    
    public GameObject GetTileFromPosition(Vector2 position)
    {
        foreach (var tile in tiles)
        {
            if (tile.GetComponent<Tile_ID>().position == position)
            {
                return tile;
            }
        }
        return null;
    }

    public float DFCFromVec2(Vector2 pos)
    {
        int xCoordinite;
        int yCoordinite;
        if (pos.x < 3)
            xCoordinite = (int)Mathf.Abs(pos.x - 4);
        else
            xCoordinite = (int)Mathf.Abs(pos.x - 3);
        if (pos.y < 3)
            yCoordinite = (int)Mathf.Abs(pos.y - 4);
        else
            yCoordinite = (int)Mathf.Abs(pos.y - 3);

        DrawBoard DB = GetComponent<DrawBoard>();

        float oneTileDistance = DB.boardSize / DB.tileCount;
        
        float xDis = (oneTileDistance * xCoordinite) - (oneTileDistance / 2);
        float yDis = (oneTileDistance * yCoordinite) - (oneTileDistance / 2);
        float DFC = Mathf.Sqrt(Mathf.Pow(xDis, 2) + Mathf.Pow(yDis, 2));
        return DFC;
    }
}
