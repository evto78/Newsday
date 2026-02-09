using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering;
using Unity.VisualScripting;
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



    void Update()
    {
        if (loading)
        {
            loadingBar();
        }

    }


    //DONE
    public void loadingBar()
    {
        if (baseUI.activeInHierarchy)
        {
            baseSlider.value = loadingCurve.Evaluate((Time.time - startTime) / loadingDuration);
        }
        else//the base
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
                break;
            case InformationType.IMAGE:
                image.SetActive(true);
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
        startTime = Time.time;
        loading = true;
        //get the information what the player has selected from the article


        //whats the type of information that we want to have selected. 


    }

}


