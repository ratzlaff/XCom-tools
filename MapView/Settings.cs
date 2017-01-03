using System;
using System.Collections.Generic;
using System.Reflection;
using System.Drawing;
using XCom;

namespace MapView
{
	public delegate string ConvertObject(object o);
	public delegate void ValueChangedDelegate(object sender, string keyword, object val);

	/// <summary>
	/// A wrapper around a Hashtable for Setting objects. Setting objects are intended to use with the CustomPropertyGrid
	/// </summary>
	public class Settings
	{
		private Dictionary<string,Setting> settings;
		private Dictionary<string,PropObj> propObj;

		private static Dictionary<Type,ConvertObject> converters;

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

			if (converters == null)
			{
				converters = new Dictionary<Type,ConvertObject>();
				converters[typeof(Color)] = new ConvertObject(convertColor);
			}
		}

		public static void ReadSettings(VarCollection vc, KeyVal kv, Settings currSettings)
		{
			while ((kv = vc.ReadLine()) != null)
			{
				switch (kv.Keyword)
				{
					case "}": // all done
						return;

					case "{": // starting out
						break;

					default:
						if (currSettings [kv.Keyword] != null)
						{
							currSettings[kv.Keyword].Value = kv.Rest;
							currSettings[kv.Keyword].FireUpdate(kv.Keyword);
						}
						break;
				}
			}
		}

		/// <summary>
		/// Get the key collection for this Settings object. Every key is a string
		/// </summary>
		public Dictionary<string,Setting>.KeyCollection Keys
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
				key = key.Replace(" ", "");
				if (settings.ContainsKey(key))
					return settings[key];

				return null;
			}

			set
			{
				key = key.Replace(" ", "");
				if (!settings.ContainsKey(key))
					settings.Add(key, value);
				else
				{
					settings[key] = value;
					value.Name = key;
				}
			}
		}

		/// <summary>
		/// adds a setting to this settings object
		/// </summary>
		/// <param name="name">property name</param>
		/// <param name="val">start value of the property</param>
		/// <param name="desc">property description</param>
		/// <param name="category">property category</param>
		/// <param name="update">event handler to recieve the PropertyValueChanged event</param>
		/// <param name="reflect">if true, an internal event handler will be created - the refObj must not be null and the name must be the name of a property of the type that refObj is</param>
		/// <param name="refObj">the object that will recieve the changed property values</param>
		public void AddSetting(
							string name,
							object val,
							string desc,
							string category,
							ValueChangedDelegate update,
							bool reflect,
							object refObj)
		{
			name = name.Replace(" ", ""); // take out all spaces

			Setting setting;
			if (!settings.ContainsKey(name))
			{
				setting = new Setting(val, desc, category);
				settings[name] = setting;
			}
			else
			{
				setting = settings[name];
				setting.Value = val;
				setting.Description = desc;
			}

			if (update != null)
				setting.ValueChanged += update;

			if (reflect && refObj != null)
			{
				propObj[name] = new PropObj(refObj, name);
				this[name].ValueChanged += reflectEvent;
			}
		}

		/// <summary>
		/// Gets the Setting object tied to the string. If there is no Setting object, one will be created with the defaultValue
		/// </summary>
		/// <param name="key">The name of the setting object</param>
		/// <param name="defaultvalue">if there is no Setting object tied to the string, a Setting will be created with this as its Value</param>
		/// <returns>The Setting object tied to the string</returns>
		public Setting GetSetting(string key, object defaultvalue)
		{
			if (!settings.ContainsKey(key))
			{
				var item = new Setting(defaultvalue, null, null);
				settings.Add(key, item);
				item.Name = key;
			}
			return settings[key];
		}

		private void reflectEvent(object sender,string key, object val)
		{
//			System.Windows.Forms.PropertyValueChangedEventArgs pe = (System.Windows.Forms.PropertyValueChangedEventArgs)e;
			propObj[key].SetValue(val);
		}

		public void Save(string name, System.IO.StreamWriter sw)
		{
			sw.WriteLine(name);
			sw.WriteLine("{");
			
			foreach(string s in settings.Keys)
				sw.WriteLine("\t" + s + ":" + convert(this[s].Value));

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
		private object _val;

		private static Dictionary<Type,parseString> converters;

		public event ValueChangedDelegate ValueChanged;

		private delegate object parseString(string s);
		 
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
				return Color.FromArgb(
								int.Parse(vals[0]),
								int.Parse(vals[1]),
								int.Parse(vals[2]));

			return Color.FromArgb(
								int.Parse(vals[0]),
								int.Parse(vals[1]),
								int.Parse(vals[2]),
								int.Parse(vals[3]));
		}

		public Setting(object val, string desc, string category)
		{
			_val = val;
			Description = desc;
			Category = category;

			if (converters == null)
			{
				converters = new Dictionary<Type, parseString>();
				converters[typeof(int)] = parseIntString;
				converters[typeof(Color)] = parseColorString;
				converters[typeof(bool)] = parseBoolString;
			}
		}

		public bool ValueBool
		{
			get
			{
				if (Value is bool)
					return (bool)Value;

				return false;
			}
		}

		public object Value
		{
			get { return _val; }
			set
			{
				if (_val == null)
				{
					_val = value;
				}
				else
				{
					var type = _val.GetType();
					if (converters.ContainsKey(type) && value is string)
						_val = converters[type]((string)value);
				}
			}
		}

		public string Description { get; set; }

		public string Category { get; set; }

		public string Name { get; set; }

		public void FireUpdate(string key, object val)
		{
			if (ValueChanged != null)
				ValueChanged(this, key, val);
		}

		public void FireUpdate(string key)
		{
			if (ValueChanged != null)
				ValueChanged(this, key, _val);
		}
	}

	internal struct PropObj
	{
		public PropertyInfo pi;
		public object obj;

		public PropObj(object obj, string property)
		{
			this.obj = obj;
			pi = obj.GetType().GetProperty(property);
		}

		public void SetValue(object o)
		{
			pi.SetValue(obj, o, new object[]{});
		}
	}
}
