using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMenu : MonoBehaviour
{
   public void Menu(int menu)
   {
      switch (menu)
      {
         case 0:
            SceneManager.LoadScene("MapSelect");
            break;
         case 1:
            Debug.Log("App Quit...");
            Application.Quit();
            break;
      }
   }
}
