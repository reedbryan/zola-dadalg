using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuDropDown : MonoBehaviour
{
    public Dropdown ThisDropdown;
    public GameObject AI_GameObject;
    public GameObject hardAIWarningText;
    private bool warningOn;
    public string color;

    void Start(){
        ThisDropdown.value = 0;
        warningOn = false;
    }

    public void OnDDChange(){        
        // Player
        if (ThisDropdown.value == 0){
            if (AI_GameObject.GetComponent<DadAlgNum2>() != null && AI_GameObject.GetComponent<DadAlgNum2>().color == color){
                Destroy(AI_GameObject.GetComponent<DadAlgNum2>());
            }
            if (AI_GameObject.GetComponent<DadAlgNum1>() != null && AI_GameObject.GetComponent<DadAlgNum1>().color == color){
                Destroy(AI_GameObject.GetComponent<DadAlgNum1>());
            }

            if (warningOn){
                warningOn = false;
                hardAIWarningText.SetActive(false);
            }
        }
        // easy AI
        if (ThisDropdown.value == 1){
            
            if (AI_GameObject.GetComponent<DadAlgNum2>() != null && AI_GameObject.GetComponent<DadAlgNum2>().color == color){
                Destroy(AI_GameObject.GetComponent<DadAlgNum2>());
            }

            DadAlgNum1 x =  AI_GameObject.AddComponent<DadAlgNum1>();
            x.color = color;

            if (warningOn){
                warningOn = false;
                hardAIWarningText.SetActive(false);
            }
        }
        // hard AI
        if (ThisDropdown.value == 2){
            
            if (AI_GameObject.GetComponent<DadAlgNum1>() != null && AI_GameObject.GetComponent<DadAlgNum1>().color == color){
                Destroy(AI_GameObject.GetComponent<DadAlgNum1>());
            }

            DadAlgNum2 x =  AI_GameObject.AddComponent<DadAlgNum2>();
            x.color = color;

            hardAIWarningText.SetActive(true);
            warningOn = true;
        }
    }
}
