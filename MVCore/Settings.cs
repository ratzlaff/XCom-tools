using System;
using System.Collections.Generic;
using System.Reflection;
using System.Drawing;
using MVCore.Parser;

namespace MVCore
{
	public delegate string ConvertObject(object o);
	public delegate void ValueChangedDelegate(Setting sender, string keyword, object val);
	/// <summary>
	/// A wrapper around a Hashtable for Setting objects. Setting objects are intended to use with the CustomPropertyGrid
	/// </summary>
	public class Settings
	{
		private Dictionary<string, Setting> settings;
		private Dictionary<string, PropObj> propObj;

		private static Dictionary<Type, ConvertObject> converters;

		public static void AddConverter(Type t, ConvertObject o)
		{
			if (converters == null)
				converters = new Dictionary<Type, ConvertObject>();

			converters[t] = o;
		}

		public Settings()
		{
			settings = new Dictionary<string, Setting>();
			propObj = new Dictionary<string, PropObj>();

			if (converters == null) {
				converters = new Dictionary<Type, ConvertObject>();
				converters[typeof(Color)] = new ConvertObject(convertColor);
			}
		}

		public static void ReadSettings(VarCollection vc, KeyVal kv, Settings currSettings)
		{
			while ((kv = vc.ReadLine()) != null) {
				switch (kv.Keyword) {
					case "}": //all done
						return;
					case "{"://starting out
						break;
					default:
						if (currSettings[kv.Keyword] != null) {
							currSettings[kv.Keyword].Value = kv.Rest;
						}
						break;
				}
			}
		}

		/// <summary>
		/// Get the key collection for this Settings object. Every key is a string
		/// </summary>
		public Dictionary<string, Setting>.KeyCollection Keys
		{
			get { return settings.Keys; }
		}

		/// <summary>
		/// Get/Set the Setting object tied to the input string
		/// </summary>
		public Setting this[string key]
		{
			get
			{
				if (settings.ContainsKey(key))
					return settings[key];
				return null;
			}
			set
			{
				if (!settings.ContainsKey(key))
					settings.Add(key, value);
				else {
					settings[key] = value;
					value.Name = key;
				}
			}
		}

		public void AddSetting(string name, Setting s)
		{
			settings[s.Name] = s;
		}

		/// <summary>
		/// adds a setting to this settings object
		/// </summary>
		/// <param name="name">property name</param>
		/// <param name="val">start value of the property</param>
		/// <param name="desc">property description</param>
		/// <param name="category">property category</param>
		/// <param name="eh">event handler to recieve the PropertyValueChanged event</param>
		public Setting AddSetting(string name, object val, string desc, string category, ValueChangedDelegate eh)
		{
			return AddSetting(name, val, desc, category, eh, "", null);
		}

		public Setting AddSetting(string name, string desc, string category, string propertyName, object refObj)
		{
			return AddSetting(name, null, desc, category, null, propertyName, refObj);
		}

		/// <summary>
		/// adds a setting to this settings object
		/// </summary>
		/// <param name="name">property name</param>
		/// <param name="val">start value of the property</param>
		/// <param name="desc">property description</param>
		/// <param name="category">property category</param>
		/// <param name="eh">event handler to recieve the PropertyValueChanged event</param>
		/// <param name="refObj">the object that will recieve the changed property values</param>
		public Setting AddSetting(string name, object val, string desc, string category, ValueChangedDelegate eh, string propertyName, object refObj)
		{
			//take out all spaces
			name = name.Replace(" ", "");
			PropObj p = null;
			object v = val;
			if (refObj != null) {
				p = new PropObj(refObj, propertyName);
				v = p.GetValue();
			}

			settings[name] = new Setting(name, v, desc, category, p);
		
			if (eh != null)
				settings[name].ValueChanged += eh;

			return settings[name];
		}

		private void refProperty(Setting sender, string key, object val)
		{
			propObj[key].SetValue(val);
		}

		/// <summary>
		/// Gets the Setting object tied to the string. If there is no Setting object, one will be created with the defaultValue
		/// </summary>
		/// <param name="key">The name of the setting object</param>
		/// <param name="defaultvalue">if there is no Setting object tied to the string, a Setting will be created with this as its Value</param>
		/// <returns>The Setting object tied to the string</returns>
		public Setting GetSetting(string key, object defaultvalue)
		{
			if (settings[key] == null) {
				settings[key] = new Setting(defaultvalue, null, null);
				settings[key].Name = key;
			}

			return settings[key];
		}

