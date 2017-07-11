using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using _3DHelper;

namespace _3D_Stoesse
{
    public class Ball:IPhysikObject
    {

        public Vector3 Position { get; set; }
        public Vector3 Speed { get; set; }
        public BoundingSphere Bound  => new BoundingSphere(Position,Model.Meshes[0].BoundingSphere.Radius * Mass);
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
