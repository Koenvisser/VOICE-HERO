using UnityEngine;
using UnityEngine.UI;

public class Streak : MonoBehaviour
{
    // keeps track of the streak and changes the streak in the player UI if needed
    public int streak;
    public Text StreakText;
    public int times = 1;
    // The streak is changed in the PlayerAction class here we only need to moniter it, update the UI and add a multiplier to the score
    // For every 10 streak 1 is added to the score multiplier
    void Update()
    {
        if (streak < 10)
        {
            times = 1;
            //FindObjectOfType<FireActive>().StopTheFire();
        }
        else if (streak >= 10 && streak < 20)
        {
            times = 2;
        }
        else if (streak >= 20 && streak < 30)
        {
            times = 3;
        }
        else if (streak >= 30)
        {
            times = 4;
            //FindObjectOfType<FireActive>().StartTheFire();
        }
        string sttext = "Streak " + streak.ToString();
        StreakText.text = sttext + System.Environment.NewLine + times.ToString() + "x";
    }
}
