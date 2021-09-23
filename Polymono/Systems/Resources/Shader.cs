using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using Polymono.Components.Resources;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Polymono.Systems.Resources
{
    public class Shader : IShader
    {
        public static int CurrentProgram => GL.GetInteger(GetPName.CurrentProgram);
        public int Handle { get; set; }
        public Dictionary<string, int> UniformLocations { get; set; }
        public string VertexPath { get; set; }
        public string FragmentPath { get; set; }

        public void Load()
        {
            Debug.WriteLine($"Reading shader from: [{VertexPath}], [{FragmentPath}]");
            string shaderSource = File.ReadAllText(VertexPath);
            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, shaderSource);
            CompileShader(vertexShader);
            shaderSource = File.ReadAllText(FragmentPath);
            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, shaderSource);
            CompileShader(fragmentShader);
            Handle = GL.CreateProgram();
            Debug.WriteLine($"Shader program: [{Handle}] from: [{VertexPath}], [{FragmentPath}]");
            GL.AttachShader(Handle, vertexShader);
            GL.AttachShader(Handle, fragmentShader);
            LinkProgram(Handle);
            GL.DetachShader(Handle, vertexShader);
            GL.DetachShader(Handle, fragmentShader);
            GL.DeleteShader(fragmentShader);
            GL.DeleteShader(vertexShader);
            GL.GetProgram(Handle, GetProgramParameterName.ActiveUniforms, out int numberOfUniforms);
            UniformLocations = new Dictionary<string, int>();
            for (int i = 0; i < numberOfUniforms; i++)
            {
                string key = GL.GetActiveUniform(Handle, i, out _, out _);
                int location = GL.GetUniformLocation(Handle, key);
                UniformLocations.Add(key, location);
            }
            Debug.WriteLine($"Finished shader[{Handle}] with: [{vertexShader}], [{fragmentShader}]");
        }

        private static void CompileShader(int shader)
        {
            GL.CompileShader(shader);
            GL.GetShader(shader, ShaderParameter.CompileStatus, out int code);
            if (code != (int)All.True)
                throw new Exception($"Error occurred whilst compiling Shader[{shader}].\n\n{GL.GetShaderInfoLog(shader)}");
        }

        private static void LinkProgram(int program)
        {
            GL.LinkProgram(program);
            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out int code);
            if (code != (int)All.True)
                throw new Exception($"Error occurred whilst linking Program[{program}]");
        }

        public void Use() => GL.UseProgram(Handle);

        public void Swap()
        {
            if (Handle != CurrentProgram)
                Use();
        }

        public uint GetAttribLocation(string name) => (uint)GL.GetAttribLocation(Handle, name);

        public void BindAttribLocation(string name, ref uint index) {
            GL.BindAttribLocation(Handle, (int)index, name);
        }

        public void SetInt(string name, int data)
        {
            if (UniformLocations.ContainsKey(name))
                GL.Uniform1(UniformLocations[name], data);
        }

        public void SetUInt(string name, uint data)
        {
            if (UniformLocations.ContainsKey(name))
                GL.Uniform1(UniformLocations[name], data);
        }

        public void SetFloat(string name, float data)
        {
            if (UniformLocations.ContainsKey(name))
                GL.Uniform1(UniformLocations[name], data);
        }

        public void SetVector2(string name, Vector2 data)
        {
            if (UniformLocations.ContainsKey(name))
                GL.Uniform2(UniformLocations[name], data);
        }

        public void SetVector3(string name, Vector3 data)
        {
            if (UniformLocations.ContainsKey(name))
                GL.Uniform3(UniformLocations[name], data);
        }

        public void SetVector4(string name, Vector4 data)
        {
            if (UniformLocations.ContainsKey(name))
                GL.Uniform4(UniformLocations[name], data);
        }

        public void SetVector4(string name, Color4 data)
        {
            if (UniformLocations.ContainsKey(name))
                GL.Uniform4(UniformLocations[name], data);
        }

        public void SetMatrix4(string name, Matrix4 data)
        {
            if (UniformLocations.ContainsKey(name))
                GL.UniformMatrix4(UniformLocations[name], true, ref data);
        }
    }
}
