﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace CloneMinecraft
{
    class Game: GameWindow
    {
        float[] verticies =
        {
            -0.5f, 0.5f, 0f, //top left vertex - 0
            0.5f, 0.5f, 0f, //top right vertex - 1
            0.5f, -0.5f, 0f //bottom right - 2
            -0.5f, -0.5f, 0f, //bottom left - 3
        };

        // RenderPipeline Variables
        int vao;
        int shaderProgram;

        // is the screeen width and height
        int width;
        int height;

        public Game(int width, int height) : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            this.width = width;
            this.height = height;   

            // Centres the program to the monitor
            this.CenterWindow(new Vector2i(width, height));
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, e.Width, e.Height);
            this.width = e.Width;
            this.height = e.Height;
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            vao = GL.GenVertexArray();

            // generates the vbo
            vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, verticies.Length * sizeof(float), verticies, BufferUsageHint.StaticDraw);

            //Bind vao
            GL.BindVertexArray(vao);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexArrayAttrib(vao, 0);

            // unbinds the respective vbo and vao
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0); 
            GL.BindVertexArray(0); 


            //gemerates ebo
            ebo = GL.GenBuffer();
            //binds the ebo to openGL as an elementarraybuffer
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            //sets the data in the Buffer
            GL.BufferData(BufferTarget.ElementArrayBuffer, indicies.Length * sizeof(uint), indicies, BufferUsageHint.StaticDraw);
            //Unbinds the ebo
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);


            // Create Shader Program
            shaderProgram = GL.CreateProgram();

            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, LoadShaderSource("Default.vert"));
            GL.CompileShader(vertexShader);

            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, LoadShaderSource("Default.frag"));
            GL.CompileShader(fragmentShader);

            GL.AttachShader(shaderProgram, vertexShader);
            GL.AttachShader(shaderProgram, fragmentShader);

            GL.LinkProgram(shaderProgram);

            //delete shaders
            GL.DeleteShader(vertexShader); 
            GL.DeleteShader(fragmentShader);
        }

        protected override void OnUnload()
        {
            base.OnUnload();

            GL.DeleteVertexArray(vao);
            GL.DeleteProgram(shaderProgram);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.ClearColor(0.6f, 0.3f, 1f, 1f);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            // draw a triangle
            GL.UseProgram(shaderProgram);
            GL.BindVertexArray(vao);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);



            Context.SwapBuffers();

            base.OnRenderFrame(args);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
        }

        // Function to load a text file and return its contents as a string
        public static string LoadShaderSource(string filePath)
        {
            string shaderSource = "";

            try
            {
                using (StreamReader reader = new StreamReader("../../../Shaders/" + filePath))
                {
                    shaderSource = reader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to load shader source file: " + e.Message);
            }

            return shaderSource;
        }
    }
}
