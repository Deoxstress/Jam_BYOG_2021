using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detection : MonoBehaviour
{
    public Collider avantC;
    public Collider arriereC;
    public bool blockedR, blockL;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Interactable")
        {
            transform.parent = other.transform;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Interactable")
        {
            transform.parent = null;
        }
    }
}
