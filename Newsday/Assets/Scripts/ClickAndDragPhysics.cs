using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickAndDragPhysics : MonoBehaviour
{
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
            holding = false; rb.gravityScale = 1;
            rb.angularDamping = 0.05f;
        }
        if (myCollider.OverlapPoint(mousePos))
        {
            if (Input.GetMouseButtonDown(0) && !holding)
            {
                holding = true; rb.bodyType = RigidbodyType2D.Dynamic;
            }
        }
        if (holding)
        {
            Vector2 curPos = transform.position;
            Vector2 tarPos = new Vector2(mousePos.x, mousePos.y);
            rb.gravityScale = 0;
            rb.angularDamping = 3f;
            rb.linearVelocity = (tarPos - curPos)*2500f*Time.deltaTime;
            //if (Vector2.Distance(curPos, tarPos) >= Vector2.Distance(curPos + rb.linearVelocity, tarPos))
            //{
            //    rb.AddForceAtPosition((tarPos - curPos) * 200f * Time.deltaTime, curPos + (tarPos - curPos)/1.5f);
            //}
            //else
            //{
            //    rb.linearVelocity /= 1 + Time.deltaTime;
            //    rb.AddForceAtPosition((tarPos - curPos) * 100f * Time.deltaTime, curPos + (tarPos - curPos)/1.5f);
            //}
            //transform.position = Vector2.Lerp(curPos, tarPos, Time.deltaTime*10);

        }
    }
}
