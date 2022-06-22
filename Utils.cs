﻿using System;
using System.IO;
using System.Text;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using flanne.UI;

namespace SpriteReplacer
{
    class Utils
    {
        // Returns true on successful patch, false otherwise
        static public bool ReplaceSpriteTexture(Sprite targetSprite)
        {
            if (targetSprite != null)
            {
                //Console.WriteLine("[SpriteReplacer] Sprite.name:" + targetSprite.name);
                Texture2D spriteTexture = targetSprite.texture;

                if (spriteTexture != null)
                {
                    string path = Path.Combine(Path.GetDirectoryName(Application.dataPath), "texturemods", spriteTexture.name);

                    //Console.WriteLine("[SpriteReplacer] Sprite.Texture.name:" + spriteTexture.name);
                    //Console.WriteLine("[SpriteReplacer] SearchPath:" + path + ".png");

                    if (File.Exists(path + ".png"))
                    {
                        Sprite ogSprite = targetSprite;
                        ogSprite.texture.LoadImage(File.ReadAllBytes(path + ".png"));
                        Vector2 standardisedPivot = new Vector2(ogSprite.pivot.x / ogSprite.rect.width, ogSprite.pivot.y / ogSprite.rect.height);
                        Sprite sprite = Sprite.Create(ogSprite.texture, ogSprite.rect, standardisedPivot, ogSprite.pixelsPerUnit);
                        sprite.name = ogSprite.name;
                        spriteTexture = sprite.texture;

                        Console.WriteLine("[SpriteReplacer] OK! Replaced: " + path + ".png");

                        return true;
                    }
                    else
                    {
                        Console.WriteLine("[SpriteReplacer] FAIL! No image at: " + path + ".png");
                    }
                }
            }

            return false;
        }

        public static SpriteReplacer Plugin;
    }
}
