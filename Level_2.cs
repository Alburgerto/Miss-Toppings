using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class Level_2 : MonoBehaviour {

    private enum DialogueTurn { Top, Pie };

    DialogueTurn m_dialogueTurn;

    private Miss_Toppings m_toppings;
    private Pierre m_pierre;

    private Vector2 m_toppingsPos;
    private Vector2 m_pierrePos;

    public Text m_toppingsText;
    public Text m_pierreText;

    private string[] m_toppingsLines;
    private string[] m_pierreLines;

    private string[] m_toppingsLines2;
    private string[] m_pierreLines2;

	// Use this for initialization
	void Start () {
        Cursor.visible = false;

        m_toppings = GameObject.FindGameObjectWithTag("Player").GetComponent<Miss_Toppings>();
        m_pierre   = GameObject.FindGameObjectWithTag("Pierre").GetComponent<Pierre>();

        m_dialogueTurn = DialogueTurn.Top;

        m_toppingsText.text = "";
        m_pierreText.text   = "";

        m_toppingsText.gameObject.SetActive(false);
        m_pierreText.gameObject.SetActive(false);

        m_toppingsLines  = new string[] { "PIERRE THE STRIPED.", "GIVE ME MY CANDLES BACK.", "THEN I'LL TAKE THEM\nBY FORCE.", "YOU ASKED FOR IT!" };
        m_pierreLines    = new string[] { "WE MEET AT LAST, MISS TOPPINGS.", "AS MUCH IMPRESSED AS I AM WITH YOUR\nDETERMINATION I'M AFRAID I CANNOT\nDO THAT.", "DO YOUR WORST." };

        m_toppingsLines2 = new string[] { "IT'S OVER, PIERRE.", "GIVE ME MY CANDLES. OR ELSE.", "WHAT ARE YOU TALKING ABOUT?\nYOU PIES ARE PRETTY SICK TOO.",
                                          "HEY, IT'S NO BIG DEAL. YOU MAY KEEP ONE OF MY CANDLES IF YOU PROMISE TO NEVER STEAL AGAIN." };

        m_pierreLines2   = new string[] { "YOU... DEFEATED ME.", "I'M... I'M SORRY. I JUST WANTED TO BE COOL WEARING THESE CANDLES LIKE YOU CAKES.",
                                          "MAYBE, BUT... WEARING CANDLES LOOKS AWESOME. I SHOULDN'T HAVE STOLEN THEM FROM YOU THOUGH. FORGIVE ME.",
                                           "THANK YOU! YOU ARE THE BEST, MISS TOPPINGS." };

        StartCoroutine("toppingsMovesRight");
	}

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene("0_Menu2");
        }
    }


    public void pierreDead() {
        m_pierre.StopCoroutine("combat");
        m_toppings.m_facingRight = true;
        m_toppings.m_move = 0;
        m_pierre.m_move = 0;
        StartCoroutine("panelTransition");
    }

    IEnumerator panelTransition() {
        StartCoroutine("fadeInPanel");
        yield return new WaitForSeconds(2);
        StartCoroutine("fadeOutPanel");
    }

    IEnumerator fadeInPanel() {
        m_toppings.m_input = false;
        m_pierre.StopCoroutine("combat");
        // Restore position
        m_toppings.m_state = Miss_Toppings.State.Idle;
        m_pierre.m_state = Pierre.State.Idle;

        SpriteRenderer panelRenderer = GameObject.FindGameObjectWithTag("Panel").GetComponent<SpriteRenderer>();
        panelRenderer.color = new Color(panelRenderer.color.r, panelRenderer.color.b, panelRenderer.color.g, 0);
        while (panelRenderer.color.a < 1) {
            panelRenderer.color = new Color(panelRenderer.color.r, panelRenderer.color.g, panelRenderer.color.b, panelRenderer.color.a + Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator fadeOutPanel() {
        m_toppings.transform.localScale = new Vector3(Mathf.Abs(m_toppings.transform.localScale.x), m_toppings.transform.localScale.y, m_toppings.transform.localScale.z);
        m_toppings.setPosition(m_toppingsPos);
        m_pierre.setPosition(m_pierrePos);

        SpriteRenderer panelRenderer = GameObject.FindGameObjectWithTag("Panel").GetComponent<SpriteRenderer>();
        panelRenderer.color = new Color(panelRenderer.color.r, panelRenderer.color.b, panelRenderer.color.g, 1);
        while (panelRenderer.color.a > 0) {
            panelRenderer.color = new Color(panelRenderer.color.r, panelRenderer.color.g, panelRenderer.color.b, panelRenderer.color.a - Time.deltaTime);
            yield return null;
        }
        StartCoroutine("secondDialogue");
    }


    IEnumerator toppingsMovesRight()
    {
        m_toppings.m_move = 1f;
        yield return new WaitForSeconds(1.5f);
        m_toppings.m_move = 0;

        // Save "respawn" positions
        m_toppingsPos = m_toppings.getPosition();
        m_pierrePos   = m_pierre.getPosition();

        StartCoroutine("firstDialogue");
    }

    IEnumerator firstDialogue()
    {
        bool topTalked = false;
        bool pieTalked = false;
        for (int j = 0; j < (m_pierreLines.Length + m_toppingsLines.Length); )
        {
            if (m_dialogueTurn == DialogueTurn.Top && j < m_pierreLines.Length) { // Only 3 lines por Toppings, 4 for Pierre 
                m_toppingsText.text = "";
                m_toppings.setTalking(true);
                for (int i = 0; i < m_toppingsLines[j].Length; ++i)
                {
                    m_toppingsText.gameObject.SetActive(true);
                    m_toppingsText.text += m_toppingsLines[j][i];
                    yield return new WaitForSeconds(0.02f);
                }
                topTalked = true;
                m_toppings.setTalking(false);
                m_dialogueTurn = DialogueTurn.Pie;
            } else if (m_dialogueTurn == DialogueTurn.Pie) {
                m_pierreText.text = "";
                m_pierre.setTalking(true);
                for (int i = 0; i < m_pierreLines[j].Length; ++i)
                {
                    m_pierreText.gameObject.SetActive(true);
                    m_pierreText.text += m_pierreLines[j][i];
                    yield return new WaitForSeconds(0.02f);
                }
                pieTalked = true;
                m_pierre.setTalking(false);
                m_dialogueTurn = DialogueTurn.Top;
            }
            yield return new WaitForSeconds(0.8f);

            if (topTalked && pieTalked) {
                ++j;
                topTalked = false;
                pieTalked = false;
            }

            if (j == m_toppingsLines.Length) { continue; }

            m_toppingsText.gameObject.SetActive(false);
            m_pierreText.gameObject.SetActive(false);
            if (j == m_pierreLines.Length) { break; }

        }  
               
        Vector3 newDelimPos = GameObject.FindGameObjectWithTag("Delim_Izda").transform.position;
        newDelimPos.x = -10.5f;
        GameObject.FindGameObjectWithTag("Delim_Izda").transform.position = newDelimPos;

        m_toppings.setInput(true);
        m_pierre.StartCoroutine("combat");
        yield return null;                      
    }

    IEnumerator secondDialogue() {
        bool topTalked = false;
        bool pieTalked = false;
        for (int j = 0; j < (m_pierreLines2.Length + m_toppingsLines2.Length);) {
            if (m_dialogueTurn == DialogueTurn.Top) // Only 3 lines por Toppings, 4 for Pierre
            {
                m_toppingsText.text = "";
                m_toppings.setTalking(true);
                for (int i = 0; i < m_toppingsLines2[j].Length; ++i) {
                    m_toppingsText.gameObject.SetActive(true);
                    m_toppingsText.text += m_toppingsLines2[j][i];
                    yield return new WaitForSeconds(0.02f);
                }
                topTalked = true;
                m_toppings.setTalking(false);
                m_dialogueTurn = DialogueTurn.Pie;
            } else if (m_dialogueTurn == DialogueTurn.Pie) {
                m_pierreText.text = "";
                m_pierre.setTalking(true);
                for (int i = 0; i < m_pierreLines2[j].Length; ++i) {
                    m_pierreText.gameObject.SetActive(true);
                    m_pierreText.text += m_pierreLines2[j][i];
                    yield return new WaitForSeconds(0.02f);
                }
                pieTalked = true;
                m_pierre.setTalking(false);
                m_dialogueTurn = DialogueTurn.Top;
            }
            yield return new WaitForSeconds(0.8f);

            if (topTalked && pieTalked) {
                ++j;
                topTalked = false;
                pieTalked = false;
            }

            m_toppingsText.gameObject.SetActive(false);
            m_pierreText.gameObject.SetActive(false);
            if (j == m_pierreLines2.Length) { break; }

        }
        m_toppings.m_happyFace = true;
        m_pierre.m_happyFace   = true;
        yield return new WaitForSeconds(3);

        StartCoroutine("fadeInPanel");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("0_Menu2");
        yield return null;
    }


}
