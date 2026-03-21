using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using Unity.VisualScripting;

public class OfficeManager : MonoBehaviour
{
    [Header("Reporter")]
    [SerializeField] private GameObject ReporterBody;
    [SerializeField] private GameObject SpeechBubble;
    [SerializeField] private TextMeshProUGUI SpeechBubbleTxt;
    [SerializeField] private Transform enterPoint, exitPoint;
    [SerializeField] private float walkingTime;//time it takes to walk from one end of the screen to the center
    private bool walking = false;
    [SerializeField] private float stepHeight = 0.2f;//how high the sprite moves up during each step
    [SerializeField] private int numOfSteps = 10;

    //ALL WIP, HANDLE USB INTERACTIONS IN OFFICE SCENE
    [Header("USB")]
    [SerializeField] private GameObject usbStick;
    [SerializeField] private Transform usbSpawnPnt;
    private Rigidbody2D usbRb;
    private ClickAndDragPhysics usbScript;
    public Transform usbSlot;

    public Button computerButton;
    private void Start()
    {

        usbRb = usbStick.AddComponent<Rigidbody2D>();
        usbRb.bodyType = RigidbodyType2D.Dynamic;

        usbScript = usbStick.AddComponent<ClickAndDragPhysics>();
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
    private void Update()
    {
        computerButton.interactable = ((usbRb.bodyType == RigidbodyType2D.Static) && (usbRb.gameObject.activeSelf));
    }

    //TODO
    private void turnOffReporter()
    {
        usbStick.SetActive(false);
        SpeechBubble.SetActive(false);
        ReporterBody.SetActive(false);
    }

    private void enterReporter()
    {
        //set up the position of the reporter and turn them on
        ReporterBody.transform.position = enterPoint.transform.position;
        ReporterBody.SetActive(true);

        //walk from the left to the center of the screen
        StartCoroutine(walkFromAToB(enterPoint.position, Vector2.Lerp(enterPoint.position, exitPoint.position, 0.5f)));
    }

    //DONE
    IEnumerator walkFromAToB(Vector2 startingPoint, Vector2 endPoint)
    {
        float timer = 0;
        walking = true;
        while (timer <= walkingTime)
        {
            timer += Time.deltaTime;
            
            float x, y;
            x = Mathf.Lerp(startingPoint.x, endPoint.x, timer / walkingTime);
            y = Mathf.Abs(stepHeight * Mathf.Sin(numOfSteps * Mathf.PI * (timer / walkingTime)));
            
            ReporterBody.transform.position = new Vector3(x, y, 0);
            
            yield return null;
        }
        walking = false;//we are done walking
    }

    //DONE
    private void reporterLeave()
    {   //turn off USB
        usbStick.SetActive(false);
        StartCoroutine(exitReporter());
    }

    //DONE
    IEnumerator exitReporter()
    {
        StartCoroutine(walkFromAToB(Vector2.Lerp(enterPoint.position, exitPoint.position, 0.5f), exitPoint.position));
        do {
            yield return null;
        }while (walking);
        //turn off reporter once they made it passed a certain point
        ReporterBody.SetActive(false);
    }

    private void dropUSB()
    {
        usbStick.transform.position = usbSpawnPnt.position;
        usbStick.SetActive(true);
    }

    private void showDebuglines()
    {
        Debug.DrawLine(enterPoint.position, exitPoint.position, Color.green);
    }
}
