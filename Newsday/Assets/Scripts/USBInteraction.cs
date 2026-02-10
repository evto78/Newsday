using UnityEngine;

public class USBInteraction : MonoBehaviour
{
    //ALL WIP, HAVE AN EFFECT HAPPEN WHEN A USB TAGGED OBJECT INTERACTS WITH THIS
    public OfficeManager manager;
    public int id;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "USB") { return; }
        manager.USBStay(id);
        if (!Input.GetMouseButton(0)) { manager.USBDrop(id); }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "USB") { return; }
        manager.USBStay(id);
        if (!Input.GetMouseButton(0)) { manager.USBDrop(id); }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "USB") { return; }
        manager.USBExit(id);
    }
}
