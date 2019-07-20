﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Materia.GLInterfaces;

namespace Materia.Textures
{
    public class GLTextuer2D : GLTexture
    {
        public int Id { get; protected set; }

        public int Width { get; protected set; }
        public int Height { get; protected set; }

        PixelInternalFormat iformat;

        public GLTextuer2D(PixelInternalFormat format)
        {
            iformat = format;
            Id = IGL.Primary.GenTexture();
        }

        /// <summary>
        /// Use this before using anything else in this class
        /// </summary>
        public void Bind()
        {
            IGL.Primary.BindTexture((int)TextureTarget.Texture2D, Id);
        }

        public static void Unbind()
        {
            IGL.Primary.BindTexture((int)TextureTarget.Texture2D, 0);
        }

        /// <summary>
        /// Only use after SetData
        /// </summary>
        public void GenerateMipMaps()
        {
            IGL.Primary.GenerateMipmap((int)GenerateMipmapTarget.Texture2D);
        }

        public void Release()
        {
            if (Id != 0)
            {
                IGL.Primary.DeleteTexture(Id);
                Id = 0;
            }
        }

        public void SetAsDepth(int width, int height)
        {
            Width = width;
            Height = height;
            IGL.Primary.TexImage2D((int)TextureTarget.Texture2D, 0, (int)PixelInternalFormat.DepthComponent24, width, height, 0, (int)PixelFormat.DepthComponent, (int)PixelType.Float, IntPtr.Zero);
        }

        public void CopyFromFrameBuffer(int width, int height)
        {
            Width = width;
            Height = height;
            IGL.Primary.CopyTexImage2D((int)TextureTarget.Texture2D, 0, (int)InternalFormat.Rgba8, 0, 0, width, height, 0);
        }

        public void SetData(IntPtr data, PixelFormat format, int width, int height, int mipLevel = 0)
        {
            Width = width;
            Height = height;
            IGL.Primary.TexImage2D((int)TextureTarget.Texture2D, mipLevel, (int)iformat, width, height, 0, (int)format, (int)PixelType.UnsignedByte, data);
        }

        public void SetData(byte[] data, PixelFormat format, int width, int height, int mipLevel = 0)
        {
            Width = width;
            Height = height;
            IGL.Primary.TexImage2D((int)TextureTarget.Texture2D, mipLevel, (int)iformat, width, height, 0, (int)format, (int)PixelType.UnsignedByte, data);
        }

        public void SetData(float[] data, PixelFormat format, int width, int height, int mipLevel = 0)
        {
            Width = width;
            Height = height;
            IGL.Primary.TexImage2D((int)TextureTarget.Texture2D, mipLevel, (int)iformat, width, height, 0, (int)format, (int)PixelType.Float, data);
        }

        public void SetDataAsFloat(byte[] data, PixelFormat format, int width, int height, int mipLevel = 0)
        {
            Width = width;
            Height = height;
            IGL.Primary.TexImage2D((int)TextureTarget.Texture2D, mipLevel, (int)iformat, width, height, 0, (int)format, (int)PixelType.Float, data);
        }

        public void SetSwizzleLuminance()
        {
            IGL.Primary.TexParameterI((int)TextureTarget.Texture2D, (int)TextureParameterName.TextureSwizzleRgba, new int[] { (int)All.Red, (int)All.Red, (int)All.Red, (int)All.One });
        }

        public void SetMaxMipLevel(int max)
        {
            IGL.Primary.TexParameter((int)TextureTarget.Texture2D, (int)TextureParameterName.TextureBaseLevel, 0);
            IGL.Primary.TexParameter((int)TextureTarget.Texture2D, (int)TextureParameterName.TextureMaxLevel, max);
        }

        public void SetFilter(int min, int mag)
        {
            IGL.Primary.TexParameter((int)TextureTarget.Texture2D, (int)TextureParameterName.TextureMinFilter, min);
            IGL.Primary.TexParameter((int)TextureTarget.Texture2D, (int)TextureParameterName.TextureMagFilter, mag);
        }

        public void SetWrap(int wrap)
        {
            IGL.Primary.TexParameter((int)TextureTarget.Texture2D, (int)TextureParameterName.TextureWrapS, wrap);
            IGL.Primary.TexParameter((int)TextureTarget.Texture2D, (int)TextureParameterName.TextureWrapT, wrap);
        }
    }
}