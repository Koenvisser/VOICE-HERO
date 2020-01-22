
using UnityEngine;

public class BeatCollision : MonoBehaviour
{
    // variables to indacate the current beat and if it is at the right position to be pressed
    public bool Presswindow;
    public GameObject CurrentBeat;

    public GameManager gameManager;

    // When a beat enters the trigger (Player + colour) it becomes pressable
    private void OnTriggerEnter(Collider colliderinfo)
    {
        CurrentBeat = colliderinfo.gameObject;
        if (colliderinfo.gameObject.tag == "BeatY" || colliderinfo.gameObject.tag == "BeatR" || colliderinfo.gameObject.tag == "BeatB")
        {
            Presswindow = true;
        }

        // If the finish Ojbect gets to the player buttons the game goes into an endscreen
        if (colliderinfo.gameObject.tag == "Finish")
        {
            gameManager.EndGame();
            GameObject.Destroy(CurrentBeat);
        }
    }

    // When a beat exits the trigger (Player + colour) it becomes impressable
    // It also resets the streak since a beat is missed
    private void OnTriggerExit(Collider colliderinfo)
    {
        if (colliderinfo.gameObject.tag == "BeatY" || colliderinfo.gameObject.tag == "BeatR" || colliderinfo.gameObject.tag == "BeatB")
        {
            Presswindow = false;
            FindObjectOfType<Streak>().streak = 0;
        }
    }
}
