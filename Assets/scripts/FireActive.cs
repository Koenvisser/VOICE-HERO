using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireActive : MonoBehaviour
{

    //Here we let each fire destroy itself after a certain time
    //The fires are instantiated when a beat is correctly pushed to indacate a correct hit

    public float Timer = 0.2f;
    private void Update()
    {
        Timer -= Time.deltaTime;

        if (Timer <= 0)
        {
            Destroy(gameObject);
        }
    }
}
