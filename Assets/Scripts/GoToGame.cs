using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToGame : MonoBehaviour
{
	public void Change()
	{
		SceneManager.LoadScene("GameStart");
	}
	public void Exit()
	{
		Application.Quit();
	}
}