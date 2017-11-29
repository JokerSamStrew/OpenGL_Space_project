using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Kursach_Cosmos2._0.Classes
{
    class Camera
    {
        public double Angle_Alpha, Angle_Beta, radius;
        public Vector3 target { get; set; }
        public int texture_id;
        public bool show_picture;

        public Camera(Vector3 ntarget)
        {
            ntarget = target;
            Angle_Alpha = 1;
            Angle_Beta = 1;
            radius = 35;
            show_picture = false;
            
        }




        public void Move()
        {

            float x = (float) (radius * Math.Sin(Angle_Alpha * Math.PI / 180) * Math.Cos(2 * Angle_Beta * Math.PI / 180))+ target.X;
            float y = (float) (radius * Math.Sin(Angle_Alpha * Math.PI / 180) * Math.Sin(2 * Angle_Beta * Math.PI / 180))+ target.Y;
            float z = (float) (radius * Math.Cos(Angle_Alpha * Math.PI / 180))+ target.Z;

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            OpenTK.Graphics.Glu.LookAt(new Vector3(x , y, z ), target, new Vector3(0, 1, 0) );
         
        }
    }
}
