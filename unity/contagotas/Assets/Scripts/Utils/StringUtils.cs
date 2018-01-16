using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Linq;
using System.Globalization;

public static class StringUtils {

	public static string RemoverAcentuacao(this string text)
	{
		return new string(text
			.Normalize(NormalizationForm.FormD)
			.Where(ch => char.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
			.ToArray());
	}

	public static string DecodeBytesForUTF8(byte[] stream)
	{
		byte[] bytes=new byte[]{116, 101, 120, 116, 105, 110, 101, 115, 115};

		string customDecoded=""; 
		foreach(var b in stream)
			customDecoded+=(char)b; 
		
		return customDecoded;
	}

}
