using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mg = MapGeneratorFromCS;

namespace MapView
{
    class MapRenderer : IDisposable
    {
        Bitmap river;
        Bitmap mount;
        Bitmap desert;
        Bitmap snow;
        Bitmap textures;
        Bitmap vis;
        Graphics gv;
        GameModel.World.World world;

        int w;
        int h;
        int worldWidth = 320;
        int worldHeight = 160;

        public const int tileSize = 64;

        int[] tileKeyI = { 45, 23, 24, 23, 13, 45, 13, 5 };
        int[] tileKeyII = { 45, 23, 22, 23, 11, 45, 11, 6 };
        int[] tileKeyIII = { 45, 1, 2, 1, 13, 45, 13, 16 };
        int[] tileKeyIV = { 45, 1, 0, 1, 11, 45, 11, 17 };
        int[] riverAndMountainKey = { 0, 4, 2, 6, 1, 5, 3, 7, 8, 12, 10, 14, 9, 13, 11, 15 };

        public MapRenderer(int w, int h, GameModel.World.World world)
        {
            this.w = w;
            this.h = h;
            this.world = world;
            river = new Bitmap(@"../../Map/RiverS.png");
            mount = new Bitmap(@"../../Map/MountainsS.png");
            textures = new Bitmap(@"../../Map/Textures1.png");
            desert = new Bitmap(@"../../Map/DesertS.png");
            snow = new Bitmap(@"../../Map/SnowS.png");
        }

        public Rectangle GetOceanRectangle(int n)
        {
            int r;
            int c;
            r = n / 11;
            c = n % 11;
            return new Rectangle(c * 33, r * 33, 32, 32);
        }

        public Bitmap Render(int x, int y)
        {
            vis = new Bitmap(w, h);
            using (gv = Graphics.FromImage(vis))
            {

                var cb = x / tileSize;
                var rb = y / tileSize;
                var ce = (x + w) / tileSize;
                var re = (y + h) / tileSize;
                var dx = -x % tileSize;
                var dy = -y % tileSize;

                for (int r = rb; r <= re; r++)
                {
                    var yd = (r - rb) * tileSize + dy;
                    for (int c = cb; c <= ce; c++)
                    {
                        var xd = (c - cb) * tileSize + dx;
                        var color = GetPixel(c, r);
                        if (color.IsOcean)
                        {
                            int n0 = CheckTile2(tileKeyI, c - 1, r, c - 1, r - 1, c, r - 1);
                            int n1 = CheckTile2(tileKeyII, c + 1, r, c + 1, r - 1, c, r - 1);
                            int n2 = CheckTile2(tileKeyIII, c - 1, r, c - 1, r + 1, c, r + 1);
                            int n3 = CheckTile2(tileKeyIV, c + 1, r, c + 1, r + 1, c, r + 1);



                            gv.DrawImage(textures, new Rectangle(xd, yd, tileSize / 2, tileSize / 2), GetOceanRectangle(n0), GraphicsUnit.Pixel);
                            gv.DrawImage(textures, new Rectangle(xd + 32, yd, tileSize / 2, tileSize / 2), GetOceanRectangle(n1), GraphicsUnit.Pixel);
                            gv.DrawImage(textures, new Rectangle(xd, yd + 32, tileSize / 2, tileSize / 2), GetOceanRectangle(n2), GraphicsUnit.Pixel);
                            gv.DrawImage(textures, new Rectangle(xd + 32, yd + 32, tileSize / 2, tileSize / 2), GetOceanRectangle(n3), GraphicsUnit.Pixel);
                        }
                        else
                        {
                            DrawGrass(xd, yd);
                            Bitmap tiles = null;
                            int n = -1;
                            if (color.IsRiver)
                            {
                                n = CheckTile(c, r, TileType.River);
                                tiles = river;
                            }
                            else if (color.IsMountain)
                            {
                                n = CheckTile(c, r, TileType.Mountain);
                                tiles = mount;
                            }
                            else if (color.IsDesert)
                            {
                                n = CheckTile(c, r, TileType.Desert);
                                tiles = desert;
                            }
                            else if (color.IsSnow)
                            {
                                n = CheckTile(c, r, TileType.Snow);
                                tiles = snow;
                            }

                            var rr = n / 4;
                            var cc = n % 4;
                            var src = new Rectangle(cc * tileSize, rr * tileSize, tileSize, tileSize);
                            var dst = new Rectangle(xd, yd, tileSize, tileSize);
                            if (n != -1)
                                gv.DrawImage(tiles, dst, src, GraphicsUnit.Pixel);
                        }
                    }
                }
                return vis;
            }
        }

        private void DrawGrass(int xd, int yd)
        {
            for (int i = 0; i < 4; i++)
            {
                gv.DrawImage(textures, new Rectangle(xd + (tileSize / 2) * (i % 2), yd + (tileSize / 2) * (i / 2), tileSize / 2, tileSize / 2), GetOceanRectangle(12), GraphicsUnit.Pixel);
            }
        }

        public int CheckTile(int x, int y, TileType tileType)
        {
            int nn;
            Func<mg.MapGeneratorFromCS.LandTerrain, bool> color =
                lt => (tileType == TileType.River && lt.IsRiver)
                || (tileType == TileType.Mountain && lt.IsMountain)
                || (tileType == TileType.Desert && lt.IsDesert)
                || (tileType == TileType.Snow && lt.IsSnow);
            var l = color(GetPixel(x - 1, y)) ? 0b1000 : 0b0;
            var t = y == 0 || color(GetPixel(x, y - 1)) ? 0b100 : 0b0;
            var r = color(GetPixel(x + 1, y)) ? 0b10 : 0b0;
            var b = y == worldHeight || color(GetPixel(x, y + 1)) ? 0b1 : 0b0;
            nn = l | t | r | b;

            return riverAndMountainKey[nn];
        }

        public int CheckTile2(int[] tilek, int x0, int y0, int x1, int y1, int x2, int y2)
        {
            var color = mg.MapGeneratorFromCS.LandTerrain.Ocean;
            var p0 = GetPixel(x0, y0) != color ? 0b100 : 0b0;
            var p1 = GetPixel(x1, y1) != color ? 0b10 : 0b0;
            var p2 = GetPixel(x2, y2) != color ? 0b1 : 0b0;

            return tilek[(p0 | p1 | p2)];

        }

        private mg.MapGeneratorFromCS.LandTerrain GetPixel(int x, int y)
        {
            mg.MapGeneratorFromCS.LandTerrain color;
            if (y > worldHeight - 1 || y < 0)
            {
                color = mg.MapGeneratorFromCS.LandTerrain.Ocean;
            }
            else
            {
                if (x > worldWidth - 1)
                {
                    //color = map.GetPixel(x - map.Width, y);
                    color = world.worldMap[new Tuple<int, int>(x - worldWidth, y)];
                }
                else if (x >= 0 && x <= worldWidth - 1)
                {
                    color = world.worldMap[new Tuple<int, int>(x, y)];
                }
                else if (x < 0)
                {
                    color = world.worldMap[new Tuple<int, int>(x + worldWidth, y)];
                }
                else color = null;
            }
            return color;
        }

        public void Dispose()
        {
            gv.Dispose();
            textures.Dispose();
        }
    }

    enum TileType
    {
        Ocean,
        River,
        Mountain,
        Grass,
        Desert,
        Snow
    }
}