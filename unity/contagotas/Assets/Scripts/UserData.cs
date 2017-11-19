using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class UserData : MonoBehaviour {

	public static UserData userData;
	public PlayerData playerData;

	// Use this for initialization
	void Awake () 
	{
		if (userData == null) {
			DontDestroyOnLoad (gameObject);
			userData = this;
		} else if (userData != this) {
			Destroy (gameObject);
		}
	}

	public void Save()
	{
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/playerInfo.dat");

		PlayerData PD = new PlayerData ();
		PD = UserData.userData.playerData;

		Debug.Log ("saving body " + PD.playerBody);

		bf.Serialize (file,PD);
		file.Close ();
	}

	public void Load()
	{
		if(File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
		{
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);

			PlayerData PD = (PlayerData)bf.Deserialize (file);
			file.Close ();

			UserData.userData.playerData = PD;
			Debug.Log ("saved body is " + PD.playerBody);

		}
	}

}


[Serializable]
public class PlayerData
{
	public string playerName;
	public string playerCity;
	public string playerState;

	public string playerBody;
	public string playerEye;
	public string playerHair;
	public string playerMouth;
	public string playerAcc;
	public string playerShirt;
	public string playerPants;
	public string playerShoe;

	public DateTime lastAccess;
	public int activeMission;
}