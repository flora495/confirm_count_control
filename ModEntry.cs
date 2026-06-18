using System.IO;
using System.Reflection;
using HarmonyLib;
using JumpKing.Mods;
using JumpKing.PauseMenu;

namespace ConfirmCountControl
{
    [JumpKingMod("F.ConfirmCountControl")]
    public static class ModEntry
    {
        private const string SettingsFileName = "F.ConfirmCountControl.Settings.xml";

        private static string SettingsPath { get; set; }

        public static Settings Settings { get; private set; }

        /// <summary>
        /// Called by Jump King before the level loads
        /// </summary>
        [BeforeLevelLoad]
        public static void BeforeLevelLoad()
        {
            SettingsPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), SettingsFileName);
            Settings = Settings.Load(SettingsPath);
            MenuFactoryPatches.Apply(new Harmony("F.ConfirmCountControl.Harmony"));
        }

        public static void SaveSettings()
        {
            Settings.Save(SettingsPath);
        }

        [PauseMenuItemSetting]
        public static EnabledToggle PauseEnabledSetting(object factory, GuiFormat format)
        {
            return new EnabledToggle();
        }

        [MainMenuItemSetting]
        public static EnabledToggle MainEnabledSetting(object factory, GuiFormat format)
        {
            return new EnabledToggle();
        }

        [PauseMenuItemSetting]
        public static ConfirmCountOption PauseSaveExitConfirmCountSetting(object factory, GuiFormat format)
        {
            return new ConfirmCountOption("Save & Exit", Settings.SaveExitConfirmCount, v => Settings.SaveExitConfirmCount = v);
        }

        [PauseMenuItemSetting]
        public static ConfirmCountOption PauseRestartConfirmCountSetting(object factory, GuiFormat format)
        {
            return new ConfirmCountOption("Restart", Settings.RestartConfirmCount, v => Settings.RestartConfirmCount = v);
        }

        [PauseMenuItemSetting]
        public static ConfirmCountOption PauseGiveUpConfirmCountSetting(object factory, GuiFormat format)
        {
            return new ConfirmCountOption("Give Up", Settings.GiveUpConfirmCount, v => Settings.GiveUpConfirmCount = v);
        }

        [MainMenuItemSetting]
        public static ConfirmCountOption MainSaveExitConfirmCountSetting(object factory, GuiFormat format)
        {
            return new ConfirmCountOption("Save & Exit", Settings.SaveExitConfirmCount, v => Settings.SaveExitConfirmCount = v);
        }

        [MainMenuItemSetting]
        public static ConfirmCountOption MainRestartConfirmCountSetting(object factory, GuiFormat format)
        {
            return new ConfirmCountOption("Restart", Settings.RestartConfirmCount, v => Settings.RestartConfirmCount = v);
        }

        [MainMenuItemSetting]
        public static ConfirmCountOption MainGiveUpConfirmCountSetting(object factory, GuiFormat format)
        {
            return new ConfirmCountOption("Give Up", Settings.GiveUpConfirmCount, v => Settings.GiveUpConfirmCount = v);
        }
    }
}
