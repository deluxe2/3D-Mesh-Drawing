using System.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _3DHelper
{
    public class Camera
    {
        private Vector3 position;
        private Vector3 target;

        private float yaw;
        private float pitch;

        private Quaternion rotation;

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


        public Matrix View => Matrix.CreateLookAt(position, target, Vector3.Transform(Vector3.Up, Matrix.CreateFromQuaternion(rotation)));

        public Matrix Projection
        {
            get { return projection; }
        }

        public Camera(Vector3 position, Vector3 target, float fovangle, float aspectratio, float near, float far,
            float speed)
        {
            CameraSpeed = speed;

            this.position = position;
            this.target = position + Vector3.Normalize(target);

            rotation = Quaternion.Identity;

            yaw = 0;
            pitch = 0;

            projection = Matrix.CreatePerspectiveFieldOfView(fovangle, aspectratio, near, far);
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
                    pitch += MathHelper.ToRadians(5.0f/60);
                }
                if (state.IsKeyDown(Keys.S))
                {
                    pitch -= MathHelper.ToRadians(5.0f / 60);
                }
                if (state.IsKeyDown(Keys.A))
                {
                    yaw += MathHelper.ToRadians(5.0f / 60);
                }
                if (state.IsKeyDown(Keys.D))
                {
                    yaw -= MathHelper.ToRadians(5.0f / 60);
                }
                rotation = Quaternion.CreateFromYawPitchRoll(yaw,pitch,0.0f);
                target = Vector3.Transform(Vector3.Forward, Matrix.CreateFromQuaternion(rotation) * Matrix.CreateTranslation(position));
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
            var add = Vector3.Transform(direction, Matrix.CreateFromQuaternion(rotation))*CameraSpeed;
            position += add;
            target += add;
        }
    }
}