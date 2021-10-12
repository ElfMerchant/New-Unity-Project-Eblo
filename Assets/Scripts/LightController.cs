using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightController : MonoBehaviour
{
    [SerializeField] private float enterItensity;
    [SerializeField] private float exitItensity;
    [SerializeField] private bool usingGlobal = false;
    [SerializeField] private bool usingPlayer = true;

    //    Light2D light2D;
    private GameObject lightObj;
    private GameObject lightObjGlobal;

    void Start()
    {
        lightObj = GameObject.Find("Eblo_sprite");
        lightObjGlobal = GameObject.Find("Global Light 2D");
    }

    private void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Eblo"))
        {

            if (usingPlayer)
                lightObj.GetComponent<Light2D>().intensity = enterItensity;

            if (usingGlobal)
                lightObjGlobal.GetComponent<Light2D>().intensity = enterItensity;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (usingPlayer)
            lightObj.GetComponent<Light2D>().intensity = exitItensity;

        if (usingGlobal)
            lightObjGlobal.GetComponent<Light2D>().intensity = exitItensity;
    }
}
