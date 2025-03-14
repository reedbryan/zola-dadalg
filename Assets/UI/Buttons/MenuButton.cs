using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    public Button thisButton;
    public GameControl GC;

    public void Start(){
        GameObject GC_Ob = GameObject.Find("Game Manager");
        GC = GC_Ob.GetComponent<GameControl>();
        thisButton.onClick.AddListener(delegate{ GC.ToMenu(); });
    }
}
