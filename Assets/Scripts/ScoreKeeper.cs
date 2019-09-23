using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour
{
    private Text text;
    private float time;
    public int score;
    

    // Start is called before the first frame update
    void Start()
    {
        
        time = 0;
        score = 0;
        text = GetComponent<Text>();
        
    }


    private void LateUpdate()
    {
        time += Time.deltaTime;
        

        text.text = "Score: " + score;//+ //dangerScore;
    }
}
