using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLog : MonoBehaviour
{
    public List<int[]> allPastMoves = new List<int[]>();
    private Text gameLogMainText;

    public GameObject GameLogPrefab;

    GameObject CurrentGameLog;

    /// <summary>
    /// Rect transform achors, Vector4 -> (minX, minY, maxX, maxY)
    /// </summary>
    private Vector4 rectAnchors = new Vector4(0.02f, 0.02f, 0.45f, 0.76f);

    public void Wipe()
    {
        Destroy(CurrentGameLog);
    }

    public void Draw()
    {
        if (CurrentGameLog != null) Destroy(CurrentGameLog);

        CurrentGameLog = Instantiate(GameLogPrefab, transform);

        // Set rect position
        RectTransform rt = CurrentGameLog.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(rectAnchors.x, rectAnchors.y);
        rt.anchorMax = new Vector2(rectAnchors.z, rectAnchors.w);
        //Debug.Log(rectAnchors);
        rt.anchoredPosition = Vector2.zero;
        rt.sizeDelta = Vector2.zero;
        rt.eulerAngles = Vector3.zero;
        rt.localScale = new Vector3(1, 1, 1);

        // Get main text feild
        gameLogMainText = CurrentGameLog.transform.GetChild(1).gameObject.GetComponent<Text>();

        Debug.Log("Draw Game Log");
    }


    /// <summary>
    /// Adds new int[6] to 'allMoves' list where (int[0], int[1]) is current position,
    /// (int[2], int[3]) is target position, int[4] is checking if its a capturing move
    /// (0 = yes, 1 = no) and int[5] is checking the color of the mover (0 = white, 1 = black).             
    ///
    /// PS: Sould be called from MovePiece in Movement class.
    /// </summary>
    /// <param name="turnNumber"></param>
    /// <param name="color"></param>
    /// <param name="origin"></param>
    /// <param name="finish"></param>
    /// <param name="capturing"></param>
    public void NewMove(int turnNumber, string color, Vector2 origin, Vector2 finish, bool capturing)
    {
        int colorInt;
        if (color == "white")
            colorInt = 0;
        else
            colorInt = 1;
        int capturingInt;
        if (capturing)
            capturingInt = 0;
        else
            capturingInt = 1;

        // prep move to int[6]
        int[] moveToBeAdded = { (int)origin.x, (int)origin.y, (int)finish.x, (int)finish.y, capturingInt, colorInt };

        // add to allPastMoves list
        allPastMoves.Add(moveToBeAdded);

        // add to text
        if (gameLogMainText.text == "")
            gameLogMainText.text = "\nMove#" + turnNumber + ": " + ParseMoveCode(moveToBeAdded);
        else
            gameLogMainText.text = "\nMove#" + turnNumber + ": " + ParseMoveCode(moveToBeAdded) + gameLogMainText.text;

        //Debug.Log("Move number " + turnNumber + ": " + ParseMoveCode(moveToBeAdded));
    }

    string ParseMoveCode(int[] moveCode)
    {
        string strOnColor;
        string strOffColor;
        if (moveCode[5] == 0)
        {
            strOnColor = "blue";
            strOffColor = "red";
        }
        else
        {
            strOnColor = "red";
            strOffColor = "blue";
        }

        string strOrigin = "(" + moveCode[0] + "," + moveCode[1] + ")";
        string strFinish = "(" + moveCode[2] + "," + moveCode[3] + ")";

        string strIsCap;
        if (moveCode[4] == 0)
            strIsCap = " takes ";
        else
        {
            strIsCap = " moves to ";
            strOffColor = "";
        }

        return strOnColor + strOrigin + strIsCap + strOffColor + strFinish;
    }
}
