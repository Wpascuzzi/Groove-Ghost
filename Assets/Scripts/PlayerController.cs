using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rbd;
    private SpriteRenderer playerSpriteRenderer;
    private Vector3 mousePosition;
    private Vector3 relativeMouse;
    private RaycastHit2D stunHit;
    private RaycastHit2D possessHit;
    private SpriteRenderer possessSprite;
    private bool possessing;
    private float stunEffectDuration;
    private float possessEffectDuration;
    private bool usingAbility;
    private string instrumentPossessed;


    private bool invincible = false;
    public float bulletSpeed = 5;
    public float beamDensity = 15;
    public int ringSize = 9;

    public int health;
    public float speed;
    public float possessRange;
    public float stunRange;
    public LayerMask possessMask;
    public Sprite playerSprite;
    public GameObject stunEffect;
    public GameObject possessEffect;
    public GameObject playerBullet;
    public Text GameOverLogo;
    public Image sadBoi;
    public Image blackBox;
    public int max_health = 3; //max health
    public float invincibilityTime = 3f;
    public float blueTime;
    public float yellowTime;
    public float pinkTime;
    public float timeInside;
    public float timeChillin;

    private float nextFire = 0;
    public float pinkFireRate = 0.2f;
    public float blueFireRate = 0.2f;
    public float yellowFireRate = 0.2f;
    public int beamCount = 10;

    public AudioClip[] clips = new AudioClip[10];
    public AudioSource source;
    public GameObject mainTheme;

    //private int[] tonePow = { 0, 2, 2, 1, 2, 2, 2, 1 };
    private int[] tonePow = { 0, 2, 4, 5, 7, 9, 11, 12 }; //big ol intervals
    int n = 0;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        Time.timeScale = 1;
        rbd = GetComponent<Rigidbody2D>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        health = max_health;
        possessing = false;
        stunEffect.SetActive(false);
        possessEffect.SetActive(false);
        usingAbility = false;
        instrumentPossessed = "None";
        Physics2D.IgnoreLayerCollision(10, 11, true); //for player bullets
        Physics2D.IgnoreLayerCollision(11, 11, true);
        Physics2D.IgnoreLayerCollision(10, 13, true);
        Physics2D.IgnoreLayerCollision(12, 13, true);
        Physics2D.IgnoreLayerCollision(11, 12, true);
        Physics2D.IgnoreLayerCollision(12, 12, true);
        GetComponent<SpriteRenderer>().color = new Color(.6f, 1f, 1f, .9f);

        GameOverLogo.color = new Color(1f, 1f, 1f, 0f);
        sadBoi.color = new Color(1f, 1f, 1f, 0f);
        blackBox.color = new Color(1f, 1f, 1f, 0f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!usingAbility)
        {
            HandleMovement();
        }
        UpdateHealth();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 11 && !invincible)
        {
            Hit();
        }
    }

    void Die()
    {
        source.clip = clips[3];
        source.Play();
        mainTheme.GetComponent<AudioSource>().Stop();
        Time.timeScale = 0;
        GameOverLogo.color = new Color(1f, 0f, 0f, 1f);
        sadBoi.color = new Color(1f, 1f, 1f, 1f);
        blackBox.color = new Color(0f, 0f, 0f, 1f);

    }

    void Hit()
    {
        source.clip = clips[4];
        source.Play();
        health--;
        StartCoroutine(Invincibility(invincibilityTime));
    }
    private void Update()
    {
        HandlePossessing();
        if (instrumentPossessed == "None")
        {
            HandleStunning();
            GetComponent<Animator>().SetBool("possessing_hg", false);
            GetComponent<Animator>().SetBool("possessing_tri", false);
            GetComponent<Animator>().SetBool("possessing_tymp", false);
        }
        else if (instrumentPossessed == "Blue(Clone)")
        {
            GetComponent<Animator>().SetBool("possessing_hg", false);
            GetComponent<Animator>().SetBool("possessing_tri", true);
            GetComponent<Animator>().SetBool("possessing_tymp", false);
            HandleBlue();
        }
        else if (instrumentPossessed == "Yellow(Clone)")
        {
            GetComponent<Animator>().SetBool("possessing_hg", true);
            GetComponent<Animator>().SetBool("possessing_tri", false);
            GetComponent<Animator>().SetBool("possessing_tymp", false);
            GetComponent<Animator>().SetBool("firing", usingAbility);
            StartCoroutine(HandleYellow());
        }
        else if (instrumentPossessed == "Pink(Clone)")
        {
            GetComponent<Animator>().SetBool("possessing_hg", false);
            GetComponent<Animator>().SetBool("possessing_tri", false);
            GetComponent<Animator>().SetBool("possessing_tymp", true);
            HandlePink();
        }
        else
        {
            HandleStunning();
        }

        if (stunEffect.activeInHierarchy)
        {
            stunEffectDuration -= Time.deltaTime;
            if (stunEffectDuration <= 0)
            {
                stunEffect.SetActive(false);
                usingAbility = false;
            }
        }
        if (possessEffect.activeInHierarchy)
        {
            possessEffectDuration -= Time.deltaTime;
            if (possessEffectDuration <= 0)
            {
                possessEffect.SetActive(false);
                usingAbility = false;
            }
        }
        if (possessing == true)
        {
            timeChillin += Time.deltaTime;
        }

        if (timeInside > 0 && possessing == true)
        {
            timeInside -= Time.deltaTime;
        }
        else if (timeInside <= 0 && possessing == true)
        {
            possessing = false;
            source.clip = clips[7];
            source.Play();
            playerSpriteRenderer.sprite = playerSprite;
            instrumentPossessed = "None";
            GetComponent<SpriteRenderer>().color = new Color(.6f, 1f, 1f, .9f);
        }

        GetComponent<Animator>().SetFloat("speed", Mathf.Abs(rbd.velocity.magnitude));
    }

    void HandleMovement()
    {
        if (Input.GetKey(KeyCode.W))
        {
            rbd.velocity = new Vector2(rbd.velocity.x, speed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            rbd.velocity = new Vector2(rbd.velocity.x, -speed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            rbd.velocity = new Vector2(-speed, rbd.velocity.y);
            GetComponent<SpriteRenderer>().flipX = false;
        }
        if (Input.GetKey(KeyCode.D))
        {
            rbd.velocity = new Vector2(speed, rbd.velocity.y);
            GetComponent<SpriteRenderer>().flipX = true;
        }


    }
    IEnumerator Invincibility(float seconds)
    {
        invincible = true;
        GetComponent<SpriteRenderer>().color = new Color(.6f, .8f, 1f, .70f);
        yield return new WaitForSeconds(seconds / 5);
        GetComponent<SpriteRenderer>().color = new Color(.6f, 1f, 1f, .9f);
        yield return new WaitForSeconds(seconds / 5);
        GetComponent<SpriteRenderer>().color = new Color(.6f, .8f, 1f, .70f);
        yield return new WaitForSeconds(seconds / 5);
        GetComponent<SpriteRenderer>().color = new Color(.6f, 1f, 1f, .9f);
        yield return new WaitForSeconds(seconds / 5);
        GetComponent<SpriteRenderer>().color = new Color(.6f, .8f, 1f, .70f);
        yield return new WaitForSeconds(seconds / 5);
        GetComponent<SpriteRenderer>().color = new Color(.6f, 1f, 1f, .9f);
        invincible = false;
        yield break;
    }

    //possession
    void HandlePossessing()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (possessing == false)
            {
                usingAbility = true;

                mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                relativeMouse = mousePosition - transform.position;

                possessEffect.SetActive(true);
                possessEffectDuration = 0.25f;
                possessEffect.transform.rotation = Quaternion.LookRotation(relativeMouse, Vector3.forward);

                possessHit = Physics2D.Raycast(transform.position, new Vector2(relativeMouse.x, relativeMouse.y).normalized, possessRange, possessMask);
                if (possessHit.collider != null && possessHit.transform.gameObject.GetComponent<EnemyInterface>().stunned == true)
                {
                    source.clip = clips[1];
                    source.Play();
                    StartCoroutine(Invincibility(1f));

                    stunEffect.SetActive(false);
                    possessEffect.SetActive(false);
                    usingAbility = false;

                    nextFire = Time.time;

                    possessing = true;
                    possessSprite = possessHit.transform.gameObject.GetComponent<SpriteRenderer>();
                    playerSpriteRenderer.sprite = possessSprite.sprite;
                    transform.position = possessHit.transform.position;
                    instrumentPossessed = GetEnemyType(possessHit.transform.gameObject);
                    Destroy(possessHit.transform.gameObject);
                    GetComponent<SpriteRenderer>().color = new Color(.6f, 1f, 1f, .9f);
                    
                    if (instrumentPossessed == "Blue(Clone)")
                    {
                        timeInside = blueTime;
                    }
                    else if (instrumentPossessed == "Yellow(Clone)")
                    {
                        timeInside = yellowTime;
                    }
                    else if (instrumentPossessed == "Pink(Clone)")
                    {
                        timeInside = pinkTime;
                    }
                }
            }
            //manual ejection
            else if (possessing == true && timeChillin > 0.3f)
            {
                GetComponent<SpriteRenderer>().color = new Color(.6f, 1f, 1f, .9f);
                possessing = false;
                source.clip = clips[7];
                source.Play();
                playerSpriteRenderer.sprite = playerSprite;
                instrumentPossessed = "None";
                timeInside = 0;
                timeChillin = 0;
            }
            Debug.Log("You are now:" + instrumentPossessed);
        }
    }

    //stunning
    void HandleStunning()
    {
        if (possessing == false)
        {
            if (Input.GetMouseButtonDown(0))
            {
                usingAbility = true;

                mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                relativeMouse = mousePosition - transform.position;

                stunEffect.SetActive(true);
                stunEffectDuration = 0.25f;
                stunEffect.transform.rotation = Quaternion.LookRotation(relativeMouse.normalized, Vector3.forward);

                stunHit = Physics2D.Raycast(transform.position, new Vector2(relativeMouse.x, relativeMouse.y).normalized, stunRange, possessMask);

                if (stunHit.collider != null)
                {
                    source.clip = clips[2];
                    source.Play();
                    EnemyInterface enemy = stunHit.transform.gameObject.GetComponent<EnemyInterface>();
                    enemy.stunned = true;

                    stunHit.transform.gameObject.GetComponent<Rigidbody2D>().mass *= 10;
                    
                    enemy.StartCoroutine(enemy.Unstun());
      
                    
                    stunHit.transform.gameObject.GetComponent<Animator>().SetBool("stunned", true);
                }
            }
        }
    }
    IEnumerator PlaySound()
    {
        
        source.PlayOneShot(source.clip);
        source.pitch *= Mathf.Pow(1.05946f, tonePow[n]);
        yield return new WaitWhile(() => source.isPlaying);
        //if (!source.isPlaying)
            source.pitch = 1.1f;
  
        if (n >= 7)
            n = 0;
        else
            n++;
        yield break;
    }

    void HandleBlue()
    {


        if (Input.GetMouseButtonDown(0) && Time.time > nextFire)
        {
            GetComponent<Animator>().SetBool("firing", true);
            source.clip = clips[0];
            StartCoroutine(PlaySound());
            
            nextFire = Time.time + (1 / blueFireRate);
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            relativeMouse = mousePosition - transform.position;
            GameObject playerBlt;
            playerBlt = Instantiate(playerBullet, transform.position, transform.rotation);
            playerBlt.GetComponent<Rigidbody2D>().velocity = new Vector2(relativeMouse.x,relativeMouse.y).normalized * bulletSpeed;
        }
        else
        {
            GetComponent<Animator>().SetBool("firing", false);
        }
    }

    
    IEnumerator HandleYellow()
    {
        if (Input.GetMouseButtonDown(0) && Time.time > nextFire)
        {
            source.clip = clips[6];
            StartCoroutine(PlaySound());
            
            nextFire = Time.time + 1 / yellowFireRate;
            usingAbility = true;
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            relativeMouse = mousePosition - transform.position;
            for (int i = 0; i < 10; i++)
            {
                GameObject playerBlt;
                playerBlt = Instantiate(playerBullet, transform.position, transform.rotation);
                playerBlt.GetComponent<Rigidbody2D>().velocity = new Vector2(relativeMouse.x, relativeMouse.y).normalized * bulletSpeed;
                yield return new WaitForSeconds(1f / beamDensity);
            }
            usingAbility = false;
        }
        yield return null;
    }

    
    void HandlePink()
    {
        if (Input.GetMouseButtonDown(0) && Time.time > nextFire)
        {
            GetComponent<Animator>().SetBool("firing", true);
            source.clip = clips[5];
            StartCoroutine(PlaySound());
           
            nextFire = Time.time + 1 / pinkFireRate;
            for (int i = 0; i < ringSize; i++)
            {
                float angle = (i * 2 * Mathf.PI) / ringSize;
                GameObject playerBlt;
                playerBlt = Instantiate(playerBullet, transform.position, transform.rotation);
                playerBlt.GetComponent<Rigidbody2D>().velocity = new Vector3(bulletSpeed * Mathf.Cos(angle), bulletSpeed * Mathf.Sin(angle), 0);
            }
        }
        else
        {
            GetComponent<Animator>().SetBool("firing", false);
        }
    }


    string GetEnemyType(GameObject enemy)
    {
        return enemy.name;
    }


    void UpdateHealth()
    {
        //if take damage decrement
        //if gain health increment
        if (Input.GetKeyDown(KeyCode.P))
        {
            health--;
        }
        if (health == 0)
        {
            Die();
        }
    }
}
