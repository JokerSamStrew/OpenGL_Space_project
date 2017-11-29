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
        private const string m_controls_path = "Control.bmp";

        private SpaceObject m_moon;
        private SpaceObject m_planet;
        private Sphere m_sun;
        private Sphere m_sky;
        private Camera m_camera;
        private const double radius_camera_lock = 35f;
        private int m_texture_controls_id;
        private const int viewport_param = 2000;
        private bool show_controls_picture;

        enum Target{ PLANET, SUN, MOON }
        private Target target_choice;
        private bool show_axis_orbits = true;

        // Определяют положение планеты и ее спутника
        private const int nx = 20;
        private double x1 = 0, z1 = 0; 
        private double ix2 = 0, ix1 = 0;
        private double x2 = 0, z2 = 0;
        private double AngleX1 = 0f, AngleX2 = 0f;



        void showPicture(double x, double y, int texture_id, double size)
        {
            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.Light0);
            GL.Disable(EnableCap.Lighting);
            

            GL.PushMatrix();
            GL.LoadIdentity();
            GL.MatrixMode(MatrixMode.Projection);

            GL.PushMatrix();
            GL.LoadIdentity();

            GL.BindTexture(TextureTarget.Texture2D, texture_id);
            GL.Enable(EnableCap.Texture2D);

            OpenTK.Graphics.Glu.Ortho2D(0, viewport_param, viewport_param, 0);
            GL.Begin(BeginMode.QuadStrip);

            GL.Color3(1f, 1f, 1f);
            GL.TexCoord2(0, 0);
            GL.Vertex2(x, y);



            GL.TexCoord2(1, 0);
            GL.Vertex2(size + x, y);


            GL.TexCoord2(0, 1);
            GL.Vertex2(x, size + y);

            GL.TexCoord2(1, 1);
            GL.Vertex2(size + x, size + y);

            GL.End();

            GL.MatrixMode(MatrixMode.Projection);
            GL.PopMatrix();
            GL.MatrixMode(MatrixMode.Modelview);
            GL.PopMatrix();

            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);
            
        }

        public ProjectWindow()
                    // set window resolution, title, and default behaviour
                    : base(1370, 720, OpenTK.Graphics.GraphicsMode.Default, "Курсовая работа - Лабзов Семен",
                          OpenTK.GameWindowFlags.Default, OpenTK.DisplayDevice.Default,
                          // ask for an OpenGL 3.0 forward compatible context
                          3, 0, OpenTK.Graphics.GraphicsContextFlags.ForwardCompatible)
                {
            System.Console.WriteLine("gl version: " + GL.GetString(StringName.Version));

        }

        protected override void OnResize(EventArgs e)
        { }
        

        protected override void OnLoad(EventArgs e)
        {
            show_controls_picture = true;
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

           
           
            
            m_sky = new Sphere(40f, 60, m_sky_path);
            m_sun = new Sphere(2f, 40, m_sun_path);

            m_moon = new SpaceObject(0.07f, 20, new Vector3d(0f, 0f, 0f), m_moon_path, true);

            m_planet = new SpaceObject(0.3f, 20, new Vector3d(0f, 1f, 1f), m_planet_path, true);
            m_planet.m_orbit_radius = 4;

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);

            m_texture_controls_id = GLTexture.LoadTextureTest(m_controls_path);
            m_camera = new Camera(new Vector3(0, 0, 0));
            m_camera.show_picture = true;
            m_camera.texture_id = m_texture_controls_id;

            Matrix4 pres = Matrix4.CreatePerspectiveFieldOfView((float)(60 * Math.PI / 180), 1, 0.2f, 5000);


            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref pres);

            GL.Viewport(-300,
                 -600,
                 viewport_param,
                 viewport_param);

        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {

            // SetLight
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);
            GL.Light(LightName.Light0, LightParameter.Diffuse, new float[] { 0.8f, 0.6f, 0.6f, 1f });
            GL.Light(LightName.Light0, LightParameter.Position, new float[] { 0f, 0f, 0f, 1f });
            GL.Light(LightName.Light0, LightParameter.Ambient, new float[] { 0.5f, 0.2f, 1f, 0f });

           

           

            if (target_choice == Target.MOON) {
                m_camera.target = new Vector3((float)(x1 + x2), 0, (float)(z1 + z2));
            } else if (target_choice == Target.PLANET) {
                m_camera.target = new Vector3((float)x1, 0, (float)z1);
            } else if (target_choice == Target.SUN) {
                m_camera.target = new Vector3(0, 0, 0);
            }

            GL.ClearColor(System.Drawing.Color.DeepSkyBlue);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            //Draw Sky
            GL.PushMatrix();
            GL.Rotate(90, new Vector3d(1f, 0f, 0f));
            m_sky.DrawReverseNormal();
            GL.PopMatrix();

            // Draw Sun
            m_sun.DrawReverseNormal();

            // Draw planet(textured sphere) whith orbit
            x1 = m_planet.m_orbit_radius * Math.Cos(2 * ix1 * Math.PI / nx);
            z1 = m_planet.m_orbit_radius * Math.Sin(2 * ix1 * Math.PI / nx);


            m_planet.show_axis = show_axis_orbits;
            m_planet.show_orbit = show_axis_orbits;
            m_planet.Draw(new Vector3d(x1, 0, z1), new Vector3d(1, 0, 1), AngleX1);

            x2 = m_moon.m_orbit_radius * Math.Cos(2 * ix2 * Math.PI / nx);
            z2 = m_moon.m_orbit_radius * Math.Sin(2 * ix2 * Math.PI / nx);

            // Draw moon(textured sphere) whith orbit 
            m_moon.m_orbit_translation = new Vector3d(x1, 0, z1);


            m_moon.show_orbit = show_axis_orbits;
            m_moon.show_axis = show_axis_orbits;
            if (show_axis_orbits)
            {
                m_moon.DrawOrbit();
            }
            GL.PushMatrix();
            // Вращение вокруг планеты
            GL.Translate(x2, 0, z2);
            GL.Rotate(1, 0, 1, AngleX2);
            // Вращение вокруг солнца
            m_moon.Draw(new Vector3d(x1, 0, z1), new Vector3d(1, 0, 1), AngleX2);
            GL.PopMatrix();

         

            ix1 += 0.1f / m_planet.m_orbit_radius;
            ix2 += 0.05f / m_moon.m_orbit_radius;
            AngleX1 -= 0.1;
            AngleX2 -= 1;

            m_camera.Move();
            m_camera.Angle_Alpha += 0.01f;

            if(show_controls_picture)
            {
                showPicture(300, 675, m_texture_controls_id, 400);
            }

            GL.Disable(EnableCap.Light0);
            GL.Disable(EnableCap.Lighting);
            GL.Flush();//информирует библиотеку OpenGL о том, что все параметры изображения были переданы
            GL.Finish();//приостанавливает работу программы до завершения формирования изображения библиотекой OpenGL
            
            this.SwapBuffers();
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            switch (e.Key)
            {
                // увеличение расстояние камеры от объекта
                case Key.W: m_camera.radius += m_camera.radius < radius_camera_lock ? 0.5f : 0f;
                            m_camera.Move();
                            break;
                // уменьшение расстояния камеры от объекта
                case Key.S: m_camera.radius -= m_camera.radius > 0 ? 0.5f : 0f ; 
                            m_camera.Move();
                            break;
                // увеличение угла Alpha
                case Key.Q: m_camera.Angle_Alpha += 1f;
                            m_camera.Move();
                            break;
                // уменьшение угла Alpha
                case Key.A: m_camera.Angle_Alpha -= 1f;
                            m_camera.Move();
                            break;
                // увеличение угла Beta
                case Key.E: m_camera.Angle_Beta += 1f;
                            m_camera.Move();
                            break;
                // уменьшение угла Beta
                case Key.D: m_camera.Angle_Beta -= 1f;
                            m_camera.Move();
                            break;
                
                // Выбор объекта наблюдение
                case Key.F1: target_choice = Target.SUN; break;
                case Key.F2: target_choice = Target.PLANET; break;
                case Key.F3: target_choice = Target.MOON; break;
                
                // Показать орибиты и оси координат
                case Key.F4: show_axis_orbits = !show_axis_orbits;
                             break;
                case Key.F5: show_controls_picture = !show_controls_picture;
                             break;

            }
            

        }
    }
}
