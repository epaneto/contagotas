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

	[SerializeField]
	GameObject existentGroupErrorScreen;

	[SerializeField]
	Text existentGroupErrorText;

	public void CreateAndJoinGroup()
	{
		if (string.IsNullOrEmpty (createInput.text) || string.IsNullOrEmpty (createPasswordInput.text))
			return;

        GameSound.gameSound.PlaySFX("button");
		StartCoroutine (CheckIfGroupExists(createInput.text,createPasswordInput.text));
	}

	IEnumerator CheckIfGroupExists(string groupName, string password)
	{
		screenManager.ShowLoadingScreen ();

		WWW result;
		yield return result = WWWUtils.DoWebRequest("exists/" + groupName + "/" );
		//yield return result = WWWUtils.DoWebRequestWithSpecificURL("http://localhost/contagotas/group/exists/" + groupName + "/" );
		Debug.Log ("url result = " + result.text);

		if (result.text.ToUpper().Contains ("ERROR")) 
		{
			screenManager.ShowErrorScreen ("error creating group:" + result.text);
			yield break;
		}

		if (result.text == "true") {
			screenManager.ShowCreateScreen ();
			ShowGroupExistentErrorScreen ();
		}
		else
			StartCoroutine (CreateGroup(groupName,password));
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

	public void ShowGroupExistentErrorScreen()
	{
		existentGroupErrorScreen.SetActive (true);
		existentGroupErrorText.text = "Infelizmente já existe um grupo chamado " + createInput.text + ". Tente outro nome.";
	}

	public void HideGroupExistentErrorScreen()
	{
		existentGroupErrorScreen.SetActive (false);
		existentGroupErrorText.text = "";
	}

}
