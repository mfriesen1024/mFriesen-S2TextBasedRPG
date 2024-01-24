using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mFriesen_S2TextBasedRPG
{
    static class TemporaryDataManager
    {
        public static List<Foe> foes;
        public static List<Area> areas;

        public static void GenerateThings()
        {

        }

        public static void GenerateFoes()
        {
            foes.Add( new Foe(1, 5, 0)); // Create a rather weak creature. Once its vitality goes away, its a 1hit.
            foes.Add(new Foe(5, 0, 2)); // Create a more resilient creture. It can take a hit or 2.
            foes.Add(new Foe(15, 10, 5)); // Create a formidable dragon.
        }
    }
}
