using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformLightDanger : Entity

{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Eblo"))
        {
            Destroy(this.gameObject);
        }
    }

}