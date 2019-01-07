using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using OpenTK;
using System.Globalization;

namespace BlackOut
{
    class MtlFileParser
    {

        private const String NEWMTL = "newmtl";
        private const String MAPKD = "map_Kd";
        private const String MAPBUMP = "map_Bump";
        private const String KA = "Ka";
        private const String KD = "Kd";
        private const String KS = "Ks";
        private const String Ns = "Ns"; //shinines
        private const Char COMMENT = '#';
        private static Char[] SEPARATORS = { ' ' };
        private static bool logEnabled = false;

        public static List<MatConTextura> parseFile(String fileName)
        {
            List<MatConTextura> materiales = new List<MatConTextura>();
            String line;
            String[] lineSplit;
            StreamReader file = new StreamReader(fileName);
            line = file.ReadLine();
            while (line != null)
            {
                line = line.Trim(); //Saco espacios en blanco.
                if ((line.Length != 0) && (!line[0].Equals(COMMENT))) //Si no es comentario
                {
                    lineSplit = line.Split(SEPARATORS, StringSplitOptions.RemoveEmptyEntries);
                    if (lineSplit[0].Equals(NEWMTL))
                    {
                        MatConTextura material = new MatConTextura();
                        material.NombreMaterial = lineSplit[1];
                        materiales.Add(material);
                    }
                    else if (lineSplit[0].Equals(MAPKD))
                    {
                        parseMapkd(materiales, lineSplit);
                    }
                    else if (lineSplit[0].Equals(MAPBUMP))
                    {
                        parseMapbump(materiales, lineSplit);
                    }
                    else if (lineSplit[0].Equals(KA))
                    {
                        parseMatKa(materiales, lineSplit);
                    }
                    else if (lineSplit[0].Equals(KD))
                    {
                        parseMatKd(materiales, lineSplit);
                    }
                    else if (lineSplit[0].Equals(KS))
                    {
                        parseMatKs(materiales, lineSplit);
                    }
                    else if (lineSplit[0].Equals(Ns))
                    {
                        parseMatNs(materiales, lineSplit);
                    }
                }
                line = file.ReadLine();
            }
            file.Close();
            return materiales;
        }

        public static void parseMatKa(List<MatConTextura> materiales, String[] args)
        {

            if (materiales.Count() > 0)
            {
                MatConTextura material = materiales.Last(); 
                switch (args.Length)
                {
                    case 4:
                        float kax = float.Parse(args[1], System.Globalization.NumberStyles.Number);
                        float kay = float.Parse(args[2], CultureInfo.InvariantCulture);
                        float kaz = float.Parse(args[3], CultureInfo.InvariantCulture);
                        material.Kambient = new Vector3(kax, kay, kaz);
                        break;
                    default:
                        break;
                }
            }
        }

        public static void parseMatKd(List<MatConTextura> materiales, String[] args)
        {
            if (materiales.Count() > 0)
            {
                MatConTextura material = materiales.Last();
                switch (args.Length)
                {
                    case 4:
                        float kdx = float.Parse(args[1], CultureInfo.InvariantCulture);
                        float kdy = float.Parse(args[2], CultureInfo.InvariantCulture);
                        float kdz = float.Parse(args[3], CultureInfo.InvariantCulture);
                        material.Kdiffuse = new Vector3(kdx, kdy, kdz);
                        break;
                    default:
                        break;
                }
            }
        }

        public static void parseMatKs(List<MatConTextura> materiales, String[] args)
        {
            if (materiales.Count() > 0)
            {
                MatConTextura material = materiales.Last();
                switch (args.Length)
                {
                    case 4:
                        float ksx = float.Parse(args[1], CultureInfo.InvariantCulture);
                        float ksy = float.Parse(args[2], CultureInfo.InvariantCulture);
                        float ksz = float.Parse(args[3], CultureInfo.InvariantCulture);
                        material.Kspecular = new Vector3(ksx, ksy, ksz);
                        break;
                    default:
                        break;
                }
            }
        }

        public static void parseMapkd(List<MatConTextura> materiales, String[] args)
        {
            if (materiales.Count() > 0)
            {
                MatConTextura material = materiales.Last();
                switch (args.Length)
                {
                    case 2:
                        String imagenTex = args[1];
                        material.ImagenTex = imagenTex;
                        break;
                    default:
                        break;
                }
            }
        }

        public static void parseMapbump(List<MatConTextura> materiales, String[] args)
        {
            if (materiales.Count() > 0)
            {
                MatConTextura material = materiales.Last();
                switch (args.Length)
                {
                    case 2:
                        String imagenTex = args[1];
                        material.ImagenTexBump = imagenTex;
                        break;
                    default:
                        break;
                }
            }
        }

        public static void parseMatNs(List<MatConTextura> materiales, String[] args)
        {
            if (materiales.Count() > 0)
            {
                MatConTextura material = materiales.Last();
                switch (args.Length)
                {
                    case 2:
                        material.Shininess = float.Parse(args[1], CultureInfo.InvariantCulture);
                        break;
                    default:
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
