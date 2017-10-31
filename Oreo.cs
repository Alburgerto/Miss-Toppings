using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oreo : MonoBehaviour {

    public Sprite[] m_sprites;

    private SpriteRenderer m_spriteRenderer;

	// Use this for initialization
	void Start () {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	public void stepUpon()
    {
        m_spriteRenderer.sprite = m_sprites[1];
    }

    public void notStepUpon()
    {
        m_spriteRenderer.sprite = m_sprites[0];
    }
}
