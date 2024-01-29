using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NOTE: this is a method class for Movement.cs
public class Restrictions : MonoBehaviour
{
    public static bool CapturingMove(GameObject selectedPiece, GameObject targetTile, bool debug)
    {
        if (debug)
            Debug.Log("Capturing move check");

        Piece_ID selectedPieceID = selectedPiece.GetComponent<Piece_ID>();
        Tile_ID currentTileID = selectedPieceID.currentTile;
        Tile_ID targetTileID = targetTile.GetComponent<Tile_ID>();

        // Self Destruction test
        if (selectedPieceID.currentTile.gameObject == targetTile)
            return false;

        // Team test
        if (selectedPieceID.color == targetTileID.occupent.GetComponent<Piece_ID>().color)
            return false;

        // In line check
        Vector2 diff = targetTileID.position - currentTileID.position;
        diff = new Vector2(Mathf.Abs(diff.x), Mathf.Abs(diff.y));
        if (diff.x > 1 && (diff.y != 0 && diff.y != diff.x))
        {
            //Debug.Log("Not legal, out of line 1, Diff: " + diff);
            return false;
        }
        if (diff.y > 1 && (diff.x != 0 && diff.x != diff.y))
        {
            //Debug.Log("Not legal, out of line 2, Diff: " + diff);
            return false;
        }

        // distance from center check
        if (targetTileID.DFC > currentTileID.DFC)
        {
            //Debug.Log("Not legal, to close to center");
            return false;
        }

        // Blocking piece check
        BoardState BS = GameObject.Find("Board").GetComponent<BoardState>();

        Vector2 rawDiff = targetTileID.position - currentTileID.position;
        int indent;
        if (rawDiff.x == 0)
            indent = (int)rawDiff.y;
        else
            indent = (int)rawDiff.x;
        indent = Mathf.Abs(indent);

        for (int i = 1; i <= indent; i++)
        {
            GameObject midTile = BS.GetTileFromPosition(currentTileID.position + ((rawDiff / indent)) * i);
            if (debug)
                Debug.Log("Mid tile#" + (i + 1) + ": " + midTile.GetComponent<Tile_ID>().position);
            if (midTile.GetComponent<Tile_ID>().occupent != null && midTile != targetTile)
            {
                if (debug)
                    Debug.Log("Is occupied");
                return false;
            }
        }

        return true;
    }

    public static bool CapturingMove_CustomPieces(Vector2 selectedPiece, Vector2 targetTile, List<Vector2> onColorPieces, List<Vector2> offColorPieces, string color)
    {
        // Self Destruction test
        if (selectedPiece == targetTile)
            return false;

        // Team test
        foreach (var piecePos in onColorPieces)
        {
            if (targetTile == selectedPiece)
            {
                Debug.Log("Fail yo");
                return false;
            }
        }

        // In line check
        Vector2 diff = targetTile - selectedPiece;
        diff = new Vector2(Mathf.Abs(diff.x), Mathf.Abs(diff.y));
        if (diff.x > 1 && (diff.y != 0 && diff.y != diff.x))
        {
            //Debug.Log("Not legal, out of line 1, Diff: " + diff);
            return false;
        }
        if (diff.y > 1 && (diff.x != 0 && diff.x != diff.y))
        {
            //Debug.Log("Not legal, out of line 2, Diff: " + diff);
            return false;
        }

        // distance from center check
        BoardState BS = GameObject.Find("Board").GetComponent<BoardState>();
        if (BS.GetTileFromPosition(targetTile).GetComponent<Tile_ID>().DFC > BS.GetTileFromPosition(selectedPiece).GetComponent<Tile_ID>().DFC)
        {
            //Debug.Log("Not legal, to close to center");
            return false;
        }

        // Blocking piece check
        Vector2 rawDiff = targetTile - selectedPiece;
        int indent;
        if (rawDiff.x == 0)
            indent = (int)rawDiff.y;
        else
            indent = (int)rawDiff.x;
        indent = Mathf.Abs(indent);

        for (int i = 1; i <= indent; i++)
        {
            GameObject midTile = BS.GetTileFromPosition(selectedPiece + ((rawDiff / indent)) * i);
            bool tileOccupied = false;
            foreach (var piece in onColorPieces)
            {
                if (midTile.GetComponent<Tile_ID>().position == piece)
                    tileOccupied = true;
            }
            foreach (var piece in offColorPieces)
            {
                if (midTile.GetComponent<Tile_ID>().position == piece)
                    tileOccupied = true;
            }

            if (tileOccupied == true && midTile.GetComponent<Tile_ID>().position != targetTile)
            {
                return false;
            }
        }

        return true;
    }

