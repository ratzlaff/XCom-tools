using System;
using System.Collections.Generic;
using System.Reflection;
using XCom.Interfaces;

namespace XCom
{
    public class SharedSpace
    {
        private static SharedSpace _instance;

        private readonly Dictionary<string, object> mySpace;

        public SharedSpace()
        {
            mySpace = new Dictionary<string, object>();
        }

        public static SharedSpace Instance
        {
            get
            {
                if (_instance == null) _instance = new SharedSpace();
                return _instance;
            }
        }

        public object GetObj(string key)
        {
            return GetObj(key, null);
        }

        public object GetObj(string key, object defaultIfNull)
        {
            if (!mySpace.ContainsKey(key))
                mySpace.Add(key, defaultIfNull);
            else if (mySpace[key] == null)
                mySpace[key] = defaultIfNull;

            return mySpace[key];
        }

        public object this[string key]
        {
            get { return mySpace[key]; }
            set { mySpace[key] = value; }
        }

        public int GetInt(string key)
        {
            return (int) mySpace[key];
        }

        public string GetString(string key)
        {
            return (string) mySpace[key];
        }

        public double GetDouble(string key)
        {
            return (double) mySpace[key];
        }

        public List<IXCImageFile> GetImageModList()
        {
            return (List<IXCImageFile>) mySpace["ImageMods"];
        }

        public Dictionary<string, Palette> GetPaletteTable()
        {
            return (Dictionary<string, Palette>)mySpace["Palettes"];
        } 
    }
}
