using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeActivate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DeActivateSelf", 1.5f);
    }

    // Update is called once per frame
    void DeActivateSelf()
    {
        gameObject.SetActive(false);
    }
}
