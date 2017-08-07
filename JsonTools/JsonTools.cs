using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace JsonTools
{
    public enum JsonFileResponse
    {
        DirectoryNotFound,
        Write_Ok,
        Write_Error
    }

    #region Extensions
    public static class Extensions
    {
        public static string ToJsonString(this object Object)
        {
            string result = string.Empty;
            try
            {
                result = JsonConvert.SerializeObject(Object);
            }
            catch { }
            return result;
        }
        public static T ToObject<T>(this string JsonString)
        {
            T result = default(T);
            try
            {
                result = JsonConvert.DeserializeObject<T>(JsonString);
            }
            catch { }
            return result;
        }
        public static JsonFileResponse Write(this JsonFile jsonfile, object data)
        {
            string json = JsonConvert.SerializeObject(data);
            string file = string.Format("{0}\\{1}", jsonfile.Directory, jsonfile.FileName);
            if (!Directory.Exists(jsonfile.Directory))
                return JsonFileResponse.DirectoryNotFound;
            try
            {
                File.WriteAllText(file, json);
            }
            catch
            {
                return JsonFileResponse.Write_Error;
            }
            return JsonFileResponse.Write_Ok;
        }
        public static T Read<T>(this JsonFile jsonfile)
        {
            string file = string.Format("{0}\\{1}", jsonfile.Directory, jsonfile.FileName);
            T result = default(T);
            try
            {
                using (StreamReader r = new StreamReader(file))
                {
                    string json = r.ReadToEnd();
                    result = JsonConvert.DeserializeObject<T>(json);
                }
            }
            catch { }
            return result;
        }
    }
    #endregion

    public class JsonFile
    {
        public JsonFile() { }

        public JsonFile(string directory, string filename)
        {
            Directory = directory;
            FileName = filename;
        }
        public string Directory { get; set; }
        public string FileName { get; set; }
    }
}
