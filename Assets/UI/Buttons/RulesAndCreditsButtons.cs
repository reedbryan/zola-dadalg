using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RulesAndCreditsButtons : MonoBehaviour
{
    public Image rulesImPrefab;
    private Image curRulesImage;

    public Image creditsImPrefab;
    private Image curCreditsImage;

    public GameControl GC;

    
    [SerializeField] float buffer = 0;

    public void Update(){
        if (GC.gameOn) {
            if (curRulesImage != null){
                Destroy(curRulesImage.gameObject);
            }
            if (curCreditsImage != null){
                Destroy(curCreditsImage.gameObject);
            }
        }
        if (Input.GetMouseButtonDown(0)){
            if (curRulesImage != null){
                buffer = 0.3f;
                Destroy(curRulesImage.gameObject);
            }
            if (curCreditsImage != null){
                buffer = 0.3f;  
                Destroy(curCreditsImage.gameObject);
            }
        }

        if (buffer > 0) buffer -= Time.deltaTime;
    }

    public void OnCreditsClick()
    {
        if (curCreditsImage == null && buffer <= 0){
            curCreditsImage = Instantiate(creditsImPrefab, transform);
        }
    }

    public void OnRulesClick()
    {
        if (curRulesImage == null && buffer <= 0){
            curRulesImage = Instantiate(rulesImPrefab, transform);
        }
    }
}
