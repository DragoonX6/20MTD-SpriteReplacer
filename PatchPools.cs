﻿using System;
using System.Collections;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using BepInEx.Logging;
using BepInEx;
using HarmonyLib;
using flanne;
using flanne.UI;
using static SpriteReplacer.SpriteReplacer;

namespace SpriteReplacer
{
    /*
        Some objects are always pooled within the Awake function and therefore have to be accessed in its prefix. See console output for the log generated in Line #20 
        If the pooled object is not listed here that means it's pooled using ObjectPooler.AddObject and can be accessed by hooking that function.
        Other objects aren't pooled at all and expose their SpriteRenderer by themselves. Such as Pickups.ChestPickup, PlayerController and PlayerState.
    */
    static class PatchPools
    {
        [HarmonyPatch(typeof(ObjectPooler), "Awake")]
        [HarmonyPrefix]
        static bool AwakePrefix(ref ObjectPooler __instance)
        {

            foreach (ObjectPoolItem objectPoolItem in __instance.itemsToPool)
            {
                GameObject GO = objectPoolItem.objectToPool;

                Log.LogDebug("[SpriteReplacer:PoolHandler>Awake] Item to pool (tag1/amount/shouldExpand/tag2): " + objectPoolItem.tag + " " + objectPoolItem.amountToPool + " " + objectPoolItem.shouldExpand + " " + objectPoolItem.objectToPool.tag);
                Log.LogDebug("[SpriteReplacer:PoolHandler>Awake] PooledItem.name: " + GO.name); // GameObject.name

                SpriteRenderer currSpriteRenderer = GO.GetComponent<SpriteRenderer>();

                if (currSpriteRenderer != null)
                {
                    if (currSpriteRenderer.sprite != null)
                    {
                        Utils.ReplaceSpriteTexture(currSpriteRenderer.sprite);

                        /*
                        Log.LogDebug("[SpriteReplacer:PoolHandler>Awake] PooledItem.sprite.name: " + currSpriteRenderer.sprite.name);

                        Texture2D spriteTexture = currSpriteRenderer.sprite.texture;

                        if (spriteTexture != null)
                        {
                            Log.LogDebug("[SpriteReplacer:PoolHandler>Awake] PooledItem.sprite.texture.name:" + spriteTexture.name);

                            string path = Path.Combine(Path.GetDirectoryName(Application.dataPath), "texturemods", spriteTexture.name);

                            if (File.Exists(path + ".png"))
                            {
                                Log.LogInfo("[SpriteReplacer:PoolHandler>Awake] OK! Replaced (" + objectPoolItem.objectToPool.name + "|" + spriteTexture.name + "): " + path + ".png");

                                Sprite ogSprite = currSpriteRenderer.sprite;
                                ogSprite.texture.LoadImage(File.ReadAllBytes(path + ".png"));
                                Vector2 standardisedPivot = new Vector2(ogSprite.pivot.x / ogSprite.rect.width, ogSprite.pivot.y / ogSprite.rect.height);
                                Sprite sprite = Sprite.Create(ogSprite.texture, ogSprite.rect, standardisedPivot, ogSprite.pixelsPerUnit);
                                sprite.name = ogSprite.name;
                                spriteTexture = sprite.texture;
                            }
                            else
                            {
                                Log.LogError("[SpriteReplacer:PoolHandler>Awake] FAIL! No image found at: " + path + ".png");
                            }
                        }
                        */
                    }
                }
            }
            return true;
        }


        [HarmonyPatch(typeof(ObjectPooler), "AddObject")]
        [HarmonyPrefix]
        static bool AddObjectPrefix(string tag, GameObject GO, int amt = 3, bool exp = true)
        {
            SpriteRenderer currSpriteRenderer = GO.GetComponent<SpriteRenderer>();

            if (currSpriteRenderer != null)
            {
                if (currSpriteRenderer.sprite != null)
                {
                    Utils.ReplaceSpriteTexture(currSpriteRenderer.sprite);

                    /*
                    Log.LogDebug("[SpriteReplacer:PoolHandler>AddObject] PooledItem.sprite.name: " + currSpriteRenderer.sprite.name);

                    Texture2D spriteTexture = currSpriteRenderer.sprite.texture;

                    if (spriteTexture != null)
                    {
                        Log.LogDebug("[SpriteReplacer:PoolHandler>AddObject] PooledItem.sprite.texture.name:" + spriteTexture.name);

                        string path = Path.Combine(Path.GetDirectoryName(Application.dataPath), "texturemods", spriteTexture.name);

                        if (File.Exists(path + ".png"))
                        {
                            Log.LogDebug("[SpriteReplacer:PoolHandler>AddObject] OK! Replaced (" + GO.name + "|" + spriteTexture.name + "): " + path + ".png");

                            Sprite ogSprite = currSpriteRenderer.sprite;
                            ogSprite.texture.LoadImage(File.ReadAllBytes(path + ".png"));
                            Vector2 standardisedPivot = new Vector2(ogSprite.pivot.x / ogSprite.rect.width, ogSprite.pivot.y / ogSprite.rect.height);
                            Sprite sprite = Sprite.Create(ogSprite.texture, ogSprite.rect, standardisedPivot, ogSprite.pixelsPerUnit);
                            sprite.name = ogSprite.name;
                            spriteTexture = sprite.texture;
                        }
                        else
                        {
                            Log.LogError("[SpriteReplacer:PoolHandler] FAIL! No image found at: " + path + ".png");
                        }
                    }
                    */
                }
            }

            return true;
        }
    }
}