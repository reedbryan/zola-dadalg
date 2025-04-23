using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardSizeDropdown : MonoBehaviour
{
    public Dropdown ThisDropdown;
    public DrawBoard DB;

    void Start(){
        ThisDropdown.value = 1;
    }

    public void OnDDChange(){        
        // 8x8
        if (ThisDropdown.value == 0){
            DB.tileCount = 8;
        }
        // 7x7
        if (ThisDropdown.value == 1){
            DB.tileCount = 6;   
        }
        // 6x6
        if (ThisDropdown.value == 2){
            DB.tileCount = 4;
        }
    }
}
