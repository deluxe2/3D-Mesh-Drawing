using System.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _3DHelper
{
    public class Camera
    {
        private float fovangle;
        private float aspectratio;
        private float near;
        private float far;

        private Vector3 position;
        private Vector3 target;

        private Matrix view;
        private readonly Matrix projection;

        public float CameraSpeed { get; set; }

        public Vector3 Position
        {
            get { return position; }
        }

        public Vector3 Target
        {
            get { return target; }
        }


        public Matrix View
        {
            get { return view; }
        }

        public Matrix Projection
        {
            get { return projection; }
        }

        public Camera(Vector3 position,Vector3 target,float fovangle, float aspectratio, float near, float far, float speed)
        {
            this.fovangle = fovangle;
            this.aspectratio = aspectratio;
            this.near = near;
            this.far = far;

            CameraSpeed = speed;

            this.position = position;
            this.target = target;

            projection = Matrix.CreatePerspectiveFieldOfView(fovangle, aspectratio, near, far);
        }

        public void UpdatePositions(Vector3 position, Vector3 target)
        {
            this.position = position;
            this.target = target;

            view = Matrix.CreateLookAt(position, target, Vector3.Up);
        }

        public void UpdateCamera()
        {

            var state = Keyboard.GetState();


            if (state.IsKeyDown(Keys.F))
            {
                target = Vector3.Zero;
                position = new Vector3(0, 0, 10);
            }

            if (state.IsKeyDown(Keys.LeftShift))
            {
                if (state.IsKeyDown(Keys.W))
                {
                    target = Vector3.Transform(Target - Position,
                        Matrix.CreateRotationX(MathHelper.ToRadians(5)) * Matrix.CreateTranslation(Position));
                }
                if (state.IsKeyDown(Keys.S))
                {
                    target = Vector3.Transform(Target - Position,
                        Matrix.CreateTranslation(Position)*Matrix.CreateRotationX(MathHelper.ToRadians(-5)) );
                }
                if (state.IsKeyDown(Keys.A))
                {
                    target = Vector3.Transform(Target - Position,
                        Matrix.CreateTranslation(Position)*Matrix.CreateRotationY(MathHelper.ToRadians(5)));
                }
                if (state.IsKeyDown(Keys.D))
                {
                    target = Vector3.Transform(Target - Position,
                        Matrix.CreateTranslation(Position)*Matrix.CreateRotationY(MathHelper.ToRadians(-5)));
                }
            }
            else
            {
                if (state.IsKeyDown(Keys.W))
                {
                    MoveCamera(Vector3.Forward);
                }

                if (state.IsKeyDown(Keys.S))
                {
                    MoveCamera(Vector3.Backward);
                }
                if (state.IsKeyDown(Keys.A))
                {
                    MoveCamera(Vector3.Left);
                }
                if (state.IsKeyDown(Keys.D))
                {
                    MoveCamera(Vector3.Right);
                }
                if (state.IsKeyDown(Keys.Q))
                {
                    MoveCamera(Vector3.Up);
                }
                if (state.IsKeyDown(Keys.E))
                {
                    MoveCamera(Vector3.Down);
                }
            }

        }

        private void MoveCamera(Vector3 direction)
        {
            target += direction*CameraSpeed;
            position += target;
        }
    }
}