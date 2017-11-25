using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Kursach_Cosmos2._0.Classes;

namespace Kursach_Cosmos2._0.Classes
{
    class Orbit
    {
        public double m_radius { set; get; }
        public System.Drawing.Color m_color { get; set; }
        public int m_lineWidth { get; set; }

        public Orbit(double rad, System.Drawing.Color color)
        {
            m_radius = rad;
            m_color = color;
            m_lineWidth = 2;
        }

        public void Draw()
        {
            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.Light0);
            GL.Disable(EnableCap.Lighting);
            double x = 0, y = 0, z = 0;
            int nx = 60;

            GL.LineWidth(m_lineWidth);
            GL.Begin(BeginMode.LineLoop);

            for (int ix = 0; ix <= nx; ++ix)
            {
                x = m_radius * Math.Cos(2 * ix * Math.PI / nx);
                z = m_radius * Math.Sin(2 * ix * Math.PI / nx);

                GL.Color3(m_color);
          
                GL.Vertex3(x, y, z);
            }
            
            GL.End();
            GL.LineWidth(1);
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);

            GL.Enable(EnableCap.Texture2D);
        }

        public void Draw(Vector3d location)
        {
            GL.PushMatrix();
            GL.Translate(location);
            
            this.Draw();

            GL.PopMatrix();
        }
    }
}
