using OpenTK;
using System;

namespace BlackOut
{
    public class MatConTextura
    {

        String nombreMaterial;
        Vector3 kambient;
        Vector3 kdiffuse;
        Vector3 kspecular;
        float shininess;
        String imagenTex; //TEXTURA
        String imagenTexBump; //TEXTURA

        public MatConTextura()
        {
            nombreMaterial = "";
            kambient = new Vector3();
            kdiffuse = new Vector3();
            kspecular = new Vector3();
            imagenTex = "";
            imagenTexBump = "";
            shininess = 0.0f;
        }

        public MatConTextura(String nombreMaterial, Vector3 kambient, Vector3 kdiffuse, Vector3 kspecular, float shininess, String imagenTex, String imagenTexBump)
        {
            this.nombreMaterial = nombreMaterial;
            this.kambient = kambient;
            this.kdiffuse = kdiffuse;
            this.kspecular = kspecular;
            this.shininess = shininess;
            this.imagenTex = imagenTex;
            this.imagenTexBump = imagenTexBump;
        }

        public Vector3 Kambient
        {
            get
            {
                return kambient;
            }
            set
            {
                this.kambient = value;
            }
        }
        public Vector3 Kdiffuse
        {
            get
            {
                return kdiffuse;
            }
            set
            {
                this.kdiffuse = value;
            }
        }
        public Vector3 Kspecular
        {
            get
            {
                return kspecular;
            }
            set
            {
                this.kspecular = value;
            }
        }
        public float Shininess
        {
            get
            {
                return shininess;
            }
            set
            {
                this.shininess = value;
            }
        }
        public String ImagenTex
        {
            get
            {
                return imagenTex;
            }
            set
            {
                this.imagenTex = value;
            }
        }
        public String NombreMaterial
        {
            get
            {
                return nombreMaterial;
            }
            set
            {
                this.nombreMaterial = value;
            }
        }

        public string ImagenTexBump
        {
            get
            {
                return imagenTexBump;
            }

            set
            {
                imagenTexBump = value;
            }
        }
    }
}
