using IPA;
using IPA.Config;
using IPA.Utilities;
using UnityEngine.SceneManagement;
using IPALogger = IPA.Logging.Logger;
using UnityEngine;



namespace SaberForge
{
    public class Plugin : IBeatSaberPlugin
    {
        internal static Ref<SaberForgeConfig> config;
        internal static IConfigProvider configProvider;
        internal static EditorController editorController;

        public void Init(IPALogger logger, [Config.Prefer("json")] IConfigProvider cfgProvider)
        {
            Logger.log = logger;
            configProvider = cfgProvider;

            config = cfgProvider.MakeLink<SaberForgeConfig>((p, v) =>
            {
                if (v.Value == null || v.Value.RegenerateConfig)
                    p.Store(v.Value = new SaberForgeConfig() { RegenerateConfig = false });
                config = v;
            });
        }

        public void OnApplicationStart()
        {
            // check for CustomSabers Mod??
            AssetLoader.Init(); 
            SaberForgeConfig.LoadConfig();

        }

        public void OnApplicationQuit()
        {
            
            SaberForgeConfig.SaveConfig();
        }

        public void OnFixedUpdate()
        {

        }

        public void OnUpdate()
        {

        }

        public void OnActiveSceneChanged(Scene prevScene, Scene nextScene)
        {

 
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
        {
            if(scene.name == "MenuCore")
            {
                SaberForgeUI.CreateMenu();

                if (editorController == null)
                {
                    editorController = new GameObject("EditorController").AddComponent<EditorController>();

                }
            }

        }

        public void OnSceneUnloaded(Scene scene)
        {

        }

        public static void ReloadAssets()
        {
            AssetLoader.Init();
            SaberForgeConfig.LoadConfig();
            PartEditor.UpdateSabers();
            UIFunctions.UpdateAllPartLabels();
            UIFunctions.UpdateAllMatLabels();
        }


    }
}
