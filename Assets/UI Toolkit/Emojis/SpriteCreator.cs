#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore;
using UnityEngine.TextCore.Text;
using UnityEngine.U2D;

public class SpriteCreator : MonoBehaviour
{
    private static void CreateSpriteAsset()
    {
        var targets = Selection.objects;

        if (targets == null)
        {
            Debug.LogWarning("A Sprite Texture must first be selected in order to create a Sprite Asset.");
            return;
        }

        // Make sure TMP Essential Resources have been imported in the user project.
        if (TMP_Settings.instance == null)
        {
            Debug.Log("Unable to create sprite asset. Please import the TMP Essential Resources.");

            // Show Window to Import TMP Essential Resources
            return;
        }

        foreach (var target in targets)
        {
            // Make sure the selection is a font file
            if (target == null || target.GetType() != typeof(Texture2D))
            {
                Debug.LogWarning("Selected Object [" + target.name + "] is not a Sprite Texture. A Sprite Texture must be selected in order to create a Sprite Asset.", target);
                continue;
            }

            CreateSpriteAssetFromSelectedObject(target);
        }
    }

    public static void CreateSpriteAssetFromSelectedObject(Object target)
    {
        Debug.Log("Creating Sprite Asset from selected object.");

        // Get the path to the selected asset.
        var filePathWithName = AssetDatabase.GetAssetPath(target);
        var fileNameWithExtension = Path.GetFileName(filePathWithName);
        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePathWithName);
        var filePath = filePathWithName.Replace(fileNameWithExtension, "");
        var uniquePath = AssetDatabase.GenerateUniqueAssetPath(filePath + fileNameWithoutExtension + "Custom.asset");

        // Create new Sprite Asset
        var spriteAsset = ScriptableObject.CreateInstance<SpriteAsset>();
        AssetDatabase.CreateAsset(spriteAsset, uniquePath);
        // spriteAsset.version = "1.1.0";

        // Compute the hash code for the sprite asset.
        spriteAsset.hashCode = TMP_TextUtilities.GetSimpleHashCode(spriteAsset.name);
        Debug.Log($"Sprite Asset Hash Code: {spriteAsset.hashCode}");

        var spriteGlyphTable = new List<SpriteGlyph>();
        var spriteCharacterTable = new List<SpriteCharacter>();

        if (target is Texture2D sourceTex)
        {
            // TODO:
            // Assign new Sprite Sheet texture to the Sprite Asset.
            // spriteAsset.spriteSheet = sourceTex;

            PopulateSpriteTables(sourceTex, ref spriteCharacterTable, ref spriteGlyphTable);

            // TODO:
            // spriteAsset.spriteCharacterTable = spriteCharacterTable;
            // spriteAsset.spriteGlyphTable = spriteGlyphTable;

            // Add new default material for sprite asset.
            AddDefaultMaterial(spriteAsset);
        }

        // Update Lookup tables.
        spriteAsset.UpdateLookupTables();

        // Get the Sprites contained in the Sprite Sheet
        EditorUtility.SetDirty(spriteAsset);

        // spriteAsset.sprites = sprites;
        // Set source texture back to Not Readable.
        //texImporter.isReadable = false;

        AssetDatabase.SaveAssets();
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(spriteAsset)); // Re-import font asset to get the new updated version.
    }

    private static void PopulateSpriteTables(Texture source, ref List<SpriteCharacter> spriteCharacterTable, ref List<SpriteGlyph> spriteGlyphTable)
    {
        Debug.Log("Creating new Sprite Asset.");

        var filePath = AssetDatabase.GetAssetPath(source);

        // Get all the Sprites sorted by Index
        var sprites = AssetDatabase.LoadAllAssetsAtPath(filePath).Select(x => x as Sprite).Where(x => x != null).OrderByDescending(x => x.rect.y).ThenBy(x => x.rect.x).ToArray();

        for (var i = 0; i < sprites.Length; i++)
        {
            Debug.Log("Adding item to Sprite Table");
            var sprite = sprites[i];

            var spriteGlyph = new SpriteGlyph
            {
                index = (uint)i,
                metrics = new GlyphMetrics(sprite.rect.width, sprite.rect.height, -sprite.pivot.x, sprite.rect.height - sprite.pivot.y, sprite.rect.width),
                glyphRect = new GlyphRect(sprite.rect),
                scale = 1.0f,
                sprite = sprite
            };

            spriteGlyphTable.Add(spriteGlyph);

            var spriteCharacter = new SpriteCharacter(0xFFFE, spriteGlyph);

            // Special handling for .notdef sprite name.
            var fileNameToLowerInvariant = sprite.name.ToLowerInvariant();
            if (fileNameToLowerInvariant == ".notdef" || fileNameToLowerInvariant == "notdef")
            {
                Debug.Log("Found def, setting unicode to 0");
                spriteCharacter.unicode = 0;
                spriteCharacter.name = fileNameToLowerInvariant;
            }
            else
            {
                Debug.Log("No def");
                if (!string.IsNullOrEmpty(sprite.name) && sprite.name.Length > 2 && sprite.name[0] == '0' && (sprite.name[1] == 'x' || sprite.name[1] == 'X'))
                {
                    spriteCharacter.unicode = (uint)TMP_TextUtilities.StringHexToInt(sprite.name.Remove(0, 2));
                }
                spriteCharacter.name = sprite.name;
            }

            spriteCharacter.scale = 1.0f;

            spriteCharacterTable.Add(spriteCharacter);
        }
    }

    private static void PopulateSpriteTables(SpriteAtlas spriteAtlas, ref List<SpriteCharacter> spriteCharacterTable, ref List<SpriteGlyph> spriteGlyphTable)
    {
        // Get number of sprites contained in the sprite atlas.
        var spriteCount = spriteAtlas.spriteCount;
        var sprites = new Sprite[spriteCount];

        // Get all the sprites
        spriteAtlas.GetSprites(sprites);

        for (var i = 0; i < sprites.Length; i++)
        {
            var sprite = sprites[i];

            var spriteGlyph = new SpriteGlyph
            {
                index = (uint)i,
                metrics = new GlyphMetrics(sprite.textureRect.width, sprite.textureRect.height, -sprite.pivot.x, sprite.textureRect.height - sprite.pivot.y, sprite.textureRect.width),
                glyphRect = new GlyphRect(sprite.textureRect),
                scale = 1.0f,
                sprite = sprite
            };

            spriteGlyphTable.Add(spriteGlyph);

            var spriteCharacter = new SpriteCharacter(0xFFFE, spriteGlyph)
            {
                name = sprite.name,
                scale = 1.0f
            };

            spriteCharacterTable.Add(spriteCharacter);
        }
    }

    /// <summary>
    /// Create and add new default material to sprite asset.
    /// </summary>
    /// <param name="spriteAsset"></param>
    private static void AddDefaultMaterial(SpriteAsset spriteAsset)
    {
        var shader = Shader.Find("TextMeshPro/Sprite");
        var material = new Material(shader);
        material.SetTexture(ShaderUtilities.ID_MainTex, spriteAsset.spriteSheet);

        spriteAsset.material = material;
        material.name = spriteAsset.name + " Material";
        AssetDatabase.AddObjectToAsset(material, spriteAsset);
    }
}
#endif