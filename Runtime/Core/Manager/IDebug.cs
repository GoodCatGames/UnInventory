using UnityEngine;

namespace UnInventory.Core.Manager
{
    public interface IDebug
    {
        GameObject PointForDebug { get; }
        bool DebugMode { get; }
    }
}