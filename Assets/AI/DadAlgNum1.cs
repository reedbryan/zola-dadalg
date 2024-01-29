using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DadAlgNum1 : MonoBehaviour
{
    public string color;

    private void Update()
    {
        GameControl GC = GameObject.Find("Game Manager").GetComponent<GameControl>();

        if (GC.gameOn && !GC.moveMade)
        {
            if (GC.whiteTurn)
            {
                if (color == "white")
                    TakeTurn();
            }
            else
            {
                if (color == "black")
                    TakeTurn();
            }
        }
    }

    Vector4 FindBestMove()
    {
        List<Vector4> allMoves = Restrictions.FindAllMovesV4(color); // all legal moves for <color> team
        Vector4 bestMove = new Vector4(0, 0, 0, 0);
        float highestDFCScore = -1000000f;

        foreach (var move in allMoves)
        {
            float futureDFCScore = GetDFCScore(color, move);
            if (futureDFCScore > highestDFCScore)
            {
                highestDFCScore = futureDFCScore;
                bestMove = move;
            }
        }

        return bestMove;
    }

    /// <summary>
    /// The exicution of the turn
    /// </summary>
    void TakeTurn()
    {
        Vector4 bestMove;
        bestMove = FindBestMove();
        BoardState BS = GameObject.Find("Board").GetComponent<BoardState>();

        // Get the GameObjects for the tile and piece of the best move
        // Check if the best move is a capturing move
        GameObject bestPiece = BS.GetTileFromPosition(new Vector2(bestMove.x, bestMove.y)).GetComponent<Tile_ID>().occupent;
        GameObject bestTargetTile = BS.GetTileFromPosition(new Vector2(bestMove.z, bestMove.w));
        bool isCapturing;
        if (bestPiece.GetComponent<Piece_ID>().currentTile.DFC >= bestTargetTile.GetComponent<Tile_ID>().DFC)
            isCapturing = true;
        else
            isCapturing = false;

        // Exicute
        Movement movement = GameObject.Find("Game Manager").GetComponent<Movement>();
        movement.MovePiece(bestPiece, bestTargetTile, isCapturing);
        GameObject.Find("Game Manager").GetComponent<GameControl>().moveMade = true;
    }

    /// <summary>
    /// Returns the difference of the scores of the two colors. Example: color="white" if total white DFC=120 , total black
    /// DFC=135 then it will return 120-135 = -15. If there is no offset pass Vector4.zero in the offsetMove slot.
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    float GetDFCScore(string color, Vector4 move)
    {
        BoardState BS = GameObject.Find("Board").GetComponent<BoardState>();

        List<Vector2> onColorPieces = new List<Vector2>();
        List<Vector2> offColorPieces = new List<Vector2>();
        foreach (var piece in BS.pieces)
        {
            if (piece.GetComponent<Piece_ID>().color == color)
                onColorPieces.Add(piece.GetComponent<Piece_ID>().currentTile.position);
            else
                offColorPieces.Add(piece.GetComponent<Piece_ID>().currentTile.position);
        } 

        // Account for offset
        if (move != Vector4.zero)
        {
            onColorPieces.Remove(BS.GetTileFromPosition(new Vector2(move.x, move.y)).GetComponent<Tile_ID>().position);
            onColorPieces.Add(new Vector2(move.z, move.w));

            // If capturing move
            if (BS.GetTileFromPosition(new Vector2(move.z, move.w)).GetComponent<Tile_ID>().occupent != null)
            {
                offColorPieces.Remove(new Vector2(move.z, move.w));
            }
        }

        float onColorDFCScore = 0;
        foreach (var piece in onColorPieces)
        {
            onColorDFCScore += BS.GetTileFromPosition(piece).GetComponent<Tile_ID>().DFC;
        }
        float offColorDFCScore = 0;
        foreach (var piece in offColorPieces)
        {
            offColorDFCScore += BS.GetTileFromPosition(piece).GetComponent<Tile_ID>().DFC;
        }

        return onColorDFCScore - offColorDFCScore;
    }
}
