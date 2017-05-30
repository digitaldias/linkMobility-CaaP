using Link.Domain.Contracts;
using Microsoft.Azure;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Link.Data.File
{
    [Serializable]
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
            if (_json != null)
                return _json[key].Value<string>();

            LoadJsonFromFile();

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
