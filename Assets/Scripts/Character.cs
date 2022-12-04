using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Character : MonoBehaviour
{

    // Comportement du personnage

    [SerializeField] public int life = 15;

    public float speed = 2;
    private float jumpPower = 250;
    private bool isGrounded = false;
    private bool isJumping = false;
    private bool isCrawling = false;
    private float groundCheckRadius = 0.2f;
    private float topCheckRadius = 0.2f;

    //// Environnement

    private float horizontalMove;
    private float verticalMove;
    private List<GameObject> listSpirits = new List<GameObject>();

    [SerializeField] private int collectibleLayer = 6;
    [SerializeField] private bool lamp = false;
    [SerializeField] private bool battery = false;
    //[SerializeField] private UnityEngine.Rendering.Universal.Light2D characterLight;


    [SerializeField] private string itemLifeSecond = "lamp";
    [SerializeField] private string itemLifeFirst = "battery";

    //Vector2 movement;

    public Animator animator;
    private Rigidbody2D rb2D;
    private Rigidbody2D rb;

    [SerializeField] Collider2D standingCollider;
    [SerializeField] Transform groundCheck;
    [SerializeField] Transform topCheck;
    [SerializeField] private LayerMask groundedLayer;
    [SerializeField] private LayerMask deathZoneLayer;


    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        Debug.Log(this);
        //characterLight = GameObject.Find("CharacterLight").GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        //characterLight.pointLightOuterRadius = 43;

        GameObject spiritList = GameObject.Find("Spirits");

        for (int i = 0; i < spiritList.transform.childCount; i++)
        {
            GameObject dialogue = spiritList.transform.GetChild(i).gameObject;
            listSpirits.Add(dialogue);
        }
    }

    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");
        verticalMove = Input.GetAxisRaw("Vertical");
        animator.SetFloat("yVelocity", rb.velocity.y);

        if (Input.GetButtonDown("Crawl"))
        {
            isCrawling = true;
        }
        else if (Input.GetButtonUp("Crawl"))
        {
            isCrawling = false;
        }
        if (animator.GetFloat("yVelocity") > 0)
        {
            animator.SetBool("IsJumping", true);
        }
        else
        {
            animator.SetBool("IsJumping", false);
        }
        if (isGrounded)
        {
            animator.SetBool("IsGrounded", true);
        }
        else
        {
            animator.SetBool("IsGrounded", false);
        }

        //animator.SetFloat("Horizontal", horizontalMove);
        //animator.SetFloat("Vertical", verticalMove);

        animator.SetFloat("Speed", 0);
    }

    void FixedUpdate()
    {
        GroundCheck();
        //Move(horizontalMove);
        Crawl(isCrawling);
        Jump();

    }

    //    private void Move(float direction)
    //    {
    //        float xAvis = direction * speed * 100 * Time.fixedDeltaTime;
    //        Vector2 targetSpeed = new Vector2(xAvis, rb.velocity.y);

    //        rb.velocity = targetSpeed;
    //    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            isJumping = true;
            rb.AddForce(new Vector2(0f, jumpPower));
        }
    }

    private void Crawl(bool isCrawling)
    {
        standingCollider.enabled = !isCrawling;

        if (Physics2D.OverlapCircle(topCheck.position, topCheckRadius, groundedLayer))
        {
            if (!isCrawling)
            {
                standingCollider.enabled = false;
            }
        }
    }

    private void GroundCheck()
    {
        isGrounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundCheckRadius, groundedLayer);
        if (colliders.Length > 0)
        {
            isGrounded = true;
        }
    }

    private void UpdateSpirits()
    {
        if (life == 15)
        {
            //characterLight.pointLightOuterRadius = 43;
            listSpirits[1].SetActive(true);
            listSpirits[2].SetActive(true);
        }
        else if (life == 10)
        {
            //characterLight.pointLightOuterRadius = 33;
            listSpirits[2].SetActive(false);
        }
        else if (life == 5)
        {
            //characterLight.pointLightOuterRadius = 23;
            listSpirits[1].SetActive(false);
        }

    }

    private void GameOver()
    {
        SceneManager.LoadScene("gameOver");
    }

    private void UpdateLife(int value, bool isCompleted = false)
    {
        if (life > 0 && !isCompleted)
        {
            life += value;
            UpdateSpirits();

            if (life == 0)
            {
                GameOver();
            }
        }
        else if (life <= 0)
        {
            GameOver();
        }
        else if (isCompleted)
        {
            life = value;
            UpdateSpirits();
        }
    }

    void TriggerMemory(int level)
    {
        string memory = "memory-" + level.ToString();
        GameManager.ChangeScene(memory);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "zone51")
        {
            GameOver();
        }
        else if (collision.gameObject.tag == "enemy")
        {
            UpdateLife(-5);
        }
        else if (collision.gameObject.layer == collectibleLayer)
        {
            if (collision.gameObject.tag != "memory")
            {
                PickItem(collision.gameObject);
            }
            else
            {
                TriggerMemory(GameManager.currentLevel);
            }
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.tag == "loader")
        {
            GameObject dialogueScene = GameObject.Find("DialoguesContainer").transform.GetChild(0).gameObject;
            dialogueScene.SetActive(true);

            StartCoroutine(MemoryTiming());
        }
    }
    IEnumerator MemoryTiming()
    {
        yield return new WaitForSeconds(3);

        GameManager.ChangeLevel();
    }

    private void PickItem(GameObject item)
    {
        string itemTag = item.tag;

        if (itemTag == itemLifeFirst)
        {
            battery = true;
            Inventory.DisplayInventory(itemTag);
        }
        else if (itemTag == itemLifeSecond)
        {
            lamp = true;
            Inventory.DisplayInventory(itemTag);
        }
        if (battery && lamp)
        {
            UpdateLife(15, true);
            Inventory.MaskInventory();
        }
    }
}
