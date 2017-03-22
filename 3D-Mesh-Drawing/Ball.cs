﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using _3DHelper;

namespace _3D_Mesh_Drawing
{
    public class Ball:IPhysikObject
    {

        public Vector3 Position { get; set; }
        public Vector3 Speed { get; set; }
        public BoundingSphere Bound  => new BoundingSphere(Position,Mass);
        public bool Static { get; set; }
        public float Mass { get; }
        public float Elasticity { get; }
        public Model Model { get; }

        public Matrix Translation =>  Matrix.CreateScale(Mass)* Matrix.CreateTranslation(Position);



        public Ball(Model model, Vector3 position, Vector3 speed, bool stat,float elasticity, float mass)
        {
            Model = model;
            Position = position;
            Speed = speed;
            Static = stat;
            Elasticity = elasticity;
            Mass = mass;
        }

    }
}
