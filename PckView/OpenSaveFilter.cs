using System;
using System.Collections.Generic;
using System.Text;
using UtilLib;
using XCom.Images;

namespace PckView
{
	public class OpenSaveFilter : IFilter<xcImageFile>
	{
		private xcImageFile.Filter filterBy;

		public OpenSaveFilter()
		{
			filterBy = xcImageFile.Filter.Open;
		}

		public void SetFilter(xcImageFile.Filter filter)
		{
			filterBy = filter;
		}

		public bool FilterObj(xcImageFile obj)
		{
			//Console.WriteLine("Filter: {0} -> {1}", filterBy, obj.FileOptions[filterBy]);
			return obj.FileOptions[filterBy];
		}
	}
}
