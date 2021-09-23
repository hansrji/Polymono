using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using Polymono.Components.Resources;
using SharpFont;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Polymono.Systems.Resources
{
    public class FreeTypeFont : IFreeTypeFont
    {
        readonly Dictionary<uint, Character> Characters = new();
        readonly int VAO;
        readonly int VBO;

        public IShader Shader { get; set; }

        public FreeTypeFont(ref IShader shader, uint pixelheight)
        {
            Shader = shader;

            // initialize library
            Library lib = new();
            Face face = new(lib, "Resources/arial.ttf");

            face.SetPixelSizes(0, pixelheight);

            // set 1 byte pixel alignment 
            GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);

            // set texture unit
            GL.ActiveTexture(TextureUnit.Texture0);

            // Load first 128 characters of ASCII set
            for (uint c = 0; c < 128; c++)
            {
                try
                {
                    // load glyph
                    //face.LoadGlyph(c, LoadFlags.Render, LoadTarget.Normal);
                    face.LoadChar(c, LoadFlags.Render, LoadTarget.Normal);
                    GlyphSlot glyph = face.Glyph;
                    FTBitmap bitmap = glyph.Bitmap;

                    // create glyph texture
                    int texObj = GL.GenTexture();
                    GL.BindTexture(TextureTarget.Texture2D, texObj);
                    GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.R8, bitmap.Width, bitmap.Rows, 0,
                                  PixelFormat.Red, PixelType.UnsignedByte, bitmap.Buffer);

                    // set texture parameters
                    GL.TextureParameter(texObj, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                    GL.TextureParameter(texObj, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                    GL.TextureParameter(texObj, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
                    GL.TextureParameter(texObj, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);

                    // add character
                    Character ch = new()
                    {
                        TextureID = texObj,
                        Size = new Vector2(bitmap.Width, bitmap.Rows),
                        Bearing = new Vector2(glyph.BitmapLeft, glyph.BitmapTop),
                        Advance = glyph.Advance.X.Value
                    };
                    Characters.Add(c, ch);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            // bind default texture
            GL.BindTexture(TextureTarget.Texture2D, 0);

            // set default (4 byte) pixel alignment 
            GL.PixelStore(PixelStoreParameter.UnpackAlignment, 4);

            float[] vquad =
            {
            // x      y      u     v    
                0.0f, -1.0f,   0.0f, 0.0f,
                0.0f,  0.0f,   0.0f, 1.0f,
                1.0f,  0.0f,   1.0f, 1.0f,
                0.0f, -1.0f,   0.0f, 0.0f,
                1.0f,  0.0f,   1.0f, 1.0f,
                1.0f, -1.0f,   1.0f, 0.0f
            };

            // Create [Vertex Buffer Object](https://www.khronos.org/opengl/wiki/Vertex_Specification#Vertex_Buffer_Object)
            VBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, 4 * 6 * 4, vquad, BufferUsageHint.StaticDraw);

            // [Vertex Array Object](https://www.khronos.org/opengl/wiki/Vertex_Specification#Vertex_Array_Object)
            VAO = GL.GenVertexArray();

            uint vPosition = Shader.GetAttribLocation("in_pos");
            uint vNormal = Shader.GetAttribLocation("in_uv");

            GL.BindVertexArray(VAO);
            GL.EnableVertexAttribArray(vPosition);
            GL.VertexAttribPointer(vPosition, 2, VertexAttribPointerType.Float, false, 4 * 4, 0);
            GL.EnableVertexAttribArray(vNormal);
            GL.VertexAttribPointer(vNormal, 2, VertexAttribPointerType.Float, false, 4 * 4, 2 * 4);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
        }

        protected void Render(PolyFrameEventArgs e)
        {
            //Matrix4 projectionM = Matrix4.CreateScale(new Vector3(1f / e.Size.X, 1f / e.Size.Y, 1.0f));
            Matrix4 projectionM = Matrix4.CreateOrthographicOffCenter(0.0f, e.Size.X, e.Size.Y, 0.0f, -1.0f, 1.0f);

            GL.Enable(EnableCap.Blend);
            //GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.BlendFunc(0, BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            Shader.Swap();

            Shader.SetMatrix4("model", Matrix4.Identity);
            Shader.SetMatrix4("projection", projectionM);

            Shader.SetVector3("textColor", new Vector3(0.5f, 0.8f, 0.2f));
            RenderText("This took 4 hours", 25.0f, 50.0f, 1.2f, new Vector2(1f, 0f));

            Shader.SetVector3("textColor", new Vector3(0.3f, 0.7f, 0.9f));
            RenderText("Fuck me", 50.0f, 200.0f, 0.9f, new Vector2(1.0f, -0.25f));
        }

        public void RenderText(string text, float x, float y, float scale, Vector2 dir)
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindVertexArray(VAO);

            float angle_rad = (float)Math.Atan2(dir.Y, dir.X);
            Matrix4 rotation = Matrix4.CreateRotationZ(angle_rad);
            Matrix4 transOriginM = Matrix4.CreateTranslation(new Vector3(x, y, 0f));

            // Iterate through all characters
            float characterOffset = 0.0f;
            foreach (char character in text)
            {
                if (Characters.ContainsKey(character) == false)
                    continue;
                Character ch = Characters[character];

                float width = ch.Size.X * scale;
                float height = ch.Size.Y * scale;
                float xRelative = characterOffset + ch.Bearing.X * scale;
                float yRelative = (ch.Size.Y - ch.Bearing.Y) * scale;

                // Now advance cursors for next glyph (note that advance is number of 1/64 pixels)
                characterOffset += (ch.Advance >> 6) * scale; // Bitshift by 6 to get value in pixels (2^6 = 64 (divide amount of 1/64th pixels by 64 to get amount of pixels))

                Matrix4 scaleM = Matrix4.CreateScale(new Vector3(width, height, 1.0f));
                Matrix4 transRelM = Matrix4.CreateTranslation(new Vector3(xRelative, yRelative, 0.0f));

                Matrix4 modelM = scaleM * transRelM * rotation * transOriginM; // OpenTK `*`-operator is reversed
                Shader.SetMatrix4("model", modelM);

                // Render glyph texture over quad
                GL.BindTexture(TextureTarget.Texture2D, ch.TextureID);

                // Render quad
                GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
            }

            GL.BindVertexArray(0);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }
    }
}
