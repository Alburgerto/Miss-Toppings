using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour {
    
    public AudioClip[] m_audioClips;
    private AudioSource m_audioSource;

    private void Start() {
        m_audioSource = gameObject.GetComponent<AudioSource>();
        m_audioSource.loop = false;
    }

    public void playClip(int l_clipId) {
        m_audioSource.clip = m_audioClips[l_clipId];
        m_audioSource.Play();
    }
}
