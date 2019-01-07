using System;
using OpenTK;

namespace CGUNS.Cameras
{
    /// <summary>
    /// Representa una Camara en primera persona
    /// La cámara comienza en la posición (0,2,0)
    /// El vector "up" de la camara es el eje "Y" (0,3,0).
    /// Es posible realizar giros a izquierda y derecha
    /// </summary>
    class FirstPersonCamera
    {
        private const float DEG2RAD = (float)(Math.PI / 180.0); //Para pasar de grados a radianes

        private Matrix4 projMatrix; //Matriz de Proyeccion.

        private float theta; //Angulo en el plano horizontal (XZ) desde el eje X+ (grados)
        private float phi; //Angulo desde el eje Y+. (0, 180)  menos un epsilon. (grados)

        //Valores necesarios para calcular la Matriz de Vista.
        private Vector3 eye = new Vector3(3.0f, 4.0f, -3.0f);
        public Vector3 target = new Vector3(3.0f, 4.0f, -20.0f);
        private Vector3 up = Vector3.UnitY;
        public Vector3 direccion;

        public FirstPersonCamera()
        {
            //La matriz de proyeccion queda fija
            float fovy = 50 * DEG2RAD; //50 grados de angulo.
            float aspectRadio = 1; //Cuadrado
            float zNear = 0.1f; //Plano Near
            float zFar = 100f;  //Plano Far
            projMatrix = Matrix4.CreatePerspectiveFieldOfView(fovy, aspectRadio, zNear, zFar);
            direccion = target - eye;
        }

        /// <summary>
        /// Retorna la Matriz de Projeccion que esta utilizando esta camara.
        /// </summary>
        /// <returns></returns>
        public Matrix4 getProjectionMatrix()
        {
            return projMatrix;
        }
        /// <summary>
        /// Retorna la Matriz de Vista que representa esta camara.
        /// </summary>
        /// <returns></returns>
        public Matrix4 getViewMatrix()
        {
            return Matrix4.LookAt(eye, target, up);
        }

        public Vector3 getPosition()
        {
            return eye;
        }

        public void MirarIzquierda(Matrix4 rotacion)
        {
            direccion = Vector3.Transform(direccion, rotacion);
            target = direccion + eye;
        }

        public void MirarDerecha(Matrix4 rotacion)
        {
            direccion = Vector3.Transform(direccion, rotacion);
            target = direccion + eye;
        }

        public void Atras(Matrix4 transform)
        {
            eye = transform.Row3.Xyz + new Vector3(0.0f, 2.0f, 0.0f);
            target = direccion + eye;
        }

        public void Adelante(Matrix4 transform)
        {
            eye = transform.Row3.Xyz + new Vector3(0.0f, 2.0f, 0.0f);
            target = direccion + eye;
        }

        private void log(String format, params Object[] args)
        {
            System.Diagnostics.Debug.WriteLine(String.Format(format, args), "[Camera]");
        }
    }
}