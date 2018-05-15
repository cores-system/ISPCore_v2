using System;
using Newtonsoft.Json;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using ISPCore.Models.api;
using ISPCore.Engine.Base;
using ISPCore.Models.Security;
using ISPCore.Models.core.Cache.CheckLink;
using ISPCore.Models.Base.WhiteList;
using ISPCore.Models.Base;

namespace ISPCore.Models.Databases.json
{
    public class JsonDB
    {
        public Base Base { get; set; } = new Base();
        public Cache Cache { get; set; } = new Cache();
        public API API { get; set; } = new API();
        public Security Security { get; set; } = new Security();
        public List<WhiteListModel> WhiteList { get; set; } = new List<WhiteListModel>();
        public AntiDdos AntiDdos { get; set; } = new AntiDdos();
        public AntiVirus AntiVirus { get; set; } = new AntiVirus();
        public AntiBot AntiBot { get; set; } = new AntiBot();
        public ServiceBot ServiceBot { get; set; } = new ServiceBot();
        public BruteForceConf BruteForceConf { get; set; } = new BruteForceConf();
        public List<ProjectNews> ProjectNews { get; set; } = new List<ProjectNews>();
        public List<ProjectChange> ProjectChange { get; set; } = new List<ProjectChange>();

        public void Save()
        {
            jsonDB = this;
            File.WriteAllText($"{Folders.Databases}/ISPCore.json", JsonConvert.SerializeObject(this, Formatting.Indented));
        }

        private static JsonDB jsonDB = null;
        static JsonDB()
        {
            if (File.Exists($"{Folders.Databases}/ISPCore.json")) {
                jsonDB = JsonConvert.DeserializeObject<JsonDB>(File.ReadAllText($"{Folders.Databases}/ISPCore.json"));
            }
        }

        public JsonDB()
        {
            if (jsonDB != null)
            {
                Base = jsonDB.Base;
                Cache = jsonDB.Cache;
                API = jsonDB.API;
                Security = jsonDB.Security;
                WhiteList = jsonDB.WhiteList;
                AntiDdos = jsonDB.AntiDdos;
                AntiBot = jsonDB.AntiBot;
                AntiVirus = jsonDB.AntiVirus;
                ServiceBot = jsonDB.ServiceBot;
                BruteForceConf = jsonDB.BruteForceConf;
                ProjectNews = jsonDB.ProjectNews;
                ProjectChange = jsonDB.ProjectChange;
            }
        }
    }
}
