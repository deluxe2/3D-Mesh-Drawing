using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace _3DHelper
{
    public static class VectorHelper
    {
        public static Tuple<Matrix,Vector3> GetXRotatedVector(Vector3 vector)
        {
            float angleY = (float)Math.Atan2(vector.Z, vector.X);
            float angleZ = (float)Math.Atan2(vector.Y, vector.X);
            var rotationmatrix = Matrix.CreateRotationY(-angleY) * Matrix.CreateRotationZ(-angleZ);

            return Tuple.Create(rotationmatrix, Vector3.Transform(vector, rotationmatrix));
        }

        public static Tuple<Matrix, Vector3> GetYRotatedVector(Vector3 vector)
        {
            float angleY = (float)Math.Atan2(vector.Z, vector.X);
            float angleZ = (float)Math.Atan2(vector.Y, vector.X);
            var rotationmatrix = Matrix.CreateRotationY(-angleY) * Matrix.CreateRotationZ(-angleZ);

            return Tuple.Create(rotationmatrix, Vector3.Transform(vector, rotationmatrix));
        }

        public static Tuple<Matrix, Vector3> GetZRotatedVector(Vector3 vector)
        {
            float angleY = (float)Math.Atan2(vector.Z, vector.X);
            float angleZ = (float)Math.Atan2(vector.Y, vector.X);
            var rotationmatrix = Matrix.CreateRotationY(-angleY) * Matrix.CreateRotationZ(-angleZ);

            return Tuple.Create(rotationmatrix, Vector3.Transform(vector, rotationmatrix));
        }
    }
}
