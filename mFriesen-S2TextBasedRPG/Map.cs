namespace mFriesen_S2TextBasedRPG
{
    internal class Map
    {
        Entity[] entities;
        Tile[,] tiles;
        string fName;

        public Map()
        {

        }
        Tile[,] GetMap() { return tiles; }
        void LoadMap() { }
        void RenderMap() { }
        void RenderRegion(Vector2 topLeft, Vector2 bottomRight) { }

    }
}
