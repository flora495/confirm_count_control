using System;
using System.Reflection;
using BehaviorTree;
using HarmonyLib;
using JumpKing.PauseMenu;
using JumpKing.PauseMenu.BT;
using LanguageJK;
using Microsoft.Xna.Framework;

namespace ConfirmCountControl
{
    /// <summary>
    /// MenuFactory is internal, so its Create* methods can't be targeted with [HarmonyPatch]
    /// attributes directly; they're patched manually via reflection instead.
    /// </summary>
    internal static class MenuFactoryPatches
    {
        private static MethodInfo s_addDrawableMethod;

        public static void Apply(Harmony harmony)
        {
            Type menuFactoryType = AccessTools.TypeByName("JumpKing.PauseMenu.MenuFactory");
            s_addDrawableMethod = AccessTools.Method(menuFactoryType, "AddDrawable");

            harmony.Patch(AccessTools.Method(menuFactoryType, "CreateExitToMenu"),
                postfix: new HarmonyMethod(typeof(MenuFactoryPatches), nameof(CreateExitToMenuPostfix)));
            harmony.Patch(AccessTools.Method(menuFactoryType, "CreateRestart"),
                postfix: new HarmonyMethod(typeof(MenuFactoryPatches), nameof(CreateRestartPostfix)));
            harmony.Patch(AccessTools.Method(menuFactoryType, "CreateGiveUp"),
                postfix: new HarmonyMethod(typeof(MenuFactoryPatches), nameof(CreateGiveUpPostfix)));
        }

        private static void CreateExitToMenuPostfix(object __instance, GuiFormat p_format, ref PopupSelector __result)
        {
            RebuildConfirmChain(__instance, p_format, ref __result, ModEntry.Settings.SaveExitConfirmCount);
        }

        private static void CreateRestartPostfix(object __instance, GuiFormat p_format, ref PopupSelector __result)
        {
            RebuildConfirmChain(__instance, p_format, ref __result, ModEntry.Settings.RestartConfirmCount);
        }

        private static void CreateGiveUpPostfix(object __instance, GuiFormat p_format, ref PopupSelector __result)
        {
            RebuildConfirmChain(__instance, p_format, ref __result, ModEntry.Settings.GiveUpConfirmCount);
        }

        private static void RebuildConfirmChain(object factory, GuiFormat p_format, ref PopupSelector __result, int confirmCount)
        {
            if (!ModEntry.Settings.IsEnabled || confirmCount == Settings.DefaultConfirmCount)
            {
                return;
            }

            TextButton finalButton = (TextButton)__result.Children[2];
            IBTnode finalAction = finalButton.Child;
            string finalText = finalButton.Text;

            if (confirmCount <= 0)
            {
                __result = new InstantConfirmSelector(p_format, finalAction);
                return;
            }

            PopupSelector[] popups = new PopupSelector[confirmCount];
            for (int i = 0; i < confirmCount; i++)
            {
                popups[i] = new PopupSelector(p_format, 128);
                s_addDrawableMethod.Invoke(factory, new object[] { popups[i] });
            }

            PopupSelector outer = popups[0];
            for (int i = 0; i < confirmCount; i++)
            {
                bool isLast = i == confirmCount - 1;
                IBTnode yesTarget = isLast ? finalAction : popups[i + 1];
                string yesText = isLast ? finalText : language.MENUFACTORY_YES;

                string stepText = $"{language.MENUFACTORY_SAVEGAME_AREYOUSURE} ({i + 1}/{confirmCount})";
                popups[i].AddChild(new TextInfo(stepText, Color.Red));
                popups[i].AddChild(new TextButton(language.MENUFACTORY_NO, new MenuSelectorBack(outer)));
                popups[i].AddChild(new TextButton(yesText, yesTarget));
                popups[i].Initialize(p_include_back: false);
            }

            __result = outer;
        }
    }
}
