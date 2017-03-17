using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

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
            oct = new Octree(4,0,world.Max.X*2,Vector3.Zero);
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
                    o.Speed += Vector3.Down * Gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;
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
                            
                        }
                    }
                }

            }

        }

        private void Collision(IPhysikObject obj1, IPhysikObject obj2)
        {
            // TODO: Make Collision 
        }
    }
}
