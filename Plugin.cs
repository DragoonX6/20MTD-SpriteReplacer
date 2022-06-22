﻿using BepInEx;
using BepInEx.Logging;
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using HarmonyLib;

namespace SpriteReplacer
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class SpriteReplacer : BaseUnityPlugin
    {
        private void Awake()
        {
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

            Logger.LogInfo("Path to texturemods directory: " + Application.dataPath + "texturemods");

            // Debug options, in case you want to test specific patchers
            bool doPanels = true; // GUI, and everything in the menus before a run (incl. character sprites, weapons, rune icons)
            bool doPools = true; // Loads of sprites, incl. enemies and pickups
            bool doMisc = true; // Misc things that aren't covered bythe above

            if (doPanels)
            {
                try
                {
                    // Panels: Any GUI element
                    // Includes basically everything on the title/loadout screen, except the blinking eyes and character portraits
                    Harmony.CreateAndPatchAll(typeof(PatchPanels));
                }
                catch
                {
                    Logger.LogError($"{PluginInfo.PLUGIN_GUID} failed to patch methods (PanelHandler).");
                }
            }

            if (doPools)
            {
                try
                {
                    // Poolhandler: Most things in the battle (ie. game) screen
                    Harmony.CreateAndPatchAll(typeof(PatchPools));
                }
                catch
                {
                    Logger.LogError($"{PluginInfo.PLUGIN_GUID} failed to patch methods (PoolHandler).");
                }
            }

            if (doMisc)
            {
                try
                {
                    // MiscHandler: Everything else, special cases
                    Harmony.CreateAndPatchAll(typeof(PatchMisc));
                }
                catch
                {
                    Logger.LogError($"{PluginInfo.PLUGIN_GUID} failed to patch methods (MiscHandler).");
                }
            }
        }
    }
}