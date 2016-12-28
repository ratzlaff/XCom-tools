using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using XCom.Interfaces.Base;

namespace XCom.GameFiles.Map.RmpData
{
	public class RmpService
	{
		public void ReviewRouteEntries(IMap_Base map)
		{
			var xMap = map as XCMapFile;
			if (xMap != null)
			{
				var entryOutside = new List<RmpEntry>();

				// TODO: Check x/y bounds also.

				foreach (RmpEntry entry in xMap.Rmp)
					if (RmpFile.IsOutsideHeight(entry, map.MapSize.Height))
						entryOutside.Add(entry);

				if (entryOutside.Count > 0)
				{
					var result = MessageBox.Show(
											"There are route entries outside the vertical limits of this map. Do you want to remove them?",
											"Incorrect Routes",
											MessageBoxButtons.YesNo);

					if (result == DialogResult.Yes)
					{
						foreach (var rmpEntry in entryOutside)
							xMap.Rmp.RemoveEntry(rmpEntry);

						xMap.MapChanged = true;
					}
				}
			}
		}
	}
}
