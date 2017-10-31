using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Menu1 : MonoBehaviour {

    private SpriteRenderer m_spriteRenderer;

    private void Awake() {
        Cursor.visible = false;
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine("fadeInScreen");
    }

    IEnumerator fadeInScreen() {
        yield return new WaitForSeconds(3);
        m_spriteRenderer.color = new Color(m_spriteRenderer.color.r, m_spriteRenderer.color.b, m_spriteRenderer.color.g, 0);
        while (m_spriteRenderer.color.a < 1) {
            m_spriteRenderer.color = new Color(m_spriteRenderer.color.r, m_spriteRenderer.color.b, m_spriteRenderer.color.g, m_spriteRenderer.color.a + Time.deltaTime);
            yield return null;
        }
        SceneManager.LoadScene("0_Menu2");
        yield return null;
    }
}
