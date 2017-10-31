using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Camera_Follow : MonoBehaviour {

    public GameObject m_player;
    public GameObject m_delimIzda;
    public GameObject m_delimDcha;
    public float m_speed = 2;
    private float interpolation;
    private Vector3 m_respawn;

    private void Awake() {
        if (SceneManager.GetActiveScene().name == "2_Level") {
            transform.position = new Vector3(0, 0, -10);
        } else {
            transform.position = new Vector3(-4.36f, 0, -10);
        }
        m_respawn = transform.position;
    }

    public void deadToppings() {
        transform.position = m_respawn;
    }

    // Update is called once per frame
    void Update() {
        if (m_player && SceneManager.GetActiveScene().name != "2_Level") {
            if (m_delimIzda.transform.position.x < m_player.transform.position.x - 8 && m_delimDcha.transform.position.x > m_player.transform.position.x + 8) {
                interpolation = m_speed * Time.deltaTime;
                Vector3 pos = transform.position;
                pos.x = Mathf.Lerp(transform.position.x, m_player.transform.position.x, interpolation);
                transform.position = pos;
            }
        } 
    }

    IEnumerator shake() {
        Vector3 originalPos = transform.position;
        float shakeDuration = 0.5f;
        float shakeAmount = 0.1f;
        float decreaseFactor = 1;
        while (true) {
            if (shakeDuration > 0) {
                transform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
                shakeDuration -= Time.deltaTime * decreaseFactor;
            } else {
                shakeDuration = 0f;
                transform.localPosition = originalPos;
                break;
            }
            yield return new WaitForEndOfFrame();
        }
    }

}
