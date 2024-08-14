using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace BardHelper.Windows;

public class ConfigWindow : Window, IDisposable
{
    private Configuration Configuration;

    public ConfigWindow(Plugin plugin) : base("Bard Helper###Config")
    {
        Flags = ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar |
                ImGuiWindowFlags.NoScrollWithMouse;

        Size = new Vector2(232, 90);
        SizeCondition = ImGuiCond.Always;

        Configuration = plugin.Configuration;
    }

    public void Dispose() { }

    public override void PreDraw()
    {
        // Flags must be added or removed before Draw() is being called, or they won't apply
        // if (Configuration.IsConfigWindowMovable)
        // {
        //     Flags &= ~ImGuiWindowFlags.NoMove;
        // }
        // else
        // {
        //     Flags |= ImGuiWindowFlags.NoMove;
        // }
    }

    public override void Draw()
    {
        // can't ref a property, so use a local copy.
        var procActive = Configuration.ProcHelperEnabled;
        if (ImGui.Checkbox("Proc Helper Enabled", ref procActive))
        {
            Configuration.ProcHelperEnabled = procActive;
            Configuration.Save();
        }

        var songActive = Configuration.SongHelperEnabled;
        if (ImGui.Checkbox("Song Helper Enabled", ref songActive))
        {
            Configuration.SongHelperEnabled = songActive;
            Configuration.Save();
        }
    }
}
