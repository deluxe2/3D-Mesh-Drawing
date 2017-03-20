using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using _3DHelper;

namespace _3D_Mesh_Drawing
{
    class World :IWorld
    {
        public float CubeSize { get; }

        public Model Model { get; }

        private Matrix world;

        public World(float CubeSize, Model Model)
        {
            this.CubeSize = CubeSize;
            this.Model = Model;
            world = Matrix.CreateScale(CubeSize / 2);
        }
        public void WorldCollision(IPhysikObject obj)
        {
            foreach (var modelMesh in Model.Meshes)
            {
                var transformedSphere = modelMesh.BoundingSphere.Transform(world);
                if (transformedSphere.Intersects(obj.Bound))
                {
                    if (transformedSphere.Center.X!=0.0f)
                    {
                        obj.Speed = new Vector3(-obj.Speed.X,obj.Speed.Y,obj.Speed.Z);
                    }
                    if (transformedSphere.Center.Y != 0.0f)
                    {
                        obj.Speed = new Vector3(obj.Speed.X, -obj.Speed.Y, obj.Speed.Z);
                    }
                    if (transformedSphere.Center.Z != 0.0f)
                    {
                        obj.Speed = new Vector3(obj.Speed.X, obj.Speed.Y, -obj.Speed.Z);
                    }
                    return;
                }
            }
        }

        public void DrawWorld(Camera camera)
        {
                foreach (ModelMesh mesh in Model.Meshes)
                {
                    foreach (var effect1 in mesh.Effects)
                    {
                        var effect = (BasicEffect)effect1;
                        effect.World = world;
                        effect.View = camera.View;
                        effect.Projection = camera.Projection;
                        effect.EnableDefaultLighting();
                        effect.PreferPerPixelLighting = true;
                    }
                    mesh.Draw();
                }
        }
    }
}
