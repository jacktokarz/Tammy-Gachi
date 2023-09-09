using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class ChangeScenes : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Debug.Log("Escape was pressed");
            Exit();
        }
    }

    public void Proceed()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "Title")
        {
            SceneManager.LoadScene("Tutorial");
        }
        else if (scene.name == "Tutorial")
        {
            SceneManager.LoadScene("GameStart");
        }
    }
    public void Exit()
    {
        Application.Quit();
    }
}
