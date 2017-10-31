using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TextFade : MonoBehaviour {

    public Text m_text;

    private void Start()
    {
        m_text.enabled = false;
    }

    public void fadeIn()
    {
        StartCoroutine("fadeInCor");
    }

    public void fadeOut()
    {
        StartCoroutine("fadeOutCor");
    }

    IEnumerator fadeInCor()
    {
        m_text.color = new Color(m_text.color.r, m_text.color.b, m_text.color.g, 0);
        while(m_text.color.a < 1)
        {
            m_text.color = new Color(m_text.color.r, m_text.color.b, m_text.color.g, m_text.color.a + Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator fadeOutCor()
    {
        m_text.color = new Color(m_text.color.r, m_text.color.b, m_text.color.g, 1);
        while (m_text.color.a > 0)
        {
            m_text.color = new Color(m_text.color.r, m_text.color.b, m_text.color.g, m_text.color.a - Time.deltaTime);
            yield return null;
        }
        m_text.enabled = false;
    }

}
