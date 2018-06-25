using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapView
{
    class MapRenderer : IDisposable
    {
        Bitmap map;
        Bitmap river;
        Bitmap mount;
        Bitmap textures;
        Bitmap vis;
        Graphics gv;

        int w;
        int h;

        public const int tileSize = 64;

        int[] tileKeyI = { 45, 23, 24, 23, 13, 45, 13, 5 };
        int[] tileKeyII = { 45, 23, 22, 23, 11, 45, 11, 6 };
        int[] tileKeyIII = { 45, 1, 2, 1, 13, 45, 13, 16 };
        int[] tileKeyIV = { 45, 1, 0, 1, 11, 45, 11, 17 };
        int[] riverAndMountainKey = { 0, 4, 2, 6, 1, 5, 3, 7, 8, 12, 10, 14, 9, 13, 11, 15 };

        public MapRenderer(int w, int h)
        {
            this.w = w;
            this.h = h;

            map = new Bitmap(@"../../Map/map.png");
            river = new Bitmap(@"../../Map/RiverS.png");
            mount = new Bitmap(@"../../Map/MountainsS.png");
            textures = new Bitmap(@"../../Map/Textures1.png");
        }

        public Bitmap Render(int x, int y)
        {
            vis = new Bitmap(w, h);
            using (gv = Graphics.FromImage(vis))
            {

                var cb = x / tileSize;
                var rb = y / tileSize ;
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
                        if (color.Name == "ff0000ff")
                        {
                            int n0 = CheckTile2(tileKeyI, c - 1, r, c - 1, r - 1, c, r - 1);
                            int n1 = CheckTile2(tileKeyII, c + 1, r, c + 1, r - 1, c, r - 1);
                            int n2 = CheckTile2(tileKeyIII, c - 1, r, c - 1, r + 1, c, r + 1);
                            int n3 = CheckTile2(tileKeyIV, c + 1, r, c + 1, r + 1, c, r + 1);

                            gv.DrawImage(textures, new Rectangle(xd, yd, tileSize / 2, tileSize / 2), GetTileRectangle(n0, TileType.Ocean), GraphicsUnit.Pixel);
                            gv.DrawImage(textures, new Rectangle(xd + 32, yd, tileSize / 2, tileSize / 2), GetTileRectangle(n1, TileType.Ocean), GraphicsUnit.Pixel);
                            gv.DrawImage(textures, new Rectangle(xd, yd + 32, tileSize / 2, tileSize / 2), GetTileRectangle(n2, TileType.Ocean), GraphicsUnit.Pixel);
                            gv.DrawImage(textures, new Rectangle(xd + 32, yd + 32, tileSize / 2, tileSize / 2), GetTileRectangle(n3, TileType.Ocean), GraphicsUnit.Pixel);
                        }
                        else if (color.Name == "ff3232ff")
                        {
                            int n = CheckTile(c, r, TileType.River);
                            DrawGrass(xd, yd);
                            gv.DrawImage(river, new Rectangle(xd, yd, tileSize, tileSize), GetTileRectangle(n, TileType.River), GraphicsUnit.Pixel);
                        }
                        else if (color.Name == "ff969696")
                        {
                            int n = CheckTile(c, r, TileType.Mountain);
                            DrawGrass(xd, yd);
                            var src = GetTileRectangle(n, TileType.Mountain);
                            var dst = new Rectangle(xd, yd, tileSize, tileSize);
                            gv.DrawImage(mount, dst, src, GraphicsUnit.Pixel);

                        }
                        else if (color != Color.Black)
                            DrawGrass(xd, yd);
                        //gv.FillRectangle(GetPixel(c, r).Name == "ff0000ff" ? Brushes.Blue : Brushes.Green, new Rectangle(xd+15,yd+15, 5,5));
                    }
                }

                //for (int i = 0; i < 100; i++)
                //{
                //    gv.DrawLine(Pens.Black, i * 64, 0, i * 64, 1000);
                //    gv.DrawLine(Pens.Black, 0, i * 64, 1000, i * 64);

                //}
                return vis;
            }
        }

        private void DrawGrass(int xd, int yd)
        {
            for (int i = 0; i < 4; i++)
            {
                gv.DrawImage(textures, new Rectangle(xd + (tileSize / 2) * (i % 2), yd + (tileSize / 2) * (i / 2), tileSize / 2, tileSize / 2), GetTileRectangle(12, TileType.Grass), GraphicsUnit.Pixel);
            }
        }

        public Rectangle GetTileRectangle(int n, TileType tileType)
        {
            int r;
            int c;
            if (tileType == TileType.River)
            {
                r = n / 4;
                c = n % 4;
                return new Rectangle(c * tileSize, r * tileSize, tileSize, tileSize);
            }
            else if (tileType == TileType.Mountain)
            {
                r = n / 4;
                c = n % 4;
                return new Rectangle(c * tileSize, r * tileSize, tileSize, tileSize);
            }
            else
            {
                r = n / 11;
                c = n % 11;
                return new Rectangle(c * 33, r * 33, 32, 32);
            }

        }

        public int CheckTile(int x, int y, TileType tileType)
        {
            int nn;
            var color = tileType == TileType.River ? "ff3232ff" : "ff969696";
                var l = GetPixel(x - 1, y).Name == color ? 0b1000 : 0b0;
                var t = y == 0 || GetPixel(x, y - 1).Name == color ? 0b100 : 0b0;
                var r = GetPixel(x + 1, y).Name == color ? 0b10 : 0b0;
                var b = y == map.Height || GetPixel(x, y + 1).Name == color ? 0b1 : 0b0;
                nn = l | t | r | b;

            return riverAndMountainKey[nn] ;
        }

        public int CheckTile2(int[] tilek, int x0, int y0, int x1, int y1, int x2, int y2)
        {
            var p0 = GetPixel(x0, y0).Name != "ff0000ff" ? 0b100 : 0b0;
            var p1 = GetPixel(x1, y1).Name != "ff0000ff" ? 0b10 : 0b0;
            var p2 = GetPixel(x2, y2).Name != "ff0000ff" ? 0b1 : 0b0;

            return tilek[(p0 | p1 | p2)];

        }


        private Color GetPixel(int x, int y)
        {
            var color = new Color();
            if (y > map.Height - 1 || y < 0)
            {
                color = Color.Black;
            }
            else
            {
                if (x >319)
                {
                    color = map.GetPixel(x - map.Width, y);
                }
                else if (x >= 0 && x <= 319)
                {
                    color = map.GetPixel(x, y);
                }
                else if (x < 0)
                {
                    color = map.GetPixel(x + map.Width, y);
                }
            }
            return color;
        }

        public void Dispose()
        {
            gv.Dispose();
            textures.Dispose();
            map.Dispose();
        }
    }

    enum TileType
    {
        Ocean,
        River,
        Mountain,
        Grass
        
    }
}