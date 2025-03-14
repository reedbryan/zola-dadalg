using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    Movement movement;

    private void Awake()
    {
        movement = GameObject.Find("Game Manager").GetComponent<Movement>();
    }

    private void Update()
    {
        Movement();

        if (GameObject.Find("Game Manager").GetComponent<GameControl>().gameOn) {
            Detection();
        }
    }

    void Movement()
    {
        Vector3 pos = Input.mousePosition;
        pos.z = transform.position.z - Camera.main.transform.position.z;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(pos);

        transform.position = mousePos;
    }

    void Detection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Collider2D cursorHit = Physics2D.OverlapCircle(transform.position, 0.1f);
            if (cursorHit)
            {
                GameObject hit = cursorHit.gameObject;
                if (hit.CompareTag("Tile"))
                {
                    movement.TileHit(hit);
                }
            }
        }
    }
}
