using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawBoard : MonoBehaviour
{
    public GameObject tilePrefab;
    public GameObject piecePrefab;

    // Board peramiters - - - - - - - - -
    [SerializeField] public float boardSize;
    [SerializeField] public int tileCount;
    public Color whiteColor_Piece;
    public Color blackColor_Piece;
    public Color selectedColor_Piece;
    [SerializeField] Color whiteColor_Board;
    [SerializeField] Color blackColor_Board;
    // - - - - - - - - - - - - - - - - - 

    public float pieceScale;

    public void Draw()
    {
        Debug.Log("Draw Board");

        float tileIndent = boardSize / tileCount;
        Vector2 zeroZero = new Vector2(boardSize / -2, boardSize / -2);

        int index = 0;
        for (int y = 0; y < tileCount; y++)
        {
            for (int x = 0; x < tileCount; x++)
            {
                // Space tile
                Vector2 spawnPos = new Vector2(x * tileIndent, y * tileIndent) + zeroZero + new Vector2(tileIndent / 2, tileIndent / 2);
                GameObject newTile = Instantiate(tilePrefab, spawnPos, transform.rotation);
                newTile.transform.parent = transform;

                // Size tile
                float tileSize = tileIndent;
                newTile.transform.localScale = new Vector3(tileSize, tileSize, tileSize);

                // Setup Tile_ID
                Tile_ID ID = newTile.GetComponent<Tile_ID>();
                ID.position = new Vector2(x + 1, y + 1);
                SetColor(newTile, index, y, ID);

                // Add and setup piece
                GameObject newPiece = Instantiate(piecePrefab, newTile.transform.position, newTile.transform.rotation);
                Piece_ID PID = newPiece.GetComponent<Piece_ID>();
                newPiece.transform.parent = transform;
                newPiece.transform.localScale = new Vector3(tileSize, tileSize, tileSize) / 2f;
                pieceScale = tileSize / 2f;
                ID.occupent = newPiece;
                PID.currentTile = ID;
                SpriteRenderer psr = newPiece.GetComponent<SpriteRenderer>();
                PID.color = ID.color;
                if (PID.color == "white")
                    psr.color = whiteColor_Piece;
                if (PID.color == "black")
                    psr.color = blackColor_Piece;

                // Indent arrays, ints
                BoardState BS = GetComponent<BoardState>();

                BS.pieces.Add(newPiece);
                BS.tiles.Add(newTile);
                if (PID.color == "white")
                {
                    BS.whitePieces.Add(newPiece);
                }
                else
                {
                    BS.blackPieces.Add(newPiece);
                }
                index++;
            }
        }

    }

    void SetColor(GameObject newTile, int index, int y, Tile_ID ID)
    {
        SpriteRenderer sr = newTile.GetComponent<SpriteRenderer>();
        if (tileCount % 2 == 0)
        {
            if (y % 2 == 0)
            {
                if (index % 2 == 0)
                {
                    sr.color = whiteColor_Board;
                    ID.color = "white";
                }
                else
                {
                    sr.color = blackColor_Board;
                    ID.color = "black";
                }
            }
            else
            {
                if (index % 2 == 0)
                {
                    sr.color = blackColor_Board;
                    ID.color = "black";
                }
                else
                {
                    sr.color = whiteColor_Board;
                    ID.color = "white";
                }
            }
        }
        else
        {
            if (index % 2 == 0)
            {
                sr.color = whiteColor_Board;
                ID.color = "white";
            }
            else
            {
                sr.color = blackColor_Board;
                ID.color = "black";
            }
        }
    }
}
