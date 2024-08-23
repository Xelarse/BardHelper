using Dalamud.Configuration;
using System;

namespace BardHelper;

[Serializable]
public class Configuration : IPluginConfiguration
{
    public int Version { get; set; } = 0;

    public bool ProcHelperEnabled { get; set; } = true;
    public bool ProcHelperLockUI { get; set; } = false;

    public bool SongHelperEnabled { get; set; } = true;
    public bool SongHelperLockUI { get; set; } = false;

    public void Save()
    {
        Plugin.PluginInterface.SavePluginConfig(this);
    }
}
