using System;
using System.Collections.Generic;
using System.Text;

namespace XCom.Interfaces.Base
{
    public class MapPosition
    {
        public int MaxH;
        public int MaxC;
        public int MaxR;
        public int H;
        public int C;
        public int R;

        public int GetIntLocation()
        {
            return (MaxR * MaxC * H) + (MaxC * R) + C;
        }
    }
}
