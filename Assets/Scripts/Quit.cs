using UnityEngine;
using UnityEngine.UI;

public class Quit : MonoBehaviour
{
   [SerializeField] private GameObject back;
   private bool flag;
   
   public void Exit()
   {
      Application.Quit();
   }

   public void Resume()
   {
      flag = false;
      back.SetActive(false);
   }

   public void ExitWindow()
   {
      flag = true;
      back.SetActive(true);
   }
   
   void Update()
   {
      if (Input.GetKeyDown(KeyCode.Escape))
      {
         flag = !flag;
         
         if (flag)
         {
            back.SetActive(true);
         }
         else
         {
            back.SetActive(false);
         }
      }
   }
}
