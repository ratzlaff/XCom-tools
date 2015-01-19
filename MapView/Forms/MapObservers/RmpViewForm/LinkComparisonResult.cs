using System;
using System.Collections.Generic;
using System.Text;

namespace MapView.Forms.MapObservers.RmpViewForm
{
    public class LinkComparisonResult
    {
        public readonly  bool ExistingLink;
        public readonly bool SpaceAvailable;
        public readonly int SpaceAt;
        
        public LinkComparisonResult(bool existingLink, bool spaceAvailable, int spaceAt)
        {
            ExistingLink = existingLink;
            SpaceAvailable = spaceAvailable;
            SpaceAt = spaceAt;
        }
    }
}
