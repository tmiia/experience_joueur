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
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 9;
    [SerializeField] private float groundedDistance = 8f;
    [SerializeField] public int life = 15;
    [SerializeField] private int collectibleLayer = 6;
    [SerializeField] private bool lamp = false;
    [SerializeField] private bool battery = false;
    [SerializeField] private UnityEngine.Rendering.Universal.Light2D characterLight;


    [SerializeField] private string itemLifeSecond = "lamp";
    [SerializeField] private string itemLifeFirst = "battery";


    [SerializeField] private GameObject character;


    [SerializeField] private LayerMask groundedLayer;
    [SerializeField] private LayerMask deathZoneLayer;

    private List<GameObject> listSpirits = new List<GameObject>();

    Vector2 movement;

    private Rigidbody2D rb2D;
    public Animator animator;

    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        characterLight = GameObject.Find("CharacterLight").GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        characterLight.pointLightOuterRadius = 43;
        GameObject spiritList = GameObject.Find("Spirits");
        for (int i = 0; i < spiritList.transform.childCount; i++)
        {
            GameObject dialogue = spiritList.transform.GetChild(i).gameObject;
            listSpirits.Add(dialogue);
        }
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.x);

        animator.SetFloat("Speed", 0);


        if (IsGrounded())
        {
            animator.SetBool("IsJumping", false);
        }

        Move();
        Jump();
        Crawl();
    }

    private void Move()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position = transform.position + Time.deltaTime * speed * Vector3.right;
            animator.SetFloat("Speed", movement.magnitude);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position = transform.position + Time.deltaTime * speed * Vector3.left;
            animator.SetFloat("Speed", movement.magnitude);
        }
    }

    private void Jump()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            animator.SetBool("IsJumping", true);

            if (IsGrounded())
            {
                rb2D.velocity = new Vector2(0, jumpForce);
            }

        }
    }

    private void Crawl()
    {
        if (Input.GetKey(KeyCode.DownArrow))
        {
            animator.SetBool("IsCrawling", true);
        }
        else
        {
            animator.SetBool("IsCrawling", false);
        }
    }

    bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.down, groundedDistance, groundedLayer);

        if (hit.collider != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void UpdateSpirits()
    {
        if (life == 15)
        {
            characterLight.pointLightOuterRadius = 43;
            listSpirits[1].SetActive(true);
            listSpirits[2].SetActive(true);
        }
        else if (life == 10)
        {
            characterLight.pointLightOuterRadius = 33;
            listSpirits[2].SetActive(false);
        }
        else if (life == 5)
        {
            characterLight.pointLightOuterRadius = 23;
            listSpirits[1].SetActive(false);
        }

    }

    private void GameOver()
    {
        character.SetActive(false);
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

        GameManager.ChangeScene("memory-0");
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
