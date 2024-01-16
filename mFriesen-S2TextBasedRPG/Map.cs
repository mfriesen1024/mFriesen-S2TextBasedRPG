namespace mFriesen_S2TextBasedRPG
{
    internal class Map
    {
        public Entity[] entities;
        Tile[,] tiles;
        string fName;

        public Map(string fName)
        {
            this.fName = fName;
        }

        public Tile[,] GetMap() { return tiles; }
        public void LoadMap()
        {

        }
        public void RenderMap() { }
        public void RenderRegion(Vector2 topLeft, Vector2 bottomRight) { }

    }
}
