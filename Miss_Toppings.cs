using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Miss_Toppings : MonoBehaviour {

    public enum State { Idle, Walking, Attacking, Talking };

    public Sprite[] m_sprites;
    public float m_move     = 0;
    public int m_speed      = 4;
    public int m_jumpThrust = 450;
    public int m_health     = 5;

    private float m_timeBetweenAttacks = 1;
    private float m_currentTimeBetweenAttacks = 0;

    public Transform m_groundCheck;
    public LayerMask m_layerMask;
    public float m_groundRadius = 0.2f;

    private int m_currentSprite = 0;
    private SpriteRenderer m_spriteRenderer;
    private Vector3 m_respawn;
    private Vector3 m_cameraRespawn;
    private bool m_grounded     = false;
    private bool m_hurting      = false;
    private bool m_isEnemyInRange = false;
    private GameObject m_enemyInRange;
    private Rigidbody2D m_rigidBody;
    public State m_state;
    public bool m_facingRight = true;
    public bool m_happyFace;

    private PlaySound m_soundPlayer;
    private GameController m_gameController;
    public bool m_input;

    private void Awake()
    {
        m_cameraRespawn = new Vector3(-4.36f, 0, -10);
        if (SceneManager.GetActiveScene().name == "2_Level")
        {
            m_respawn = new Vector3(0, 0, 0);
            m_input = false;
        } else {
            m_respawn = new Vector3(-10, -0.75f, 0);
            m_input = true;
        }
    }

    void Start () {
        m_state = State.Idle;
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        m_soundPlayer = GameObject.FindGameObjectWithTag("Sounds").GetComponent<PlaySound>();

        StartCoroutine("animations");
    }

    void Update()
    {
        m_currentTimeBetweenAttacks += Time.deltaTime;
        if (m_input)
        {
            if (m_grounded && Input.GetKeyDown(KeyCode.Z)) // Jump
            {
                m_soundPlayer.playClip(0);
                m_rigidBody.AddForce(new Vector2(0, m_jumpThrust));
                m_grounded = false;
            }

            if (Input.GetKeyDown(KeyCode.X) && m_state != State.Attacking && (m_currentTimeBetweenAttacks > m_timeBetweenAttacks))   // Attack
            {
                m_currentTimeBetweenAttacks = 0;
                m_state = State.Attacking;
                StartCoroutine("attacking");

                if (m_isEnemyInRange && m_enemyInRange) {
                    m_soundPlayer.playClip(1);
                    if (m_enemyInRange.name == "Cupcake") {
                        m_enemyInRange.GetComponent<Cupcake>().hurt();
                    } else if (m_enemyInRange.name == "Flying_Candy") {
                        m_enemyInRange.GetComponent<Flying_Candy>().hurt();
                    } else if (m_enemyInRange.name == "Pierre") {
                        m_enemyInRange.GetComponent<Pierre>().hurt();
                    }
                }
            }
        }

        if (!m_grounded) // Gravity
        {
            m_rigidBody.AddForce(Vector2.down * 8.5f);
        }
    }

    // Flips sprite, determines if grounded and moves horizontally
    private void FixedUpdate()
    {
        m_grounded = Physics2D.OverlapCircle(m_groundCheck.position, m_groundRadius, m_layerMask);
        if(m_input)
        {
            m_move = Input.GetAxis("Horizontal");
        }

        if (m_move != 0 && m_state == State.Idle)
        {
            m_state = State.Walking;
        } else if (m_move == 0 && (m_state == State.Idle || m_state == State.Walking))
        {
            m_state = State.Idle;
        }

        m_rigidBody.velocity = new Vector2(m_move * m_speed, m_rigidBody.velocity.y);

        if ((m_move > 0 && !m_facingRight) || (m_move < 0 && m_facingRight))
        {
            flip();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Pierre")) {
            m_isEnemyInRange = true;
            m_enemyInRange = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Pierre")) {
            m_isEnemyInRange = false;
            m_enemyInRange = null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Pierre")) {
            takeDamage();
            if (collision.gameObject.transform.position.x > 0) {
                transform.position = new Vector3(-7.5f, 0, 0);
            } else {
                transform.position = new Vector3(7.5f, 0, 0);
            }
        } else if ((collision.gameObject.CompareTag("Spikes")) || (collision.gameObject.CompareTag("Suelo"))) {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera_Follow>().StopCoroutine("shake");
            takeDamage();
            transform.position = m_respawn;
            if (SceneManager.GetActiveScene().name != "2_Level") {
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera_Follow>().deadToppings();
            }
        } else if (!m_hurting && (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Pierre"))) {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera_Follow>().StartCoroutine("shake");
            takeDamage();
            if (collision.gameObject.transform.position.x < transform.position.x) {
                m_rigidBody.AddForce(new Vector2(5000, 0));
            } else {
                m_rigidBody.AddForce(new Vector2(-5000, 0));
            }

        } else if (collision.gameObject.CompareTag("Oreo") && collision.gameObject.transform.position.y < transform.position.y) {
            collision.gameObject.GetComponent<Oreo>().stepUpon();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Oreo"))
        {
            collision.gameObject.GetComponent<Oreo>().notStepUpon();
        }
    }

    void flip() {
  //      if (m_state == State.Attacking) {
            m_state = State.Walking;
   //     }
        m_facingRight = !m_facingRight;
        Vector2 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void takeDamage() {
        m_hurting = true;
        --m_health;
        if (m_health <= 0) {
            Destroy(gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        m_gameController.loseLife();
        StartCoroutine("hurt");
    }

    public void setTalking(bool l_talking)
    {
        if (l_talking) {
            m_state = State.Talking;
        } else {
            m_state = State.Idle;
        }
    }

    public void setInput(bool l_input)
    {
        m_input = l_input;
    }

    public void setPosition(Vector2 l_pos) {
        transform.position = l_pos;
    }

    public Vector2 getPosition() {
        return transform.position;
    }

    IEnumerator attacking()
    {
        yield return new WaitForSeconds(0.3f);
        m_state = State.Idle;
        StopCoroutine("attacking");
    }

    IEnumerator hurt() {
        yield return new WaitForSeconds(1);
        m_hurting = false;
        StopCoroutine("hurt");
    }

    IEnumerator animations()
    {
        while (true)
        {
            if (m_happyFace) {
                m_currentSprite = 15;
                m_spriteRenderer.sprite = m_sprites[m_currentSprite];
                break;
            }

            if (m_hurting)
            {
                m_currentSprite = 12;
                m_spriteRenderer.sprite = m_sprites[m_currentSprite];
                break;
            }

            if (m_state == State.Talking) {
                if (m_currentSprite < 13 || m_currentSprite >= 14) {
                    
                    m_currentSprite = 13;
                    m_spriteRenderer.sprite = m_sprites[m_currentSprite];
                } else {
                    ++m_currentSprite;
                    m_spriteRenderer.sprite = m_sprites[m_currentSprite];
                }
            }
            else if (m_state == State.Idle)
            {
                if (m_currentSprite >= 4)
                {
                    m_currentSprite = 0;
                    m_spriteRenderer.sprite = m_sprites[m_currentSprite];
                }
                else 
                {
                    ++m_currentSprite;
                    m_spriteRenderer.sprite = m_sprites[m_currentSprite];
                } 
            }
            else if (m_state == State.Walking)
            {
                if (m_currentSprite < 5 || m_currentSprite >= 8)
                {
                    m_currentSprite = 5;
                    m_spriteRenderer.sprite = m_sprites[m_currentSprite];
                }
                else
                {
                    ++m_currentSprite;
                    m_spriteRenderer.sprite = m_sprites[m_currentSprite];
                }
            }
            else if (m_state == State.Attacking)
            {
                if (m_currentSprite < 9 || m_currentSprite >= 11)
                {
                    m_currentSprite = 9;
                    m_spriteRenderer.sprite = m_sprites[m_currentSprite];
                }
                else
                {
                    ++m_currentSprite;
                    m_spriteRenderer.sprite = m_sprites[m_currentSprite];
                }
            }
            yield return new WaitForSeconds(0.1f);  // Change every sprite every 0.2 sec, regardless of state
        }
        yield return new WaitForSeconds(0.2f); // Hurting
        StartCoroutine("animations");
    }



}
