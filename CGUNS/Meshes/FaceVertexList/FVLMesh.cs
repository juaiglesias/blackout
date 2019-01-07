using System;
using OpenTK;
using System.Text;
using CGUNS.Shaders;
using OpenTK.Graphics.OpenGL;
using gl = OpenTK.Graphics.OpenGL.GL;
using BlackOut;
using System.Collections.Generic;
using System.Linq;

namespace CGUNS.Meshes.FaceVertexList
{
    public class FVLMesh
    {
        private List<FVLFace> faceList;
        private List<Vector3> vertexList;
        private List<Vector2> texCordList;
        private List<Vector3> vertexNormalList;
        private MatConTextura material;
        private String nombreObjeto;

        private int[] indices;  //Los indices para formar las caras.

        public FVLMesh()
        {
            faceList = new List<FVLFace>();
            vertexList = new List<Vector3>();
            vertexNormalList = new List<Vector3>();
            texCordList = new List<Vector2>();
            material = new MatConTextura();
            NombreObjeto = "";

        }

        public List<Vector3> VertexList
        {
            get { return vertexList; }
        }

        public List<FVLFace> FaceList
        {
            get { return faceList; }
        }

        public List<Vector3> VertexNormalList
        {
            get { return vertexNormalList; }
        }

        public List<Vector2> TexCordList
        {
            get { return texCordList; }
        }

        public int VertexCount
        {
            get { return vertexList.Count; }
        }

        public int FaceCount
        {
            get { return faceList.Count; }
        }

        public MatConTextura Material
        {
            get
            {
                return material;
            }

            set
            {
                material = value;
            }
        }

        public string NombreObjeto
        {
            get
            {
                return nombreObjeto;
            }

            set
            {
                nombreObjeto = value;
            }
        }

        public int AddVertex(Vector3 vertex)
        {
            vertexList.Add(vertex);
            return vertexList.Count - 1;
        }

        public int AddVertexNormal(Vector3 normal)
        {
            vertexNormalList.Add(normal);
            return vertexNormalList.Count - 1;
        }

        public int AddTexCord(Vector2 texCord)
        {
            texCordList.Add(texCord);
            return texCordList.Count - 1;
        }

        public int AddFace(FVLFace face)
        {
            faceList.Add(face);
            return faceList.Count - 1;
        }

        public void PrintLists()
        {
            String sender = "FVLMesh.printLists: ";
            FVLFace face;
            List<int> faceVertexes;
            log(sender, "Vertex List has {0} items.", vertexList.Count);
            for (int i = 0; i < vertexList.Count; i++)
            {
                log("", "V[{0}] = ({1}, {2}, {3})", i, vertexList[i].X, vertexList[i].Y, vertexList[i].Z);
            }
            int cantFaces = faceList.Count;
            log(sender, "Face List has {0} items.", cantFaces);
            for (int i = 0; i < cantFaces; i++)
            {
                face = faceList[i];
                faceVertexes = face.VertexIndexes;
                String format = "F[{0}] = ";
                for (int j = 0; j < faceList[i].VertexCount; j++)
                {
                    format = format + " V[" + faceVertexes[j] + "],";
                }
                log("", format, i);
            }
            log(sender, "End!");
        }

        private void log(String sender, String format, params Object[] args)
        {
            Console.Out.WriteLine(sender + format, args);
        }

        public void Build(ShaderProgram sProgram)
        {
            CrearVBOs();
            CrearVAO(sProgram);
        }
        private int h_VBO_p; //Handle del Vertex Buffer Object (posiciones de los vertices)
        private int h_VBO_n; //Handle del VBO de normales.
        private int h_VBO_t; //Handle del VBO de texturas.
        private int h_EBO; //Handle del Elements Buffer Object (indices)
        private int h_VAO; //Handle del Vertex Array Object (Configuracion de los dos anteriores)


