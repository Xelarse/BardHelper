using System;
using Dalamud.Plugin.Services;
using Dalamud.Game.ClientState.JobGauge.Enums;
using Dalamud.Game.ClientState.JobGauge.Types;
using BardHelper.Windows;
using Lumina.Excel.GeneratedSheets;


namespace BardHelper.Features;

public class ProcTracker : IDisposable {
    private const int ProcDelta = 3;

    public bool IsEnabled { get; set; }

    private readonly Plugin Plugin;
    private ProcTrackerWindow Hud { get; init; }

    public ProcTracker(Plugin plugin, IJobGauges jobGauges) {
        Plugin = plugin;
        Hud = new ProcTrackerWindow(Plugin.Configuration);
        Plugin.WindowSystem.AddWindow(Hud);
    }

    public void Dispose() {
        Hud.Dispose();
    }

    public void Draw() {
        if (ShouldProcess()) {
            Plugin.Logger.Debug($"Flags: {Hud.Flags}");
            Hud.Draw();
        }
    }

    public void Tick(IFramework framework) {
        if (!ShouldProcess()) {
            return;
        }

        // Set the proc tracker UI active and set the value displayed to be relative to the currently playing song.
        var bardGauge = Plugin.JobGauges.Get<BRDGauge>();
        Hud.IsOpen = true;
        if (bardGauge.Song == Song.NONE) {
            Hud.DisplayedValue = 0;
            return;
        }

        // Procs happen in 3s intervals. So it's a good use of modulo for this
        var timeInSeconds = (int)(bardGauge.SongTimer * 10e-4 + 1); // +1 to account for round down
        Hud.DisplayedValue = ProcDelta - (timeInSeconds % ProcDelta);
    }

    public void OnConfigUpdate(Configuration configuration) {
        Hud.OnConfigUpdate(configuration);
        IsEnabled = configuration.ProcHelperEnabled;
    }

    private bool ShouldProcess() {
        return IsEnabled && Plugin.ClientState.LocalPlayer?.ClassJob.Id == 23;
    }
}
