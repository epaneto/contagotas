﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenURL : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void openLandPage()
    {
        Application.OpenURL("http://www.contagotascaixa.com.br/");
    }
}
