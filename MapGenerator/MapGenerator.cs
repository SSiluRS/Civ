using System;

namespace MapGenerator
{
    public enum Biomes
    {
        Ocean,
        River,
        Mountains,
        Desert,
        Snow,
        Forest,
        Tundra,
        Grass
    }

    public enum Interpolation
    {
        None,
        Linear,
        Cosine
    }

    public class MapGenerator
    {
        public Biomes[,] LoadMapFromFile (string path)
        {
            Biomes[,] map = new Biomes[320,160];
            using (var s = System.IO.File.Open(path, System.IO.FileMode.Open))
            using (var b = new System.IO.BinaryReader(s))
            {
                var width = map.GetLength(0);
                var height = map.GetLength(1);
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        map[x, y] = (Biomes)b.ReadInt32();
                    }
                }
            }
            return map;
        }

        public Biomes[,] GenerateMap1 ()
        {
            return GenerateMap(0.625f, 10, 0.08f, true,true,true, Interpolation.Linear);
        }

        public void GenerateToFile (string path)
        {
            var map = GenerateMap1();

            using (var s = System.IO.File.Create(path))
            using (var b = new System.IO.BinaryWriter(s)) 
            {
                var width = map.GetLength(0);
                var height = map.GetLength(1);
                
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        b.Write((int)map[x, y]);
                    }
                }
            }
        }

        public Biomes[,] GenerateMap(float persistence, float amplitude, float frequency, bool smoothing, bool random, bool seamless, Interpolation interpolation)
        {
            int WidthPicture = 320, HeightPicture = 160;
            float Persistence = persistence, StartAmplitude = amplitude, StartFrequency = frequency;

            InretpilationFunc IF = null;
            if (interpolation == Interpolation.None) IF = None_Interpolate;
            else if (interpolation == Interpolation.Linear) IF = Linear_Interpolate;
            else if (interpolation == Interpolation.Cosine) IF = Cosine_Interpolate;

            bool Smoothing = smoothing;
            bool Random = random;

            PerlinNoise2D PN2D = new PerlinNoise2D(WidthPicture, HeightPicture, Persistence, StartAmplitude, StartFrequency, 4, IF, Smoothing, Random, seamless);
            PN2D.Generate();

            Tuple<int,int,int>[,] BMResult = new Tuple<int,int,int>[WidthPicture,HeightPicture];
            for (int x = 0; x < WidthPicture; ++x)
                for (int y = 0; y < HeightPicture; ++y)
                {
                    int c;
                    c = (int)(255.0f * (PN2D.GetResultNoise(x, y) - PN2D.GetMin()) / (PN2D.GetMax() - PN2D.GetMin()));
                    BMResult[x,y] = new Tuple<int, int, int>(c, c, c);
                }

            return DrawMap(BMResult);
        }

        static float None_Interpolate(float a, float b, float x)
        {
            if (x < 0.5f) return a;
            else return b;
        }

        static float Linear_Interpolate(float a, float b, float x)
        {
            return a * (1.0f - x) + b * x;
        }

        static float Cosine_Interpolate(float a, float b, float x)
        {
            float ft = x * (float)Math.PI;
            float f = (1 - (float)Math.Cos(ft)) * 0.5f;
            return a * (1.0f - f) + b * f;
        }

        private Biomes[,] DrawMap(Tuple<int,int,int>[,] noise)
        {
            Random rnd = new Random(1);

            int width = noise.GetLength(0), height = noise.GetLength(1);
            var newNoise = new Biomes[width,height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var p = new Biomes();
                    var h = (255 - noise[x,y].Item1) * (255 - noise[x, y].Item2) * (255 - noise[x, y].Item3);
                    if (h <= 130 * 130 * 130) //water
                        p = Biomes.Ocean;
                    else if (h > 180 * 180 * 180) // mountains
                    {
                        var v = rnd.Next(4);
                        if (v >= 1)
                            p = Biomes.Mountains;
                        else
                            p = Biomes.Grass;
                    }
                    else //grass
                        p = Biomes.Grass;
                    newNoise[x, y] = p;
                }
            }

            for (int x = 0; x < width; x++) //desert
            {
                for (int y = 0; y < height; y++)
                {
                    if (noise[x,y] != new Tuple<int, int, int>(0,0,255) && noise[x, y] != new Tuple<int, int, int>(150, 150, 150) && noise[x, y] != new Tuple<int, int, int>(255, 255, 255))
                    {
                        var v = height / 2 - Math.Abs(height / 2 - y);
                        if (rnd.Next(v) > height / 3)
                            newNoise[x, y] = Biomes.Desert;
                    }
                }
            }

            for (int x = 0; x < width; x++) //rivers
            {
                for (int y = 0; y < height; y++)
                {
                    var p = newNoise[x, y];
                    if (p != Biomes.Grass)
                    {
                        var d = rnd.Next(4);
                        int x1 = x, y1 = y;
                        var v1 = rnd.Next(50);
                        if (v1 < 1)
                            while (newNoise[x1,y1] != Biomes.Ocean && x1 > 2 && x1 < 318 && y1 > 2 && y1 < 158)
                            {
                                newNoise[x1, y1] = Biomes.River;
                                if (d == 0)
                                {
                                    var yd = rnd.Next(3) - 1;
                                    if (yd == 0 && x1 < 320 - 1)
                                        x1++;
                                    else if (y1 > 0 && y1 < 160 - 1 && newNoise[x1, y1 + yd] != Biomes.River)
                                        y1 += yd;
                                    else if (y1 > 0 && y1 < 160 - 1)
                                        y1 -= yd;
                                }
                                else if (d == 1)
                                {
                                    var xd = rnd.Next(3) - 1;
                                    if (xd == 0 && y1 < 160 - 1)
                                        y1++;
                                    else if (x1 > 0 && x1 < 320 - 1 && newNoise[x1 + xd, y1] != Biomes.River)
                                        x1 += xd;
                                    else if (x1 > 0 && x1 < 320 - 1)
                                        x1 -= xd;
                                }
                                else if (d == 2)
                                {
                                    var yd = rnd.Next(3) - 1;
                                    if (yd == 0 && x1 > 0)
                                        x1--;
                                    else if (y1 > 0 && y1 < 160 - 1 && newNoise[x1, y1 + yd] != Biomes.River)
                                        y1 += yd;
                                    else if (y1 > 0 && y1 < 160 - 1)
                                        y1 -= yd;
                                }
                                else
                                {
                                    var xd = rnd.Next(3) - 1;
                                    if (xd == 0 && y1 > 0)
                                        y1--;
                                    else if (x1 > 0 && x1 < 320 - 1 && newNoise[x1 + xd, y1] != Biomes.River)
                                        x1 += xd;
                                    else if (x1 > 0 && x1 < 320 - 1)
                                        x1 -= xd;
                                }

                            }
                    }
                }
            }

            for (int x = 0; x < width; x++) //Snow
            {
                for (int y = 0; y < height; y++)
                {
                    var v = (y == 0 || y == 159) ? 1 : (y == 1 || y == 158) ? rnd.Next(2) : (y < 5 || y > 155) ? rnd.Next(5) : 0;
                    if ((v == 1 && (y < 2 || y > 157)) || ((v == 1 && y > 1 && y < 158) && ((x < 319 && newNoise[x + 1, y] == Biomes.Snow) || newNoise[x, y + 1] ==Biomes.Snow || (x > 0 && newNoise[x - 1, y] == Biomes.Snow) || newNoise[x, y - 1] == Biomes.Snow)))
                    {
                        newNoise[x, y] = Biomes.Snow;
                    }
                }
            }

            for (int x = 0; x < width; x++) //Tundra
            {
                for (int y = 0; y < height; y++)
                {
                    var v = (y < 3 || y > 156) ? 0 : (y < 40 || y > 120) ? rnd.Next(10) : 0;
                    if (v != 0 && v < 5 && noise[x, y] == new Tuple<int, int, int>(0, 150, 0))
                        newNoise[x, y] = Biomes.Tundra;
                }
            }

            for (int x = 0; x < width; x++) //Forests
            {
                for (int y = 0; y < height; y++)
                {
                    var v = (y < 10 || y > 150) ? 0 : (y < 60 || y > 100) ? rnd.Next(10) : rnd.Next(1000);
                    if (v != 0 && v < 5 && noise[x, y] == new Tuple<int, int, int>(0, 150, 0))
                        newNoise[x, y] = Biomes.Forest;
                }
            }

            return newNoise;
        }
    }

    delegate float InretpilationFunc(float a, float b, float x);

    class PerlinNoise2D
    {
        float[,,] m_OctavesNoise;
        float[,] m_ResultNoise;
        int m_WidthPicture, m_HeightPicture;
        float m_Persistence, m_StartAmplitude, m_StartFrequency;
        int m_NumberOfOctaves;
        InretpilationFunc m_IF;
        bool m_Smoothing;
        bool m_Seamless;

        int m_SeedX, m_SeedY;

        float m_Min, m_Max;

        public float GetMin() { return m_Min; }
        public float GetMax() { return m_Max; }
        public float GetResultNoise(int x, int y) { return m_ResultNoise[x, y]; }
        public float GetOctavesNoise(int o, int x, int y) { return m_OctavesNoise[o, x, y]; }

        public PerlinNoise2D(int WidthPicture, int HeightPicture, float Persistence, float StartAmplitude, float StartFrequency, int NumberOfOctaves, InretpilationFunc IF, bool Smoothing, bool Random, bool Seamless)
        {
            m_Min = m_Max = 0.0f;
            m_WidthPicture = WidthPicture; m_HeightPicture = HeightPicture;
            m_Persistence = Persistence;
            m_StartAmplitude = StartAmplitude;
            m_StartFrequency = StartFrequency;
            m_NumberOfOctaves = NumberOfOctaves;
            m_IF = IF;
            m_Smoothing = Smoothing;
            m_Seamless = Seamless;
            if (Random)
            {
                Random R = new Random();
                m_SeedX = R.Next(50000);
                m_SeedY = R.Next(50000);
            }
            else
            {
                m_SeedX = 0;
                m_SeedY = 0;
            }

            m_ResultNoise = new float[m_WidthPicture, m_HeightPicture];
            m_OctavesNoise = new float[m_NumberOfOctaves, m_WidthPicture, m_HeightPicture];
        }

        public void Generate()
        {
            for (int x = 0; x < m_WidthPicture; ++x)
                for (int y = 0; y < m_HeightPicture; ++y)
                {
                    float TotalResult = 0;
                    float frequency = m_StartFrequency;
                    float amplitude = m_StartAmplitude;
                    for (int i = 0; i < m_NumberOfOctaves; ++i)
                    {
                        float OctavResult;
                        OctavResult = InterpolatedNoise_1(x * frequency, y * frequency) * amplitude;
                        OctavResult *= amplitude;
                        m_OctavesNoise[i, x, y] = OctavResult;
                        TotalResult += OctavResult;
                        frequency *= 2;
                        amplitude *= m_Persistence;
                        m_Min = Math.Min(m_Min, OctavResult);
                        m_Max = Math.Max(m_Max, OctavResult);
                    }
                    m_ResultNoise[x, y] = TotalResult;
                    m_Min = Math.Min(m_Min, TotalResult);
                    m_Max = Math.Max(m_Max, TotalResult);
                }
        }

        float InterpolatedNoise_1(float x, float y)
        {
            int integer_X = (int)x;
            float fractional_X = x - integer_X;

            int integer_Y = (int)y;
            float fractional_Y = y - integer_Y;

            float v1, v2, v3, v4;
            if (m_Smoothing)
            {
                v1 = SmoothedNoise1(integer_X, integer_Y);
                v2 = SmoothedNoise1(integer_X + 1, integer_Y);
                v3 = SmoothedNoise1(integer_X, integer_Y + 1);
                v4 = SmoothedNoise1(integer_X + 1, integer_Y + 1);
            }
            else
            {
                v1 = Noise2D(integer_X, integer_Y);
                v2 = Noise2D(integer_X + 1, integer_Y);
                v3 = Noise2D(integer_X, integer_Y + 1);
                v4 = Noise2D(integer_X + 1, integer_Y + 1);
            }

            float i1 = m_IF(v1, v2, fractional_X);
            float i2 = m_IF(v3, v4, fractional_X);

            return m_IF(i1, i2, fractional_Y);
        }

        float SmoothedNoise1(int x, int y)
        {
            float corners = (Noise2D(x - 1, y - 1) + Noise2D(x + 1, y - 1) + Noise2D(x - 1, y + 1) + Noise2D(x + 1, y + 1)) / 16.0f;
            float sides = (Noise2D(x - 1, y) + Noise2D(x + 1, y) + Noise2D(x, y - 1) + Noise2D(x, y + 1)) / 8.0f;
            float center = Noise2D(x, y) / 4.0f;
            return corners + sides + center;
        }

        void SeamlessFunc(ref int x, ref int y)
        {
            while ((x >= m_WidthPicture) || (x < 0))
                if (x >= m_WidthPicture) x -= m_WidthPicture;
                else if (x < 0) x += m_WidthPicture;
            while ((y >= m_HeightPicture) || (y < 0))
                if (y >= m_HeightPicture) y -= m_HeightPicture;
                else if (y < 0) y += m_HeightPicture;
        }

        float Noise2D2(int x, int y)
        {
            SeamlessFunc(ref x, ref y);
            return Noise2D1(x, y);
        }
        float Noise2D1(int x, int y)
        {
            x += m_SeedX;
            y += m_SeedY;
            int n = x + y * 57;
            n = (n << 13) ^ n;
            return (1.0f - ((n * (n * n * 15731 + 789221) + 1376312589) & 0x7fffffff) / 1073741824.0f);
        }

        float Noise2D(int x, int y)
        {
            if (m_Seamless) return Noise2D2(x, y);
            else return Noise2D1(x, y);
        }

    }
}