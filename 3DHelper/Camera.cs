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
    }
}