    public static bool NonCapturingMove(GameObject selectedPiece, GameObject targetTile)
    {
        Piece_ID selectedPieceID = selectedPiece.GetComponent<Piece_ID>();
        Tile_ID currentTileID = selectedPieceID.currentTile;
        Tile_ID targetTileID = targetTile.GetComponent<Tile_ID>();

        // Self Destruction test
        if (selectedPieceID.currentTile.gameObject == targetTile)
            return false;

        // one space check
        Vector2 diff = targetTileID.position - currentTileID.position;
        diff = new Vector2(Mathf.Abs(diff.x), Mathf.Abs(diff.y));
        if (diff.x > 1 || diff.y > 1)
        {
            //Debug.Log("Not legal, to many spaces");
            return false;
        }

        // distance from center check
        if (targetTileID.DFC <= currentTileID.DFC)
        {
            //Debug.Log("Not legal, to close to center");
            return false;
        }

        return true;
    }

    public static bool NonCapturingMove_CustomPieces(Vector2 selectedPiece, Vector2 targetTile)
    {
        // Self Destruction test
        if (selectedPiece == targetTile)
            return false;

        // one space check
        Vector2 diff = targetTile - selectedPiece;
        diff = new Vector2(Mathf.Abs(diff.x), Mathf.Abs(diff.y));
        if (diff.x > 1 || diff.y > 1)
        {
            //Debug.Log("Not legal, to many spaces");
            return false;
        }

        BoardState BS = GameObject.Find("Board").GetComponent<BoardState>();

        // distance from center check
        if (BS.GetTileFromPosition(targetTile).GetComponent<Tile_ID>().DFC <= BS.GetTileFromPosition(selectedPiece).GetComponent<Tile_ID>().DFC)
        {
            //Debug.Log("Not legal, to close to center");
            return false;
        }

        return true;
    }

    /*
    public static List<Tile_ID> FindAllMoves(string color)
    {
        DrawBoard DB = GameObject.Find("Board").GetComponent<DrawBoard>();

        List<GameObject> releventPieces = new List<GameObject>();

        foreach (var piece in DB.pieces)
        {
            if (piece.GetComponent<Piece_ID>().color == color)
                releventPieces.Add(piece);
        }

        List<Tile_ID> allMoves = new List<Tile_ID>();

        foreach (var piece in releventPieces)
        {
            foreach (var tile in DB.tiles)
            {
                if (tile.GetComponent<Tile_ID>().occupent != null)
                {
                    if (CapturingMove(piece, tile, false))
                        allMoves.Add(tile.GetComponent<Tile_ID>());
                }
                else
                {
                    if (NonCapturingMove(piece, tile))
                        allMoves.Add(tile.GetComponent<Tile_ID>());
                }
            }
        }

        return allMoves;
    }
    */

    /// <summary>
    /// Returns all posible moves in int[5] form where (int[0], int[1]) is current position,
    /// (int[2], int[3]) is target position and int[4] is for checking if its a capturing move
    /// (0 = yes, 1 = no)
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public static List<int[]> FindAllMovesV5(string color)
    {
        BoardState BS = GameObject.Find("Board").GetComponent<BoardState>();

        List<GameObject> releventPieces = new List<GameObject>();

        foreach (var piece in BS.pieces)
        {
            if (piece.GetComponent<Piece_ID>().color == color)
                releventPieces.Add(piece);
        }

        List<int[]> allMoves = new List<int[]>();

        foreach (var piece in releventPieces)
        {
            foreach (var tile in BS.tiles)
            {
                if (tile.GetComponent<Tile_ID>().occupent != null)
                {
                    if (CapturingMove(piece, tile, false))
                    {
                        int[] move = {(int)piece.GetComponent<Piece_ID>().currentTile.position.x,
                                      (int)piece.GetComponent<Piece_ID>().currentTile.position.y,
                                      (int)tile.GetComponent<Tile_ID>().position.x,
                                      (int)tile.GetComponent<Tile_ID>().position.y,
                                      0 };
                        allMoves.Add(move);
                    }
                }
                else
                {
                    if (NonCapturingMove(piece, tile))
                    {
                        int[] move = {(int)piece.GetComponent<Piece_ID>().currentTile.position.x,
                                      (int)piece.GetComponent<Piece_ID>().currentTile.position.y,
                                      (int)tile.GetComponent<Tile_ID>().position.x,
                                      (int)tile.GetComponent<Tile_ID>().position.y,
                                      1 };
                        allMoves.Add(move);
                    }
                }
            }
        }

        return allMoves;
    }

