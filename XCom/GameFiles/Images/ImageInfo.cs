using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;

namespace XCom
{
	public class ImageInfo:FileDesc
	{
		private readonly Dictionary<string,ImageDescriptor> _images;

		public ImageInfo(string inFile, VarCollection v)
			: base(inFile)
		{
			_images = new Dictionary<string, ImageDescriptor>();
			Load(inFile, v);
		}

		public ImageDescriptor this[string name]
		{
			get
			{
				var key = name.ToUpper();
				if (!_images.ContainsKey(key)) return null;
				return _images[key];
			}
			set{_images[name.ToUpper()]=value;}
		}

		public void Load(string inFile, VarCollection v)
		{
			using (var sr = new StreamReader(File.OpenRead(inFile)))
			{
				var vars = new VarCollection(sr, v);

				KeyVal kv;
				while ((kv = vars.ReadLine()) != null)
				{
					var img = new ImageDescriptor(kv.Keyword.ToUpper(), kv.Rest);
					_images[kv.Keyword.ToUpper()] = img;
				}
				sr.Close();
			}
		}

		public override void Save(string outFile)
		{
			using (var sw = new StreamWriter(outFile))
			{
				var a = new List<string>(_images.Keys);
				a.Sort();
				var vars = new Dictionary<string, Variable>();

				foreach(string str in a)
				{
					if (_images[str] == null) continue;
					var id = _images[str];
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
		}

		public ImagesAccessor Images
		{
			get{return new ImagesAccessor(_images);}
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

			public IEnumerable<string> Keys
			{
				get { return _images.Keys; }
			}
			public IEnumerable<ImageDescriptor> ImageDescriptors
			{
				get { return _images.Values; }
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