        private void CrearVBOs()
        {
            BufferTarget bufferType; //Tipo de buffer (Array: datos, Element: indices)
            IntPtr size;             //Tamanio (EN BYTES!) del buffer.
                                     //Hint para que OpenGl almacene el buffer en el lugar mas adecuado.
                                     //Por ahora, usamos siempre StaticDraw (buffer solo para dibujado, que no se modificara)
            BufferUsageHint hint = BufferUsageHint.StaticDraw;

            /////////////////////////////////////////////////////////////////////VBO POSICIONES
            //VBO con el atributo "posicion" de los vertices.
            Vector3[] posiciones = ordenarPosiciones();

            bufferType = BufferTarget.ArrayBuffer;
            size = new IntPtr(posiciones.Length * Vector3.SizeInBytes);
            h_VBO_p = gl.GenBuffer();  //Le pido un Id de buffer a OpenGL
            gl.BindBuffer(bufferType, h_VBO_p); //Lo selecciono como buffer de Datos actual.
            gl.BufferData<Vector3>(bufferType, size, posiciones, hint); //Lo lleno con la info.
            gl.BindBuffer(bufferType, 0); // Lo deselecciono (0: ninguno)

            //////////////////////////////////////////////////////////////////////VBO NORMALES
           //VBO con otros atributos de los vertices (color, normal, textura, etc).
            h_VBO_n = gl.GenBuffer();
            gl.BindBuffer(bufferType, h_VBO_n);
            Vector3[] normales = ordenarNormales();
            Vector3[] suavizadas = suavizarNormales();
            gl.BufferData<Vector3>(bufferType, size, suavizadas, hint);//buffertype, size es el mismo q posiciones.
            gl.BindBuffer(bufferType, 0);

            //////////////////////////////////////////////////////////////////////VBO TEXTURAS
            h_VBO_t = gl.GenBuffer();
            gl.BindBuffer(bufferType, h_VBO_t);
            Vector3[] texturas = ordenarTexturas();
            gl.BufferData<Vector3>(bufferType, size, texturas, hint);//buffertype, size es el mismo q posiciones.
            gl.BindBuffer(bufferType, 0);

            ////////////////////////////////////////////////////////////////////EBO, buffer con los indices.
            bufferType = BufferTarget.ElementArrayBuffer;
            h_EBO = gl.GenBuffer();

            indices = new int[posiciones.Length];
            for (int i = 0; i < posiciones.Length; i++)
                indices[i] = i;

            size = new IntPtr(indices.Length * sizeof(int));
            gl.BindBuffer(bufferType, h_EBO); //Lo selecciono como buffer de elementos actual.
            gl.BufferData<int>(bufferType, size, indices, hint);
            gl.BindBuffer(bufferType, 0);
        }

        private void CrearVAO(ShaderProgram sProgram)
        {
            // Indice del atributo a utilizar. Este indice se puede obtener de tres maneras:
            // Supongamos que en nuestro shader tenemos un atributo: "in vec3 vPos";
            // 1. Dejar que OpenGL le asigne un indice cualquiera al atributo, y para consultarlo hacemos:
            //    attribIndex = gl.GetAttribLocation(programHandle, "vPos") DESPUES de haberlo linkeado.
            // 2. Nosotros le decimos que indice queremos que le asigne, utilizando:
            //    gl.BindAttribLocation(programHandle, desiredIndex, "vPos"); ANTES de linkearlo.
            // 3. Nosotros de decimos al preprocesador de shader que indice queremos que le asigne, utilizando
            //    layout(location = xx) in vec3 vPos;
            //    En el CODIGO FUENTE del shader (Solo para #version 330 o superior)      
            int attribIndex;
            int cantComponentes; //Cantidad de componentes de CADA dato.
            VertexAttribPointerType attribType; // Tipo de CADA una de las componentes del dato.
            int stride; //Cantidad de BYTES que hay que saltar para llegar al proximo dato. (0: Tightly Packed, uno a continuacion del otro)
            int offset; //Offset en BYTES del primer dato.
            BufferTarget bufferType; //Tipo de buffer.

            // 1. Creamos el VAO
            h_VAO = gl.GenVertexArray(); //Pedimos un identificador de VAO a OpenGL.
            gl.BindVertexArray(h_VAO);   //Lo seleccionamos para trabajar/configurar.

            //2. Configuramos el VBO de posiciones.
            attribIndex = sProgram.GetVertexAttribLocation("vPos"); //Yo lo saco de mi clase ProgramShader.
            cantComponentes = 3;   // 3 componentes (x, y, z)
            attribType = VertexAttribPointerType.Float; //Cada componente es un Float.
            stride = 0;  //Los datos estan uno a continuacion del otro.
            offset = 0;  //El primer dato esta al comienzo. (no hay offset).
            bufferType = BufferTarget.ArrayBuffer; //Buffer de Datos.

            gl.EnableVertexAttribArray(attribIndex); //Habilitamos el indice de atributo.
            gl.BindBuffer(bufferType, h_VBO_p); //Seleccionamos el buffer a utilizar.
            gl.VertexAttribPointer(attribIndex, cantComponentes, attribType, false, stride, offset);//Configuramos el layout (como estan organizados) los datos en el buffer.
            
            // 2.a.El bloque anterior se repite para cada atributo del vertice (color, normal, textura..)
            attribIndex = sProgram.GetVertexAttribLocation("vNormal"); //Yo lo saco de mi clase ProgramShader.
            cantComponentes = 3;   // 3 componentes (x, y, z)
            attribType = VertexAttribPointerType.Float; //Cada componente es un Float.
            stride = 0;  //Los datos estan uno a continuacion del otro.
            offset = 0;  //El primer dato esta al comienzo. (no hay offset).
            bufferType = BufferTarget.ArrayBuffer; //Buffer de Datos.

            gl.EnableVertexAttribArray(attribIndex); //Habilitamos el indice de atributo.
            gl.BindBuffer(bufferType, h_VBO_n); //Seleccionamos el buffer a utilizar.
            gl.VertexAttribPointer(attribIndex, cantComponentes, attribType, false, stride, offset);//Configuramos el layout (como estan organizados) los datos en el buffer.
            
            // 2.a.El bloque anterior se repite para cada atributo del vertice (color, normal, textura..)
            attribIndex = sProgram.GetVertexAttribLocation("TexCoord"); //Yo lo saco de mi clase ProgramShader.
            cantComponentes = 3;   // 3 componentes (x, y, z)
            attribType = VertexAttribPointerType.Float; //Cada componente es un Float.
            stride = 0;  //Los datos estan uno a continuacion del otro.
            offset = 0;  //El primer dato esta al comienzo. (no hay offset).
            bufferType = BufferTarget.ArrayBuffer; //Buffer de Datos.

            gl.EnableVertexAttribArray(attribIndex); //Habilitamos el indice de atributo.
            gl.BindBuffer(bufferType, h_VBO_t); //Seleccionamos el buffer a utilizar.
            gl.VertexAttribPointer(attribIndex, cantComponentes, attribType, false, stride, offset);//Configuramos el layout (como estan organizados) los datos en el buffer.


            // 3. Configuramos el EBO a utilizar. (como son indices, no necesitan info de layout)
            bufferType = BufferTarget.ElementArrayBuffer;
            gl.BindBuffer(bufferType, h_EBO);

            // 4. Deseleccionamos el VAO.
            gl.BindVertexArray(0);
        }

