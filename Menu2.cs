using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Menu2 : MonoBehaviour {

    private void Awake() {
        Cursor.visible = false;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            SceneManager.LoadScene("1_Level");
        } else if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
    }
}
