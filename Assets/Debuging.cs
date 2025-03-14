using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debuging : MonoBehaviour
{
    public Vector2 DFCpoint;

    private void Start()
    {
        Debug.Log("start");
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            BoardState BS = GameObject.Find("Board").GetComponent<BoardState>();    
            Debug.Log(BS.DFCFromVec2(DFCpoint));


            /*
            List<int[]> allLegalMoves = Restrictions.FindAllMovesV5("white");
            foreach (var item in allLegalMoves)
            {
                foreach (var unit in item)
                {
                    Debug.Log(unit);
                }
            }
            */
        }

    }
}
