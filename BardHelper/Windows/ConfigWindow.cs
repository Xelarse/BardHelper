using System;
using System.Numerics;
using Dalamud.Interface.Internal;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using ImGuiNET;

namespace BardHelper.Windows;

public class ConfigWindow : Window, IDisposable {
    private Plugin Plugin;
    private Configuration Configuration;

    public ConfigWindow(Plugin plugin)
        : base("Bard Helper##Main", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse) {
        SizeConstraints = new WindowSizeConstraints {
            MinimumSize = new Vector2(375, 330),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        Plugin = plugin;
        Configuration = plugin.Configuration;
    }

    public void Dispose() { }

    public override void Draw() {
        if (ImGui.CollapsingHeader("Proc Tracker Settings")) {
            CheckBoxSetting("Enabled##ProcTracker", Configuration.ProcHelperEnabled,
                            value => Configuration.ProcHelperEnabled = value);
            CheckBoxSetting("LockUI##ProcTracker", Configuration.ProcHelperLockUI,
                            value => Configuration.ProcHelperLockUI = value);
            CheckBoxSetting("Background##ProcTracker", Configuration.ProcHelperBackground,
                            value => Configuration.ProcHelperBackground = value);
        }

        if (ImGui.CollapsingHeader("Song Helper Settings")) {
            CheckBoxSetting("Enabled##SongHelper", Configuration.SongHelperEnabled,
                            value => Configuration.SongHelperEnabled = value);
            CheckBoxSetting("LockUI##SongTracker", Configuration.SongHelperLockUI,
                            value => Configuration.SongHelperLockUI = value);
        }
    }

    private void CheckBoxSetting(string settingName, bool currentSetting, Action<bool> updateSetting) {
        var enabled = currentSetting;
        if (ImGui.Checkbox($"{settingName}", ref enabled)) {
            updateSetting(enabled);
            Configuration.Save();
        }
    }
}


// Reference code

// if (ImGui.Button("Show Settings"))
// {
//     Plugin.ToggleConfigUI();
// }
//
// ImGui.Spacing();
//
// ImGui.Text("Have a goat:");
// var goatImage = Plugin.TextureProvider.GetFromFile(GoatImagePath).GetWrapOrDefault();
// if (goatImage != null)
// {
//     ImGuiHelpers.ScaledIndent(55f);
//     ImGui.Image(goatImage.ImGuiHandle, new Vector2(goatImage.Width, goatImage.Height));
//     ImGuiHelpers.ScaledIndent(-55f);
// }
// else
// {
//     ImGui.Text("Image not found.");
// }

