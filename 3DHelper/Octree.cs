using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace _3DHelper
{
    class Octree
    {
        private const int CHILDREN = 8;


        private List<IPhysikObject> _gameObjects;
        private float worldsize;
        private BoundingBox bound;
        private int depth;

        private Vector3 Center;

        public List<IPhysikObject> gameObjects { get { return _gameObjects; } }

        private Octree[] ChildTrees;

        public Octree(int maxdepth, int depth, float worldsize, Vector3 center)
        {
            _gameObjects = new List<IPhysikObject>();

            bound = new BoundingBox(new Vector3(-worldsize / 2.0f), new Vector3(worldsize / 2.0f));

            this.worldsize = worldsize;
            this.depth = depth;
            Center = center;

            if (depth < maxdepth)
            {
                this.Split(maxdepth);
            }
        }


        public Octree Add(IPhysikObject obj)
        {
            if (ChildTrees != null)
            {
                foreach (var childTree in ChildTrees)
                {
                    if (childTree.bound.Contains(obj.Position) == ContainmentType.Contains)
                    {
                        return childTree.Add(obj);
                    }
                }
            }
            _gameObjects.Add(obj);
            return this;
        }

        public List<Octree> getContainingOctrees(IPhysikObject o)
        {
            var returnedtrees = new List<Octree>();
            if(this.ChildTrees != null)
            {
                foreach (var childTree in ChildTrees)
                {
                    returnedtrees.AddRange(childTree.getContainingOctrees(o));
                }
            }
            else
            {
                if (this.ContainsOrIntersects(o))
                {
                    returnedtrees.Add(this);
                }
            }
            return returnedtrees;
        }

        private bool ContainsOrIntersects(IPhysikObject o)
        {
            switch (bound.Contains(o.Position))
            {
                case ContainmentType.Contains:
                    return true;
                case ContainmentType.Intersects:
                    return true;
            }
            return false;
        }

        private bool Contains(IPhysikObject o)
        {
            if (this.bound.Contains(o.Position) == ContainmentType.Contains)
                return true;
            return false;
        }

        public bool Remove(IPhysikObject o)
        {
            if (ChildTrees != null)
            {
                foreach (var childTree in ChildTrees)
                {
                    var removed = childTree.Remove(o);
                    if (removed)
                    {
                        return true;
                    }
                }
            }
            else
            {
                return gameObjects.Remove(o);
            }
            return false;
        }

        public List<IPhysikObject> Update()
        {
            List<IPhysikObject> changedObjects = new List<IPhysikObject>();
            if (ChildTrees != null)
            {
                foreach (var childTree in ChildTrees)
                {
                    changedObjects.AddRange(childTree.Update());
                }
            }
            else
            {
                foreach (var o in gameObjects)
                {
                    if (!Contains(o))
                    {
                        Remove(o);
                        changedObjects.Add(o);
                    }
                }
                return changedObjects;
            }
            return changedObjects;
        }

        private void Split(int maxdepth)
        {
            this.ChildTrees = new Octree[CHILDREN];
            int depth = this.depth + 1;
            float quarter = this.worldsize / 4f;

            this.ChildTrees[0] = new Octree(maxdepth, depth, worldsize / 2.0f,
                this.Center + new Vector3(-quarter, quarter, -quarter));
            this.ChildTrees[1] = new Octree(maxdepth, depth, worldsize / 2.0f,
                this.Center + new Vector3(quarter, quarter, -quarter));
            this.ChildTrees[2] = new Octree(maxdepth, depth, worldsize / 2.0f,
                this.Center + new Vector3(-quarter, quarter, quarter));
            this.ChildTrees[3] = new Octree(maxdepth, depth, worldsize / 2.0f,
                this.Center + new Vector3(quarter, quarter, quarter));
            this.ChildTrees[4] = new Octree(maxdepth, depth, worldsize / 2.0f,
                this.Center + new Vector3(-quarter, -quarter, -quarter));
            this.ChildTrees[5] = new Octree(maxdepth, depth, worldsize / 2.0f,
                this.Center + new Vector3(quarter, -quarter, -quarter));
            this.ChildTrees[6] = new Octree(maxdepth, depth, worldsize / 2.0f,
                this.Center + new Vector3(-quarter, -quarter, quarter));
            this.ChildTrees[7] = new Octree(maxdepth, depth, worldsize / 2.0f,
                this.Center + new Vector3(quarter, -quarter, quarter));
        }
    }
}