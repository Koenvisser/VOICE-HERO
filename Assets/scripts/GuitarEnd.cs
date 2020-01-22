using UnityEngine;

public class GuitarEnd : MonoBehaviour
{
    // This method erases beats that were missed
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name != "Plane") 
        {
            Destroy(collision.gameObject);
        }
    }
}
