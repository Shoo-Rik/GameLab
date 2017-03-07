﻿using Common.Data;
using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace GamePrototype.Models
{
    [XmlRoot]
    public class XmlMapInfo
    {
        [XmlAttribute]
        public int Width { get; set; }

        [XmlAttribute]
        public int Length { get; set; }

        [XmlAttribute]
        public int Color { get; set; }

        [XmlAttribute]
        public int Step { get; set; }

        [XmlElement("Info")]
        public RegionInformation[] Info { get; set; }
    }

    public static class MapInfoSerializer
    {
        private static readonly XmlSerializer Serializer = new XmlSerializer(typeof(XmlMapInfo));

        public static MapInfo Deserialize(string filePath)
        {
            var result = new MapInfo();

            using (var reader = new StreamReader(filePath))
            {
                var map = (XmlMapInfo) Serializer.Deserialize(reader);

                if(map.Width < 1 || map.Length < 1 || map.Info == null)
                    throw new ArgumentException("Width or Length < 1 or Info is null");

                if(map.Width * map.Length != map.Info.Length)
                    throw new ArgumentException("Incorrect array length.");

                result.OwnColor = map.Color;
                result.Step = map.Step;
                result.Info = new RegionInformation[map.Width, map.Length];

                foreach (var info in map.Info)
                {
                    result.Info[info.Coordinates.X, info.Coordinates.Y] = info;
                }
            }

            return result;
        }

        public static void Serialize(MapInfo mapInfo, string filePath)
        {
            if (mapInfo == null || mapInfo.Info == null)
                throw new ArgumentNullException(nameof(mapInfo));

            var xmlMapInfo = new XmlMapInfo
            {
                Width = mapInfo.Info.GetLength(0),
                Length = mapInfo.Info.GetLength(1),
                Color = mapInfo.OwnColor,
                Step = mapInfo.Step,
                Info = mapInfo.Info.Cast<RegionInformation>().ToArray()
            };

            using (var writer = new StreamWriter(filePath, false))
            {
                Serializer.Serialize(writer, xmlMapInfo);
            }
        }
    }
}