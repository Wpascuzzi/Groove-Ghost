using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveCounter : MonoBehaviour
{
    public GameObject waveGen;
    private Text text;
    public int waveNum;


    // Start is called before the first frame update
    void Start()
    {
        
        text = GetComponent<Text>();

    }


    private void LateUpdate()
    {
        waveNum = waveGen.GetComponent<WaveSpawner>().currentWave;
        text.text = "Wave: " + waveNum;
    }
}
