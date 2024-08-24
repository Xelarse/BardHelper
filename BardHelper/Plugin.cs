using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Logging;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using BardHelper.Windows;
using BardHelper.Features;

namespace BardHelper;

public sealed class Plugin : IDalamudPlugin {

    // Dalamud Services
    [PluginService]
    internal static IDalamudPluginInterface PluginInterface { get; private set; } = null!;

    [PluginService]
    internal static ITextureProvider TextureProvider { get; private set; } = null!;

    [PluginService]
    internal static ICommandManager CommandManager { get; private set; } = null!;

    [PluginService]
    internal static IClientState ClientState { get; private set; } = null!;

    [PluginService]
    internal static IFramework FrameworkManager { get; private set; } = null!;

    [PluginService]
    internal static IJobGauges JobGauges { get; private set; } = null!;

    [PluginService]
    internal static IPluginLog Logger { get; private set; } = null!;

    // Base plugin settings
    public Configuration Configuration { get; init; }
    public readonly WindowSystem WindowSystem = new("BardHelper");

    private const string CommandName = "/bh";
    private ConfigWindow ConfigWindow { get; init; }

    // Plugin features
    private ProcTracker ProcTracker { get; init; }

    public Plugin() {
        Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
        ConfigWindow = new ConfigWindow(this);
        WindowSystem.AddWindow(ConfigWindow);

        ProcTracker = new ProcTracker(this, JobGauges);
        Configuration.OnConfigurationUpdatedEvent += ProcTracker.OnConfigUpdate;

        CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand) {
            HelpMessage = "Opens the Bard Helper configuration."
        });

        PluginInterface.UiBuilder.Draw += DrawUI;
        PluginInterface.UiBuilder.OpenConfigUi += ToggleConfigUI;
        FrameworkManager.Update += OnFrameworkUpdate;
    }

    public void Dispose() {
        WindowSystem.RemoveAllWindows();
        ConfigWindow.Dispose();

        CommandManager.RemoveHandler(CommandName);

        FrameworkManager.Update -= OnFrameworkUpdate;

        ProcTracker.Dispose();
    }

    private void OnFrameworkUpdate(IFramework framework) {
        ProcTracker.Tick(framework);
    }

    private void OnCommand(string command, string args) {
        // in response to the slash command, just toggle the display status of our main ui
        ToggleConfigUI();
    }

    private void DrawUI() {
        WindowSystem.Draw();
        ProcTracker.Draw();
    }

    public void ToggleConfigUI() => ConfigWindow.Toggle();
}

// // you might normally want to embed resources and load them from the manifest stream
// var goatImagePath = Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "goat.png");
