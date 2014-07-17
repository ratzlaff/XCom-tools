using System;

namespace XCom.Interfaces.Base
{
    public interface IInfo
    {
        int ID { get; }
        sbyte TileOffset { get; }
        sbyte StandOffset { get; }
        TileType TileType { get; }
        SpecialType TargetType { get; }
        bool HumanDoor { get; }
        bool UFODoor { get; }
    }
}
