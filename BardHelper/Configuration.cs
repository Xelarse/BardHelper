using Dalamud.Configuration;
using Dalamud.Plugin;
using System;

namespace BardHelper;

[Serializable]
public class Configuration : IPluginConfiguration
{
    public int Version { get; set; } = 0;

    public bool ProcHelperEnabled { get; set; } = true;

    public bool SongHelperEnabled { get; set; } = true;

    public void Save()
    {
        Plugin.PluginInterface.SavePluginConfig(this);
    }
}
