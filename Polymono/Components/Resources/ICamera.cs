using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using System.Threading.Tasks;

namespace Polymono.Components.Resources
{
    public interface ICamera
    {
        Vector3 Position { get; set; }

        Vector3 Front { get; }
        Vector3 Up { get; }
        Vector3 Right { get; }

        float AspectRatio { set; }
        float Fov { get; set; }
        float Pitch { get; set; }
        float Yaw { get; set; }

        Matrix4 GetProjectionMatrix();
        Matrix4 GetViewMatrix();
        Task Update(PolyFrameEventArgs e);
        Task Resize(ResizeEventArgs e);
        Task MouseWheel(MouseWheelEventArgs e);
    }
}