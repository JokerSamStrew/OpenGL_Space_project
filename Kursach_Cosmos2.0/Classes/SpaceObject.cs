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
    class SpaceObject
    {
        public double m_orbit_radius { get; set; }
        public Vector3d m_orbit_translation { get; set; }
        private Sphere m_sphere;
        public Orbit m_orbit;
        private Axis m_axis;
        public bool show_orbit { get; set; }
        public bool show_axis { get; set; }



        public SpaceObject(double r, int det, Vector3d coord, string texture_path, bool draw_orbit)
        {
            m_sphere = new Sphere(r, det, texture_path);
            m_axis = new Axis(2 * r);
            show_orbit = draw_orbit;
            show_axis = true;
            m_orbit_radius = 1;
            m_orbit = new Orbit(m_orbit_radius, System.Drawing.Color.White);
            m_orbit_translation = new Vector3d(0, 0, 0);
        }

        public void DrawOrbit()
        {
            show_orbit = false; // Предотвращаем прорисовку в фукнции Draw
            m_orbit.m_radius = m_orbit_radius;
            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            m_orbit.Draw(m_orbit_translation);
            GL.PopMatrix();
        }

        public void Draw(Vector3d translation, Vector3d rotation_coord, double rotation_angle)
        {
            m_orbit.m_radius = m_orbit_radius;
            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            if (show_orbit)
            {
                m_orbit.Draw(m_orbit_translation);
            }

            GL.Translate(translation);
            GL.Rotate(rotation_angle, rotation_coord);

            if (show_axis)
            {
                m_axis.Draw();
            }
            m_sphere.Draw();



            GL.PopMatrix();
        }
        
    }
}
