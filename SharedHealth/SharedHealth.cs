using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

// SharedHealth.cs
namespace Casualty.SharedHealth;

[BepInPlugin("Casualty.SharedHealth", "SharedHealth", "1.2.0")]
public class SharedHealthPlugin : BaseUnityPlugin
{
    internal static SharedHealthPlugin Instance { get; private set; } = null!;
    internal static ManualLogSource SharedHealthModLogger => Instance._sharedHealthLogger;

    private ManualLogSource _sharedHealthLogger => base.Logger;


    private Harmony? _harmony;

    private void Awake()
    {
        Instance = this;
        transform.parent = null;
        hideFlags = HideFlags.HideAndDontSave;

        Patch();

        SharedHealthModLogger.LogInfo($"{Info.Metadata.GUID} v{Info.Metadata.Version} has loaded!");
    }

    internal void Patch()
    {
        _harmony ??= new Harmony(Info.Metadata.GUID);
        _harmony.PatchAll();
    }

    internal void Unpatch()
    {
        _harmony?.UnpatchSelf();
    }
}
