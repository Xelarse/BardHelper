using System;
using System.Collections.Generic;
using System.IO;
using Dalamud.Interface.Utility;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using ImGuiNET;

namespace BardHelper.Utilities;

public class Fonts : IDisposable {
    private Plugin Plugin { get; init; }
    private ImGuiIOPtr IoPtr { get; init; }
    private string AssemblyPath { get; init; }
    private List<ImFontPtr> LoadedFonts = new();

    public Fonts(Plugin plugin, IDalamudPluginInterface pluginInterface) {
        Plugin = plugin;
        IoPtr = ImGui.GetIO();
        AssemblyPath = pluginInterface.AssemblyLocation.Directory?.FullName!;
    }

    public void Dispose() {
        foreach (var font in LoadedFonts) {
            font.Destroy();
        }
        IoPtr.Destroy();
    }

    public ImFontPtr? LoadFont(string fontName, float pixelSize) {
        var fullPath = Path.Combine(AssemblyPath, fontName);
        Plugin.Logger.Debug($"Font path: {fullPath}");

        if (!File.Exists(fullPath)) {
            return null;
        }

        byte[] fontData = File.ReadAllBytes(fullPath);

        ImFontPtr? fontPtr;
        unsafe {
            fixed (byte* fontDataPtr = fontData) {
                fontPtr = IoPtr.Fonts.AddFontFromMemoryTTF(new IntPtr(fontDataPtr), fontData.Length, pixelSize);
                IoPtr.Fonts.Build();
            }
        }

        if (fontPtr.Value.IsNotNullAndLoaded()) {
            LoadedFonts.Add(fontPtr.Value);
            return fontPtr;
        }

        return null;
    }
}
