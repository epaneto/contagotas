using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class CreateNewGroupScreenManager : BaseAssetsGroupManager {

	[SerializeField]
	InputField createInput;

	[SerializeField]
	InputField createPasswordInput;

	[SerializeField]
	JoinScreenManager joinManager;

	public void CreateAndJoinGroup()
	{
		if (string.IsNullOrEmpty (createInput.text) || string.IsNullOrEmpty (createPasswordInput.text))
			return;

        GameSound.gameSound.PlaySFX("button");
		StartCoroutine (CreateGroup(createInput.text,createPasswordInput.text));
	}

	IEnumerator CreateGroup(string groupName, string password)
	{
		StringBuilder sb = new StringBuilder ();
		sb.Append ("data={");
		sb.Append ("\"name\"");
		sb.Append (":\"");
		sb.Append (groupName);
		sb.Append ("\"");
		sb.Append (",");
		sb.Append ("\"password\"");
		sb.Append (":\"");
		sb.Append (password);
		sb.Append ("\"");
		sb.Append ("}");

		screenManager.ShowLoadingScreen ();

		WWW result;
		yield return result = WWWUtils.DoWebRequest("create/",sb.ToString());
		Debug.Log ("url result = " + result.text);

		if (result.text.ToUpper().Contains ("ERROR")) 
		{
			screenManager.ShowErrorScreen ("error creating group:" + result.text);
			yield break;
		}

		int groupID;
		if(!int.TryParse(result.text, out groupID))
		{
			screenManager.ShowErrorScreen ("error on group id:" + result.text);
			yield break;
		}

		StartCoroutine (joinManager.Join (groupID));
	}

	public void GoBack()
	{
        GameSound.gameSound.PlaySFX("button");
		screenManager.ShowNewGroupScene();
	}

}
