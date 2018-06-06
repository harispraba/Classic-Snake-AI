using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour {

    public static UIController instance = null;

    public GameObject gameOverUI;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);
    }

    public void ShowGameOverUI() {
        gameOverUI.SetActive(true);
    }

    public void ButtonRestart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
