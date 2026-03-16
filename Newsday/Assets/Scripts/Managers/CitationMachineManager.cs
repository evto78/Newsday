using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;


public class CitationMachineManager : MonoBehaviour
{
    [Header("Printer slip")]
    [SerializeField] private GameObject printerSlip;
    [SerializeField] float printingDuration = 1;
    [SerializeField] private AnimationCurve printerSlipCurve;
    [SerializeField] private float printingDistance;
    [Header("Citation Menu")]
    [SerializeField] private GameObject citationSlip;
    [SerializeField] private TextMeshProUGUI citationTextTMP;
    [SerializeField] private AnimationCurve verticalCurve;
    [SerializeField] float openCiteDuration = 1;
    private float height;
    void Start()
    {

        //deactivate all the citations so that they don't show
        citationSlip.SetActive(false);
        printerSlip.SetActive(false);

        //figure out how far the distanc is between the bottom of the
        //screen and the current position of the citation slip
        height = citationSlip.GetComponent<RectTransform>().rect.height;
        
        //we use the the same code as above until the art is done to get a proper model
        printingDistance = printerSlip.GetComponent<RectTransform>().rect.height;

        //printerslip button deactivated so they don't click on a ghost button by accident.
        printerSlip.GetComponent<Button>().enabled = false;

        //testing script to see if it prints
        //printCitation("This is a test");
    }

    //this is called in the game manager when a mistake has been made and a citation needs to be printed.
    public void printCitation(string reason)
    {
        printerSlip.SetActive(true);
        StartCoroutine(printingCitation());

        //add the reason to the citation textTMP
        citationTextTMP.text = reason;
    }

    //this function is called when the player clicks on the script in the menu
    public void openCitation()
    {
        printerSlip.GetComponent<Button>().enabled = false;
        printerSlip.SetActive(false);
        citationSlip.SetActive(true);
        StartCoroutine(openingCitation());
    }


    //used for UI so that it closes
    public void closeCitation()
    {
        citationSlip.SetActive(false);
    }
    IEnumerator printingCitation()
    {
        Vector3 endPos = printerSlip.transform.position;

        printerSlip.transform.position -= new Vector3(0,printingDistance,0);
        Vector3 startPos = printerSlip.transform.position;
        float time = 0;
        while(time < printingDuration)
        {
            printerSlip.transform.position = Vector3.Lerp(startPos, endPos, printerSlipCurve.Evaluate(time / printingDuration));
            yield return null;
            time += Time.deltaTime;
        }
        //once the slip is done printing then enable the button on the slip
        printerSlip.GetComponent<Button>().enabled = true;
    }
    IEnumerator openingCitation()
    {
        Vector2 endPosition = citationSlip.transform.position;

        //starting position is below the frame
        Vector2 startPosition = citationSlip.transform.position - new Vector3(0, height, 0);
        float time = 0;
        
        while (time < openCiteDuration)
        {
            //we get the lerp between the starting position and end based on the progress of our animation curve, this creates
            //a smoother transition for when it appears or moves into frame. 
            citationSlip.transform.position = Vector3.Lerp(startPosition, endPosition, verticalCurve.Evaluate(time/openCiteDuration));
            
            yield return null;

            //add the miss time between the last frame
            time += Time.deltaTime;
        }
    }
}
