using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3DHelper
{
    public interface IPhysikObject
    {
        Vector3 Position { get; set; }
        Vector3 Speed { get; set; }
        BoundingSphere Bound { get; }
        bool Static { get; set; }
        float Mass { get; }
        float Elasticity { get; }
        Model Model { get; }

        Matrix Translation { get; }
    }
}
