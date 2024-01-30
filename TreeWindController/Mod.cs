﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TreeWindController {
    using System.IO;
    using System.Reflection;
    using Colossal.IO.AssetDatabase;
    using Colossal.Logging;
    using Game;
    using Game.Modding;
    using Game.Rendering;
    using Game.SceneFlow;
    using Game.UI;
    using TreeWindController.Systems;
    using Unity.Entities;

    /// <summary>
    /// The base mod class for instantiation by the game.
    /// </summary>
    public sealed class Mod : IMod {
        /// <summary>
        /// The mod's default name.
        /// </summary>
        public const string ModName = "TreeWindController";

        /// <summary>
        /// Gets the active instance reference.
        /// </summary>
        public static Mod Instance { get; private set; }

        /// <summary>
        /// Gets the mod's active log.
        /// </summary>
        internal ILog Log { get; private set; }

        /// <summary>
        /// Called by the game when the mod is loaded.
        /// </summary>
        public void OnLoad() {
            // Set instance reference.
            Instance = this;

            // Initialize logger.
            Log = LogManager.GetLogger(ModName);
#if DEBUG
            Log.Info("setting logging level to Debug");
            Log.effectivenessLevel = Level.Debug;
#endif

            Log.Info($"loading {ModName} version {Assembly.GetExecutingAssembly().GetName().Version}");
        }

        /// <summary>
        /// Called by the game when the game world is created.
        /// </summary>
        /// <param name="updateSystem">Game update system.</param>
        public void OnCreateWorld(UpdateSystem updateSystem) {
            Log.Info("OnCreateWorld");

            updateSystem.World.GetOrCreateSystem<SettingsSystem>();
            updateSystem.World.GetOrCreateSystem<SettingsUISystem>();
            updateSystem.UpdateBefore<SettingsSystem>(SystemUpdatePhase.Rendering);
            updateSystem.UpdateAt<SettingsUISystem>(SystemUpdatePhase.UIUpdate);
        }

        /// <summary>
        /// Called by the game when the mod is disposed of.
        /// </summary>
        public void OnDispose() {
            Log.Info("OnDispose");
            Instance = null;
        }

    }
}