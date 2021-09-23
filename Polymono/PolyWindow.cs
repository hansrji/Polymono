using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Threading;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Polymono.Managers;
using Polymono.Systems;
using Polymono.Systems.Resources;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Polymono
{
    public delegate void KeyEvent(KeyboardKeyEventArgs e);
    public delegate void ClickEvent(MouseButtonEventArgs e, MouseState state);
    public delegate void ResizeEvent(ResizeEventArgs e);

    class PolyWindow : GameWindow, IPolySystems
    {
        public KeyEvent KeyPressed = delegate
        {
            Debug.WriteLine($"{Util.ThreadID}: OnKeyDown triggered.");
        };
        public ClickEvent ClickPressed = delegate
        {
            Debug.WriteLine($"{Util.ThreadID}: OnMouseDown triggered.");
        };
        public ResizeEvent ResizeEvent = delegate { };

        public ulong UpdateCount { get; set; } = 0;
        public ulong RenderCount { get; set; } = 0;

        #region Update/Render timers
        private readonly ulong updateFreq = 60ul;
        private readonly ulong renderFreq = 60ul;
        private readonly double[] updateTimes = default;
        private readonly double[] renderTimes = default;
        private double updateTotalTime = 0d;
        private double renderTotalTime = 0d;
        #endregion

        public IParallelRunner Runner { get; set; }

        // Create public list of systems
        public SequentialSystem<PolyFrameEventArgs> UpdateSystems { get; set; }
        public SequentialSystem<double> DrawSystems { get; set; }

        public PolyWindow(GameWindowSettings gameSettings, NativeWindowSettings nativeSettings)
            : base(gameSettings, nativeSettings)
        {
            Debug.WriteLine($"{Util.ThreadID}: PolyWindow ctor");
            Title = "";
            updateFreq = Convert.ToUInt64(UpdateFrequency);
            renderFreq = Convert.ToUInt64(RenderFrequency);
            updateTimes = new double[updateFreq];
            renderTimes = new double[renderFreq];
        }

        protected override void OnLoad()
        {
            Debug.WriteLine($"{Util.ThreadID}: PolyWindow load");
            //Debug.WriteLine("PolyWindow: Load init.");
            GL.ClearColor(Color4.MediumPurple);

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.DebugOutput);
            GL.Enable(EnableCap.DebugOutputSynchronous);
            GL.DebugMessageCallback((DebugSource source, DebugType type, int id,
                DebugSeverity severity, int length, IntPtr message, IntPtr userParam) =>
            {
                switch (type)
                {
                    case DebugType.DebugTypeError:
                        Debug.WriteLine($"OpenGL error:{Util.NL + Util.Tab}ID: {id + Util.NL + Util.Tab}Message: {Marshal.PtrToStringAnsi(message, length)}");
                        Debug.WriteLine($"Callstack: {Environment.StackTrace}");
                        break;
                    case DebugType.DebugTypeDeprecatedBehavior:
                    case DebugType.DebugTypeUndefinedBehavior:
                    case DebugType.DebugTypePortability:
                    case DebugType.DebugTypePerformance:
                    case DebugType.DebugTypeOther:
                    case DebugType.DebugTypeMarker:
                    case DebugType.DebugTypePushGroup:
                    case DebugType.DebugTypePopGroup:
                    default:
                        Debug.WriteLine($"OpenGL debug:{Util.NL + Util.Tab}ID: {id + Util.NL + Util.Tab}Message: {Marshal.PtrToStringAnsi(message, length)}");
                        break;
                }
            }, IntPtr.Zero);
            // Setup systems.
            World world = new();
            TextureManager textureManager = new();
            textureManager.Manage(world);
            ShaderManager<Shader> shaderManager = new();
            shaderManager.Manage(world);
            ModelManager<Model> modelManager = new();
            modelManager.Manage(world);
            Runner = new DefaultParallelRunner(Environment.ProcessorCount);
            UpdateSystems = new SequentialSystem<PolyFrameEventArgs>(
                new InputSystem(world, Runner, ref KeyPressed),
                new UIInteractionSystem(world, Runner, ref ClickPressed),
                new GameSystem(world, Close),
                new CameraView(world, Runner),
                new WindowResizeSystem(world, Runner),
                new SimpleMovement(world, Runner),
                new PreDrawSystem(world, Runner));
            DrawSystems = new SequentialSystem<double>(
                new DrawSystem(world));
            
            // We make the mouse cursor invisible and captured so we can have proper FPS-camera movement
            CursorGrabbed = true;
            KeyPressed += (KeyboardKeyEventArgs e) =>
            {
                if (e.Key == Keys.Escape)
                    Close();
                if (e.Key == Keys.R)
                {
                    Focus();
                    CursorGrabbed = true;
                    Debug.WriteLine($"{Util.ThreadID}: Grabbed: {CursorGrabbed} Visible: {CursorVisible}");
                }
                if (e.Key == Keys.T)
                {
                    Focus();
                    CursorGrabbed = false;
                    CursorVisible = true;
                    Debug.WriteLine($"{Util.ThreadID}: Grabbed: {CursorGrabbed} Visible: {CursorVisible}");
                }
            };
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            //Debug.WriteLine("PolyWindow: Update init.");

            UpdateTimer();
            UpdateTitle();

            //if (!IsFocused) // check to see if the window is focused
            //    return;

            // Call update event.
            PolyFrameEventArgs state = new(e.Time, Size, KeyboardState, MouseState, IsFocused, CursorGrabbed);
            UpdateSystems.Update(state);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            //Debug.WriteLine("PolyWindow: Render init.");

            UpdateRenderTimer();

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Call render event.
            DrawSystems.Update(e.Time);

            SwapBuffers();
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {

        }

        protected override void OnResize(ResizeEventArgs e)
        {
            ResizeEvent(e);
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            KeyPressed(e);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            ClickPressed(e, MouseState);
        }

        protected override void OnUnload()
        {

        }

        private void UpdateTitle()
        {
            string title = $"Update: {UpdateCount} Render: {RenderCount} U Freq: {UpdateFrequency} U Time: {updateTotalTime:N3} R Time: {renderTotalTime:N3}";
            if (IsRunningSlowly)
                Title = $"[X] {CursorGrabbed} " + title;
            else
                Title = $"[/] {CursorGrabbed} " + title;
        }

        private void UpdateTimer()
        {
            updateTotalTime = 0d;
            updateTimes[UpdateCount % updateFreq] = UpdateTime;
            // Calculate all values in array.
            for (int i = 0; i < updateTimes.Length; i++)
            {
                updateTotalTime += updateTimes[i];
            }
            UpdateCount++;
        }

        private void UpdateRenderTimer()
        {
            renderTotalTime = 0d;
            renderTimes[RenderCount % renderFreq] = RenderTime;
            // Calculate all values in array.
            for (int i = 0; i < renderTimes.Length; i++)
            {
                renderTotalTime += renderTimes[i];
            }
            RenderCount++;
        }
    }
}
/*
ICamera camera = new Camera(this, Vector3.UnitZ * 3, Size.X / (float)Size.Y);
IShader shader = new Shader(this, "Shaders/shader.vert", "Shaders/shader.frag");
_ = new Volume(this, ref shader, ref camera);
_ = new Volume(this, ref shader, ref camera)
{
    Texture = Texture.LoadFromFile("Resources/MinaDoubt.png"),
    Model = Matrix4.CreateScale(0.5f) * Matrix4.CreateTranslation(new Vector3(5f, 0f, 0f))
};
_ = new ViewVolume(this, ref shader, ref camera)
{
    Model = Matrix4.CreateTranslation(new Vector3(-0.5f, 0f, 0f))
};
IShader ttfShader = new Shader(this, "Shaders/ttf.vert", "Shaders/ttf.frag");
FreeTypeFont font = new(this, ref ttfShader, 32);
*/