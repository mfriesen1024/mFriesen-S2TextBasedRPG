using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mFriesen_S2TextBasedRPG
{
    enum healthStatus
    {
        Healthy,
        Injured,
        Bloodied,
        Critical,
        Dead
    }

    static class HUD
    {
        public static Player player; static healthStatus playerStatus;
        static int playerHP, playerMaxHP, playerAP, playerDR, playerSTR;
        public static Foe recentFoe; static healthStatus foeStatus;
        static int foeSTR; static string foeName;



        static healthStatus GetHealthStatus(int hp, int maxhp, int ap = 0)
        {
            healthStatus result;
            int ehp = hp + ap;
            if (ehp > maxhp) { result = healthStatus.Healthy; }
            else if (ehp > (int)(maxhp * 0.5f)) {  result = healthStatus.Injured; }
            else if (ehp > (int)(maxhp * 0.1f)) { result = healthStatus.Bloodied; }
            else if (ehp > 0) {  result = healthStatus.Critical; }
            else { result = healthStatus.Dead; }

            return result;
        }

        static void UpdateStatus() // This gets the mob statmanagers and retrieves some stats from them.
        {
            // find statmanagers
            StatManager playerSM = player.statManager;
            StatManager foeSM = recentFoe.statManager;

            // Get stats and status.
            playerHP = playerSM.GetStat(statname.hp);
            playerMaxHP = playerSM.maxHP;
            playerAP = playerSM.GetStat(statname.ap);
            playerDR = playerSM.GetStat(statname.dr);
            playerSTR = playerSM.GetStat(statname.str);
            playerStatus = GetHealthStatus(playerHP, playerSM.maxHP, playerSM.GetStat(statname.ap));
            foeName = recentFoe.name;
            foeSTR = foeSM.GetStat(statname.str);
            foeStatus = GetHealthStatus(foeSM.GetStat(statname.hp), foeSM.maxHP, foeSM.GetStat(statname.ap));
        }

        public static void Update()
        {
            // First, lets update the stats.

            string playerStats = $"Player";
        }
    }
}
