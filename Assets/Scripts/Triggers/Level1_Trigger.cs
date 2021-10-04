using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class Level1_Trigger : MonoBehaviour
{
    public Flowchart myFlowchart;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Eblo.Instance.Reading();
        myFlowchart.ExecuteBlock("Scene end");
    }
}
