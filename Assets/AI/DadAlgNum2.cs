using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DadAlgNum2 : MonoBehaviour
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

    float GetDFCScore(List<Vector2> onColorPieces, List<Vector2> offColorPieces)
    {
        //Debug.Log("onColorPieces: " + onColorPieces.Count);
        //Debug.Log("offColorPieces: " +offColorPieces.Count);

        BoardState BS = GameObject.Find("Board").GetComponent<BoardState>();

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

        float score = onColorDFCScore - offColorDFCScore;
        //lastOnColorPieces = onColorPieces;
        //lastOffColorPieces = offColorPieces;

        return score;
    }


    Vector4 FindBestMove()
    {
        //Debug.Log("Find best move");

        BoardState BS = GameObject.Find("Board").GetComponent<BoardState>();

        string enemyColor;
        if (color == "white")
            enemyColor = "black";
        else
            enemyColor = "white";

        // Creates a list for both friendly pieces and enemy pieces
        List<Vector2> onColorPieces = new List<Vector2>();
        List<Vector2> offColorPieces = new List<Vector2>();
        foreach (var piece in BS.pieces)
        {
            if (piece.GetComponent<Piece_ID>().color == color)
                onColorPieces.Add(piece.GetComponent<Piece_ID>().currentTile.position);
            else
                offColorPieces.Add(piece.GetComponent<Piece_ID>().currentTile.position);
        }

        // Create list off all current moves for on color pieces
        List<int[]> allLegalMoves = Restrictions.FindAllMovesV5(color);
        float highestBestEnemyDFCScore = float.NegativeInfinity; // The highest of all the best enemy scores
        Vector4 bestMove = Vector4.zero;

        foreach (var move in allLegalMoves) // Friendly moves
        {
            //Debug.Log(highestBestEnemyDFCScore);
            //Debug.Log(move[0] + ", " + move[1] + ", " + move[2] + ", " + move[3]);

            List<Vector2> newBoardState_onColorPieces = new List<Vector2>();
            List<Vector2> newBoardState_offColorPieces = new List<Vector2>();
            foreach (var piece in onColorPieces)
            {
                newBoardState_onColorPieces.Add(piece);
            }
            foreach (var piece in offColorPieces)
            {
                newBoardState_offColorPieces.Add(piece);
            }

            if (newBoardState_onColorPieces.Contains(new Vector2(move[0], move[1])))
            {
                newBoardState_onColorPieces.Remove(new Vector2(move[0], move[1]));
                newBoardState_onColorPieces.Add(new Vector2(move[2], move[3]));
                if (newBoardState_offColorPieces.Contains(new Vector2(move[2], move[3])))
                {
                    newBoardState_offColorPieces.Remove(new Vector2(move[2], move[3]));
                    //Debug.Log("yo");
                }
            }

            float currentDFCScore = GetDFCScore(newBoardState_onColorPieces, newBoardState_offColorPieces);

            //Debug.Log(currentDFCScore);

            float bestEnemyDFCScore = 100000f;

            List<int[]> allLegalMoves2 = Restrictions.FindAllMoves_CustomPieces(offColorPieces, newBoardState_onColorPieces, enemyColor);

            foreach (var enemyMove in allLegalMoves2) // Enemy moves
            {
                float enemyDFCScoreToRemouve = BS.DFCFromVec2(new Vector2(enemyMove[0], enemyMove[1]));
                float enemyDFCScoreToAdd = BS.DFCFromVec2(new Vector2(enemyMove[2], enemyMove[3]));
                float friendlyDFCScoreToRemouve = 0;
                if (enemyMove[4] == 0)
                    friendlyDFCScoreToRemouve = BS.DFCFromVec2(new Vector2(enemyMove[2], enemyMove[3]));

                float newDFCScore = currentDFCScore - enemyDFCScoreToAdd + enemyDFCScoreToRemouve - friendlyDFCScoreToRemouve;

                if (newDFCScore <= bestEnemyDFCScore)
                {
                    //Debug.Log("Old: " + bestEnemyDFCScore + "   New: " + newDFCScore);
                    bestEnemyDFCScore = newDFCScore;
                }
            }

            if (bestEnemyDFCScore >= highestBestEnemyDFCScore)
            {
                //Debug.Log("(Second check) Old: " + highestBestEnemyDFCScore + "   New: " + bestEnemyDFCScore);

                highestBestEnemyDFCScore = bestEnemyDFCScore;
                bestMove = new Vector4(move[0], move[1], move[2], move[3]);

                //Debug.Log(move[0] + ", " + move[1] + ", " + move[2] + ", " + move[3]);
            }
        }

        return bestMove;
    }
}
