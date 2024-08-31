using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace BardHelper.Windows;

public class ProcTrackerWindow : Window, IDisposable {

    public int DisplayedValue { get; set; }

    public ProcTrackerWindow(Configuration configuration) : base("Bard Helper###ProcTrackerWindow") {
        SizeConstraints = new WindowSizeConstraints {
            MinimumSize = new Vector2(50, 50),
            MaximumSize = new Vector2(400, 400)
        };

        Flags |= ImGuiWindowFlags.NoTitleBar |
                 ImGuiWindowFlags.NoScrollbar |
                 ImGuiWindowFlags.NoCollapse |
                 ImGuiWindowFlags.NoBackground |
                 ImGuiWindowFlags.NoDocking;

        OnConfigUpdate(configuration);
    }

    public void Dispose() { }

    public override void PreDraw() { }

    public override void Draw() {
        Plugin.Logger.Debug($"ProcTrackerWindow Flags: {Flags}");
        ImGui.Text($"{DisplayedValue}");
    }

    public void OnConfigUpdate(Configuration configuration) {
        if (configuration.ProcHelperLockUI) {
            LockUI();
        } else {
            UnlockUI();
        }
    }

    public void LockUI() {
        Flags |= ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoInputs;
    }

    public void UnlockUI() {
        Flags &= ~(ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoInputs);
    }
}
