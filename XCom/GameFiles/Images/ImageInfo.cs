using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;

namespace XCom
{
	public class ImageInfo:FileDesc
	{
		private Dictionary<string,ImageDescriptor> images;

		public ImageInfo():base("")
		{
			images = new Dictionary<string, ImageDescriptor>();
		}

		public ImageDescriptor this[string name]
		{
		    get
		    {
		        var key = name.ToUpper();
		        if (!images.ContainsKey(key)) return null;
                return images[key];
		    }
			set{images[name.ToUpper()]=value;}
		}

		public ImageInfo(string inFile,VarCollection v):base(inFile)
		{
			images = new Dictionary<string, ImageDescriptor>();
			StreamReader sr = new StreamReader(File.OpenRead(inFile));
			VarCollection vars = new VarCollection(sr,v);

			KeyVal kv = null;

			while((kv=vars.ReadLine())!=null)
			{
				ImageDescriptor img = new ImageDescriptor(kv.Keyword.ToUpper(),kv.Rest);
				images[kv.Keyword.ToUpper()] = img;
			}
			sr.Close();
		}

		public override void Save(string outFile)
		{
			StreamWriter sw = new StreamWriter(outFile);

			List<string> a = new List<string>(images.Keys);
			a.Sort();
			Dictionary<string, Variable> vars = new Dictionary<string, Variable>();

			foreach(string str in a)
            {
                if (images[str] == null) continue;
                ImageDescriptor id = images[str];
                if(!vars.ContainsKey(id.BasePath))
                    vars[id.BasePath]=new Variable(id.BaseName+":",id.BasePath);
                else
                    vars[id.BasePath].Inc(id.BaseName+":");
            }

			foreach(string basePath in vars.Keys)
				vars[basePath].Write(sw);

			sw.Flush();
			sw.Close();
		}

        public ImagesAccessor Images
		{
			get{return new ImagesAccessor(images);}
		}

        /// <summary>
        /// Helps making sure images are accessed with upper case keys
        /// </summary>
	    public class ImagesAccessor
	    {
	        private readonly Dictionary<string, ImageDescriptor> _images;

	        public ImagesAccessor(Dictionary<string, ImageDescriptor> images)
	        {
	            _images = images;
	        }

	        public IEnumerable Keys
	        {
	            get { return _images.Keys; }
	        }

	        public void Remove(string toString)
	        {
	            _images.Remove(toString.ToUpper());
	        }

            public ImageDescriptor this[string imageSet]
            {
                get { return _images[imageSet.ToUpper()]; }
            }
	    }
	}
}
