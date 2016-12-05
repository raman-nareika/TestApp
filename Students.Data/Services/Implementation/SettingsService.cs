using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Students.Data.Data;
using Students.Data.Services.Interfaces;

namespace Students.Data.Services.Implementation
{
    public class SettingsService : ISettingsService
    {
        private const string FileName = "Settings.xml";

        public void Save(Settings settings)
        {
            var serializer = new XmlSerializer(typeof(Settings));

            using (var fs = new FileStream(FileName, FileMode.OpenOrCreate))
            {
                serializer.Serialize(fs, settings);
            }

        }

        public Settings Open()
        {
            try
            {
                if (!File.Exists(FileName)) return null;

                var serializer = new XmlSerializer(typeof (Settings));
                Settings settings;

                using (var fs = new FileStream(FileName, FileMode.Open))
                {
                    var reader = XmlReader.Create(fs);
                    settings = (Settings) serializer.Deserialize(reader);
                }

                return settings;
            }
            catch
            {
                return null;
            }
        }

        public string GetSourceFile()
        {
            try
            {
                var settings = Open();

                return settings?.SourceFilePath;
            }
            catch
            {
                return null;
            }
        }
    }
}
