#if UNITY_EDITOR
#nullable enable
using System.Collections.Generic;

namespace DD.Dev
{
	public class BaseEmoji
	{
		public string unified { get; set; } = "";
		public string non_qualified { get; set; } = "";
		public string image { get; set; } = "";
		public int sheet_x { get; set; } = 0;
		public int sheet_y { get; set; } = 0;
		public string added_in { get; set; } = "";
		public bool has_img_apple { get; set; } = false;
		public bool has_img_google { get; set; } = false;
		public bool has_img_twitter { get; set; } = false;
		public bool has_img_facebook { get; set; } = false;
	}
	
	public class Emoji : BaseEmoji
	{
		public string name { get; set; } = "";
		public string docomo { get; set; } = "";
		public string au { get; set; } = "";
		public string softbank { get; set; } = "";
		public string google { get; set; } = "";
		public string short_name { get; set; } = "";
		public string[] short_names { get; set; } = new string[0];
		public string text { get; set; } = "";
		public string[] texts { get; set; } = new string[0];
		public string category { get; set; } = "";
		public string subcategory { get; set; } = "";
		public int sort_order { get; set; } = 0;
		
		public KeyValuePair<string, BaseEmoji>? skin_variations { get; set; }
		public string? obsoletes { get; set; }
		public string? obsoleted_by { get; set; }
	}
}
#endif