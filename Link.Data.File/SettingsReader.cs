using Link.Domain.Contracts;
using Microsoft.Azure;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace Link.Data.File
{
    public class SettingsReader : ISettingsReader
    {
        private JObject _json;

        public string this[string key]
        {
            get
            {
                string setting = CloudConfigurationManager.GetSetting(key);

                if (string.IsNullOrEmpty(setting))
                    return GetSettingFromJsonFile(key);

                return setting;
            }
        }


        private string GetSettingFromJsonFile(string key)
        {
            if (_json == null)
                LoadJsonFromFile();

            if (_json == null)
                return string.Empty;

            return _json[key].Value<string>();
        }


        public void LoadJsonFromFile()
        {
            var documentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var filePath = Path.Combine(documentsFolder, "LogisticsBot-Secrets.json");

            if (!System.IO.File.Exists(filePath))
                return;

            var filecontent = System.IO.File.ReadAllText(filePath);
            _json = JsonConvert.DeserializeObject<JObject>(filecontent);
        }
    }
}
