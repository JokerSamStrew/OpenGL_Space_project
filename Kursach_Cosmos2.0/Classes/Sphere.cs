using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Kursach_Cosmos2._0.Classes;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;

namespace Kursach_Cosmos2._0.Classes
{


    class Sphere
    {
        private double m_radius { get; }
        private int m_detalization { get; set; }
        
        private string m_texture_path;
        private System.Drawing.Bitmap m_texture;
        private int m_texture_id;
     

        public Sphere(double r, int det, string texture_path)
        {
            m_radius = r;
            m_detalization = det;
            m_texture_path = texture_path;
            //m_texture = new System.Drawing.Bitmap(m_texture_path);
            m_texture_id = GLTexture.LoadTextureTest(m_texture_path);
        }

        public void Draw()
        {
            GL.BindTexture(TextureTarget.Texture2D, m_texture_id);
            GL.Enable(EnableCap.Texture2D);
            //GL.Enable(EnableCap.Lighting);
            //GL.Enable(EnableCap.Light1);
            //GL.Light(LightName.Light1, LightParameter.Position, new float[] { -2f, -2f, -20f, 1f });  //позиции света
            //GL.Light(LightName.Light1, LightParameter.Diffuse, new float[] { 1f, 1f, 1f, 1f });
            //GL.Light(LightName.Light1, LightParameter.Ambient, new float[] { 1f, 1f, 1f, 1f});
            
            double x, y, z;
            for (int iy = 0; iy < m_detalization; ++iy)
            {
                GL.Begin(BeginMode.QuadStrip);
               
                for (int ix = 0; ix <= m_detalization; ++ix)
                {
                    x = m_radius * Math.Sin(iy * Math.PI / m_detalization) * Math.Cos(2 * ix * Math.PI / m_detalization);
                    y = m_radius * Math.Sin(iy * Math.PI / m_detalization) * Math.Sin(2 * ix * Math.PI / m_detalization);
                    z = m_radius * Math.Cos(iy * Math.PI / m_detalization);


                    GL.Normal3(x, y, z);//нормаль направлена от центра
                    GL.TexCoord2((double)ix / (double)m_detalization, (double)iy / (double)m_detalization);

                    GL.Vertex3(x, y, z);

                    x = m_radius * Math.Sin((iy + 1) * Math.PI / m_detalization) * Math.Cos(2 * ix * Math.PI / m_detalization);
                    y = m_radius * Math.Sin((iy + 1) * Math.PI / m_detalization) * Math.Sin(2 * ix * Math.PI / m_detalization);
                    z = m_radius * Math.Cos((iy + 1) * Math.PI / m_detalization);

                    GL.Normal3(x, y, z);
                    GL.TexCoord2((double)ix / (double)m_detalization, (double)(iy + 1) / (double)m_detalization);



                    GL.Vertex3(x, y, z);
                }
                GL.End();
            }
           
            //GL.Disable(EnableCap.Light1);
            //GL.Disable(EnableCap.Lighting);
        }

        public void DrawReverseNormal()
        {
            GL.BindTexture(TextureTarget.Texture2D, m_texture_id);
            //GLTexture.LoadTexture(m_texture);
            GL.Enable(EnableCap.Texture2D);
            //GL.Enable(EnableCap.Lighting);
            //GL.Enable(EnableCap.Light1);
            //GL.Light(LightName.Light1, LightParameter.Position, new float[] { -2f, -2f, -20f, 1f });  //позиции света
            //GL.Light(LightName.Light1, LightParameter.Diffuse, new float[] { 1f, 1f, 1f, 1f });
            //GL.Light(LightName.Light1, LightParameter.Ambient, new float[] { 1f, 1f, 1f, 1f});

            double x, y, z;
            for (int iy = 0; iy < m_detalization; ++iy)
            {
                GL.Begin(BeginMode.QuadStrip);

                for (int ix = 0; ix <= m_detalization; ++ix)
                {
                    x = m_radius * Math.Sin(iy * Math.PI / m_detalization) * Math.Cos(2 * ix * Math.PI / m_detalization);
                    y = m_radius * Math.Sin(iy * Math.PI / m_detalization) * Math.Sin(2 * ix * Math.PI / m_detalization);
                    z = m_radius * Math.Cos(iy * Math.PI / m_detalization);


                    GL.Normal3(-x, -y, -z);//нормаль направлена от центра
                    GL.TexCoord2((double)ix / (double)m_detalization, (double)iy / (double)m_detalization);

                    GL.Vertex3(x, y, z);

                    x = m_radius * Math.Sin((iy + 1) * Math.PI / m_detalization) * Math.Cos(2 * ix * Math.PI / m_detalization);
                    y = m_radius * Math.Sin((iy + 1) * Math.PI / m_detalization) * Math.Sin(2 * ix * Math.PI / m_detalization);
                    z = m_radius * Math.Cos((iy + 1) * Math.PI / m_detalization);

                    GL.Normal3(-x, -y, -z);
                    GL.TexCoord2((double)ix / (double)m_detalization, (double)(iy + 1) / (double)m_detalization);



                    GL.Vertex3(x, y, z);
                }
                GL.End();
            }
            //GL.Disable(EnableCap.Light1);
            //GL.Disable(EnableCap.Lighting);
        }
    }
}
