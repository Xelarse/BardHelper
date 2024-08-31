using Dalamud.Configuration;
using System;

namespace BardHelper;

[Serializable]
public class Configuration : IPluginConfiguration {
    public delegate void ConfigurationUpdated(Configuration configuration);

    public event ConfigurationUpdated? OnConfigurationUpdatedEvent;

    public int Version { get; set; } = 0;

    public bool ProcHelperEnabled { get; set; } = true;
    public bool ProcHelperLockUI { get; set; } = false;
    public bool ProcHelperBackground { get; set; } = true;

    public bool SongHelperEnabled { get; set; } = true;
    public bool SongHelperLockUI { get; set; } = false;

    public void Save()
    {
        Plugin.PluginInterface.SavePluginConfig(this);
        OnConfigurationUpdatedEvent?.Invoke(this);
    }
}
