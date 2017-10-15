using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class UserData : MonoBehaviour {

	public static UserData userData;

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
		BinaryFormatter bf = BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/playerInfo.dat");

		PlayerData PD = PlayerData ();
		PD.playerName = "Epa";
		PD.playerCity = "SAo Paulo";
		PD.playerState = "SP";

		bf.Serialize (file,PD);
		file.Close ();
	}

	public void Load()
	{
		if(File.Exists(Application.persistentDataPath + "playerInfo.dat"))
		{
			BinaryFormatter bf = BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);

			PlayerData PD = (PlayerData)bf.Deserialize (file);
			file.Close ();
		}
	}

}


[Serializable]
class PlayerData
{
	public string playerName;
	public string playerCity;
	public string playerState;
}