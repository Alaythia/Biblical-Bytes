﻿using System.ComponentModel.Composition;
using System.Waf.Applications;
using System.Waf.Applications.Services;
using System.Windows.Input;
using Common.Commands;
using Common.Contracts.Services;
using Common.Contracts.ViewModels;
using Common.Contracts.Views;
using Infrastructure.Application.Properties;
using Infrastructure.Application.Services;

namespace Infrastructure.Application.ViewModels;

[Export, Export(typeof(IShellViewModel))]
public class ShellViewModel : ViewModel<IShellView>, IShellViewModel
{
    private readonly IMessageService messageService;
    private readonly AppSettings settings;

    [ImportingConstructor]
    public ShellViewModel(IShellView view, IMessageService messageService, ShellService shellService, NavigationService navigationService, ISettingsService settingsService) : base(view)
    {
        this.messageService = messageService;
        ShellService = shellService;
        NavigationService = navigationService;
        ExitCommand = new DelegateCommand(Close);
        AboutCommand = new DelegateCommand(ShowAboutMessage);
        settings = settingsService.Get<AppSettings>();
        view.Closed += ViewClosed;

        // Restore the window size when the values are valid.
        if (settings.Left >= view.VirtualScreenLeft && settings.Top >= view.VirtualScreenTop
            && settings.Width > 0 && settings.Left + settings.Width <= view.VirtualScreenLeft + view.VirtualScreenWidth
            && settings.Height > 0 && settings.Top + settings.Height <= view.VirtualScreenTop + view.VirtualScreenHeight)
        {
            view.Left = settings.Left;
            view.Top = settings.Top;
            view.Height = settings.Height;
            view.Width = settings.Width;
        }
        view.IsMaximized = settings.IsMaximized;
    }

    public string Title => ApplicationInfo.ProductName;

    public IShellService ShellService { get; }

    public NavigationService NavigationService { get; }

    public ICommand ExitCommand { get; }

    public ICommand AboutCommand { get; }

    public void Show() => ViewCore.Show();

    public void Close() => ViewCore.Close();

    private void ViewClosed(object? sender, EventArgs e)
    {
        settings.Left = ViewCore.Left;
        settings.Top = ViewCore.Top;
        settings.Height = ViewCore.Height;
        settings.Width = ViewCore.Width;
        settings.IsMaximized = ViewCore.IsMaximized;
    }

    private void ShowAboutMessage()
    {
        messageService.ShowMessage(View, "{0} {1}\n\nThis software is a reference sample of the Win Application Framework (WAF).\n\nhttps://github.com/jbe2277/waf\n\n.NET Runtime: {2}",
            ApplicationInfo.ProductName, ApplicationInfo.Version, Environment.Version);
    }

    public void AddToolBarCommands(IReadOnlyList<ToolBarCommand> commands) => ViewCore.AddToolBarCommands(commands);

    public void ClearToolBarCommands() => ViewCore.ClearToolBarCommands();
}
