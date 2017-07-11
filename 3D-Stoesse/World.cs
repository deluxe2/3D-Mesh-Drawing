using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using _3DHelper;

namespace _3D_Mesh_Drawing
{
    class World : IWorld
    {
        private float cubesize;

        public float CubeSize
        {
            get { return this.cubesize; }
            set
            {
                cubesize = value;
                ConstructCube();
            }
        }

        private BoundingBox bound;

        private GraphicsDevice device;

        private List<Line> cubelines;

        public World(float CubeSize, GraphicsDevice Device)
        {
            device = Device;
            this.CubeSize = CubeSize;
        }

        public void WorldCollision(IPhysikObject obj)
        {
            if (bound.Contains(obj.Bound) != ContainmentType.Contains)
            {
                var radius = obj.Bound.Radius;
                if ((obj.Position + obj.Speed).Length() > obj.Position.Length())
                {
                    if (Math.Abs(obj.Position.X) >= CubeSize / 2)
                    {
                        obj.Speed = new Vector3(-obj.Speed.X, obj.Speed.Y, obj.Speed.Z) * obj.Elasticity;

                        if (obj.Position.X < 0)
                        {
                            obj.Position = new Vector3(-(CubeSize / 2)+radius, obj.Position.Y, obj.Position.Z);
                        }
                        else
                        {
                            obj.Position = new Vector3((CubeSize / 2)-radius, obj.Position.Y, obj.Position.Z);
                        }
                    }
                    if (Math.Abs(obj.Position.Y) >= CubeSize / 2)
                    {
                        obj.Speed = new Vector3(obj.Speed.X, -obj.Speed.Y, obj.Speed.Z) * obj.Elasticity;
                        if (obj.Position.Y < 0)
                        {
                            obj.Position = new Vector3(obj.Position.X, -(CubeSize / 2)+radius, obj.Position.Z);
                        }
                        else
                        {
                            obj.Position = new Vector3(obj.Position.X, (CubeSize / 2)-radius, obj.Position.Z);
                        }
                    }
                    if (Math.Abs(obj.Position.Z) >= CubeSize / 2)
                    {
                        obj.Speed = new Vector3(obj.Speed.X, obj.Speed.Y, -obj.Speed.Z) * obj.Elasticity;
                        if (obj.Position.Z < 0)
                        {
                            obj.Position = new Vector3(obj.Position.X, obj.Position.Y, -(CubeSize / 2)+radius);
                        }
                        else
                        {
                            obj.Position = new Vector3(obj.Position.X, obj.Position.Y, (CubeSize / 2)-radius);
                        }
                    }
                }
            }
        }

        public void DrawWorld(Camera camera)
        {
            foreach (var line in cubelines)
            {
                line.Draw(camera);
            }
        }

        private void ConstructCube()
        {
            float halfsize = CubeSize / 2.0f;
            cubelines = new List<Line>()
            {
                new Line(new Vector3(halfsize), new Vector3(halfsize, halfsize, -halfsize), device),
                new Line(new Vector3(halfsize), new Vector3(-halfsize, halfsize, halfsize), device),
                new Line(new Vector3(halfsize), new Vector3(halfsize, -halfsize, halfsize), device),
                //
                new Line(new Vector3(-halfsize), new Vector3(-halfsize, -halfsize, halfsize), device),
                new Line(new Vector3(-halfsize), new Vector3(halfsize, -halfsize, -halfsize), device),
                new Line(new Vector3(-halfsize), new Vector3(-halfsize, halfsize, -halfsize), device),
                //
                new Line(new Vector3(halfsize, halfsize, -halfsize), new Vector3(halfsize, -halfsize, -halfsize),
                    device),
                new Line(new Vector3(halfsize, halfsize, -halfsize), new Vector3(-halfsize, halfsize, -halfsize),
                    device),
                new Line(new Vector3(halfsize, -halfsize, -halfsize), new Vector3(halfsize, -halfsize, halfsize),
                    device),
                //
                new Line(new Vector3(-halfsize, halfsize, halfsize), new Vector3(-halfsize, halfsize, -halfsize),
                    device),
                new Line(new Vector3(-halfsize, halfsize, halfsize), new Vector3(-halfsize, -halfsize, halfsize),
                    device),
                new Line(new Vector3(-halfsize, -halfsize, halfsize), new Vector3(halfsize, -halfsize, halfsize),
                    device)
            };
            bound = new BoundingBox(new Vector3(-CubeSize / 2), new Vector3(CubeSize / 2));
        }
    }
}