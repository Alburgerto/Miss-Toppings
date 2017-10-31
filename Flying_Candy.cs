using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flying_Candy : MonoBehaviour {

    public enum State { Flying, Attacking };

    public State m_state;
    public int m_health = 3;
    public Sprite[] m_sprites;

    private bool m_facingRight;
    private bool m_attacking;
    private int m_currentSprite;
    private SpriteRenderer m_spriteRenderer;
    private GameObject m_toppings;
    private Miss_Toppings m_toppingsScript;

    // Use this for initialization
    void Start() {
        m_facingRight = true;
        m_state = State.Flying;
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_toppings = GameObject.FindGameObjectWithTag("Player");
        m_toppingsScript = m_toppings.GetComponent<Miss_Toppings>();
        StartCoroutine("chooseAction");
        StartCoroutine("animations");
    }

    private void FixedUpdate() {
        if ((!m_facingRight && (m_toppingsScript.getPosition().x - transform.position.x > 0) || (m_facingRight && (m_toppingsScript.getPosition().x - transform.position.x <= 0)))) {
            flip();
        }
    }

    void flip() {
        m_facingRight = !m_facingRight;
        Vector2 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }


    IEnumerator animations()
    {
        while (true)
        {
            if (m_state == State.Flying) {
                if (m_currentSprite >= 3) {
                    m_spriteRenderer.sprite = m_sprites[0];
                    m_currentSprite = 0;
                } else {
                    ++m_currentSprite;
                    m_spriteRenderer.sprite = m_sprites[m_currentSprite];
                }
                yield return new WaitForSeconds(0.3f);
            } else if (m_state == State.Attacking) {
                if (m_currentSprite < 4 || m_currentSprite >= 6) {
                    m_currentSprite = 4;
                    m_spriteRenderer.sprite = m_sprites[m_currentSprite];
                } else {
                    ++m_currentSprite;
                    m_spriteRenderer.sprite = m_sprites[m_currentSprite];
                }
                yield return new WaitForSeconds(0.5f);
            }

            
        }
    }

    IEnumerator chooseAction() {
        Vector2 playerPos;
        GameObject candy;
        while (true) {
            if (!m_toppings) {
                yield return new WaitForSeconds(1.5f);
            }
            playerPos = m_toppings.transform.position;
            if (Vector2.Distance(transform.position, playerPos) <= 10 ) {
                m_state = State.Attacking;
                candy = Instantiate(Resources.Load("ThrowableSweet"), transform.position, Quaternion.identity) as GameObject;
            } else {
                m_state = State.Flying;
            }
            yield return new WaitForSeconds(1.5f);
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
