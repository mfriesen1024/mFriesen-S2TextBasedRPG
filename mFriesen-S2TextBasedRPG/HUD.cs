using System;

namespace mFriesen_S2TextBasedRPG
{
    enum healthStatus
    {
        Healthy,
        Injured,
        Bloodied,
        Critical,
        Dying
    }

    static class HUD
    {
        public static Player player; static healthStatus playerStatus;
        static int playerHP, playerMaxHP, playerAP, playerDR, playerSTR;
        public static Foe recentFoe; static healthStatus foeStatus;
        static int foeSTR; static string foeName;
        static bool foeCheck;

        static string legend = "";



        static healthStatus GetHealthStatus(int hp, int maxhp, int ap = 0)
        {
            healthStatus result;
            int ehp = hp + ap;
            if (ehp >= maxhp) { result = healthStatus.Healthy; }
            else if (ehp >= (int)(maxhp * 0.5f)) { result = healthStatus.Injured; }
            else if (ehp >= (int)(maxhp * 0.1f)) { result = healthStatus.Bloodied; }
            else if (ehp > 0) { result = healthStatus.Critical; }
            else { result = healthStatus.Dying; }

            return result;
        }

        static void UpdateStatus() // This gets the mob statmanagers and retrieves some stats from them.
        {
            StatManager playerSM = player.statManager;
            if (recentFoe == null) { foeCheck = false; } else { foeCheck = true; }

            // Get stats and status.
            WeaponItem playerWeapon;
            playerHP = playerSM.GetStat(statname.hp);
            playerMaxHP = playerSM.maxHP;
            playerAP = playerSM.GetStat(statname.ap);
            playerDR = playerSM.GetStat(statname.dr);
            playerSTR = playerSM.GetStat(statname.str);
            playerStatus = GetHealthStatus(playerHP, playerSM.maxHP, playerSM.GetStat(statname.ap));

            if (foeCheck)
            {
                StatManager foeSM = recentFoe.statManager;

                foeName = recentFoe.name;
                foeSTR = foeSM.GetStat(statname.str);
                foeStatus = GetHealthStatus(foeSM.GetStat(statname.hp), foeSM.maxHP, foeSM.GetStat(statname.ap));
            }
        }

        public static void Update(bool print = true)
        {
            // First, lets update the stats.
            UpdateStatus();

            string playerStats = $"Player: Health == {playerHP}/{playerMaxHP} Absorption == {playerAP} Strength == {playerSTR} " +
                $"Damage Reduction == {playerDR} Effective Health == {playerHP + playerAP + playerDR}\n" +
                $"The player is {playerStatus}";

            string recentFoeStats = "No recent foe.";

            if (foeCheck) { recentFoeStats = $"Recently encountered: Type == {foeName} Str == {foeSTR} Foe status is {foeStatus}"; }

            if (print)
            {
                Console.WriteLine(playerStats);
                Console.WriteLine(recentFoeStats);
                Console.WriteLine(legend);
            }
        }
    }
}
