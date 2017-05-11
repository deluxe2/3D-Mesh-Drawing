using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OpenTK.Graphics;
using GL = OpenTK.Graphics.OpenGL.GL;
using ShaderType = OpenTK.Graphics.OpenGL.ShaderType;
using StringName = OpenTK.Graphics.OpenGL.StringName;
using Vector3 = Microsoft.Xna.Framework.Vector3;

namespace _3DHelper
{
    public class PhysikEngine
    {
        private List<IPhysikObject> objects;

        //private Octree oct;

        private IWorld World;

        public float Gravity { get; set; }

        private bool Gpu;
        private int computeobject;
        private int program;
        private GraphicsContext context;

        public PhysikEngine(float gravity, IWorld world, bool gpu)
        {
            Gravity = gravity;
            Gpu = gpu;
            objects = new List<IPhysikObject>();
            World = world;
            //oct = new Octree(4, 0, world.CubeSize, Vector3.Zero);
            if (gpu)
            {
                var window = new OpenTK.GameWindow();
                context = new GraphicsContext(GraphicsMode.Default, window.WindowInfo);
                Version version = new Version(GL.GetString(StringName.Version).Substring(0, 3));
                Version target = new Version(4, 3);
                if (version < target)
                {
                    throw new NotSupportedException(String.Format("OpenGL {0} is required (you only have {1}).", target,
                        version));
                }
                using (var stream = new StreamReader("Shader/collision.glsl"))
                {
                    CreateShaders(stream.ReadToEnd(),out computeobject, out program);
                }
            }
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

            //var add = oct.Update();
            //foreach (var physikObject in add)
            //{
            //    oct.Add(physikObject);
            //}

            for (int j = 0; j < objects.Count; j++)
            {
                for (int i = j + 1; i < objects.Count; i++)
                {
                    if (objects[i].Bound.Intersects(objects[j].Bound))
                    {
                        Collision(objects[i], objects[j]);
                    }
                }
                World.WorldCollision(objects[j]);
            }
        }

        private void Collision(IPhysikObject obj1, IPhysikObject obj2)
        {
            if (!obj1.Equals(obj2))
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

                var rotationmatrix2 = Matrix.CreateRotationZ(angleZ)*Matrix.CreateRotationY(angleY) ;

                obj1.Speed = Vector3.Transform(new Vector3(v1, v1Transformed.Y, v1Transformed.Z) * obj1.Elasticity,
                    rotationmatrix2);
                obj2.Speed = Vector3.Transform(new Vector3(v2, v2Transformed.Y, v2Transformed.Z) * obj2.Elasticity,
                    rotationmatrix2);

                float radidiff = obj1.Bound.Radius + obj2.Bound.Radius - vMittelpunkt.Length();

                obj1.Position += Vector3.Transform(new Vector3(-radidiff / 2.0f, 0, 0), rotationmatrix2);
                obj2.Position += Vector3.Transform(new Vector3(radidiff / 2.0f, 0, 0), rotationmatrix2);

                if (vMittelpunkt.Length() > (obj1.Position - obj2.Position).Length())
                {
                    obj1.Position += Vector3.Transform(new Vector3(radidiff, 0, 0), rotationmatrix2);
                    obj2.Position += Vector3.Transform(new Vector3(-radidiff, 0, 0), rotationmatrix2);
                }
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

        void CreateShaders(string cs, out int computeObject, out int program)
        {
            int status_code;
            string info;

            computeObject = GL.CreateShader(ShaderType.ComputeShader);

            // Compile compute shader
            GL.ShaderSource(computeObject, cs);
            GL.CompileShader(computeObject);
            GL.GetShaderInfoLog(computeObject, out info);
            GL.GetShader(computeObject, OpenTK.Graphics.OpenGL.ShaderParameter.CompileStatus, out status_code);

            if (status_code != 1)
                throw new ApplicationException(info);

            program = GL.CreateProgram();
            GL.AttachShader(program, computeObject);

            GL.DeleteShader(computeObject);

            GL.LinkProgram(program);
            GL.UseProgram(program);
        }
    }
}