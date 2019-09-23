using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Handles time left possessing enemy
 */
public class TimeLeft : MonoBehaviour
{
    public GameObject player;
    private float timeleft;
    private Text text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    private void LateUpdate() //update time left
    {
        timeleft = player.GetComponent<PlayerController>().timeInside;
        text.color = new Color(1f, 1f, 1f, 0f);

        if (timeleft > 0)
        {
            text.color = new Color(255f, 255f, 255f);
            text.text = "" + (int)timeleft; //concat
            text.color = new Color(1f, 1f, 1f, 1f);
        }
        else if (timeleft == 0)
        {
            text.color = new Color(1f, 1f, 1f, 0f);
        }
    }
}
