using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverText : MonoBehaviour
{
    public Text GOtextPrefab;
    private Text CurrentGOtext;
    
    public void Draw(string winner){
        CurrentGOtext = Instantiate(GOtextPrefab, transform);
        CurrentGOtext.text = "GAME OVER, " + winner + " WINS!";

        Debug.Log("GOT draw");
    }

    public void Wipe(){
        Destroy(CurrentGOtext);
        Debug.Log("GOT wipe");
    }
}