    /// <summary>
    /// Returns all posible moves in Vector4 form where (x, y) is current position,
    /// (z, w) is target position
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public static List<Vector4> FindAllMovesV4(string color)
    {
        BoardState BS = GameObject.Find("Board").GetComponent<BoardState>();

        List<GameObject> releventPieces = new List<GameObject>();

        foreach (var piece in BS.pieces)
        {
            if (piece.GetComponent<Piece_ID>().color == color)
                releventPieces.Add(piece);
        }

        List<Vector4> allMoves = new List<Vector4>();

        foreach (var piece in releventPieces)
        {
            foreach (var tile in BS.tiles)
            {
                if (tile.GetComponent<Tile_ID>().occupent != null)
                {
                    if (CapturingMove(piece, tile, false))
                    {
                        allMoves.Add(TwoVec2ToVec4(piece.GetComponent<Piece_ID>().currentTile.position, tile.GetComponent<Tile_ID>().position));
                    }
                }
                else
                {
                    if (NonCapturingMove(piece, tile))
                    {
                        allMoves.Add(TwoVec2ToVec4(piece.GetComponent<Piece_ID>().currentTile.position, tile.GetComponent<Tile_ID>().position));

                    }
                }
            }
        }

        return allMoves;
    }

    static Vector4 TwoVec2ToVec4(Vector2 a, Vector2 b)
    {
        Vector4 c = new Vector4(a.x, a.y, b.x, b.y);
        return c;
    }


    /// <summary>
    /// Returns all legal moves for the passed color with the passed piece information
    /// </summary>
    /// <param name="onColorPieces"></param>
    /// <param name="offColorPieces"></param>
    /// <param name="color"></param>
    /// <returns></returns>
    public static List<int[]> FindAllMoves_CustomPieces(List<Vector2> onColorPieces, List<Vector2> offColorPieces, string color)
    {
        BoardState BS = GameObject.Find("Board").GetComponent<BoardState>();

        List<Vector2> allPieces = new List<Vector2>();
        foreach (var item in onColorPieces)
        {
            allPieces.Add(item);
        }
        foreach (var item in offColorPieces)
        {
            allPieces.Add(item);
        }

        List<int[]> allMoves = new List<int[]>();

        foreach (var piecePos in onColorPieces)
        {
            foreach (var tile in BS.tiles)
            {
                bool tileOccupied = false;
                foreach (var piece in allPieces)
                {
                    if (tile.GetComponent<Tile_ID>().position == piece)
                        tileOccupied = true;
                }

                if (tileOccupied)
                {
                    if (CapturingMove_CustomPieces(piecePos, tile.GetComponent<Tile_ID>().position, onColorPieces, offColorPieces, color))
                    {
                        int[] move = {(int)piecePos.x,
                                      (int)piecePos.y,
                                      (int)tile.GetComponent<Tile_ID>().position.x,
                                      (int)tile.GetComponent<Tile_ID>().position.y,
                                      0 };

                        allMoves.Add(move);
                        //Debug.Log("Capturing move found at: " + tile.GetComponent<Tile_ID>().position);
                    }
                }
                else
                {
                    if (NonCapturingMove_CustomPieces(piecePos, tile.GetComponent<Tile_ID>().position))
                    {
                        int[] move = {(int)piecePos.x,
                                      (int)piecePos.y,
                                      (int)tile.GetComponent<Tile_ID>().position.x,
                                      (int)tile.GetComponent<Tile_ID>().position.y,
                                      1 };

                        allMoves.Add(move);
                        //Debug.Log("Non capturing move found at: " + tile.GetComponent<Tile_ID>().position);
                    }
                }
            }
        }

        return allMoves;
    }
}
