﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Materia.Textures;
using Materia.Shaders;
using Materia.Math3D;
using Materia.GLInterfaces;

namespace Materia.Imaging.GLProcessing
{
    public class BlurProcessor : ImageProcessor
    {
        public float Intensity { get; set; }

        IGLProgram shader;

        public BlurProcessor() : base()
        {
            shader = GetShader("image.glsl", "blur.glsl");
        }

        /// <summary>
        /// This would be too slow for real time performance at 60fps
        /// but since we are not doing 60fps it is fine
        /// it is a fast box gaussian blur
        /// produces results better than adobes gaussian blur
        /// and faster than theirs!
        /// can easily blur 4kx4k textures in low ms on a 1080 gtx gpu with 64+ intensity
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="tex"></param>
        /// <param name="output"></param>
        public override void Process(int width, int height, GLTextuer2D tex, GLTextuer2D output)
        {
            base.Process(width, height, tex, output);

            colorBuff.Bind();
            colorBuff.ClampToEdge();
            GLTextuer2D.Unbind();

            if (shader != null)
            {
                ResizeViewTo(tex, output, tex.Width, tex.Height, width, height);
                tex = output;
                IGL.Primary.Clear((int)(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit));

                Vector2 tiling = new Vector2(TileX, TileY);
                float[] boxes = Materia.Nodes.Helpers.Blur.BoxesForGaussian(Intensity, 3);

                shader.Use();
                shader.SetUniform2("tiling", ref tiling);
                shader.SetUniform("MainTex", 0);
                shader.SetUniform("horizontal", true);
                shader.SetUniform("intensity", (boxes[0] - 1.0f) / 2.0f);
                IGL.Primary.ActiveTexture((int)TextureUnit.Texture0);
                tex.Bind();

                //clamp to prevent blur wrap
                tex.ClampToEdge();

                if (renderQuad != null)
                {
                    renderQuad.Draw();
                }

                GLTextuer2D.Unbind();
                /*output.Bind();
                output.CopyFromFrameBuffer(width, height);
                GLTextuer2D.Unbind();*/
                Blit(output, width, height);

                tex.Bind();
                tex.Repeat();
                GLTextuer2D.Unbind();

                shader.SetUniform("horizontal", false);
                IGL.Primary.ActiveTexture((int)TextureUnit.Texture0);
                output.Bind();

                //clamp to prevent blur wrap
                output.ClampToEdge();

                IGL.Primary.Clear((int)ClearBufferMask.ColorBufferBit);
                IGL.Primary.Clear((int)ClearBufferMask.DepthBufferBit);

                if (renderQuad != null)
                {
                    renderQuad.Draw();
                }

                GLTextuer2D.Unbind();
                /*output.Bind();
                output.CopyFromFrameBuffer(width, height);
                GLTextuer2D.Unbind();*/
                Blit(output, width, height);

                //begin second box
                shader.SetUniform("horizontal", true);
                shader.SetUniform("intensity", (boxes[1] - 1.0f) / 2.0f);
                IGL.Primary.ActiveTexture((int)TextureUnit.Texture0);
                output.Bind();

                IGL.Primary.Clear((int)ClearBufferMask.ColorBufferBit);
                IGL.Primary.Clear((int)ClearBufferMask.DepthBufferBit);

                if (renderQuad != null)
                {
                    renderQuad.Draw();
                }

                GLTextuer2D.Unbind();
                /*output.Bind();
                output.CopyFromFrameBuffer(width, height);
                GLTextuer2D.Unbind();*/
                Blit(output, width, height);

                shader.SetUniform("horizontal", false);
                shader.SetUniform("intensity", (boxes[1] - 1.0f) / 2.0f);
                IGL.Primary.ActiveTexture((int)TextureUnit.Texture0);
                output.Bind();

                IGL.Primary.Clear((int)ClearBufferMask.ColorBufferBit);
                IGL.Primary.Clear((int)ClearBufferMask.DepthBufferBit);

                if (renderQuad != null)
                {
                    renderQuad.Draw();
                }

                GLTextuer2D.Unbind();
                /*output.Bind();
                output.CopyFromFrameBuffer(width, height);
                GLTextuer2D.Unbind();
                */

                Blit(output, width, height);

                //begin third box
                shader.SetUniform("horizontal", true);
                shader.SetUniform("intensity", (boxes[2] - 1.0f) / 2.0f);
                IGL.Primary.ActiveTexture((int)TextureUnit.Texture0);
                output.Bind();

                IGL.Primary.Clear((int)ClearBufferMask.ColorBufferBit);
                IGL.Primary.Clear((int)ClearBufferMask.DepthBufferBit);

                if (renderQuad != null)
                {
                    renderQuad.Draw();
                }

                GLTextuer2D.Unbind();
                /*output.Bind();
                output.CopyFromFrameBuffer(width, height);
                GLTextuer2D.Unbind();
                */

                Blit(output, width, height);

                shader.SetUniform("horizontal", false);
                shader.SetUniform("intensity", (boxes[2] - 1.0f) / 2.0f);
                IGL.Primary.ActiveTexture((int)TextureUnit.Texture0);
                output.Bind();

                IGL.Primary.Clear((int)ClearBufferMask.ColorBufferBit);
                IGL.Primary.Clear((int)ClearBufferMask.DepthBufferBit);

                if (renderQuad != null)
                {
                    renderQuad.Draw();
                }

                GLTextuer2D.Unbind();
                /*output.Bind();
                output.CopyFromFrameBuffer(width, height);
                GLTextuer2D.Unbind();*/
                Blit(output, width, height);
            }

            output.Bind();
            output.Repeat();
            GLTextuer2D.Unbind();

            colorBuff.Bind();
            colorBuff.Repeat();
            GLTextuer2D.Unbind();
        }

        public override void Release()
        {
            base.Release();
        }
    }
}
