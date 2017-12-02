using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAssetsGroupManager : MonoBehaviour {

	[Header("Group Screen References")]
	[SerializeField]
	GameObject GroupGameObjects;

	protected GroupManager screenManager;

	public void Initialize(GroupManager screenManager)
	{
		this.screenManager = screenManager;
	}

	public void SetScreen(bool value)
	{
		GroupGameObjects.SetActive (value);
	}

}
