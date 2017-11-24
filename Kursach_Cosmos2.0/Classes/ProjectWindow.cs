using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Kursach_Cosmos2._0.Classes
{
    class ProjectWindow : OpenTK.GameWindow
    {



        private const string m_planet_path = "MARS.bmp";
        private const string m_moon_path = "MOON.bmp";
        private const string m_sky_path = "nSky2.bmp";
        private const string m_sun_path = "SUN.bmp";
        private SpaceObject m_moon;
        private SpaceObject m_planet;
        private Sphere m_sun;
        private Sphere m_sky;
        private Camera m_camera;

        enum Target{ PLANET, SUN, MOON }
        private Target target_choice;
        


        public ProjectWindow()
                    // set window resolution, title, and default behaviour
                    : base(1000, 720, OpenTK.Graphics.GraphicsMode.Default, "Курсовая работа - Лабзов Семен",
                          OpenTK.GameWindowFlags.Default, OpenTK.DisplayDevice.Default,
                          // ask for an OpenGL 3.0 forward compatible context
                          3, 0, OpenTK.Graphics.GraphicsContextFlags.ForwardCompatible)
                {
            Console.WriteLine("gl version: " + GL.GetString(StringName.Version));
            
           
        }

        protected override void OnResize(EventArgs e)
        {
            
            GL.Viewport(0, -200, 1000, 1000);
        }

        protected override void OnLoad(EventArgs e)
        {
            // this is called when the window starts running

            GL.Enable(EnableCap.Normalize); //нормализаци нормалей, необходимо

            //GL.Enable(EnableCap.Multisample);
       
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha); //Определение функции смешивания
            GL.Enable(EnableCap.Blend); 
            // Сглаживание точек
            GL.Enable(EnableCap.PointSmooth);
            // Сглаживание линий
            GL.Enable(EnableCap.LineSmooth);
            GL.Hint(HintTarget.PointSmoothHint, HintMode.Nicest);
            // Сглаживание полигонов 
            GL.Enable(EnableCap.PolygonSmooth);
            GL.Hint(HintTarget.PolygonSmoothHint, HintMode.Nicest);

            GL.Enable(EnableCap.DepthTest);

            m_camera = new Camera(new Vector3(0, 0, 0));
           
            
            m_sky = new Sphere(150f, 60, m_sky_path);
            m_sun = new Sphere(14f, 40, m_sun_path);

            m_moon = new SpaceObject(0.7f, 20, new Vector3d(0f, 0f, 0f), m_moon_path, true);

            m_planet = new SpaceObject(3f, 20, new Vector3d(0f, 1f, 1f), m_planet_path, true);  
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);

           

            //Matrix4 lookat = Matrix4.LookAt(2, 60, 60, 0, 0, 0, 0.0f, 1.0f, 0.0f);
            Matrix4 pres = Matrix4.CreatePerspectiveFieldOfView((float)(60 * Math.PI / 180), 1, 0.2f, 5000);

            GL.MatrixMode(MatrixMode.Modelview);
            
            OpenTK.Graphics.Glu.LookAt(2, 60, 60, 0, 0, 0, 0.0f, 1.0f, 0.0f);

            

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref pres);
            

           // GL.Light(LightName.Light0, LightParameter.Ambient, new float[] { 0.2f, 0.2f, 0.8f, 0f });
           // GL.Light(LightName.Light0, LightParameter.Specular, new float[] { 1f, 1f, 1f, 1f });
        }

        private double ix1 = 0;
        private int nx = 20;
        private double x1 = 0, z1 = 0;
        private double ix2 = 0;
        private double x2 = 0, z2 = 0;
        private double AngleX1 = 0f, AngleX2 = 0f;

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            // SetLight
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);
            GL.Light(LightName.Light0, LightParameter.Diffuse, new float[] { 1f, 0.6f, 0.6f, 1f });  //позиции света
            GL.Light(LightName.Light0, LightParameter.Position, new float[] { 0f, 0f, 0f, 1f });


            m_camera.Move();
            m_camera.Angle_Alpha += 0.1f;
            if(target_choice == Target.MOON)
            {
                m_camera.target = new Vector3((float)(x1 + x2), 0, (float)(z1 + z2));
            }else if(target_choice == Target.PLANET){
                m_camera.target = new Vector3((float)x1, 0, (float)z1);
            }else if(target_choice == Target.SUN){
                m_camera.target = new Vector3(0, 0, 0);
            }

            GL.ClearColor(System.Drawing.Color.DeepSkyBlue);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);




            //Draw Sky
            GL.PushMatrix();
            GL.Rotate(90, new Vector3d(1f, 0f, 0f));
            m_sky.DrawReverseNormal();
            GL.PopMatrix();

            //m_sky.DrawReverseNormal();

            // Draw Sun
            m_sun.DrawReverseNormal();

            // Draw planet(textured sphere) whith orbit
            m_planet.m_orbit_radius = 40;
            
            x1 = m_planet.m_orbit_radius * Math.Cos(2 * ix1 * Math.PI / nx);
            z1 = m_planet.m_orbit_radius * Math.Sin(2 * ix1 * Math.PI / nx);

         

            //GL.Enable(EnableCap.DepthTest);
            m_planet.Draw(new Vector3d(x1, 0, z1), new Vector3d(1, 0, 1), AngleX1);
            //GL.Disable(EnableCap.DepthTest);

            x2 = m_moon.m_orbit_radius * Math.Cos(2 * ix2 * Math.PI / nx);
            z2 = m_moon.m_orbit_radius * Math.Sin(2 * ix2 * Math.PI / nx);

            // Draw moon(textured sphere) whith orbit 
            
            m_moon.m_orbit_translation = new Vector3d(x1, 0, z1);
            m_moon.DrawOrbit();

            GL.PushMatrix();
            // Вращение вокруг планеты
            GL.Translate(x2, 0, z2);
            GL.Rotate(1, 0, 1, AngleX2);
            // Вращение вокруг солнца
            m_moon.Draw(new Vector3d(x1, 0, z1), new Vector3d(1, 1, 0), AngleX2);
            GL.PopMatrix();

            GL.Disable(EnableCap.Light0);
            GL.Disable(EnableCap.Lighting);
            


            ix1 += 0.1f / m_planet.m_orbit_radius;
            ix2 += 0.2f / m_moon.m_orbit_radius;
            AngleX1 -= 0.1;
            AngleX2 -= 1;

            

            GL.Flush();//информирует библиотеку OpenGL о том, что все параметры изображения были переданы
            GL.Finish();//приостанавливает работу программы до завершения формирования изображения библиотекой OpenGL
         
            this.SwapBuffers();

            
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            const double AngleDl = 1;

            switch (e.Key)
            {
                
     

                case Key.W: m_camera.radius += 1f;
                            m_camera.Move();
                            break;
                case Key.S: m_camera.radius -= m_camera.radius > 0 ? 2f : 0f ; 
                            m_camera.Move();
                            break;
                case Key.Q: m_camera.Angle_Alpha += 1f;
                            m_camera.Move();
                            break;
                case Key.A: m_camera.Angle_Alpha -= m_camera.Angle_Alpha > 0 ? 1f : 0f;
                            m_camera.Move();
                            break;
                case Key.E: m_camera.Angle_Beta += 1f;
                            m_camera.Move();
                            break;
                case Key.D: m_camera.Angle_Beta -= m_camera.Angle_Beta > 0 ? 1f : 0f;
                            m_camera.Move();
                            break;


                case Key.F1: target_choice = Target.SUN; break;//m_camera.target = new Vector3(0, 0, 0); break;
                case Key.F2: target_choice = Target.PLANET; break;//m_camera.target = new Vector3((float)x1, 0, (float)z1); break;
                case Key.F3: target_choice = Target.MOON; break;//m_camera.target = new Vector3((float)x2, 0, (float)z2); break;

          
            }
            

        }
    }
}
