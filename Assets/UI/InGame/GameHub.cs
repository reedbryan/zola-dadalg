using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHub : MonoBehaviour
{
    bool isDrawn;

    public DrawBoard DB;
    public GameControl GC;
    public GameObject GameHubPrefab;

    GameObject CurrentGameHub;

    // Counters
    [SerializeField] private Text turnCountText;
    [SerializeField] private Text turnColorText;
    [SerializeField] private Text possibleMovesText;

    /// <summary>
    /// Rect transform achors, Vector4 -> (minX, minY, maxX, maxY)
    /// </summary>
    public Vector4 rectAnchors = new Vector4(0.02f, 0.79f, 0.45f, 0.98f);

    private void Start()
    {
        isDrawn = false;
    }


    public void Wipe()
    {
        Destroy(CurrentGameHub);
    }

    // Called from GameControl.StartGame()
    public void Draw()
    {
        if (CurrentGameHub != null) Destroy(CurrentGameHub);
        
        CurrentGameHub = Instantiate(GameHubPrefab, transform);

        // Set rect position
        RectTransform rt = CurrentGameHub.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(rectAnchors.x, rectAnchors.y);
        rt.anchorMax = new Vector2(rectAnchors.z, rectAnchors.w);
        rt.anchoredPosition = Vector2.zero;
        rt.sizeDelta = Vector2.zero;
        rt.eulerAngles = Vector3.zero;
        rt.localScale = new Vector3(1, 1, 1);

        // Get text feilds
        turnCountText = CurrentGameHub.transform.GetChild(0).gameObject.GetComponent<Text>();
        turnColorText = CurrentGameHub.transform.GetChild(1).gameObject.GetComponent<Text>();
        possibleMovesText = CurrentGameHub.transform.GetChild(2).gameObject.GetComponent<Text>();

        isDrawn = true;

        Debug.Log("Draw Game Hub");
    }

    private void Update()
    {
        if (!isDrawn)
            return;

        turnCountText.text = "Total Moves: " + GC.turnCount;
        possibleMovesText.text = "Possible Moves: " + GC.possibleMoves;

        string turnColor;
        if (GC.whiteTurn)
            turnColor = "Blue";
        else
            turnColor = "Red";
        turnColorText.text = "To Move: " + turnColor;


        // Testing for rect transform position reset
        /*
        if (Input.GetKeyDown(KeyCode.Y))
        {
            RectTransform rt = GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(rectAnchors.x, rectAnchors.y);
            rt.anchorMax = new Vector2(rectAnchors.z, rectAnchors.w);
            rt.anchoredPosition = Vector2.zero;
            rt.sizeDelta = Vector2.zero;
            rt.eulerAngles = Vector3.zero;
            rt.localScale = new Vector3(1, 1, 1);
        }
        */
    }
}
