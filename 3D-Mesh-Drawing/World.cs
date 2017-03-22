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
    class World : IWorld
    {
        public float CubeSize { get; }

        public Model Model { get; }

        private Matrix world;

        private BoundingBox bound;

        public World(float CubeSize, Model Model)
        {
            this.CubeSize = CubeSize;
            this.Model = Model;
            world = Matrix.CreateScale(CubeSize);
            bound = new BoundingBox(new Vector3(-CubeSize / 2), new Vector3(CubeSize / 2));
        }

        public void WorldCollision(IPhysikObject obj)
        {
            if (bound.Contains(obj.Bound) != ContainmentType.Contains)
            {
                if (Math.Abs(obj.Position.X) >= CubeSize / 2)
                {
                    obj.Speed = new Vector3(-obj.Speed.X, obj.Speed.Y, obj.Speed.Z) * obj.Elasticity;
                }
                if (Math.Abs(obj.Position.Y) >= CubeSize / 2)
                {
                    obj.Speed = new Vector3(obj.Speed.X, -obj.Speed.Y, obj.Speed.Z) * obj.Elasticity;

                }
                if (Math.Abs(obj.Position.Z) >= CubeSize / 2)
                {
                    obj.Speed = new Vector3(obj.Speed.X, obj.Speed.Y, -obj.Speed.Z) * obj.Elasticity;
                }
            }
        }

        public void DrawWorld(Camera camera)
        {
            foreach (ModelMesh mesh in Model.Meshes)
            {
                foreach (var effect1 in mesh.Effects)
                {
                    var effect = (BasicEffect) effect1;
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