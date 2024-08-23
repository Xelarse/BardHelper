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

        Flags = ImGuiWindowFlags.NoBackground | ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoDocking |
                ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoDecoration | ImGuiWindowFlags.NoInputs;

        if (configuration.ProcHelperLockUI) {
            LockUI();
        } else {
            UnlockUI();
        }

        IsOpen = true;
    }

    public void Dispose() { }

    public override void PreDraw() { }

    public override void Draw() {
        ImGui.Text($"{DisplayedValue}");
    }

    public void LockUI() {
        Flags |= ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoResize;
    }

    public void UnlockUI() {
        Flags &= ~(ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoResize);
    }
}
