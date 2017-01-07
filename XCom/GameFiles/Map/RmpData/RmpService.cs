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
				foreach (RmpEntry entry in xMap.Rmp)
					if (RmpFile.IsOutsideMap(
										entry,
										map.MapSize.Cols,
										map.MapSize.Rows,
										map.MapSize.Height))
					{
						entryOutside.Add(entry);
					}

				if (entryOutside.Count > 0)
				{
					var result = MessageBox.Show(
											"There are route entries outside the bounds of this Map. Do you want to remove them?",
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
