using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace _3DHelper
{
    public interface IPhysikObject
    {
        Vector3 Position { get; set; }
        Vector3 Speed { get; set; }
        BoundingSphere Bound { get; set; }
        bool Static { get; set; }
        bool Destructable { get; set; }
    }
}
