using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
	public void GoToTutorial()
	{
		SceneManager.LoadScene("Tutorial");
	}
	public void Exit()
	{
		Application.Quit();
	}
}
