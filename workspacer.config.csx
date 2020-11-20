#r "C:\Program Files\workspacer\workspacer.Shared.dll"
#r "C:\Program Files\workspacer\plugins\workspacer.Bar\workspacer.Bar.dll"
#r "C:\Program Files\workspacer\plugins\workspacer.ActionMenu\workspacer.ActionMenu.dll"
#r "C:\Program Files\workspacer\plugins\workspacer.FocusIndicator\workspacer.FocusIndicator.dll"
#r "C:\Program Files\workspacer\workspacer.Shared.dll"

using System; 
using workspacer;
using workspacer.Bar;
using workspacer.ActionMenu;
using workspacer.FocusIndicator;

static void doConfig(IConfigContext context)
{
    context.AddBar(new BarPluginConfig()
    {
        BarTitle = "workspacer.Bar",
        BarHeight = 32,
        FontSize = 14,
        FontName = "Hack",
        Background = Color.White,
    });

	// when selecting a window do this event
    context.AddFocusIndicator(new FocusIndicatorPluginConfig()
    {
        BorderColor = Color.Red,
        TimeToShow = 1000, //
    });

	// get all monitors and assign them for easy access
    var monitors = context.MonitorContainer.GetAllMonitors();
    var ultrawide = monitors[0];
    var vertical = monitors[1];

    var sticky = new StickyWorkspaceContainer(context, StickyWorkspaceIndexMode.Local);
    context.WorkspaceContainer = sticky;

	// create the workspaces and assign them to the specific monitor
    sticky.CreateWorkspace(ultrawide, "one");
	sticky.CreateWorkspace(ultrawide, "two", new VertLayoutEngine(), new TallLayoutEngine(), new FullLayoutEngine());
	sticky.CreateWorkspace(vertical, "three", new HorzLayoutEngine(), new TallLayoutEngine());

	// set routing for some apps to always start on specific workspaces
    context.WindowRouter.AddRoute((window) => window.Title.Contains("Warcraft") ? context.WorkspaceContainer["one"] : null);
    context.WindowRouter.AddRoute((window) => window.Title.Contains("WhatsApp") ? context.WorkspaceContainer["three"] : null);
    context.WindowRouter.AddRoute((window) => window.Title.Contains("Deluge") ? context.WorkspaceContainer["three"] : null);

    // keybinds
    var mod = KeyModifiers.Alt;
    context.Keybinds.Subscribe(mod, Keys.F, () => System.Diagnostics.Process.Start("explorer.exe", @"C:\Users\Radu\Desktop"));
    context.Keybinds.Subscribe(mod, Keys.B, () => System.Diagnostics.Process.Start(@"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe"));
}

return new Action<IConfigContext>(doConfig);


