using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour {

    public Sprite[] m_sprites;
    private SpriteRenderer m_spriteRenderer;
    private GameObject m_player;

	// Use this for initialization
	void Start () {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
        if (m_player)   // Alive
        {
            if (m_player.transform.position.x < transform.position.x - 3)
            {
                m_spriteRenderer.sprite = m_sprites[0];
            }
            else if (m_player.transform.position.x < transform.position.x || transform.position.x + 3 > m_player.transform.position.x)
            {
                m_spriteRenderer.sprite = m_sprites[1];
            }
            else
            {
                m_spriteRenderer.sprite = m_sprites[2];
            }
        }
	}
}
