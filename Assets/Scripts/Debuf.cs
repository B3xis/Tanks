using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debuf : MonoBehaviour
{
    int timer;
    // Start is called before the first frame update
    void Start()
    {
        DeleteDebuf();
    }
    void DeleteDebuf()
    {
        timer++;
        Invoke("DeleteDebuf", 0.3f);
        if (timer >= 10)
        {
            CancelInvoke("DeleteDebuf");
            timer = 0;
            GameObject.Destroy(gameObject);
        }
    }
    
}
