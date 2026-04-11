using System;

namespace LD.FormsX.Features.Common
{
    public interface IDataGridEditingControl
    {
        void FocusEditor();
        bool TryCommitSelection();
        bool TryCommitSelectionFromKeyboard();
    }
}
