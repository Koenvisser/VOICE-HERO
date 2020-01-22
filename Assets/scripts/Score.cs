using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    // this UI component keeps track of the score
    
    // references to the streak and to the text in the UI that needs to be changed
    public Text Scoretext;
    public Streak streak;

    int OldScore = 0;
    int NewPoints = 0;
    public int score;
    void Update()
    {
        // Changes the score this happens when the score changes because a player pressed a beat a the right time
        // When the PlayerAction class changes the score Oldscore and score will not be equal starting the if statement
        // Only the new points need to be multiplied by the streak so we find how many points where added multiply and add it to the old score
        // Then the text element of the UI is changed to be the score at that time
        if (OldScore != score)
        {
            NewPoints = score - OldScore;
            NewPoints *= streak.times;
            score = OldScore + NewPoints;
            OldScore = score;
        }
        
        string sctext = "Score " + score.ToString();


        Scoretext.text = sctext;
    }
}
