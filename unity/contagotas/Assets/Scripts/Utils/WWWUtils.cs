using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public static class WWWUtils {

	static string urlBase = "http://www.contagotas.online/services/group/";
	//static string urlBase = "http://localhost/contagotas/group/";

	public static WWW DoWebRequest(string url, string data = "data=")
	{
		Hashtable headers = new Hashtable ();
		headers.Add ("User-Agent", "app-contagotas");

		string finalUrl = urlBase + url;

		return new WWW(finalUrl, Encoding.UTF8.GetBytes(data), headers);
	}

	public static WWW DoWebRequestWithSpecificURL(string url, string data = "data=")
	{
		Hashtable headers = new Hashtable ();
		headers.Add ("User-Agent", "app-contagotas");
		return new WWW(url, Encoding.UTF8.GetBytes(data), headers);
	}
}
