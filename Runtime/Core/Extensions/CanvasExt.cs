using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnInventory.Core.Extensions
{
    public static class CanvasExt
    {
        public static IEnumerable<Canvas> GetRootSceneCanvases()
        {
            var canvases = Object.FindObjectsOfType<Canvas>().Where(canvas => canvas.isRootCanvas).ToArray();
            return canvases;
        }
    }
}
