using System;
using System.Collections.Generic;
using System.Text;

/* Copyright (c) 2021 Rugbug Redfern */

namespace RedlabsUpdateUtility
{
	/// <summary>
	/// Generates a URL which will bypass some servers cached value by appending a random value to the end of the URL
	/// </summary>
	static class AntiCache
	{
		// Unfortunately github's raw files are cached for 5 minutes and there is no real way to get around it. This should help for most other services though.

		/// <summary>
		/// Generate a URL which will bypass the some servers cached value by appending a random value to the end of the URL
		/// </summary>
		public static string ToAntiCacheURL(this string value)
		{
			return $"{value}?rugbug={DateTime.Now.Millisecond}"; // "rugbug" because that's probably something that doesn't show up in a url much
		}
	}
}
