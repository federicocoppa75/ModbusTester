using Registers.Models.Enums;

namespace Registers.Models.Interface
{
    public interface IBaseData
    {
        DataCategory DataCategory { get; set; }
        DataDirection DataDirection { get; set; }
        string Name { get; set; }
        int Register { get; set; }
    }
}