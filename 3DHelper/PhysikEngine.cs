using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3DHelper
{
    public class PhysikEngine
    {
        private List<IPhysikObject> objects;

        private IWorld World;

        public float Gravity { get; set; }


        public PhysikEngine(float gravity, IWorld world)
        {
            Gravity = gravity;
            objects = new List<IPhysikObject>();
            World = world;
            //oct = new Octree(4, 0, world.CubeSize, Vector3.Zero);
        }

        public void AddObject(IPhysikObject obj)
        {
            objects.Add(obj);
        }

        public void Update(GameTime gameTime)
        {
            foreach (var o in objects)
            {
                if (!o.Static)
                {
                    o.Speed += Vector3.Down * Gravity * (float) gameTime.ElapsedGameTime.TotalSeconds;
                }
                o.Position += o.Speed;
            }

            for (int j = 0; j < objects.Count; j++)
            {
                for (int i = j + 1; i < objects.Count; i++)
                {
                    if (objects[i].Bound.Intersects(objects[j].Bound) && !objects[i].Equals(objects[j]))
                    {
                        var i1 = i;
                        var j1 = j;
                        ThreadPool.QueueUserWorkItem(o => Collision(objects[i1], objects[j1]));
                    }
                }
                World.WorldCollision(objects[j]);
            }
        }

        private void Collision(IPhysikObject obj1, IPhysikObject obj2)
        {
            var vMittelpunkt = obj1.Position - obj2.Position;

            var angleY = (float) Math.Atan2(vMittelpunkt.Z, vMittelpunkt.X);
            var angleZ = (float) Math.Atan2(vMittelpunkt.Y, vMittelpunkt.X);

            var rotationmatrix = Matrix.CreateRotationY(-angleY) * Matrix.CreateRotationZ(-angleZ);

            var v1Transformed = Vector3.Transform(obj1.Speed, rotationmatrix);
            var v2Transformed = Vector3.Transform(obj2.Speed, rotationmatrix);

            var v1 = 2 * ((v1Transformed.X * obj1.Mass + obj2.Mass * v2Transformed.X) / (obj1.Mass + obj2.Mass)) -
                     v1Transformed.X;
            var v2 = 2 * ((v1Transformed.X * obj1.Mass + obj2.Mass * v2Transformed.X) / (obj1.Mass + obj2.Mass)) -
                     v2Transformed.X;

            var rotationmatrix2 = Matrix.CreateRotationZ(angleZ) * Matrix.CreateRotationY(angleY);

            obj1.Speed = Vector3.Transform(new Vector3(v1, v1Transformed.Y, v1Transformed.Z) * obj1.Elasticity,
                rotationmatrix2);
            obj2.Speed = Vector3.Transform(new Vector3(v2, v2Transformed.Y, v2Transformed.Z) * obj2.Elasticity,
                rotationmatrix2);

            var radidiff = obj1.Bound.Radius + obj2.Bound.Radius - vMittelpunkt.Length();

            obj1.Position += Vector3.Transform(new Vector3(-radidiff / 2.0f, 0, 0), rotationmatrix2);
            obj2.Position += Vector3.Transform(new Vector3(radidiff / 2.0f, 0, 0), rotationmatrix2);

            if (vMittelpunkt.Length() > (obj1.Position - obj2.Position).Length())
            {
                obj1.Position += Vector3.Transform(new Vector3(radidiff, 0, 0), rotationmatrix2);
                obj2.Position += Vector3.Transform(new Vector3(-radidiff, 0, 0), rotationmatrix2);
            }
        }

        public void Draw(Camera camera)
        {
            foreach (var physikObject in objects)
            {
                foreach (ModelMesh mesh in physikObject.Model.Meshes)
                {
                    foreach (var effect1 in mesh.Effects)
                    {
                        var effect = (BasicEffect) effect1;
                        effect.World = physikObject.Translation;
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
}