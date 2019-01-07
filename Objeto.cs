using CGUNS.Meshes.FaceVertexList;
using OpenTK.Graphics.OpenGL;
using gl = OpenTK.Graphics.OpenGL.GL;
using OpenTK; //La matematica
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using CGUNS.Shaders;

namespace BlackOut
{
    class Objeto
    {
        private List<FVLMesh> meshes;
        private List<int> listaTexturas = new List<int>();
        private List<int> listaTexturasBump = new List<int>();
        private BoundingBox bb;

        private float MaxXinicial;
        private float MinXinicial;
        private float MaxZinicial;
        private float MinZinicial;

        private ShaderProgram shader;

        public Objeto(String objFileName, String mtlFileName, ShaderProgram sp)
        {
            meshes = ObjMatFileParser.parseFile(objFileName, mtlFileName);
            this.shader = sp;
            foreach (FVLMesh m in meshes)
            {
                m.Build(shader);
                listaTexturas.Add(CargarTextura(m.Material.ImagenTex));
                if (m.Material.ImagenTexBump != "")
                {
                    listaTexturasBump.Add(CargarTextura(m.Material.ImagenTexBump));
                }

            }

            bb = new BoundingBox(this);
            MaxXinicial = bb.MaxX;
            MinXinicial = bb.MinX;
            MaxZinicial = bb.MaxZ;
            MinZinicial = bb.MinZ;
        }

        public void paint(Matrix4 modelMatrix)
        {
            int i = 0;
            foreach (FVLMesh m in meshes)
            {
                shader.SetUniformValue("material.Ka", m.Material.Kambient);
                shader.SetUniformValue("material.Kd", m.Material.Kdiffuse);
                shader.SetUniformValue("material.Ks", m.Material.Kspecular);
                shader.SetUniformValue("material.shininess", m.Material.Shininess);

                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, listaTexturas.ElementAt(i));
                i++;

                //Dibujamos el objeto.
                m.Dibujar(shader);

                //Actualiza la posición del bounding box
                // bb.PosicionMundo = new Vector4(0.0f, 0.0f, 0.0f, 1.0f), modelMatrix);
                bb.PosicionMundo = modelMatrix.Row3;
                bb.MaxX = MaxXinicial * modelMatrix.M11;
                bb.MinX = MinXinicial * modelMatrix.M11;
                bb.MaxZ = MaxZinicial * modelMatrix.M33;
                bb.MinZ = MinZinicial * modelMatrix.M33;

            }
        }


        public void paintBump(Matrix4 modelMatrix)
        {
            int i = 0;
            foreach (FVLMesh m in meshes)
            {
                shader.SetUniformValue("material_diffuse", new Vector4(m.Material.Kdiffuse, 1.0f));
                shader.SetUniformValue("material_specular", new Vector4(m.Material.Kspecular, 1.0f));
                shader.SetUniformValue("material_shininess", m.Material.Shininess);

                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, listaTexturas.ElementAt(i));
                //shader.SetUniformValue("DifusseMap", listaTexturas.ElementAt(i));
                GL.ActiveTexture(TextureUnit.Texture1);
                GL.BindTexture(TextureTarget.Texture2D, listaTexturasBump.ElementAt(i));
                //shader.SetUniformValue("NormalMap", listaTexturasBump.ElementAt(i));
                i++;

                //Dibujamos el objeto.
                m.Dibujar(shader);

                //Actualiza la posición del bounding box
                // bb.PosicionMundo = new Vector4(0.0f, 0.0f, 0.0f, 1.0f), modelMatrix);
                bb.PosicionMundo = modelMatrix.Row3;
                bb.MaxX = MaxXinicial * modelMatrix.M11;
                bb.MinX = MinXinicial * modelMatrix.M11;
                bb.MaxZ = MaxZinicial * modelMatrix.M33;
                bb.MinZ = MinZinicial * modelMatrix.M33;

            }
        }

        private int CargarTextura(String imagenTex)
        {
            int texId = gl.GenTexture();
            gl.BindTexture(TextureTarget.Texture2D, texId);


            Bitmap bitmap = new Bitmap(Image.FromFile(imagenTex));

            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                             ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);


            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            bitmap.UnlockBits(data);
            return texId;

        }


        public List<FVLMesh> Meshes
        {
            get
            {
                return meshes;
            }
        }

        internal BoundingBox Bb
        {
            get
            {
                return bb;
            }
        }
    }
}
