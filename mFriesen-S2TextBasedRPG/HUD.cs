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
        public static Player player; public static healthStatus status;
        public static Foe recentFoe; public static healthStatus foeStatus;

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


    }
}
