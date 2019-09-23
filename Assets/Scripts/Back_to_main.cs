using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Back_to_main : MonoBehaviour
{


    public void playGame()
    {
        Application.LoadLevel(1);
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
