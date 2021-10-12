using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class Level1_Trigger : MonoBehaviour
{
    [SerializeField] string executeBlock;
    [SerializeField] private bool triggered = false;

    public Flowchart myFlowchart;
  
    


    private void OnTriggerEnter2D(Collider2D collision)
    {
        

        if (!triggered)
        {
            triggered = true;
            if (collision.gameObject.name.Equals("Eblo"))
            {
                Eblo.Instance.Reading();
                myFlowchart.ExecuteBlock(executeBlock);
            }
        }
    }
}
