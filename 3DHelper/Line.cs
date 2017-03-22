using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3DHelper
{
    public class Line
    {
        private Vector3 A;
        private Vector3 B;

        private VertexBuffer vertexBuffer;
        private GraphicsDevice device;

        private BasicEffect basicEffect;
        private Matrix world;

        public Line(Vector3 A, Vector3 B, GraphicsDevice device)
        {
            this.A = A;
            this.B = B;
            this.device = device;
            world = Matrix.CreateTranslation(0, 0, 0);
            basicEffect = new BasicEffect(device);

            VertexPositionColor[] vertices = new VertexPositionColor[2];
            vertices[0] = new VertexPositionColor(this.A, Color.White);
            vertices[1] = new VertexPositionColor(this.B, Color.White);


            vertexBuffer = new VertexBuffer(device, typeof(VertexPositionColor), 2, BufferUsage.WriteOnly);
            vertexBuffer.SetData(vertices);
        }

        public void Draw(Camera camera)
        {
            basicEffect.World = world;
            basicEffect.View = camera.View;
            basicEffect.Projection = camera.Projection;
            basicEffect.VertexColorEnabled = true;

            device.SetVertexBuffer(vertexBuffer);

            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            device.RasterizerState = rasterizerState;

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.DrawPrimitives(PrimitiveType.LineStrip, 0, 1);
            }
        }


    }
}