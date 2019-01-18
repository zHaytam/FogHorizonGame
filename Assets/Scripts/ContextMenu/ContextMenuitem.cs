using System;

namespace Assets.Scripts.ContextMenu
{
    public class ContextMenuItem
    {

        #region Properties

        public string Text { get; }

        public Action Action { get; }

        #endregion

        public ContextMenuItem(string text, Action action)
        {
            Text = text;
            Action = action;
        }

    }
}