		public void Save(string name, System.IO.StreamWriter sw)
		{
			sw.WriteLine(name);
			sw.WriteLine("{");

			foreach (string s in settings.Keys) {
				object o = null;

				if (propObj.ContainsKey(s))
					o = propObj[s].GetValue();
				else
					o = settings[s].Value;

				sw.WriteLine("\t" + s + ":" + convert(o));	
			}
				
			sw.WriteLine("}");
		}

		private string convert(object o)
		{
			if (converters.ContainsKey(o.GetType()))
				return converters[o.GetType()](o);
			return o.ToString();
		}

		private static string convertColor(object o)
		{
			Color c = (Color)o;
			if (c.IsKnownColor || c.IsNamedColor || c.IsSystemColor)
				return c.Name;
			return c.A + "," + c.R + "," + c.G + "," + c.B;
		}
	}

	/// <summary>
	/// Stores information to be used in the CustomPropertyGrid
	/// </summary>
	public class Setting
	{
		private bool changingValue=false;
		private object val;
		private PropObj propVal;

		private static Dictionary<Type, parseString> converters;
		public event ValueChangedDelegate ValueChanged;

		private delegate object parseString(string s);

		public Setting(string name, object val, string desc, string category, PropObj pObj)
		{
			this.val = val;
			this.Name = name;
			this.Description = desc;
			this.Category = category;
			this.propVal = pObj;

			IsVisible = true;

			if (converters == null) {
				converters = new Dictionary<Type, parseString>();
				converters[typeof(float)] = parseFloatString;
				converters[typeof(double)] = parseDoubleString;
				converters[typeof(int)] = parseIntString;
				converters[typeof(System.Drawing.Color)] = parseColorString;
				converters[typeof(bool)] = parseBoolString;
			}
		}

		private static object parseFloatString(string s)
		{
			return float.Parse(s);
		}

		private static object parseDoubleString(string s)
		{
			return double.Parse(s);
		}

		private static object parseBoolString(string s)
		{
			return bool.Parse(s);
		}

		private static object parseIntString(string s)
		{
			return int.Parse(s);
		}

		private static object parseColorString(string s)
		{
			string[] vals = s.Split(',');
			if (vals.Length == 1)
				return Color.FromName(s);
			if (vals.Length == 3)
				return Color.FromArgb(int.Parse(vals[0]), int.Parse(vals[1]), int.Parse(vals[2]));
			return Color.FromArgb(int.Parse(vals[0]), int.Parse(vals[1]), int.Parse(vals[2]), int.Parse(vals[3]));
		}

		public Setting(string name, object val, string desc, string category) : this(name, val, desc, category, null) { }
		public Setting(object val, string desc, string category) : this(null, val, desc, category) { }
		public Setting(object val, string desc) : this(val, desc, null) { }
		public Setting(object val) : this(val, null) { }

		public object Value
		{
			get
			{
				if (propVal != null)
					return propVal.GetValue();
				return val;
			}
			set
			{
				if (!changingValue) {
					changingValue = true;

					if (val != null && converters.ContainsKey(val.GetType()) && value.GetType() == typeof(string))
						val = converters[val.GetType()]((string)value);
					else
						val = value;

					if (propVal != null)
						propVal.SetValue(val);

					if (ValueChanged != null)
						ValueChanged(this, Name, val);
					changingValue = false;
				}
			}
		}

		public string Description { get; set; }
		public string Category { get; set; }
		public string Name { get; set; }
		public bool IsVisible { get; set; }
	}

	public class PropObj
	{
		public PropertyInfo pi;
		public MethodInfo delayCheck, delayCache;
		public string propertyName;
		public object obj;

		public PropObj(object obj, string property)
		{
			this.obj = obj;
			pi = obj.GetType().GetProperty(property);
			delayCache = obj.GetType().GetMethod("CacheProperty");
		}

		public object GetValue()
		{
			return pi.GetValue(obj, new object[] { });
		}

		public void SetValue(object o)
		{
			bool delay = false;
			if (delayCache != null)
				delay = (bool)delayCache.Invoke(obj, new object[] { pi, o });

			if (!delay)
				pi.SetValue(obj, o, new object[] { });
		}
	}
}
