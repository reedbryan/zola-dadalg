using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_ID : MonoBehaviour
{
    public Vector2 position;
    public string color;
    public GameObject occupent;
    /// <summary>
    /// Distance From Center
    /// </summary>
    public float DFC;

    private void Awake()
    {
        float distance = Mathf.Sqrt(Mathf.Pow(transform.position.x, 2) + Mathf.Pow(transform.position.y, 2));
        DFC = Mathf.Round(distance * 100f) / 100f;
    }
}
