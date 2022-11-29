using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
//using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Character : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10;
    [SerializeField] private float groundedDistance = 2f;


    [SerializeField] private GameObject character;
    [SerializeField] private LayerMask groundedLayer;
    [SerializeField] private LayerMask deathZoneLayer;

    private Rigidbody2D rb2D;

    // Awake : l'intérêt est de paraméter l'objet lui-même (couleur, position), il se prépare
    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    
    }

    // Start is called before the first frame update --> quand on fait le lien entre les objets
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
    }

    private void Move()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position = transform.position + Time.deltaTime * speed * Vector3.right; // deltatime = le temps qui s'est écoulé entre deux updates
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position = transform.position + Time.deltaTime * speed * Vector3.left;
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && (IsGrounded()))
        { // avec GetKeyDown c'est au moment où j'appuie, et pas tant que j'appuie comme GetKey
            rb2D.velocity = new UnityEngine.Vector2(0, jumpForce);
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
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "zone51")
        {
            GameOver();
        }
    }
}
