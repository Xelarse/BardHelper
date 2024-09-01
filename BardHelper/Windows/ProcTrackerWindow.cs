using System;
using System.Numerics;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace BardHelper.Windows;

public class ProcTrackerWindow : Window, IDisposable {

    internal int DisplayedValue { get; set; }
    internal bool ShouldRender { get; set; }

    private ImFontPtr? TextFont { get; init; }

    public ProcTrackerWindow(Configuration configuration, ImFontPtr? font) : base("Bard Helper##ProcTrackerWindow") {
        SizeConstraints = new WindowSizeConstraints {
            MinimumSize = new Vector2(50, 50),
            MaximumSize = new Vector2(400, 400)
        };

        Flags |= ImGuiWindowFlags.None |
            ImGuiWindowFlags.NoTitleBar |
            ImGuiWindowFlags.NoScrollbar |
            ImGuiWindowFlags.NoCollapse |
            ImGuiWindowFlags.NoDocking;

        TextFont = font;

        OnConfigUpdate(configuration);
    }

    public void Dispose() { }

    public override void Draw() {
        IsOpen = ShouldRender;
        if (!ShouldRender) {
            return;
        }

        ImGui.Begin("Bard Helper##ProcTrackerWindowUI", Flags);
        if (TextFont.HasValue && TextFont.Value.IsNotNullAndLoaded()) {
            ImGui.PushFont(TextFont.Value);
            ImGui.Text($"{DisplayedValue}");
            ImGui.PopFont();
        } else {
            ImGui.Text("Failed to load font.");
        }
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
