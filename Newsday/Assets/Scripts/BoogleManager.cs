using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;
public class BoogleManager : MonoBehaviour
{
    

    [SerializeField] private Slider baseSlider, searchSlider;
    [SerializeField] private AnimationCurve loadingCurve;
    [SerializeField] GameObject baseUI, searchUI;
    public GameObject article;
    private bool loading = false;

    [SerializeField] private float textSearchTime = 2, imageSearchTime = 5;
    [SerializeField] private float baseSearchTime = 1;

    private float startTime = 0;
    private float loadingDuration = 0;

    enum InformationType { TEXT, IMAGE, GRAPH }
    InformationType selectionType;

    [SerializeField] private GameObject text, image;

    void Start()
    {
        //set the sliders to 0
        baseSlider.value = 0;
        searchSlider.value = 0;

        //set up the base of the UI.
        searchUI.SetActive(false);
        baseUI.SetActive(true);

        unloadAssets();
    }

    [SerializeField] TMP_Text mainSearchTextField, subSearchTextField;
    public void updateSearchBarText(string engineText)
    {
        mainSearchTextField.text = engineText;
        subSearchTextField.text = engineText;
    }

    void Update()
    {
        
        

        //update the current progress of the loading bar if 
        if (loading)
        {
            loadingBar();
        }

    }

    public void imageResult(Sprite result)
    {
        selectionType = InformationType.IMAGE;
        tempImgResult = result;
    }

    public void textResult(string result)
    {
        selectionType = InformationType.TEXT;
        tempTextResult = result;
    }
    Sprite tempImgResult;
    string tempTextResult;

    //DONE
    public void loadingBar()
    {
        unloadAssets();
        if (baseUI.activeInHierarchy)
        {
            baseSlider.value = loadingCurve.Evaluate((Time.time - startTime) / loadingDuration);
        }
        else//the base isn't active and its the regular search UI
        {
            searchSlider.value = loadingCurve.Evaluate((Time.time - startTime) / loadingDuration);
        }

        //when we have fully loaded then we load in the assets
        if (Time.time - startTime >= loadingDuration)
        {
            baseSlider.value = 0;
            searchSlider.value = 0;
            loading = false;
            unloadAssets();//off load the previous search bar
            loadAssets();//load the new assets
        }
    }

    //DONE I THINK
    public void loadAssets()
    {
        //only switch after we are done loading
        //if we are still in the base search UI
        if (baseUI.activeInHierarchy)
        {
            baseUI.SetActive(false);
            searchUI.SetActive(true);
        }



        //load the current item
        switch (selectionType)
        {
            case InformationType.TEXT:
                text.SetActive(true);
                text.GetComponent<TextMeshProUGUI>().text = tempTextResult;
                break;
            case InformationType.IMAGE:
                image.SetActive(true);
                image.GetComponent<Image>().sprite = tempImgResult;
                break;
        }
    }

    //DONE
    public void unloadAssets()
    {
        text.SetActive(false);
        image.SetActive(false);
    }


    //When the search button is pressed
    public void search()
    {
        //reset the slider values to 0
        baseSlider.value = 0;
        searchSlider.value = 0;

        //mark the current time to know how much time has passed
        startTime = Time.time;
        loading = true;


        //get the information what the player has selected from the article
        //article.get_info

        //whats the type of information that we want to have selected. 
        
        
        
        switch (selectionType)
        {
            case InformationType.TEXT:
                loadingDuration = textSearchTime;
                break;
            case InformationType.IMAGE:
                loadingDuration = imageSearchTime;
                break;
            default:
                loadingDuration = baseSearchTime; break;
        }
    }

}


