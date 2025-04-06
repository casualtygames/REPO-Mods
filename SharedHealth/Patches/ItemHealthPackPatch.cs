using System.Collections.Generic;
using System.Threading.Tasks;
using Casualty.SharedHealth;
using HarmonyLib;
using Photon.Pun;

namespace SharedHealth.Patches
{
    [HarmonyPatch(typeof(ItemHealthPack), "UsedRPC")]
    public static class ItemHealthPackPatch
    {
        [HarmonyPostfix]
        public static void Postfix(ItemHealthPack __instance)
        {
            // Get the player who used the health pack
            Photon.Realtime.Player? user = __instance.photonView?.Owner;

            List<PlayerAvatar> players = SemiFunc.PlayerGetAll();
            int healedCount = 0;

            foreach (var player in players)
            {
                if (player != null && player.playerHealth != null)
                {
                    // Heal everyone including the user
                    player.playerHealth.HealOther(__instance.healAmount, effect: true);
                    healedCount++;
                }
            }

            SharedHealthPlugin.SharedHealthModLogger.LogInfo(
                $"[SharedHealth] Health pack used by '{user?.NickName ?? "Unknown"}'. Healed {healedCount} player(s) for {__instance.healAmount}.");
        }
    }
}