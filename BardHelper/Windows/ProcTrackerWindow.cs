using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace BardHelper.Windows;

public class ProcTrackerWindow : Window, IDisposable {

    public int DisplayedValue { get; set; }
    public bool ShouldRender { get; set; }

    public ProcTrackerWindow(Configuration configuration) : base("Bard Helper##ProcTrackerWindow") {
        SizeConstraints = new WindowSizeConstraints {
            MinimumSize = new Vector2(50, 50),
            MaximumSize = new Vector2(400, 400)
        };

        Flags |= ImGuiWindowFlags.None |
            ImGuiWindowFlags.NoTitleBar |
            ImGuiWindowFlags.NoScrollbar |
            ImGuiWindowFlags.NoCollapse |
            ImGuiWindowFlags.NoDocking;

        OnConfigUpdate(configuration);
    }

    public void Dispose() { }

    public override void Draw() {
        IsOpen = ShouldRender;
        if (!ShouldRender) {
            return;
        }


        ImGui.Begin("Bard Helper##ProcTrackerWindowUI", Flags);
        Plugin.Logger.Debug($"Font Scale: {ImGui.GetFontSize()}");
        ImGui.Text($"{DisplayedValue}");
        ImGui.End();
    }

    public void OnConfigUpdate(Configuration configuration) {
        if (configuration.ProcHelperLockUI) {
            LockUI();
        } else {
            UnlockUI();
        }

        if (configuration.ProcHelperBackground) {
            Flags &= ~ImGuiWindowFlags.NoBackground;
        } else {
            Flags |= ImGuiWindowFlags.NoBackground;
        }
    }

    public void LockUI() {
        Flags |= ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoInputs;
    }

    public void UnlockUI() {
        Flags &= ~(ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoInputs);
    }
}