        public void Dibujar(ShaderProgram sProgram)
        {

            PrimitiveType primitive; //Tipo de Primitiva a utilizar (triangulos, strip, fan, quads, ..)
            int offset; // A partir de cual indice dibujamos?
            int count;  // Cuantos?
            DrawElementsType indexType; //Tipo de los indices.

            primitive = PrimitiveType.Triangles;  //Usamos trianglos.
            offset = 0;  // A partir del primer indice.

            count = indices.Length; // Todos los indices.
            indexType = DrawElementsType.UnsignedInt; //Los indices son enteros sin signo.

            gl.BindVertexArray(h_VAO); //Seleccionamos el VAO a utilizar.
            gl.DrawElements(primitive, count, indexType, offset); //Dibujamos utilizando los indices del VAO.
            gl.BindVertexArray(0); //Deseleccionamos el VAO
        }

        /*

                void OrdenarDatos(bool siText, bool siNormal, out Vector3[] posiciones, out Vector3[] coorTex, out Vector3[] normales, out int[] Indices)
                {
                    int k = 0;
                    int cantCaras = faceList.Count;
                    Indices = new int[cantCaras * 3]; //OJO solo si  TODAS las caras son triágulos
                    posiciones = new Vector3[cantCaras * 3];
                    coorTex = new Vector3[cantCaras * 3];
                    normales = new Vector3[cantCaras * 3];

                    for (int i = 0; i < cantCaras; i++)
                    {
                        FVLFace Cara = faceList[i];
                        int[] indPosF = Cara.VertexIndexes.ToArray();
                        int[] indTexF = Cara.TexCordIndexes.ToArray();
                        int[] indNormF = Cara.NormalIndexes.ToArray();

                        for (int j = 0; j < 3; j++)
                        {
                            posiciones[k] = VertexList[indPosF[j]];

                            if (siText)
                                coorTex[k] = new Vector3(texCordList[indTexF[j]].X, texCordList[indTexF[j]].Y, 1.0f);

                            if (siNormal)
                                normales[k] = VertexNormalList[indNormF[j]];

                            Indices[k] = k;
                            k = k + 1;
                        }

                    }

                }
                */


        //Devuelve un arreglo con las posiciones ordenadas (repetidas) segun el indice.
        public Vector3[] ordenarPosiciones()
        {
            List<Vector3> ListaPosiciones = new List<Vector3>();
            int cantFaces = faceList.Count;            
            for (int f = 0; f < cantFaces; f++)
            {
                FVLFace cara = faceList[f];
                List<int> pos = cara.VertexIndexes;
                for (int n = 0; n < cara.VertexCount; n++)
                {
                    ListaPosiciones.Add(vertexList[pos[n]]);
                }
            }
            return ListaPosiciones.ToArray();

        }

