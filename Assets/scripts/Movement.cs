using UnityEngine;

public class Movement : MonoBehaviour
{
    // moves a beat towards the player
    public Rigidbody rb;
    public int speed = 100;

    // The beats get a velocity at the start of the level
    void Start()
    {
        rb.AddForce(0, 0, -speed, ForceMode.VelocityChange);
    }
}