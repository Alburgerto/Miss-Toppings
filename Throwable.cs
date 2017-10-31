using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Throwable : MonoBehaviour {
    private Vector2 m_playerPos;

	// Use this for initialization
	void Start () {
        m_playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
    }

    private void FixedUpdate() {
        transform.position = Vector2.MoveTowards(transform.position, m_playerPos, 0.1f);
        if (transform.position.x == m_playerPos.x && transform.position.y == m_playerPos.y) {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.name == "Flying_Candy" || collision.gameObject.name == "Pierre") {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>(), true);
        }
        if (collision.gameObject.CompareTag("Player")) {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera_Follow>().StartCoroutine("shake");
            collision.gameObject.GetComponent<Miss_Toppings>().takeDamage();
        }
        if (collision.gameObject.name != "Flying_Candy" && !collision.gameObject.CompareTag("Pierre")) {
            Destroy(gameObject);
        }
    }
}
