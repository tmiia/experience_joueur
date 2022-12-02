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
    [SerializeField] public int life = 5;
    [SerializeField] private int collectibleLayer = 6;
    [SerializeField] private bool lamp = false;
    [SerializeField] private bool battery = false;


    [SerializeField] private string itemLifeSecond = "lamp";
    [SerializeField] private string itemLifeFirst = "battery";


    [SerializeField] private GameObject character;


    [SerializeField] private LayerMask groundedLayer;
    [SerializeField] private LayerMask deathZoneLayer;


    Vector2 movement;

    private Rigidbody2D rb2D;
    public Animator animator;

    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();

    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.x);
        animator.SetFloat("Speed", movement.magnitude);

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
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position = transform.position + Time.deltaTime * speed * Vector3.left;
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
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
        } else
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

    private void GameOver()
    {
        character.SetActive(false);
        SceneManager.LoadScene("gameOver");
    }

    private void UpdateLife(int value, bool isCompleted = false)
    {
        if(life > 0 && !isCompleted)
        {
            life += value;
            if(life == 0)
            {
                GameOver();
            }
        } else if(life <= 0)
        {
            GameOver();
        } else if (isCompleted)
        {
            life = value;
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
        } else if (collision.gameObject.tag == "enemy")
        {   
            UpdateLife(-5);
        } else if (collision.gameObject.layer == collectibleLayer)
        {
            if (collision.gameObject.tag != "memory")
            {
                PickItem(collision.gameObject);
            } else
            {
                TriggerMemory(GameManager.currentLevel);
            }
            Destroy(collision.gameObject);
        }
    }

    private void PickItem(GameObject item)
    {
        string itemTag = item.tag;
       
        if (itemTag == itemLifeFirst)
        {
            battery = true;
            Inventory.DisplayInventory(itemTag);
        } else if (itemTag == itemLifeSecond)
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
