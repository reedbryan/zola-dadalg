using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Manager : MonoBehaviour
{
    public GameObject DadAlg1Prefab;
    public GameObject DadAlg2Prefab;

    public GameObject whiteTeamAI;
    public GameObject blackTeamAI;


    public void InsertAI(GameObject whiteAI, GameObject blackAI)
    {
        if (whiteAI != null)
            whiteTeamAI = Instantiate(whiteAI, transform);
        if (blackAI != null)
            blackTeamAI = Instantiate(blackAI, transform);

    }
}
