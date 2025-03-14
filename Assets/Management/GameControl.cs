using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    public bool gameOn = false;
    public bool whiteTurn = true;
    /// <summary>
    /// Stops the AI from making multiple moves while pieces are physicly moving
    /// </summary>
    public bool moveMade = false;
    public int turnCount = 0;
    public int possibleMoves;

    [SerializeField] int recursionCounter;

    /// <summary>
    /// Draw Board
    /// </summary>
    public DrawBoard DB;
    /// <summary>
    /// Game Hub
    /// </summary>
    public GameHub GH;
    /// <summary>
    /// Game Log
    /// </summary>
    public GameLog GL;
    /// <summary>
    /// Board State
    /// </summary>
    public BoardState BS;
    public Movement movement;
    /// <summary>
    /// Gameover Text
    /// </summary>
    public GameOverText GOT;

    public GameObject menuCanvas;
    
    private void Start()
    {
        // Set values
        turnCount = 0; whiteTurn = true; gameOn = false; moveMade = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToMenu();
        }
    }

    public void StartGame()
    {        
        if (gameOn || GOT.CurrentGOtext != null)
        {
            RestartGame();
            return;
        }

        GOT.Wipe();
        GH.Draw();
        DB.Draw();
        GL.Draw();
        menuCanvas.SetActive(false);
        moveMade = false;
        gameOn = true;
    }
    void RestartGame()
    {
        GOT.Wipe();
        GH.Wipe();
        BS.Wipe();
        GL.Wipe();

        GH.Draw();
        DB.Draw();
        GL.Draw();
        movement.StopMovement();

        moveMade = false;
        gameOn = true;
    }

    void EndGame(string winner)
    {
        gameOn = false;
        movement.movingPiece = false;
        GOT.Draw(winner);
    }
    void EndGame(){
        gameOn = false;
        movement.movingPiece = false;
    }

    public void ToMenu()
    {
        EndGame();
        GH.Wipe();
        BS.Wipe();
        GL.Wipe();
        GOT.Wipe();
        menuCanvas.SetActive(true);
    }


    // Called in MovePiece func in Movement class
    // ** recursive **
    public void TurnDone()
    {
        //Debug.Log("Turn Done");

        turnCount++;

        // Check for game over
        BoardState BS = GameObject.Find("Board").GetComponent<BoardState>();
        if (BS.whitePieces.Count == 0){
            EndGame("RED");
        }
        if (BS.blackPieces.Count == 0){
            EndGame("BLUE");
        }
        moveMade = false;
        if (whiteTurn)
            whiteTurn = false;
        else
            whiteTurn = true;


        // Check for availiable moves
        string turnColor;
        if (whiteTurn)
            turnColor = "white";
        else
            turnColor = "black";

        // Get all possible moves for team on given turn
        List<Vector4> allMoves = Restrictions.FindAllMovesV4(turnColor);
        possibleMoves = allMoves.Count;

        if (possibleMoves <= 0)
        {
            //Debug.Log("No Moves, Pass turn");
            if (recursionCounter <= 5)
            {
                recursionCounter++;
                TurnDone();
            }
        }
        recursionCounter = 0;
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
