using System;
using JumpKing.PauseMenu.BT.Actions;

namespace ConfirmCountControl
{
    public sealed class ConfirmCountOption : IOptions
    {
        private readonly string m_label;
        private readonly Action<int> m_setter;

        public ConfirmCountOption(string label, int currentValue, Action<int> setter)
            : base(Settings.MaxConfirmCount - Settings.MinConfirmCount + 1, currentValue, EdgeMode.Clamp)
        {
            m_label = label;
            m_setter = setter;
        }

        protected override bool CanChange()
        {
            return ModEntry.Settings.IsEnabled;
        }

        protected override string CurrentOptionName()
        {
            return $"{m_label}: {base.CurrentOption}";
        }

        protected override void OnOptionChange(int option)
        {
            m_setter(option);
            ModEntry.SaveSettings();
        }
    }
}
