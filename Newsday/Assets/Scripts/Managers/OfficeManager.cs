using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using Unity.VisualScripting;

public class OfficeManager : MonoBehaviour
{
    [SerializeField] private ArticleManager articleManager;
    [SerializeField] private string[] bossIntroDialogue;

    [Header("Reporter")]
    [SerializeField] private GameObject ReporterBody;
    [SerializeField] private Transform enterPoint, exitPoint;
    [SerializeField] private float walkingTime;//time it takes to walk from one end of the screen to the center
    private bool walking = false;
    [SerializeField] private float stepHeight = 0.2f;//how high the sprite moves up during each step
    [SerializeField] private int numOfSteps = 10;
    [SerializeField] private GameObject[] reporterSprite = new GameObject[3];

    //ALL WIP, HANDLE USB INTERACTIONS IN OFFICE SCENE
    [Header("USB")]
    [SerializeField] private GameObject usbStick;
    [SerializeField] private Transform usbSpawnPnt;
    private Rigidbody2D usbRb;
    private ClickAndDragPhysics usbScript;
    public Transform usbSlot;

    [Header("Speech Bubble")]
    [SerializeField] private StringTyper stringTyper;
    [SerializeField] private GameObject SpeechBubble;
    [SerializeField] private TextMeshProUGUI SpeechBubbleTxt;

    [Space]
    public Button computerButton;
    public Button doorButton;
    private void Start()
    {

        usbRb = usbStick.GetComponent<Rigidbody2D>();
        usbRb.bodyType = RigidbodyType2D.Dynamic;
        usbScript = usbStick.GetComponent<ClickAndDragPhysics>();
        turnOffReporterSprites();
        turnOffReporter();
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

    //DONE
    private void turnOffReporter()
    {
        usbStick.SetActive(false);
        SpeechBubble.SetActive(false);
        ReporterBody.SetActive(false);
    }

    
    //person comes in, talks and drops usb off
    IEnumerator newPersonCommingIn(string[] dialogue)
    {
        //1. they walk in
        //set up the position of the reporter and turn them on
        ReporterBody.transform.position = enterPoint.transform.position;
        ReporterBody.SetActive(true);

        //walk from the left to the center of the screen
        StartCoroutine(walkFromAToB(enterPoint.position, Vector2.Lerp(enterPoint.position, exitPoint.position, 0.5f)));

        while (walking) yield return null;

        //2. Start talking
        SpeechBubble.SetActive(true);
        
        /* We can't use a for loop cause we have to let the player decide on when to click next
        //cycle through all the lines of dialogue
        for(int i = 0; i < dialogue.Length; i++)
        {
            stringTyper.StartTyping(dialogue[i]);
            while (stringTyper.isTyping) yield return null;
        }
        */
        
        //we start talking to the player
        stringTyper.startConversation(dialogue);

        while(stringTyper.isTalking) yield return null;
        
        //turn off the speech bubble when we are done talking
        SpeechBubble.SetActive(false);


        //3. Drop the usb
        dropUSB();
    }


    private bool bossIntroDone = false;
    //DONE
    public void doorKnock()
    {
       doorButton.interactable = false;
        if (!bossIntroDone) // our boss is breaking down the rules of the game
        {
            //set the reporter sprite
            changeCharacterSprite(0);

            bossIntroDone = true;
            StartCoroutine(bossIntro(bossIntroDialogue));
        }
        else
        {
            //set the reporter sprite
            changeCharacterSprite(articleManager.getReporterSpriteIndex());

            //the reporter comes in afterwards
            StartCoroutine(newPersonCommingIn(articleManager.getCurrentArticleDialogue()));
        }
    } 

    private void changeCharacterSprite(int spriteIndex)
    {
        turnOffReporterSprites();
        reporterSprite[spriteIndex].SetActive(true);
    }

    private void turnOffReporterSprites()
    {
        foreach(GameObject sprite in reporterSprite)
        {
            sprite.SetActive(false);
        }
    }


    IEnumerator bossIntro(string[] dialogue)
    {
        //1. they walk in
        //set up the position of the reporter and turn them on
        ReporterBody.transform.position = enterPoint.transform.position;
        ReporterBody.SetActive(true);

        //walk from the left to the center of the screen
        StartCoroutine(walkFromAToB(enterPoint.position, Vector2.Lerp(enterPoint.position, exitPoint.position, 0.5f)));

        while (walking) yield return null;

        //2. Start talking
        SpeechBubble.SetActive(true);

        //we start talking to the player
        stringTyper.startConversation(dialogue);

        while (stringTyper.isTalking) yield return null;

        //turn off the speech bubble when we are done talking
        SpeechBubble.SetActive(false);

        // 3. we walk off screen
        StartCoroutine(exitReporter());
    }

    //DONE
    IEnumerator walkFromAToB(Vector2 startingPoint, Vector2 endPoint)
    {
        if (walkingTime == 0)
        {
            Debug.LogError("Walking time is 0, please set it");
        }

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

    //DONE - to be used in game manager
    public void reporterLeave()
    {   //turn off USB
        usbStick.SetActive(false);
        StartCoroutine(exitReporter());
    }

    //DONE
    IEnumerator exitReporter()
    {
        StartCoroutine(walkFromAToB(Vector2.Lerp(enterPoint.position, exitPoint.position, 0.5f), exitPoint.position));
        while(walking) yield return null;
        //turn off reporter once they made it passed a certain point
        ReporterBody.SetActive(false);
        //reactivate button
        doorButton.interactable = true;
    }

    //DONE
    private void dropUSB()
    {
        usbStick.transform.position = usbSpawnPnt.position;
        usbStick.SetActive(true);
        usbRb.bodyType = RigidbodyType2D.Dynamic;
        usbScript.holding = false; usbRb.gravityScale = 1;
        usbRb.angularDamping = 0.05f;
    }

    private void showDebuglines()
    {
        Debug.DrawLine(enterPoint.position, exitPoint.position, Color.green);
    }
}
