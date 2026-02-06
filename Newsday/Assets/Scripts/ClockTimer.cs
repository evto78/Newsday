using TMPro;
using UnityEngine;

public class ClockTimer : MonoBehaviour
{
    public int startingTime = 0900;//HH:MM
    public int workDayLength = 1000;//MM:SS
    public float timer = 0;
    public string clockTime;
    public TextMeshProUGUI clock;
    
    void Start()
    {
        timer = 0;
    }

    
    void Update()
    {
        timer += Time.deltaTime;    
        clock.text = timeText();
    }

    private string timeText()
    {
        float baseTime = (startingTime%100) + Mathf.FloorToInt(startingTime/100)*60 + timer;
        
        int hour, minute;
        hour = Mathf.FloorToInt(baseTime / 60);//this works
        minute = Mathf.FloorToInt(baseTime) - hour*60;

        if (minute < 10) return hour+":0"+minute;
        else return hour+":" + minute;
    }
}
