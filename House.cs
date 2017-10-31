using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class House : MonoBehaviour {

    public Text m_houseText;
    private bool m_canEnter = false;

    private void Start()
    {
        //      m_houseText = GameObject.FindGameObjectWithTag("HouseText");
        m_houseText.enabled = false;
    }

    private void Update()
    {
        if (m_canEnter && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("2_Level");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("GroundCheck"))
        {
            m_houseText.enabled = true;
            m_houseText.SendMessage("fadeIn");
            m_canEnter = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("GroundCheck"))
        {
            m_houseText.SendMessage("fadeOut");
            m_canEnter = false;
        }
    }
}
