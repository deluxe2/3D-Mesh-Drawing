using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DHelper
{
    public interface IWorld
    {
        float CubeSize { get; }
        void WorldCollision(IPhysikObject obj);

        void DrawWorld(Camera camera);
    }
}