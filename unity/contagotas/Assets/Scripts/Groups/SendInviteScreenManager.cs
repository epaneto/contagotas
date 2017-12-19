using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using Facebook.MiniJSON;
using System.Linq;

public class SendInviteScreenManager : BaseAssetsGroupManager {

	[SerializeField]
	FacebookInviteManager facebookAPI;
	[SerializeField]
	Transform SendInviteGroupParent;
	[SerializeField]
	GameObject SendInvitePrefab;

	public void LoadFriendsToInvite()
	{
		facebookAPI.GetFriends (ReceivedFriendList);
	}

	public void ReceivedFriendList(IResult result)
	{
		var dict = Json.Deserialize(result.RawResult)
			as Dictionary<string,object>;

		var friendList = new List<object>();
		friendList = (List<object>)(dict["data"]);

		foreach (var item in screenManager.temporaryObjsList) {
			Destroy (item);
		}

		screenManager.temporaryObjsList.Clear ();

		foreach (var friend in friendList) {

			var info = ((IEnumerable)friend).Cast<object> ()
				.Select (x => x.ToString ())
				.ToArray ();

			string name = info [1].Replace ('[', ' ').Replace (']', ' ').Trim ().Split (',') [1];
			string facebook_id = info [3].Replace ('[', ' ').Replace (']', ' ').Trim ().Split (',') [1];

			var pictureRawData = (friend as Dictionary<string,object>) ["picture"];
			var data = (pictureRawData as Dictionary<string,object>) ["data"];
			string facebook_image_url = (data as Dictionary<string,object>) ["url"].ToString();

			GameObject item = Instantiate (SendInvitePrefab, SendInviteGroupParent);
			screenManager.temporaryObjsList.Add (item);
			FacebookInvite inviteObj = item.GetComponent<FacebookInvite> (); 
			inviteObj.SetupInviteInfo (name, facebook_id, facebook_image_url);
		}

		screenManager.ShowGroup (ScreenType.SEND_INVITES);
	}

	public void Retry()
	{
		SceneController.sceneController.FadeAndLoadScene("Group", true);
		//UnityEngine.SceneManagement.SceneManager.LoadScene ("Group");
	}

	public void Share()
	{
		NativeSharePhoto.Share("Olá! Jogue esse jogo que é demais! clique aqui: http://contagotas.online/services/redirect/", null, "contagotas.online/services/redirect/", "", "text/html", true, "");
	}
}
