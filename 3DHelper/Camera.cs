﻿using System;
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

        public Vector3 Position => position;

        public Vector3 Target => target;


        public Matrix View
            =>
                Matrix.CreateLookAt(position, target,
                    Vector3.Transform(Vector3.Up, Matrix.CreateFromQuaternion(rotation)));

        public Matrix Projection => projection;

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
                target = Vector3.Forward;
                position = new Vector3(0, 0, 10);
            }

            if (state.IsKeyDown(Keys.LeftShift))
            {
                if (state.IsKeyDown(Keys.W))
                {
                    pitch += MathHelper.ToRadians(CameraSpeed / 60);
                }
                if (state.IsKeyDown(Keys.S))
                {
                    pitch -= MathHelper.ToRadians(CameraSpeed / 60);
                }
                if (state.IsKeyDown(Keys.A))
                {
                    yaw += MathHelper.ToRadians(CameraSpeed / 60);
                }
                if (state.IsKeyDown(Keys.D))
                {
                    yaw -= MathHelper.ToRadians(CameraSpeed / 60);
                }
                rotation = Quaternion.CreateFromYawPitchRoll(yaw, pitch, 0.0f);
                target = Vector3.Transform(Vector3.Forward,
                    Matrix.CreateFromQuaternion(rotation) * Matrix.CreateTranslation(position));
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

        public void Draw(SpriteBatch batch, SpriteFont font,Texture2D texture, Vector2 position)
        {
            batch.Begin();
            batch.DrawString(font, position.ToString(), new Vector2(10, 10), Color.White);
            batch.Draw(texture,position,Color.White);
            batch.End();
        }

        private void MoveCamera(Vector3 direction)
        {
            var add = Vector3.Transform(direction, Matrix.CreateFromQuaternion(rotation)) * CameraSpeed;
            position += add;
            target += add;
        }
    }
}