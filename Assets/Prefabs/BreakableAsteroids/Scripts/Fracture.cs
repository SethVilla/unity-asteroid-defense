using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fracture : MonoBehaviour
{
    [Tooltip("\"Fractured\" is the object that this will break into")]
    public GameObject fractured;
 
    public void FractureObject()
    {
        GameObject fracturedInstance = Instantiate(fractured, transform.position, transform.rotation);
        fracturedInstance.transform.localScale = transform.localScale * 10f;
        
        // Destroy the fractured debris after 2 seconds
        Destroy(fracturedInstance, 2f);
        
        Destroy(gameObject);
    }
}
