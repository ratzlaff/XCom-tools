using System;
using System.Collections.Generic;
using System.IO;
using XCom.Interfaces.Base;

namespace XCom
{
    public class XCMapDesc : IMapDesc
    {
        public XCMapDesc(
            string basename,
            string basePath,
            string blankPath,
            //string tileset,
            string rmpPath,
            string[] dependencies,
            Palette palette) : base(basename)
        {
            Palette = palette;
            Basename = basename;
            BasePath = basePath;
            RmpPath = rmpPath;
            BlankPath = blankPath;
            //this.tileset = tileset;
            Dependencies = dependencies;
            IsStatic = false;
        }

        public string[] Dependencies { get; set; }
        public Palette Palette { get; protected set; }
        public string Basename { get; protected set; }
        public string BasePath { get; protected set; }
        public string RmpPath { get; protected set; }
        public string BlankPath { get; protected set; }

        public bool IsStatic { get; set; }

        public string FilePath
        {
            get { return BasePath + Basename + ".MAP"; }
        }

        public int CompareTo(object other)
        {
            if (other is XCMapDesc)
            {
                return Basename.CompareTo(((XCMapDesc) other).Basename);
            }
            return 1;
        }
    }
}
