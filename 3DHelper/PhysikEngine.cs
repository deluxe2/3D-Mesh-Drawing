using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3DHelper
{
    public class PhysikEngine
    {
        private List<IPhysikObject> objects;

        private Octree oct;

        private BoundingBox World;

        public float Gravity { get; set; }

        public PhysikEngine(float gravity, BoundingBox world)
        {
            Gravity = gravity;
            objects = new List<IPhysikObject>();
            World = world;
            oct = new Octree(4, 0, world.Max.X * 2, Vector3.Zero);
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

            var add = oct.Update();
            foreach (var physikObject in add)
            {
                oct.Add(physikObject);
            }

            foreach (var physikObject in objects)
            {
                foreach (var tree in oct.getContainingOctrees(physikObject))
                {
                    foreach (var obj in tree.gameObjects)
                    {
                        if (physikObject.Bound.Intersects(obj.Bound))
                        {
                            Collision(physikObject, obj);
                        }
                    }
                }
            }
        }

        private void Collision(IPhysikObject obj1, IPhysikObject obj2)
        {
            var vMittelpunkt = obj1.Position - obj2.Position;

            float angleY = (float) Math.Atan2(vMittelpunkt.Z, vMittelpunkt.X);
            float angleZ = (float) Math.Atan2(vMittelpunkt.Y, vMittelpunkt.X);

            var rotationmatrix = Matrix.CreateRotationY(-angleY) * Matrix.CreateRotationZ(-angleZ);

            var v1Transformed = Vector3.Transform(obj1.Speed, rotationmatrix);
            var v2Transformed = Vector3.Transform(obj2.Speed, rotationmatrix);

            var v1 = 2 * ((v1Transformed.X * obj1.Mass + obj2.Mass * v2Transformed.X) / (obj1.Mass + obj2.Mass)) -
                     v1Transformed.X;
            var v2 = 2 * ((v1Transformed.X * obj1.Mass + obj2.Mass * v2Transformed.X) / (obj1.Mass + obj2.Mass)) -
                     v2Transformed.X;

            obj1.Speed = Vector3.Transform(new Vector3(v1 * obj1.Elasticity, v1Transformed.Y, v1Transformed.Z),
                -rotationmatrix);
            obj2.Speed = Vector3.Transform(new Vector3(v2 * obj2.Elasticity, v2Transformed.Y, v2Transformed.Z),
                -rotationmatrix);
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