using System;
using Dalamud.Plugin.Services;
using Dalamud.Game.ClientState.JobGauge.Enums;
using Dalamud.Game.ClientState.JobGauge.Types;
using BardHelper.Windows;


namespace BardHelper.Features;

public class ProcTracker : IDisposable {
    private const int ProcDelta = 3;

    public bool IsEnabled { get; set; }

    private readonly Plugin Plugin;
    private ProcTrackerWindow Hud { get; init; }

    public ProcTracker(Plugin plugin, IJobGauges jobGauges) {
        Plugin = plugin;
        Hud = new ProcTrackerWindow(Plugin.Configuration);
        Hud.ShouldRender = ShouldProcess();
    }

    public void Dispose() {
        Hud.Dispose();
    }

    public void Draw() {
        Hud.Draw();
    }

    public void Tick(IFramework framework) {
        if (!ShouldProcess()) {
            Hud.ShouldRender = false;
            return;
        }

        // If we should be processing then ensure the UI is enabled.
        Hud.ShouldRender = true;

        // Set the proc tracker UI active and set the value displayed to be relative to the currently playing song.
        var bardGauge = Plugin.JobGauges.Get<BRDGauge>();
        if (bardGauge.Song == Song.NONE) {
            Hud.DisplayedValue = 0;
            return;
        }

        // Procs happen in 3s intervals. So it's a good use of modulo for this
        var timeInMili = (int)(bardGauge.SongTimer * 10e-3);
        var roundedUpSeconds = (timeInMili + 9) / 10;   // Need to round up to the nearest second to match in game timer
        Hud.DisplayedValue = ProcDelta - (roundedUpSeconds % ProcDelta);
    }

    public void OnConfigUpdate(Configuration configuration) {
        Hud.OnConfigUpdate(configuration);
        IsEnabled = configuration.ProcHelperEnabled;
    }

    private bool ShouldProcess() {
        return IsEnabled && Plugin.ClientState.LocalPlayer?.ClassJob.Id == 23;
    }
}
