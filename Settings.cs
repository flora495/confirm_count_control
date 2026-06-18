using System.IO;
using System.Xml.Linq;

namespace ConfirmCountControl
{
    public sealed class Settings
    {
        public const int MinConfirmCount = 0;
        public const int MaxConfirmCount = 5;
        public const int DefaultConfirmCount = 1;

        public bool IsEnabled { get; set; }
        public int SaveExitConfirmCount { get; set; } = DefaultConfirmCount;
        public int RestartConfirmCount { get; set; } = DefaultConfirmCount;
        public int GiveUpConfirmCount { get; set; } = DefaultConfirmCount;

        public static Settings Load(string path)
        {
            if (!File.Exists(path))
            {
                return new Settings();
            }
            XElement root = XDocument.Load(path).Root;
            return new Settings
            {
                IsEnabled = (bool?)root.Element("IsEnabled") ?? false,
                SaveExitConfirmCount = ReadValue(root, "SaveExitConfirmCount"),
                RestartConfirmCount = ReadValue(root, "RestartConfirmCount"),
                GiveUpConfirmCount = ReadValue(root, "GiveUpConfirmCount")
            };
        }

        public void Save(string path)
        {
            new XDocument(new XElement("Settings",
                new XElement("IsEnabled", IsEnabled),
                new XElement("SaveExitConfirmCount", SaveExitConfirmCount),
                new XElement("RestartConfirmCount", RestartConfirmCount),
                new XElement("GiveUpConfirmCount", GiveUpConfirmCount))).Save(path);
        }

        private static int ReadValue(XElement root, string elementName)
        {
            int value = (int?)root.Element(elementName) ?? DefaultConfirmCount;
            return Clamp(value);
        }

        private static int Clamp(int value)
        {
            if (value < MinConfirmCount) return MinConfirmCount;
            if (value > MaxConfirmCount) return MaxConfirmCount;
            return value;
        }
    }
}
