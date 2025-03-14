using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public GameLog GL;
    public GameControl GC;

    [SerializeField] GameObject selectedPiece;

    public void TileHit(GameObject tile)
    {
        Tile_ID tileID = tile.GetComponent<Tile_ID>();
        
        if (tileID.occupent != null)
        {
            if (selectedPiece != null)
            {
                // -
                // Tile hit with piece while a piece is selected
                // -
                if (tile.GetComponent<Tile_ID>().occupent == selectedPiece)
                {
                    DeSelectPiece();
                }
                else
                {
                    if (Restrictions.CapturingMove(selectedPiece, tile, false))
                        MovePiece(selectedPiece, tile, true);
                    else
                        SelectPiece(tileID.occupent);
                }
            }
            else
            {
                SelectPiece(tileID.occupent);
            }
        }
        else
        {
            if (selectedPiece != null)
            {
                // -
                // Empty tile hit while a piece is selected
                // -
                if (Restrictions.NonCapturingMove(selectedPiece, tile))
                    MovePiece(selectedPiece, tile, false);
            }
        }
    }

    public void MovePiece(GameObject piece, GameObject targetTile, bool isCapturing)
    {
        GC.moveMade = true;

        // update game log
        Vector2 origin = piece.GetComponent<Piece_ID>().currentTile.position;
        Vector2 finish = targetTile.GetComponent<Tile_ID>().position;
        GL.NewMove(GameObject.Find("Game Manager").GetComponent<GameControl>().turnCount + 1, piece.GetComponent<Piece_ID>().color, origin, finish, isCapturing);

        if (isCapturing)
        {
            Tile_ID newTileID = targetTile.GetComponent<Tile_ID>();
            BoardState BS = GameObject.Find("Board").GetComponent<BoardState>();
            BS.pieces.Remove(newTileID.occupent);
            if (newTileID.occupent.GetComponent<Piece_ID>().color == "white")
            {
                BS.whitePieces.Remove(newTileID.occupent);
            }
            else
            {
                BS.blackPieces.Remove(newTileID.occupent);
            }
            Destroy(newTileID.occupent);
        }

        // Physical piece movement
        movingPiece = true;
        PIM = piece.transform;
        PIM_targetTile = targetTile.transform;

        // Set values
        Piece_ID pieceID = piece.GetComponent<Piece_ID>();
        Tile_ID targetTileID = targetTile.GetComponent<Tile_ID>();
        pieceID.currentTile.occupent = null;
        pieceID.currentTile = targetTileID;
        targetTileID.occupent = piece;

        DeSelectPiece();
    }
    
    /// <summary>
    /// PIM -> Piece In Motion (the piece that is currently being moved if there is none this value should be null)
    /// </summary>
    Transform PIM;
    Transform PIM_targetTile;
    [SerializeField] float pieceMovementSpeed;
    public bool movingPiece;
    public void StopMovement()
    {
        PIM = null;
        PIM_targetTile = null;
        movingPiece = false;
    }
    private void Update()
    {
        if (movingPiece && GC.gameOn)
        {
            /*
            Debug.Log("Game on: " + GC.gameOn);
            Debug.Log("Piece in motion: " + PIM);
            Debug.Log("Piece in motion target tile: " + PIM_targetTile);
            Debug.Log("Moving piece: " + movingPiece);
            */
            Vector3 deltaPosition = -1f * (PIM.position - PIM_targetTile.position).normalized;
            PIM.position += deltaPosition * (pieceMovementSpeed / 100f);
            if (Mathf.Abs(deltaPosition.x) <= 0.01f && Mathf.Abs(deltaPosition.y) <= 0.01f)
            {
                GC.TurnDone();
                //Debug.Log("Done moving piece");
                movingPiece = false;
            }
        }
    }


    private void SelectPiece(GameObject newSelected)
    {
        DrawBoard DB = GameObject.Find("Board").GetComponent<DrawBoard>();
        GameControl GC = GameObject.Find("Game Manager").GetComponent<GameControl>();

        if (newSelected.GetComponent<Piece_ID>().color == "white")
        {
            if (!GC.whiteTurn)
                return;
        }
        else
        {
            if (GC.whiteTurn)
                return;
        }

        if (selectedPiece != null)
        {
            // UnSelected current selectedPiece

            /* with color
            if (selectedPiece.GetComponent<Piece_ID>().color == "white")
                selectedPiece.GetComponent<SpriteRenderer>().color = DB.whiteColor_Piece;
            if (selectedPiece.GetComponent<Piece_ID>().color == "black")
                selectedPiece.GetComponent<SpriteRenderer>().color = DB.blackColor_Piece;
            */

            // with size
            selectedPiece.transform.localScale -= new Vector3(DB.pieceScale / 3f, DB.pieceScale / 3f, DB.pieceScale / 3f);
        }

        selectedPiece = newSelected;

        // with color
        //selectedPiece.GetComponent<SpriteRenderer>().color = DB.selectedColor_Piece;

        // with size
        selectedPiece.transform.localScale += new Vector3(DB.pieceScale / 3f, DB.pieceScale / 3f, DB.pieceScale / 3f);
    }

    private void DeSelectPiece()
    {
        if (selectedPiece == null)
            return;
        DrawBoard DB = GameObject.Find("Board").GetComponent<DrawBoard>();
        selectedPiece.transform.localScale -= new Vector3(DB.pieceScale / 3f, DB.pieceScale / 3f, DB.pieceScale / 3f);
        selectedPiece = null;
    }
}
