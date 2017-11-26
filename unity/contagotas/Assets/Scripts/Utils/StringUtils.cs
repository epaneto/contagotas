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
}
