using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthKeeper : MonoBehaviour
{
    public GameObject player;
    public Sprite[] healthArray = new Sprite[6] ;
    private int health;
    private Image healthBar;

    // Start is called before the first frame update
    void Start()
    {
        health = 5;
        healthBar = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        health = player.GetComponent<PlayerController>().health;
        healthBar.sprite = healthArray[health];
    }
}
