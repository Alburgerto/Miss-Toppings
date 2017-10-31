using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameController : MonoBehaviour {

    public GameObject[] m_lostLifeIcon;
    public GameObject[] m_livesIcons;
    public bool[] m_livesLost;
    private Miss_Toppings m_toppings;

    private void Start()
    {
        Cursor.visible = false;
        m_toppings = GameObject.FindGameObjectWithTag("Player").GetComponent<Miss_Toppings>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene("0_Menu2");
        }
    }


    public void loseLife()
    {
        int i = 4;
        while (i >= 0)
        {
            if (m_livesLost[i] == true)
            {
                m_livesLost[i] = false;
                // Switch icons
                Vector3 temp = m_lostLifeIcon[i].transform.position;
                m_lostLifeIcon[i].transform.position = m_livesIcons[i].transform.position;
                m_livesIcons[i].transform.position = temp;
                if (i == 0) // DEATH
                {
                    
                    for (int j = 0; j < m_livesIcons.Length; ++j)
                    {

                    }
                }
                break;
            }
            --i;
        }
    }
    
}
