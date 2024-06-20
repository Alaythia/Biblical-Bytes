using Common.Commands;

namespace Common.Contracts.ViewModels;

public interface IShellViewModel
{
    object View { get; }

    void AddToolBarCommands(IReadOnlyList<ToolBarCommand> commands);

    void ClearToolBarCommands();
}
