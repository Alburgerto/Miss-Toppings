using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Pierre : MonoBehaviour {

    public enum State { Idle, Walking, MeleeAttack, HariboAttack, JumpAttack, Talking, HappyEnding };

    public State m_state;
    private bool m_pierreMadBro;
    private bool m_grounded;
    private bool m_hurting;
    private bool m_facingRight;
    private bool m_isEnemyInRange;
    private bool m_scriptedSceneRunning;

    private Miss_Toppings m_toppings;
    private Rigidbody2D m_rigidBody;
    private SpriteRenderer m_spriteRenderer;
    private int m_currentSprite;
    public Sprite[] m_sprites;
    public int m_health;
    public float m_move;
    public float m_speed;
    public int m_jumpThrust;
    public bool m_happyFace;

    private List<State> m_actionOptions;

    public Transform m_groundCheck;
    public LayerMask m_layerMask;
    public float m_checkRadius;

    private void Awake() {
        if (SceneManager.GetActiveScene().name == "2_Level") {
            m_scriptedSceneRunning = true;
        } else {
            m_scriptedSceneRunning = false;
        }
        m_state = State.Idle;
        m_pierreMadBro = false;
        m_grounded = false;
        m_hurting = false;
        m_facingRight = false;
        m_isEnemyInRange = false;
        m_toppings = GameObject.FindGameObjectWithTag("Player").GetComponent<Miss_Toppings>();
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_actionOptions = new List<State>();
    }

    // Use this for initialization
    void Start() {
        StartCoroutine("animations");
    }

    private void Update() {
        if (!m_grounded) { // Gravity 
            m_rigidBody.AddForce(Vector2.down * 8);
        }
    }


    private void FixedUpdate() {
        m_grounded = Physics2D.OverlapCircle(m_groundCheck.transform.position, m_checkRadius, m_layerMask);

        if (m_move != 0 && m_state == State.Idle) {
            m_state = State.Walking;
        } else if (m_move == 0 && m_state == State.Idle && m_state == State.Walking) {
            m_state = State.Idle;
        }

        m_rigidBody.velocity = new Vector2(m_move * m_speed, m_rigidBody.velocity.y);

        if ((!m_facingRight && (m_toppings.getPosition().x - transform.position.x > 0) || (m_facingRight && (m_toppings.getPosition().x - transform.position.x <= 0)))) {
            flip();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            m_isEnemyInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            m_isEnemyInRange = false;
        }
    }

    void flip() {
        m_facingRight = !m_facingRight;
        Vector2 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void setCombat() {
        StartCoroutine("combat");
    }

    public void setScriptedScene(bool l_scriptedScene) {
        m_scriptedSceneRunning = l_scriptedScene;
    }

    public void setTalking(bool l_talking) {
        if (l_talking) {
            m_state = State.Talking;
        } else {
            m_state = State.Idle;
        }
    }

    public void setPosition(Vector2 l_pos) {
        transform.position = l_pos;
    }

    public Vector2 getPosition() {
        return transform.position;
    }

    public void hurt() {
        --m_health;
        m_hurting = true;
        if (m_health == 5) {
            m_pierreMadBro = true;
            ++m_speed; 
            GameObject.FindGameObjectWithTag("Sounds").GetComponent<PlaySound>().playClip(2);
        }
        if (m_health <= 0) {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<Level_2>().pierreDead();
        }
        StartCoroutine("hurtCor");
    }

    IEnumerator hurtCor() {
        yield return new WaitForSeconds(0.5f);
        m_hurting = false;
        StopCoroutine("hurtCor");
    }

    /*
    * 0     -> Idle
    * 1-4   -> Walking
    * 5-6   -> Melee1
    * 7-9   -> Melee2
    * 10-11 -> HariboAttack
    * 12-13 -> BombJump
    * 14-15 -> Talking
    * 16    -> Hurting
    * 
*/
    IEnumerator animations() {
        while (true) {
            if (m_happyFace) {
                m_currentSprite = 17;
                m_spriteRenderer.sprite = m_sprites[m_currentSprite];
                break;
            }
            if (m_hurting) {
                m_currentSprite = 16;
                m_spriteRenderer.sprite = m_sprites[m_currentSprite];
                break;
            }
            if (m_state == State.Talking) {
                if (m_currentSprite < 14 || m_currentSprite >= 15) {

                    m_currentSprite = 14;
                    m_spriteRenderer.sprite = m_sprites[m_currentSprite];
                } else {
                    ++m_currentSprite;
                    m_spriteRenderer.sprite = m_sprites[m_currentSprite];
                }
            } else if (m_state == State.Idle) {
                if (m_currentSprite != 0) {
                    m_currentSprite = 0;
                    m_spriteRenderer.sprite = m_sprites[m_currentSprite];
                }
            } else if (m_state == State.Walking) {
                if (m_currentSprite < 1 || m_currentSprite >= 4) {
                    m_currentSprite = 1;
                    m_spriteRenderer.sprite = m_sprites[m_currentSprite];
                } else {
                    ++m_currentSprite;
                    m_spriteRenderer.sprite = m_sprites[m_currentSprite];
                }
            } else if (m_state == State.MeleeAttack) {
                if (m_pierreMadBro) {
                    if (m_currentSprite < 7 || m_currentSprite >= 9) {
                        m_currentSprite = 7;
                        m_spriteRenderer.sprite = m_sprites[m_currentSprite];
                    } else {
                        ++m_currentSprite;
                        m_spriteRenderer.sprite = m_sprites[m_currentSprite];
                    }
                } else {
                    if (m_currentSprite < 5 || m_currentSprite >= 6) {
                        m_currentSprite = 5;
                        m_spriteRenderer.sprite = m_sprites[m_currentSprite];
                    } else {
                        ++m_currentSprite;
                        m_spriteRenderer.sprite = m_sprites[m_currentSprite];
                    }
                }
            } else if (m_state == State.HariboAttack) {
                if (m_currentSprite <= 10 || m_currentSprite > 11) {
                    m_currentSprite = 11;
                    m_spriteRenderer.sprite = m_sprites[m_currentSprite];
                } else {
                    --m_currentSprite;
                    m_spriteRenderer.sprite = m_sprites[m_currentSprite];
                }
                yield return new WaitForSeconds(0.6f);
            } else if (m_state == State.JumpAttack) {
                if (m_currentSprite < 12 || m_currentSprite >= 13) {
                    m_currentSprite = 12;
                    m_spriteRenderer.sprite = m_sprites[m_currentSprite];
                } else {
                    ++m_currentSprite;
                    m_spriteRenderer.sprite = m_sprites[m_currentSprite];
                }

            }
            yield return new WaitForSeconds(0.1f); 
        }
        yield return new WaitForSeconds(0.2f); // Hurting
        StartCoroutine("animations");
    }


    IEnumerator combat() {
        float offsetY;
        float offsetX;
        float numberOptions;
        GameObject haribo;
        while (true) {
            if (m_hurting) {
                m_move = 0;
                yield return new WaitForSeconds(0.5f);
            }
            numberOptions = 0;
            m_actionOptions.Clear();
            offsetY = Mathf.Abs(m_toppings.getPosition().y - transform.position.y);
            offsetX = Mathf.Abs(m_toppings.getPosition().x - transform.position.x);

            // Determine all possible routes of attack
            if (offsetX > 5.5f) {
                m_actionOptions.Add(State.HariboAttack);
                ++numberOptions;
            }
            if (m_isEnemyInRange) {
                m_actionOptions.Add(State.MeleeAttack);
                ++numberOptions;
            }
            if (offsetY < 2.5f && offsetX >= 3) {
                m_actionOptions.Add(State.Walking);
                ++numberOptions;
            }
 //           m_actionOptions.Add(State.JumpAttack); // May jump-attack regardless of Toppings' position
 //           ++numberOptions;

            int currentAction = Mathf.FloorToInt(Random.Range(0, numberOptions));
            if (m_actionOptions.Count > 0) {
                m_state = m_actionOptions[currentAction];
            } 

            // Attack if in range
            if (m_state == State.MeleeAttack) {
                m_move = 0;
                m_toppings.takeDamage();
            } else if (m_state == State.Walking) { // Walk in Toppings direction
                if (m_toppings.getPosition().x - transform.position.x > 0) {
                    m_move = 0.8f;
                } else {
                    m_move = -0.8f;
                } 
            } else if (m_state == State.HariboAttack) {
                m_move = 0;
                haribo = Instantiate(Resources.Load("Haribo"), transform.position, Quaternion.identity) as GameObject;
            }/* else if (m_state == State.JumpAttack) {
                while (transform.position.y != 4.5f) {

                }
            }  */

            yield return new WaitForSeconds(1.5f);
        }
        yield return null;
    }


}