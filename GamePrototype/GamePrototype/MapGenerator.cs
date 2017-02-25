using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Xml.Serialization;
using Common.Data;

namespace GamePrototype
{
    public class MapGenerator
    {
        public static int RegionSize = 64;

        public static void GenerateMap()
        {
            //Console.WriteLine("Сколько Вы хотите генерировать изображений?");
            //int n = Convert.ToInt32(Console.ReadLine());
            Invite invite = new Invite();
            /*for (int index = 0; index < n; index++)
            {
                String name = Convert.ToString(index + 1) + ".jpg";
                invite.Inviter(name, 200);
            }*/
            var info1 = new RegionInformation { LandId = 0xFF0000 };
            var info2 = new RegionInformation { LandId = 0x0000FF };

            RegionInformation[,] infos = new RegionInformation[2,3]
            {
                { info1, info1, info2 },
                { info1, info2, info2 },
            };
            invite.GenerateMap(RegionSize, infos, @"C:\Projects\Temp\aaa.jpg");
        }
    }

    public class Invite
    {
        private static readonly Random RandomGen = new Random();

        public void GenerateMap(int regionSize, RegionInformation[,] infos, string filePath)
        {
            Bitmap bmp = GenerateMap(regionSize, infos);
            bmp.Save(filePath, ImageFormat.Jpeg);
        }

        public static Bitmap GenerateMap(int regionSize, RegionInformation[,] infos)
        {
            int widthCount = infos.GetLength(0);
            int heightCount = infos.GetLength(1);

            Bitmap bmp = new Bitmap(regionSize * widthCount, regionSize * heightCount);
            Graphics gr = Graphics.FromImage(bmp);

            for (int widthIndex = 0; widthIndex < widthCount; ++widthIndex)
            {
                for (int heightIndex = 0; heightIndex < heightCount; ++heightIndex)
                {
                    gr.FillRectangle(CreateBrush(infos[widthIndex, heightIndex].LandId), 
                        new Rectangle(widthIndex * regionSize, heightIndex * regionSize - 1, regionSize - 1, regionSize - 1));
                    /*gr.FillRectangle(CreateBrush(0), 
                        new Rectangle((widthIndex + 1) * regionSize - 1, heightIndex * regionSize, 1, regionSize));
                    gr.FillRectangle(CreateBrush(0),
                        new Rectangle(widthIndex * regionSize, (heightIndex + 1) * regionSize - 1, regionSize, 1));*/
                }
            }
            return bmp;
        }

        public void Inviter(string filename, int size)
        {
            Bitmap bmp = new Bitmap(size, size);
            int width = size/2;
            
            Graphics gr = Graphics.FromImage(bmp);
            gr.FillRectangle(CreateRandomBrush(), new Rectangle(0, 0, width, width));
            gr.FillRectangle(CreateRandomBrush(), new Rectangle(0, width, width, width));
            gr.FillRectangle(CreateRandomBrush(), new Rectangle(width, width, width, width));
            gr.FillRectangle(CreateRandomBrush(), new Rectangle(width, 0, width, width));
            bmp.Save(filename, ImageFormat.Jpeg);
        }

        public static SolidBrush CreateBrush(int colorId)
        {
            int red = (colorId & 0xFF0000) >> 16;
            int green = (colorId & 0x00FF00) >> 8;
            int blue = colorId & 0x0000FF;
            return new SolidBrush(Color.FromArgb(red, green, blue));
        }

        private SolidBrush CreateRandomBrush()
        {
            return new SolidBrush(Color.FromArgb(RandomGen.Next(255), RandomGen.Next(255), RandomGen.Next(255)));
        }
    }
}
