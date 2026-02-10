using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickAndDragPhysics : MonoBehaviour
{
    //2D Physics handler to have an object be click and draggable with the cursor
    Rigidbody2D rb;
    Collider2D myCollider;
    Camera cam;
    public bool holding = false;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();
        cam = Camera.main;
        rb.gravityScale = 1;
        rb.angularDamping = 0.05f;
    }
    private void Update()
    {
        if (rb.bodyType == RigidbodyType2D.Static) { holding = false;}
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        if (!Input.GetMouseButton(0) && holding)
        {
            //If not holding down click, stop holding
            holding = false; rb.gravityScale = 1;
            rb.angularDamping = 0.05f;
        }
        if (myCollider.OverlapPoint(mousePos))
        {
            //If the object is clicked, hold the object
            if (Input.GetMouseButtonDown(0) && !holding)
            {
                holding = true; rb.bodyType = RigidbodyType2D.Dynamic;
            }
        }
        if (holding)
        {
            //If holding the object, move it towards the mouse
            Vector2 curPos = transform.position;
            Vector2 tarPos = new Vector2(mousePos.x, mousePos.y);
            rb.gravityScale = 0;
            rb.angularDamping = 3f;
            rb.linearVelocity = (tarPos - curPos)*2500f*Time.deltaTime;
        }
    }
}