        //Devuelve un arreglo con las normales ordenadas para cada vertice de posiciones (repetidas).
        public Vector3[] ordenarNormales()
        {
            List<Vector3> ListaNormales = new List<Vector3>();
            int cantFaces = faceList.Count;
            for (int f = 0; f < cantFaces; f++)
            {
                FVLFace cara = faceList[f];
                List<int> nor = cara.NormalIndexes;
                for (int n = 0; n < cara.VertexCount; n++)
                {
                    ListaNormales.Add(vertexNormalList[nor[n]]);
                }
            }
            return ListaNormales.ToArray();

        }

        //Devuelve un arreglo con las normales ordenadas para cada vertice de posiciones (repetidas).
        public Vector3[] ordenarTexturas()
        {
            List<Vector3> ListaTexturas = new List<Vector3>();
            int cantFaces = faceList.Count;
            for (int f = 0; f < cantFaces; f++)
            {
                FVLFace cara = faceList[f];
                List<int> texInd = cara.TexCordIndexes;
                for (int n = 0; n < cara.VertexCount; n++)
                {
                    ListaTexturas.Add(new Vector3(texCordList[texInd[n]].X, texCordList[texInd[n]].Y, 1.0f));
                }
            }
            return ListaTexturas.ToArray();

        }
        /*
        //Devuelve un arreglo con las normales ordenadas para cada vertice de vertexList.
        public Vector3[] ordenarTexturas()
        {

            Vector3[] ListaTexturas = new Vector3[vertexList.Count];
            int cantFaces = faceList.Count;
            for (int f = 0; f < cantFaces; f++)
            {
                FVLFace cara = faceList[f];
                List<int> texInd = cara.TexCordIndexes;
                for (int n = 0; n < cara.VertexCount; n++)
                {
                    float x = texCordList[texInd[n]].X;
                    float y = texCordList[texInd[n]].Y;
                    ListaTexturas[cara.VertexIndexes[n]] = new Vector3(x, y, 1.0f);
                }
            }
            return ListaTexturas;
        }
        */
        //Devuelve un arreglo donde la componente 2i es la posicion, y la 2i+1 es la normal; para dibujar las normales
        private Vector3[] posicionesNormales(Vector3[] listanormales, Vector3[] listaVert)
        {

            Vector3[] lista = new Vector3[listanormales.Length + listaVert.Length];

            for (int i = 0; i < listaVert.Length; i++)
            {
                lista[2 * i] = listaVert[i];
                lista[2 * i + 1] = listanormales[i] / 10 + listaVert[i];
            }

            return lista;
        }
        //Devuelve un arreglo con las normales suavizadas (promedio de todas las del vertice) para cada vertice de posiciones (repetidas).
        public Vector3[] suavizarNormales()
        {

            Vector3[] ListaNormales = new Vector3[vertexList.Count];
            int cantFaces = faceList.Count;
            for (int f = 0; f < cantFaces; f++)
            {
                FVLFace cara = faceList[f];
                List<int> nor = cara.NormalIndexes;
                for (int n = 0; n < cara.VertexCount; n++)
                {
                    if (ListaNormales[cara.VertexIndexes[n]] == null)
                        ListaNormales[cara.VertexIndexes[n]] = Vector3.Normalize(vertexNormalList[cara.NormalIndexes[n]]);
                    else ListaNormales[cara.VertexIndexes[n]] = Vector3.Normalize(ListaNormales[cara.VertexIndexes[n]] + vertexNormalList[cara.NormalIndexes[n]]);
                }
            }

            int[] indicesPos = CarasToIndices();
            List<Vector3> ListaNormalesSuavizadas = new List<Vector3>();
            foreach (int i in indicesPos)
            {
                ListaNormalesSuavizadas.Add(ListaNormales[i]);
            }

            return ListaNormalesSuavizadas.ToArray();

        }

        //Devuelve un arreglo con los indices de VertexList ordenados segun el orden de las cara
        private int[] CarasToIndices()
        {

            int cantFaces = faceList.Count;

            int[] arrayIndices = new int[cantFaces * 3]; //OJO solo si  TODAS las caras son triágulos

            int i = 0;
            for (int f = 0; f < cantFaces; f++)
            {
                FVLFace cara = faceList[f];
                int[] indicesCara = cara.IndicesDeCara();
                int cuantosVertices = cara.VertexCount;
                for (int j = 0; j < cuantosVertices; j++)
                {
                    arrayIndices[i] = indicesCara[j];
                    i++;
                }

            }
            return arrayIndices;
        }

    }
}
    
