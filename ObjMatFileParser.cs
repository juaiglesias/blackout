using CGUNS.Meshes.FaceVertexList;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace BlackOut
{
    class ObjMatFileParser
    {
        private const String VERTEX = "v";
        private const String FACE = "f";
        private const String NORMAL = "vn";
        private const String TEXCORD = "vt";
        private const String USEMTL = "usemtl";
        private const String O = "o"; //nuevo objeto
        private const Char COMMENT = '#';
        private static Char[] SEPARATORS = { ' ', '/' };
        private static bool logEnabled = false;


        public static List<FVLMesh> parseFile(String objFileName, String mtlFileName)
        {
            List<FVLMesh> listaMeshes = new List<FVLMesh>();
            FVLMesh mesh = new FVLMesh();
            //Lista de todos los f, v, vn y vt del obj
            List<Vector3> vertexList = new List<Vector3>();
            List<Vector2> texCordList = new List<Vector2>();
            List<Vector3> vertexNormalList = new List<Vector3>();

            //Parseo el archivo de materiales
            List<MatConTextura> listaMateriales = MtlFileParser.parseFile(mtlFileName);

            String line;
            String[] lineSplit;

            StreamReader file = new StreamReader(objFileName);
            line = file.ReadLine();
            while (line != null)
            {
                line = line.Trim(); //Saco espacios en blanco.
                if ((line.Length != 0) && (!line[0].Equals(COMMENT))) //Si no es comentario
                {
                    lineSplit = line.Split(SEPARATORS, StringSplitOptions.RemoveEmptyEntries);
                    if (lineSplit[0].Equals(VERTEX))
                    {
                        parseVertex(mesh, lineSplit, vertexList);
                    }
                    else if (lineSplit[0].Equals(NORMAL))
                    {
                        parseNormal(mesh, lineSplit, vertexNormalList);
                    }

                    else if (lineSplit[0].Equals(FACE))
                    {
                        parseFace(mesh, line);//HERE!!
                    }

                    else if (lineSplit[0].Equals(TEXCORD))
                    {
                        parseTexCord(mesh, lineSplit, texCordList);
                    }

                    else if (lineSplit[0].Equals(USEMTL))
                    {
                        parseMtl(mesh, lineSplit, listaMateriales);
                    }
                    else if (lineSplit[0].Equals(O))
                    {
                        mesh = new FVLMesh();
                        mesh.NombreObjeto = lineSplit[1];
                        //Agrego todos los v, vn y vt a la lista del subobjeto (para que no se desacomoden los indices en el build)
                        foreach(Vector3 vertex in vertexList)
                        {
                            mesh.AddVertex(vertex);
                        }
                        foreach (Vector3 vertexNormal in vertexNormalList)
                        {
                            mesh.AddVertexNormal(vertexNormal);
                        }
                        foreach (Vector2 texCoord in texCordList)
                        {
                            mesh.AddTexCord(texCoord);
                        }

                        listaMeshes.Add(mesh);
                    }
                }
                line = file.ReadLine();
            }
            file.Close();
            return listaMeshes;
        }

        public static void parseVertex(FVLMesh mesh, String[] args, List<Vector3> vertexList)
        {
            String sender = "ObjFileParser.parseVertex: ";
            Vector3 vertex = new Vector3();
            switch (args.Length)
            {
                case 2:
                    log(sender, "Vertex definition must be (x,y,z [,w]). Found only x.");
                    vertex.X = float.Parse(args[1], System.Globalization.NumberStyles.Number);
                    vertex.Y = 0.0f;
                    vertex.Z = 0.0f;
                    //vertex.W = 1.0f;
                    break;
                case 3:
                    log(sender, "Vertex definition must be (x,y,z [,w]). Found only x, y.");
                    vertex.X = float.Parse(args[1], CultureInfo.InvariantCulture);
                    vertex.Y = float.Parse(args[2], CultureInfo.InvariantCulture);
                    vertex.Z = 0.0f;
                    //vertex.W = 1.0f;
                    break;
                case 4:
                    vertex.X = float.Parse(args[1], CultureInfo.InvariantCulture);
                    vertex.Y = float.Parse(args[2], CultureInfo.InvariantCulture);
                    vertex.Z = float.Parse(args[3], CultureInfo.InvariantCulture);
                    //vertex.W = 1.0f;
                    break;
                case 5:
                    log(sender, "Found (x, y, z, w). Discarding w component.");
                    vertex.X = float.Parse(args[1], CultureInfo.InvariantCulture);
                    vertex.Y = float.Parse(args[2], CultureInfo.InvariantCulture);
                    vertex.Z = float.Parse(args[3], CultureInfo.InvariantCulture);
                    //vertex.W = float.Parse(args[4], System.Globalization.NumberStyles.Number);
                    break;
                default:
                    break;
            }
            mesh.AddVertex(vertex);
            vertexList.Add(vertex);
            //vertexCount++;
        }

        public static void parseNormal(FVLMesh mesh, String[] args, List<Vector3> vertexNormalList)
        {
            Vector3 vertex = new Vector3();
            vertex.X = float.Parse(args[1], System.Globalization.NumberStyles.Float, new System.Globalization.CultureInfo("en-US"));
            vertex.Y = float.Parse(args[2], System.Globalization.NumberStyles.Float, new System.Globalization.CultureInfo("en-US"));
            vertex.Z = float.Parse(args[3], System.Globalization.NumberStyles.Float, new System.Globalization.CultureInfo("en-US"));
            mesh.AddVertexNormal(vertex);
            vertexNormalList.Add(vertex);
        }

        public static void parseTexCord(FVLMesh mesh, String[] args, List<Vector2> texCordList)
        {
            Vector2 texCord = new Vector2();
            texCord.X = float.Parse(args[1], System.Globalization.NumberStyles.Float, new System.Globalization.CultureInfo("en-US"));
            texCord.Y = float.Parse(args[2], System.Globalization.NumberStyles.Float, new System.Globalization.CultureInfo("en-US"));
            mesh.AddTexCord(texCord);
            texCordList.Add(texCord);
        }


        public static void parseFace(FVLMesh mesh, string line)
        {
            FVLFace face = new FVLFace();

            int i = 2; // componente 1 = f , comp 2 = ' '
            String vertex;
            String texCord;
            String normal;

            while (i < line.Length)
            {
                vertex = "";
                texCord = "";
                normal = "";
                while (i < line.Length && line[i] != ' ' && line[i] != '/')
                {
                    vertex = vertex + line[i];
                    i++;
                }

                if (i < line.Length && line[i] != ' ')
                {
                    i++;
                    if (line[i] != '/')
                    {
                        while (i < line.Length && line[i] != ' ' && line[i] != '/')
                        {
                            texCord = texCord + line[i];
                            i++;
                        }
                    }
                    i++;

                    if (i < line.Length && line[i] != '/')
                    {
                        while (i < line.Length && line[i] != ' ' && line[i] != '/')
                        {
                            normal = normal + line[i];
                            i++;
                        }
                    }
                }

                i++;
                face.AddVertex(Int32.Parse(vertex, NumberStyles.Integer) - 1);
                if (!normal.Equals(""))
                {
                    face.AddNormal(Int32.Parse(normal, NumberStyles.Integer) - 1);
                }
                if (!texCord.Equals(""))
                {
                    face.AddTexCord(Int32.Parse(texCord, NumberStyles.Integer) - 1);
                }
            }

            mesh.AddFace(face);
        }


        public static void parseMtl(FVLMesh mesh, String[] args, List<MatConTextura> listaMateriales)
        {
            MatConTextura material;

            foreach (MatConTextura m in listaMateriales)
            {
                if (m.NombreMaterial.Equals(args[1]))
                {
                    material = m;
                    mesh.Material = material;
                    break;
                }
            }
        }


        private static void log(String sender, String format, params Object[] args)
        {
            if (logEnabled)
            {
                System.Console.WriteLine(sender + format, args);
            }
        }
        private static void info(String sender, String format, params Object[] args)
        {
            System.Console.WriteLine(sender + format, args);
        }
    }
}
