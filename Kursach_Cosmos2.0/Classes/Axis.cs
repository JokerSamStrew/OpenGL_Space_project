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
    class Axis
    {
        private double m_length;
        public float lineWidth { get; set; }

        public Axis(double line_length)
        {
            m_length = line_length;
        }

        public void Draw()
        {
            GL.Disable(EnableCap.Light0);
            GL.Disable(EnableCap.Lighting);
            

            GL.LineWidth(lineWidth);
            GL.Begin(BeginMode.Lines);//начинать рисовать линии
                                      // ось x

            GL.Color3(1f, 1, 1); GL.Vertex2(-m_length, 0);//начальная точка отрезка x y
            GL.Color3(0f, 0, 1); GL.Vertex2(m_length, 0);//конечная точка отрезка  x y
                                                  // ось y
            GL.Color3(1f, 1, 1); GL.Vertex2(0, -m_length); //начальная точка отрезка  x y
            GL.Color3(0f, 1, 0); GL.Vertex2(0, m_length); //конечная точка отрезка  x y
                                                   // ось z
            GL.Color3(1f, 1, 1); GL.Vertex3(0, 0, -m_length); //начальная точка отрезка  x y z
            GL.Color3(1f, 0, 0); GL.Vertex3(0, 0, m_length); //конечная точка отрезка  x y z
            GL.LineWidth(1);

            GL.End();

            
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);
        }



    }
}
