using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Handles switching between scenes
 */
public class MainMenuButtons : MonoBehaviour
{
    public void playGame()
    {
        Application.LoadLevel(1);
    }

    public void toInstructions()
    {
        Application.LoadLevel(2);
    }

    public void toMainMenu()
    {
        Cursor.visible = true;
        Application.LoadLevel(0);
    }

    public void toCredits()
    {
        Application.LoadLevel(3);
    }

    public void quitGame()
    {
        Application.Quit();
    }
  
}
