using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorScreenManager : BaseAssetsGroupManager {

	[SerializeField]
	Text error_text;

	public void SetErrorMessage(string error)
	{
		error_text.text = error;
	}

	public void Retry()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene ("Group");
	}
}
