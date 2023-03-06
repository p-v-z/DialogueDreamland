using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.TextCore.Text;
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
		[SerializeField] SpriteAsset spriteAsset;
		[SerializeField] Texture2D targetTexture;
		
		private void Awake()
		{

			var emojiList = JsonConvert.DeserializeObject<List<Emoji>>(spriteData.text);
			Debug.Log($"Got {emojiList.Count} emojis");
			spriteAsset.spriteCharacterTable.Clear();
			spriteAsset.spriteCharacterTable.Capacity = emojiList.Count;
			spriteAsset.spriteGlyphTable.Clear();
			spriteAsset.spriteGlyphTable.Capacity = emojiList.Count;

			Debug.Log("==");
			var testEmoji = emojiList[0];
			Debug.Log($"First emoji: {testEmoji.name} {testEmoji.unified} {testEmoji.image}");

			SpriteCreator.CreateSpriteAssetFromSelectedObject(targetTexture);
			// TODO: Continue
		}
	}
}