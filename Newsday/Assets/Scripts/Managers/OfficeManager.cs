using UnityEngine;

public class OfficeManager : MonoBehaviour
{
    public Rigidbody2D usbRb;
    public ClickAndDragPhysics usbScript;
    public Transform usbSlot;
    private void Start()
    {
        usbRb.bodyType = RigidbodyType2D.Dynamic;
    }
    public void USBStay(int id)
    {
        if (!usbScript.holding) { return; }


    }
    public void USBExit(int id) 
    {
        
    }
    public void USBDrop(int id)
    {
        switch (id)
        {
            case 0: USBInserted(); break;
        }
    }
    void USBInserted()
    {
        usbRb.bodyType = RigidbodyType2D.Static;
        usbRb.transform.localEulerAngles = Vector3.zero;
        usbRb.transform.position = usbSlot.transform.position - Vector3.right * 0.7f;
    }
}
