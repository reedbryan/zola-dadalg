using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverText : MonoBehaviour
{
    public Text GOtextPrefab;
    public Text CurrentGOtext;
    
    public void Draw(string winner){
        if (CurrentGOtext != null){
            return;
        }
        CurrentGOtext = Instantiate(GOtextPrefab, transform);
        CurrentGOtext.text = "GAME OVER, " + winner + " WINS!";

        Debug.Log("GOT draw");
    }

    public void Wipe(){
        if (CurrentGOtext != null){
            Destroy(CurrentGOtext.gameObject);
        }
        Debug.Log("GOT wipe");
    }
}
