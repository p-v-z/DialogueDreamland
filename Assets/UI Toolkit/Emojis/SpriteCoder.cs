using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.U2D;
using TextAsset = UnityEngine.TextAsset;

namespace DD.Dev
{
	/// <summary>
	/// This class will take the JSON file containing the sprite data and convert it into a SpriteAsset
	/// </summary>
	public class SpriteCoder : MonoBehaviour
	{
		// JSON file containing the sprite data
		[SerializeField] TextAsset spriteData;
		[SerializeField] SpriteAsset targetSpriteAsset;
		[SerializeField] Texture2D targetTexture;
		
		[SerializeField] SpriteAtlas spriteAtlas;
		
		private void Awake()
		{
			BuildSpriteAsset();
		}
		
		public void ClearSpriteAsset()
		{
			targetSpriteAsset.spriteCharacterTable.Clear();
			targetSpriteAsset.spriteGlyphTable.Clear();
		}

		public void BuildSpriteAsset()
		{
			var emojiList = JsonConvert.DeserializeObject<List<Emoji>>(spriteData.text);
			Debug.Log($"Got {emojiList.Count} emojis");

			Debug.Log("==");
			var testEmoji = emojiList[0];
			Debug.Log($"First emoji: {testEmoji.name} {testEmoji.unified} {testEmoji.image}");


			// var spriteOne = targetSpriteAsset.spriteCharacterTable[0];
			// var dataOne = emojiList[0];
			// // string emojiString = "😊";
			// string emojiString = dataOne.unified;
			// if (emojiString.Contains("-"))
			// {
			// 	Debug.Log("Skip this one");
			// 	return;
			// }
			// int unicodeValue = int.Parse(emojiString, NumberStyles.HexNumber);			// string unicodeString = Convert.ToInt32(unifiedString, 16);
			// char emoji = (char)unicodeValue;
			// Debug.Log(emoji);
			// spriteOne.unicode = emoji;
			// Debug.Log("First sprite: " + spriteOne.unicode);
			// // TODO: Continue
			
			var idx = 0;
			foreach (var emoji in emojiList)
			{
				var matchingSprite = targetSpriteAsset.spriteCharacterTable.FirstOrDefault(x => x.name == $"64_{idx}");
				idx++;
				if (matchingSprite == null)
				{
					Debug.Log($"No matching sprite for {emoji.name}");
					continue;
				}
				
				var emojiString = emoji.unified;
				if (emojiString.Contains("-"))
				{
					Debug.Log("Skip this one (contains -)");
					continue;
				}
				
				var unicodeValue = int.Parse(emojiString, NumberStyles.HexNumber);			// string unicodeString = Convert.ToInt32(unifiedString, 16);
				var emojiChar = (char)unicodeValue;
				Debug.Log(emojiChar);
				matchingSprite.unicode = emojiChar;
				Debug.Log($"Updated sprite {matchingSprite.name} to {matchingSprite.unicode}");
			}
			
		}
		
		public void SetSpriteNames()
		{
			// spriteAtlas.
			
		}
	}
	
	// An editor script that has a button which calls BuildSpriteAsset()
	[CustomEditor(typeof(SpriteCoder))]
	public class SpriteCoderEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			var spriteCoder = (SpriteCoder) target;
			if (GUILayout.Button("Clear Sprite Asset"))
			{
				Debug.Log("Clearing Sprite Asset...");
				spriteCoder.ClearSpriteAsset();
			}
			if (GUILayout.Button("Set sprite names"))
			{
				spriteCoder.SetSpriteNames();
			}
			if (GUILayout.Button("Build Sprite Asset"))
			{
				Debug.Log("Building Sprite Asset...");
				spriteCoder.BuildSpriteAsset();
			}
		}
	}
}