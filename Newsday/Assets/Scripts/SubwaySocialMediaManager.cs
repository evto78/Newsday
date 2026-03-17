using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class SubwaySocialMediaManager : MonoBehaviour
{
    [Header("References")]
    public GameObject socialMediaMessegePrefab;
    List<SubwayPhoneMessege> currentMesseges = new List<SubwayPhoneMessege>();

    public ScrollRect socialMediaScrollRect;
    public Transform socialMediaMessegeHolder;

    [Header("Messege Content")]
    public List<string> quedMesseges; //Add messeges here from other scripts as a result of the player making choices
    public List<string> fillerMesseges;
    List<string> selectedMesseges;

    [Header("Messege Generation")]
    public Vector2 dailyRandomMessegesMinMax;
    public List<Sprite> possibleIcons;
    public List<Color> possibleColors;
    public List<string> possibleUsernameStarts;
    public List<string> possibleUsernameMiddles;

    void Start()
    {
        
    }
    private void OnEnable()
    {
        //Generate a new batch of random messeges to the social media app
        if (Application.isPlaying) { UpdateMesseges(); }
    }
    public void UpdateMesseges()
    {
        selectedMesseges = new List<string>();
        int loops = currentMesseges.Count;
        //Clear old messeges
        for(int i = 0; i < loops; i++)
        {
            Destroy(currentMesseges[0].gameObject);
            currentMesseges.RemoveAt(0);
        }
        loops = (quedMesseges.Count + Random.Range((int)dailyRandomMessegesMinMax.x, (int)dailyRandomMessegesMinMax.y));
        //Create new messeges
        for (int i = 0; i < loops; i++)
        {
            if (quedMesseges.Count > 0)
            {
                selectedMesseges.Add(quedMesseges[0]);
                quedMesseges.RemoveAt(0);
            }
            else
            {
                string fillerMessege = fillerMesseges[Random.Range(0, fillerMesseges.Count)];
                if (!selectedMesseges.Contains(fillerMessege))
                {
                    selectedMesseges.Add(fillerMessege);
                }
            }
        }
        loops = selectedMesseges.Count;
        for (int i = 0; i < loops; i++)
        {
            if(selectedMesseges.Count < 1) { break; }
            int selectedIndex = Random.Range(0, selectedMesseges.Count);
            CreateSocialMediaMessege(selectedMesseges[selectedIndex]);
            selectedMesseges.RemoveAt(selectedIndex);
        }
    }
    public void CreateSocialMediaMessege(string messegeText)
    {
        if (messegeText == null || messegeText == "") {  return; }
        GameObject newMessege = Instantiate(socialMediaMessegePrefab, socialMediaMessegeHolder);
        SubwayPhoneMessege newMessegeData = newMessege.GetComponent<SubwayPhoneMessege>();
        currentMesseges.Add(newMessegeData);

        GenerateMessegeUser(newMessegeData);

        newMessegeData.bodyText.text = messegeText;
    }
    void GenerateMessegeUser(SubwayPhoneMessege messege)
    {
        //Randomly generate the messeges icon, username, and color
        messege.icon.sprite = possibleIcons[Random.Range(0, possibleIcons.Count)];
        Color messegeColor = possibleColors[Random.Range(0, possibleColors.Count)];
        messege.icon.color = messegeColor;
        Color.RGBToHSV(messegeColor, out float H, out float S, out float V);
        messege.usernameText.color = Color.HSVToRGB(H, 0.5f, V);

        string username = possibleUsernameStarts[Random.Range(0, possibleUsernameStarts.Count)];
        username = username + "_" + possibleUsernameMiddles[Random.Range(0, possibleUsernameMiddles.Count)];
        username = username + "_" + Random.Range(0,10).ToString() + Random.Range(0, 10).ToString() + Random.Range(0, 10).ToString();

        messege.usernameText.text = username;
    }
}
