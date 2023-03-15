using Scene;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMenu : MonoBehaviour
{
   public void Menu(int menu)
   {
      switch (menu)
      {
         case 0:
            // SceneManager.LoadScene("MapSelect");
            SceneLoader.Instance.TransitionScene("MapSelect");
            break;
         case 1:
            Debug.Log("App Quit...");
            Application.Quit();
            break;
      }
   }
}
