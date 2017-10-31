using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cupcake : MonoBehaviour {

    public int m_health = 4;
    public Sprite[] m_sprites;

    private Vector3 m_previousPos;
    private Vector2 m_velocity = new Vector2(-3, 0);
    private int m_currentSprite;
    private bool m_grounded = false;
    private Rigidbody2D m_rigidBody;
    private SpriteRenderer m_spriteRenderer;

    // Use this for initialization
    void Start () {
        m_previousPos = transform.position;
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_rigidBody = GetComponent<Rigidbody2D>();
        StartCoroutine("moveAnim");
        StartCoroutine("changeDirection");
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Walkable"))
        {
            m_grounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Walkable"))
        {
            m_grounded = false;
        }
    }



    private void FixedUpdate()
    {
        m_rigidBody.velocity = m_velocity;

        if (!m_grounded) // Gravity
        {
            m_rigidBody.AddForce(Vector2.down * 8);
        }
    }

    IEnumerator changeDirection()
    {
        while (true)
        {
            if (m_previousPos.x == transform.position.x && m_previousPos.y == transform.position.y)
            {
                m_velocity.x = -m_velocity.x;
            }
            m_previousPos = transform.position;
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator moveAnim()
    {
        while (true)
        {
            if (m_currentSprite >= 3)
            {
                m_spriteRenderer.sprite = m_sprites[0];
                m_currentSprite = 0;
            }
            else
            {
                m_spriteRenderer.sprite = m_sprites[m_currentSprite + 1];
                ++m_currentSprite;
            }
            yield return new WaitForSeconds(0.3f);
        }
    }
    

    public void hurt()
    {
        --m_health;
        if (m_health <= 0)
        {
            Destroy(gameObject);
        }
    }

}
