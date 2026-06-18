using BehaviorTree;
using JumpKing.PauseMenu;
using JumpKing.PauseMenu.BT;

namespace ConfirmCountControl
{
    /// <summary>
    /// Used when ConfirmCount is 0: runs the wrapped action immediately, with no popup shown.
    /// </summary>
    internal sealed class InstantConfirmSelector : PopupSelector
    {
        private readonly IBTnode m_action;

        public InstantConfirmSelector(GuiFormat p_format, IBTnode p_action)
            : base(p_format)
        {
            m_action = p_action;
        }

        protected override BTresult MyRun(TickData p_data)
        {
            return m_action.Run(p_data);
        }

        protected override void OnNewRun()
        {
        }

        protected override void ResumeRun()
        {
        }

        public override void Draw()
        {
        }
    }
}
