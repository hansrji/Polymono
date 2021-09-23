using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using Polymono.Components;
using Polymono.Components.Resources;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Polymono.Systems.Resources
{
    class Model : IModel
    {
        public const string POSITION_CODE = "v";
        public const string TEXTURE_CODE = "vt";
        public const string NORMAL_CODE = "vn";
        public const string COLOUR_CODE = "vc";

        public float[] Vertices { get; set; }
        public uint[] Indices { get; set; }
        public int Stride { get; set; }

        public void Load(string path)
        {
            (NodeTypes types, uint faceCount) = HasNodes(path);
            Debug.WriteLine($"[{path}]: Pos[{types.HasPos}], Tex[{types.HasTex}], Nor[{types.HasNor}] Len[{faceCount}]");
            if (types.HasPos && types.HasTex && types.HasNor)
            {
                Debug.WriteLine($"First condition.");
                Task<IList<Vector3>> positionTask = ReadVector3Async(path, POSITION_CODE);
                Task<IList<Vector2>> textureTask = ReadVector2Async(path, TEXTURE_CODE);
                Task<IList<Vector3>> normalTask = ReadVector3Async(path, NORMAL_CODE);
                Task.WaitAll(positionTask, textureTask, normalTask);
                IList<Vector3> positions = positionTask.Result;
                IList<Vector2> textures = textureTask.Result;
                IList<Vector3> normals = normalTask.Result;
                (Vertices, Indices) = ReadFaces(path, in positions, in textures, in normals);
                unsafe
                {
                    Stride = sizeof(Vector3) + sizeof(Vector2) + sizeof(Vector3);
                }
            }
            else if (types.HasPos && types.HasTex)
            {
                Debug.WriteLine($"Second condition.");
                Task<IList<Vector3>> positionTask = ReadVector3Async(path, POSITION_CODE);
                Task<IList<Vector2>> textureTask = ReadVector2Async(path, TEXTURE_CODE);
                Task.WaitAll(positionTask, textureTask);
                IList<Vector3> positions = positionTask.Result;
                IList<Vector2> textures = textureTask.Result;
                (Vertices, Indices) = ReadFaces(path, in positions, in textures);
                unsafe
                {
                    Stride = sizeof(Vector3) + sizeof(Vector2);
                }
            }
            else if (types.HasPos)
            {
                Debug.WriteLine($"Third condition.");
                Task<IList<Vector3>> positionTask = ReadVector3Async(path, POSITION_CODE);
                Task.WaitAll(positionTask);
                IList<Vector3> positions = positionTask.Result;
                (Vertices, Indices) = ReadFaces(path, in positions);
                unsafe
                {
                    Stride = sizeof(Vector3);
                }
            }
        }

        public static (NodeTypes types, uint faceCount) HasNodes(string path)
        {
            // Open file and initialise reader.
            using StreamReader reader = new(new FileStream(path, FileMode.Open, FileAccess.Read));
            string contents = reader.ReadToEnd();
            string[] lines = contents.Split(Util.NL, StringSplitOptions.RemoveEmptyEntries);
            bool hasPos = false, hasTex = false, hasNor = false;
            uint faceCount = 0;
            foreach (string line in lines)
            {
                string[] segments = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (segments.Length < 1)
                    continue;
                switch (segments[0])
                {
                    case "v": // Vertex data
                        hasPos = true;
                        break;
                    case "vt": // Vertex texture data
                        hasTex = true;
                        break;
                    case "vn": // Vertex normal data
                        hasNor = true;
                        break;
                    case "f":
                        faceCount++;
                        break;
                    default:
                        break;
                }
            }
            return (new NodeTypes(hasPos, hasTex, hasNor), faceCount);
        }

        public static async Task<IList<Vector2>> ReadVector2Async(
            string path, string code = TEXTURE_CODE)
        {
            using StreamReader reader = new(new FileStream(path, FileMode.Open, FileAccess.Read));
            IList<Vector2> vertices = new List<Vector2>();
            while (!reader.EndOfStream)
            {
                string line = await reader.ReadLineAsync();
                // Skip line if not valid. (First position is invalid)
                if (line.Length <= 0 || line[0] == ' ')
                    continue;
                string[] segments = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (segments[0] == code)
                {
                    if (float.TryParse(segments[1], out float x)
                        && float.TryParse(segments[2], out float y))
                    {
                        vertices.Add(new Vector2(x, y));
                    }
                    else
                    {
                        Debug.WriteLine($"[{path}] has an invalid vector. Line = [{line}]");
                    }
                }
            }
            return vertices;
        }

        public static async Task<IList<Vector3>> ReadVector3Async(
            string path, string code = POSITION_CODE)
        {
            using StreamReader reader = new(new FileStream(path, FileMode.Open, FileAccess.Read));
            IList<Vector3> vertices = new List<Vector3>();
            while (!reader.EndOfStream)
            {
                string line = await reader.ReadLineAsync();
                // Skip line if not valid. (First position is invalid)
                if (line.Length <= 0 || line[0] == ' ')
                    continue;
                string[] segments = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (segments[0] == code)
                {
                    if (float.TryParse(segments[1], out float x)
                        && float.TryParse(segments[2], out float y)
                        && float.TryParse(segments[3], out float z))
                    {
                        vertices.Add(new Vector3(x, y, z));
                    }
                    else
                    {
                        Debug.WriteLine($"[{path}] has an invalid vector. Line = [{line}]");
                    }
                }
            }
            return vertices;
        }

        public static async Task<IList<Vector4>> ReadVector4Async(
            string path, string code = COLOUR_CODE)
        {
            using StreamReader reader = new(new FileStream(path, FileMode.Open, FileAccess.Read));
            IList<Vector4> vertices = new List<Vector4>();
            while (!reader.EndOfStream)
            {
                string line = await reader.ReadLineAsync();
                // Skip line if not valid. (First position is invalid)
                if (line.Length <= 0 || line[0] == ' ')
                    continue;
                string[] segments = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (segments[0] == code)
                {
                    if (float.TryParse(segments[1], out float x)
                        && float.TryParse(segments[2], out float y)
                        && float.TryParse(segments[3], out float z)
                        && float.TryParse(segments[4], out float w))
                    {
                        vertices.Add(new Vector4(x, y, z, w));
                    }
                    else
                    {
                        Debug.WriteLine($"[{path}] has an invalid vector. Line = [{line}]");
                    }
                }
            }
            return vertices;
        }

        public static (float[], uint[]) ReadFaces(string path, in IList<Vector3> positions)
        {
            using StreamReader reader = new(new FileStream(path, FileMode.Open, FileAccess.Read));

            uint index = 0;
            Vertex<Vector3> tVertex = default;
            Dictionary<int, (uint index, Vertex<Vector3> vertex)> vertexDictionary = new();
            List<uint> indices = new();
            while (!reader.EndOfStream)
            {
                // Get line
                string line = reader.ReadLine();
                // Check if line is valid.
                if (line == null || line.Length <= 0 || line[0] == ' ')
                    continue;
                // Check if line is a face.
                // segments = ["f", "v/vt/vn", "v/vt/vn", "v/vt/vn"]
                string[] segments = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (segments[0] != "f")
                    continue;
                // Loop for parsing of the three segments of the face.
                for (int i = 1; i <= 3; i++)
                {
                    // Get: f v/vt/vn
                    // vertexType = ["v", "vt", "vn"]
                    string[] vertexType = segments[i].Split('/', StringSplitOptions.RemoveEmptyEntries);
                    // Check if vertex of face is valid.
                    if (vertexType.Length != 1)
                        continue;
                    int indexOfPos = int.Parse(vertexType[0]) - 1;
                    // Check if indexes already exist.
                    if (vertexDictionary.ContainsKey((indexOfPos)))
                    {
                        // Append current index to array of indices.
                        indices.Add(vertexDictionary[(indexOfPos)].index);
                    }
                    else
                    {
                        // Create vertex and append to vertex array.
                        Vertex<Vector3> vertex = new(positions[indexOfPos]);
                        vertexDictionary[(indexOfPos)] = (index, vertex);
                        indices.Add(index);
                        index++;
                    }
                }
            }
            // Create array.
            const int NUMBER_OF_FLOATS = 3;
            int nIndex = 0;
            float[] rawVertices = new float[vertexDictionary.Count * NUMBER_OF_FLOATS];
            foreach (KeyValuePair<int, (uint, Vertex<Vector3>)> vertex in vertexDictionary)
            {
                (index, tVertex) = vertex.Value;
                float[] tRawVertex = ToArray(tVertex);
                for (int i = 0; i < tRawVertex.Length; i++)
                {
                    rawVertices[nIndex++] = tRawVertex[i];
                }
            }
            uint[] Indices = indices.ToArray();
            return (rawVertices, Indices);
        }

        public static (float[], uint[]) ReadFaces(string path, in IList<Vector3> positions,
            in IList<Vector2> textures)
        {
            using StreamReader reader = new(new FileStream(path, FileMode.Open, FileAccess.Read));

            uint index = 0;
            Vertex<Vector3, Vector2> tVertex = default;
            Dictionary<(int, int), (uint index, Vertex<Vector3, Vector2> vertex)> vertexDictionary = new();
            List<uint> indices = new();
            while (!reader.EndOfStream)
            {
                // Get line
                string line = reader.ReadLine();
                // Check if line is valid.
                if (line == null || line.Length <= 0 || line[0] == ' ')
                    continue;
                // Check if line is a face.
                // segments = ["f", "v/vt/vn", "v/vt/vn", "v/vt/vn"]
                string[] segments = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (segments[0] != "f")
                    continue;
                // Loop for parsing of the three segments of the face.
                for (int i = 1; i <= 3; i++)
                {
                    // Get: f v/vt/vn
                    // vertexType = ["v", "vt", "vn"]
                    string[] vertexType = segments[i].Split('/', StringSplitOptions.RemoveEmptyEntries);
                    // Check if vertex of face is valid.
                    if (vertexType.Length != 2)
                        continue;
                    int indexOfPos = int.Parse(vertexType[0]) - 1;
                    int indexOfTex = int.Parse(vertexType[1]) - 1;
                    int indexOfNor = int.Parse(vertexType[2]) - 1;
                    // Check if indexes already exist.
                    if (vertexDictionary.ContainsKey((indexOfPos, indexOfTex)))
                    {
                        // Append current index to array of indices.
                        indices.Add(vertexDictionary[(indexOfPos, indexOfTex)].index);
                    }
                    else
                    {
                        // Create vertex and append to vertex array.
                        Vertex<Vector3, Vector2> vertex = new(positions[indexOfPos], textures[indexOfTex]);
                        vertexDictionary[(indexOfPos, indexOfTex)] = (index, vertex);
                        indices.Add(index);
                        index++;
                    }
                }
            }
            // Create array.
            const int NUMBER_OF_FLOATS = 5;
            int nIndex = 0;
            float[] rawVertices = new float[vertexDictionary.Count * NUMBER_OF_FLOATS];
            foreach (KeyValuePair<(int, int), (uint, Vertex<Vector3, Vector2>)> vertex in vertexDictionary)
            {
                (index, tVertex) = vertex.Value;
                float[] tRawVertex = ToArray(tVertex);
                for (int i = 0; i < tRawVertex.Length; i++)
                {
                    rawVertices[nIndex++] = tRawVertex[i];
                }
            }
            uint[] Indices = indices.ToArray();
            return (rawVertices, Indices);
        }

        public static (float[], uint[]) ReadFaces(string path, in IList<Vector3> positions,
            in IList<Vector2> textures, in IList<Vector3> normals)
        {
            using StreamReader reader = new(new FileStream(path, FileMode.Open, FileAccess.Read));

            uint index = 0;
            Vertex<Vector3, Vector2, Vector3> tVertex = default;
            Dictionary<(int, int, int), (uint index, Vertex<Vector3, Vector2, Vector3> vertex)> vertexDictionary = new();
            List<uint> indices = new();
            while (!reader.EndOfStream)
            {
                // Get line
                string line = reader.ReadLine();
                // Check if line is valid.
                if (line == null || line.Length <= 0 || line[0] == ' ')
                    continue;
                // Check if line is a face.
                // segments = ["f", "v/vt/vn", "v/vt/vn", "v/vt/vn"]
                string[] segments = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (segments[0] != "f")
                    continue;
                // Loop for parsing of the three segments of the face.
                for (int i = 1; i <= 3; i++)
                {
                    // Get: f v/vt/vn
                    // vertexType = ["v", "vt", "vn"]
                    string[] vertexType = segments[i].Split('/', StringSplitOptions.RemoveEmptyEntries);
                    // Check if vertex of face is valid.
                    if (vertexType.Length != 3)
                        continue;
                    int indexOfPos = int.Parse(vertexType[0]) - 1;
                    int indexOfTex = int.Parse(vertexType[1]) - 1;
                    int indexOfNor = int.Parse(vertexType[2]) - 1;
                    // Check if indexes already exist.
                    if (vertexDictionary.ContainsKey((indexOfPos, indexOfTex, indexOfNor)))
                    {
                        // Append current index to array of indices.
                        indices.Add(vertexDictionary[(indexOfPos, indexOfTex, indexOfNor)].index);
                    }
                    else
                    {
                        // Create vertex and append to vertex array.
                        Vertex<Vector3, Vector2, Vector3> vertex = new(positions[indexOfPos], textures[indexOfTex], normals[indexOfNor]);
                        vertexDictionary[(indexOfPos, indexOfTex, indexOfNor)] = (index, vertex);
                        indices.Add(index);
                        index++;
                    }
                }
            }
            // Create array.
            const int NUMBER_OF_FLOATS = 8;
            int nIndex = 0;
            float[] rawVertices = new float[vertexDictionary.Count * NUMBER_OF_FLOATS];
            foreach (KeyValuePair<(int, int, int), (uint, Vertex<Vector3, Vector2, Vector3>)> vertex in vertexDictionary)
            {
                (index, tVertex) = vertex.Value;
                float[] tRawVertex = ToArray(tVertex);
                for (int i = 0; i < tRawVertex.Length; i++)
                {
                    rawVertices[nIndex++] = tRawVertex[i];
                }
            }
            uint[] Indices = indices.ToArray();
            return (rawVertices, Indices);
        }

        public static float[] ToArray(Vertex<Vector3> vertex)
        {
            float[] raw = new float[3];
            raw[0] = vertex.Data1.X;
            raw[1] = vertex.Data1.Y;
            raw[2] = vertex.Data1.Z;
            return raw;
        }

        public static float[] ToArray(Vertex<Vector3, Vector2> vertex)
        {
            float[] raw = new float[5];
            raw[0] = vertex.Data1.X;
            raw[1] = vertex.Data1.Y;
            raw[2] = vertex.Data1.Z;
            raw[3] = vertex.Data2.X;
            raw[4] = vertex.Data2.Y;
            return raw;
        }

        public static float[] ToArray(Vertex<Vector3, Vector2, Vector3> vertex)
        {
            float[] raw = new float[8];
            raw[0] = vertex.Data1.X;
            raw[1] = vertex.Data1.Y;
            raw[2] = vertex.Data1.Z;
            raw[3] = vertex.Data2.X;
            raw[4] = vertex.Data2.Y;
            raw[5] = vertex.Data3.X;
            raw[6] = vertex.Data3.Y;
            raw[7] = vertex.Data3.Z;
            return raw;
        }

        public void LoadBuffers(ref Drawable drawable)
        {
            // Refer
            ref IShader shader = ref drawable.Shader;
            // Setup the shader.
            shader.Swap();
            // Create buffers
            GL.CreateVertexArrays(1, out uint vao);
            GL.CreateBuffers(1, out uint vbo);
            GL.CreateBuffers(1, out uint ibo);
            // Create named buffers for the data
            GL.NamedBufferData(vbo, Vertices.Length * sizeof(float), Vertices, BufferUsageHint.StaticDraw);
            GL.NamedBufferData(ibo, Indices.Length * sizeof(uint), Indices, BufferUsageHint.StaticDraw);
            // Set the VAO and VBO, then the IBO/EBO
            GL.VertexArrayVertexBuffer(vao, 0, vbo, IntPtr.Zero, Stride);
            GL.VertexArrayElementBuffer(vao, ibo);
            // Get the count of program inputs.
            GL.GetProgramInterface(shader.Handle, ProgramInterface.ProgramInput, 
                ProgramInterfaceParameter.ActiveResources, out int output);
            Debug.WriteLine($"{Util.ThreadID}: {shader.VertexPath}: output = {output}");
            // Variable initialisers for tracking size and offset within the loop.
            uint offset = 0;
            int size;
            // Loop through active attributes
            for (uint i = 0; i < output; i++)
            {
                GL.GetProgramResourceName(shader.Handle, ProgramInterface.ProgramInput, 
                    (int)i, 16, out _, out string name);
                switch (name)
                {
                    case "vPosition":
                        size = 3;
                        GL.EnableVertexArrayAttrib(vao, i);
                        GL.VertexArrayAttribFormat(vao, i, size, VertexAttribType.Float, false, offset);
                        GL.VertexArrayAttribBinding(vao, i, 0);
                        Debug.WriteLine($"{Util.ThreadID}: {shader.VertexPath}: Attrib[{i}] ({name}) >> Offset = {offset}, Size = {size}");
                        offset += (uint)size * sizeof(float);
                        break;
                    case "vTexture":
                        size = 2;
                        GL.EnableVertexArrayAttrib(vao, i);
                        GL.VertexArrayAttribFormat(vao, i, size, VertexAttribType.Float, false, offset);
                        GL.VertexArrayAttribBinding(vao, i, 0);
                        Debug.WriteLine($"{Util.ThreadID}: {shader.VertexPath}: Attrib[{i}] ({name}) >> Offset = {offset}, Size = {size}");
                        offset += (uint)size * sizeof(float);
                        break;
                    case "vNormal":
                        size = 3;
                        GL.EnableVertexArrayAttrib(vao, i);
                        GL.VertexArrayAttribFormat(vao, i, size, VertexAttribType.Float, false, offset);
                        GL.VertexArrayAttribBinding(vao, i, 0);
                        Debug.WriteLine($"{Util.ThreadID}: {shader.VertexPath}: Attrib[{i}] ({name}) >> Offset = {offset}, Size = {size}");
                        offset += (uint)size * sizeof(float);
                        break;
                    default:
                        break;
                }
            }
            drawable.VAO = vao;
            drawable.VBO = vbo;
            drawable.IBO = ibo;
        }
    }

    readonly struct NodeTypes
    {
        public readonly bool HasPos;
        public readonly bool HasTex;
        public readonly bool HasNor;

        public NodeTypes(bool hasPos, bool hasTex, bool hasNor)
        {
            HasPos = hasPos;
            HasTex = hasTex;
            HasNor = hasNor;
        }
    }
}
