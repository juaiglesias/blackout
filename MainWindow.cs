using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using OpenTK; //La matematica
using OpenTK.Graphics.OpenGL;
using gl = OpenTK.Graphics.OpenGL.GL;
using CGUNS.Shaders;
using CGUNS.Cameras;
using CGUNS;
using CGUNS.Primitives;
using System.Drawing.Imaging;
using System.Media;

namespace BlackOut
{
    public partial class MainWindow : Form
    {
        public MainWindow(Inicio i)
        {
            InitializeComponent();
            playSound();
            a = i;
            i.SuspendLayout();
        }

        private FirstPersonCamera myCamera;  //Camara
        private Rectangle viewport; //Viewport a utilizar (Porcion del glControl en donde voy a dibujar).

        private ShaderProgram sProgram_phong; //Nuestro programa de shaders.
        private ShaderProgram sProgram_gouraud;
        private ShaderProgram sProgram_ejes;
        private ShaderProgram sProgram_textphong;
        private ShaderProgram sProgram_bump;
        private Inicio a;

        //--------------------------------------------  OBJETOS  ---------------------------------------------------------------

        private bool encontroLlave = false;
        private bool gano = false;
        private Ejes myAxis;
        private Objeto piso;
        private Objeto techo;
        private Objeto estatua;
        private Objeto barril1;
        private Objeto barril2;
        private Objeto objMovimiento;
        private Objeto pared1;
        private Objeto pared2;
        private Objeto pared3;
        private Objeto pared4;
        private Objeto pared5;
        private Objeto pared6;
        private Objeto pared7;
        private Objeto pared8;
        private Objeto pared9;
        private Objeto pared10;
        private Objeto pared11;
        private Objeto pared12;
        private Objeto pared13;
        private Objeto pared14;
        private Objeto pared15;
        private Objeto pared16;
        private Objeto pared17;
        private Objeto pared18;
        private Objeto pared19;
        private Objeto pared20;
        private Objeto pared21;
        private Objeto pared22;
        private Objeto pared23;
        private Objeto pared24;
        private Objeto pared25;
        private Objeto pared26;
        private Objeto pared27;
        private Objeto pared28;
        private Objeto pared29;
        private Objeto pared30;
        private Objeto pared31;
        private Objeto pared32;
        private Objeto pared33;
        private Objeto pared34;
        private Objeto pared35;
        private Objeto pared36;
        private Objeto pared37;
        private Objeto pared38;
        private Objeto pared39;
        private Objeto pared40;
        private Objeto araña;
        private Objeto cadena;
        private Objeto llave;
        //private Objeto cubo;
        private Objeto cofre;
        private Objeto bolsas;
        private Objeto armas;
        private Objeto puerta;
        private Objeto puerta2;



        public float timer = 0.00f;

        private List<Objeto> colisiones = new List<Objeto>();

        //MATERIALES Y LUCES
        private Material[] myMaterials = new Material[] { Material.Brass, Material.Bronze, Material.Gold, Material.Jade, Material.Obsidian };
        private int materialIndex = 0;
        private Material myMaterial;
        private Light myLight;
        private Light myLightCubo;
        private Light[] luces; //Mis multiples luces.


        private float translationX1 = 3.0f;
        private float translationZ1 = -3.0f;

        private float rotY = 0.0f;
        private float rotZ = 0.0f;


        Matrix4 transformcubo = Matrix4.Mult(Matrix4.CreateTranslation(3.0f, 2.0f, -3.0f), Matrix4.Identity);

        private float pi = 3.1415f;

        private void glControl3_Load(object sender, EventArgs e)
        {
            logContextInfo(); //Mostramos info de contexto.
            SetupShaders(); //Creamos los shaders y el programa de shader

            myCamera = new FirstPersonCamera(); //Creo una camara.       

            gl.ClearColor(Color.LightGray); //Configuro el Color de borrado.
            gl.Enable(EnableCap.DepthTest);
            gl.ClearColor(Color.White); //Configuro el Color de borrado.
            gl.PolygonMode(MaterialFace.Front, PolygonMode.Fill);
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);

            //------------------------------------------------  LUCES  --------------------------------------------------------------
            luces = new Light[0];


            myLight = new Light();
            myLight.Position = new Vector4(4.0f, 4.0f, -4.0f, 1.0f);//spot desde arriba (w=1)
            myLight.Iambient = new Vector3(0.0f, 0.0f, 0.0f);
            myLight.Idiffuse = new Vector3(0.9f, 0.9f, 0.9f);
            myLight.Ispecular = new Vector3(1.8f, 1.8f, 1.8f);
            myLight.ConeAngle = 360.0f;
            myLight.ConeDirection = new Vector3(0.0f, -1.0f, 0.0f);
            myLight.Enabled = 1;

            myLightCubo = new Light();
            myLightCubo.Position = new Vector4(0.0f, 4.0f, -2.0f, 1.0f);//spot desde arriba (w=1)
            myLightCubo.Iambient = new Vector3(0.0f, 0.0f, 0.0f);
            myLightCubo.Idiffuse = new Vector3(0.5f, 0.5f, 0.5f);
            myLightCubo.Ispecular = new Vector3(0.5f, 0.5f, 0.5f);
            myLightCubo.ConeAngle = 360.0f;
            myLightCubo.ConeDirection = new Vector3(0.0f, -1.0f, 0.0f);
            myLightCubo.Enabled = 1;


            //Creo los objetos
            myAxis = new Ejes(3.0f);
            myAxis.Build(sProgram_ejes);

            piso = new Objeto("ModelosOBJ/piso.obj", "ModelosOBJ/piso.mtl", sProgram_textphong);
            techo = new Objeto("ModelosOBJ/piso.obj", "ModelosOBJ/techo.mtl", sProgram_textphong);
            estatua = new Objeto("ModelosOBJ/estatua.obj", "ModelosOBJ/estatua.mtl", sProgram_textphong);
            barril1 = new Objeto("ModelosOBJ/barril.obj", "ModelosOBJ/barril.mtl", sProgram_textphong);
            barril2 = new Objeto("ModelosOBJ/barril.obj", "ModelosOBJ/barril.mtl", sProgram_textphong);
            objMovimiento = new Objeto("ModelosOBJ/kubo2.obj", "ModelosOBJ/kubo2.mtl", sProgram_textphong);
            pared1 = new Objeto("ModelosOBJ/pared.obj", "ModelosOBJ/pared.mtl", sProgram_textphong);
            pared2 = new Objeto("ModelosOBJ/pared.obj", "ModelosOBJ/pared.mtl", sProgram_textphong);
            pared3 = new Objeto("ModelosOBJ/pared.obj", "ModelosOBJ/pared.mtl", sProgram_textphong);
            pared4 = new Objeto("ModelosOBJ/pared.obj", "ModelosOBJ/pared.mtl", sProgram_textphong);
            pared5 = new Objeto("ModelosOBJ/pared.obj", "ModelosOBJ/pared.mtl", sProgram_textphong);
            pared6 = new Objeto("ModelosOBJ/pared.obj", "ModelosOBJ/pared.mtl", sProgram_textphong);
            pared7 = new Objeto("ModelosOBJ/pared2.obj", "ModelosOBJ/pared2.mtl", sProgram_textphong);
            pared8 = new Objeto("ModelosOBJ/pared2.obj", "ModelosOBJ/pared2.mtl", sProgram_textphong);
            pared9 = new Objeto("ModelosOBJ/pared.obj", "ModelosOBJ/pared.mtl", sProgram_textphong);
            pared10 = new Objeto("ModelosOBJ/pared.obj", "ModelosOBJ/pared.mtl", sProgram_textphong);
            pared11 = new Objeto("ModelosOBJ/pared2.obj", "ModelosOBJ/pared2.mtl", sProgram_textphong);
            pared12 = new Objeto("ModelosOBJ/pared.obj", "ModelosOBJ/pared.mtl", sProgram_textphong);
            pared13 = new Objeto("ModelosOBJ/pared.obj", "ModelosOBJ/pared.mtl", sProgram_textphong);
            pared14 = new Objeto("ModelosOBJ/pared.obj", "ModelosOBJ/pared.mtl", sProgram_textphong);
            pared15 = new Objeto("ModelosOBJ/pared2.obj", "ModelosOBJ/pared2.mtl", sProgram_textphong);
            pared16 = new Objeto("ModelosOBJ/pared2.obj", "ModelosOBJ/pared2.mtl", sProgram_textphong);
            pared17 = new Objeto("ModelosOBJ/pared2.obj", "ModelosOBJ/pared2.mtl", sProgram_textphong);
            pared18 = new Objeto("ModelosOBJ/pared2.obj", "ModelosOBJ/pared2.mtl", sProgram_textphong);
            pared19 = new Objeto("ModelosOBJ/pared2.obj", "ModelosOBJ/pared2.mtl", sProgram_textphong);
            pared20 = new Objeto("ModelosOBJ/pared2.obj", "ModelosOBJ/pared2.mtl", sProgram_textphong);
            pared21 = new Objeto("ModelosOBJ/pared2.obj", "ModelosOBJ/pared2.mtl", sProgram_textphong);
            pared22 = new Objeto("ModelosOBJ/pared2.obj", "ModelosOBJ/pared2.mtl", sProgram_textphong);
            pared23 = new Objeto("ModelosOBJ/pared.obj", "ModelosOBJ/pared.mtl", sProgram_textphong);
            pared24 = new Objeto("ModelosOBJ/pared.obj", "ModelosOBJ/pared.mtl", sProgram_textphong);
            pared25 = new Objeto("ModelosOBJ/pared.obj", "ModelosOBJ/pared.mtl", sProgram_textphong);
            pared26 = new Objeto("ModelosOBJ/pared.obj", "ModelosOBJ/pared.mtl", sProgram_textphong);
            pared27 = new Objeto("ModelosOBJ/pared.obj", "ModelosOBJ/pared.mtl", sProgram_textphong);
            pared28 = new Objeto("ModelosOBJ/pared.obj", "ModelosOBJ/pared.mtl", sProgram_textphong);
            pared29 = new Objeto("ModelosOBJ/pared2.obj", "ModelosOBJ/pared2.mtl", sProgram_textphong);
            pared30 = new Objeto("ModelosOBJ/pared2.obj", "ModelosOBJ/pared2.mtl", sProgram_textphong);
            pared31 = new Objeto("ModelosOBJ/pared2.obj", "ModelosOBJ/pared2.mtl", sProgram_textphong);
            pared32 = new Objeto("ModelosOBJ/pared.obj", "ModelosOBJ/pared.mtl", sProgram_textphong);
            pared33 = new Objeto("ModelosOBJ/pared2.obj", "ModelosOBJ/pared2.mtl", sProgram_textphong);
            pared34 = new Objeto("ModelosOBJ/pared2.obj", "ModelosOBJ/pared2.mtl", sProgram_textphong);
            pared35 = new Objeto("ModelosOBJ/pared.obj", "ModelosOBJ/pared.mtl", sProgram_textphong);
            pared36 = new Objeto("ModelosOBJ/pared2.obj", "ModelosOBJ/pared2.mtl", sProgram_textphong);
            pared37 = new Objeto("ModelosOBJ/pared2.obj", "ModelosOBJ/pared2.mtl", sProgram_textphong);
            pared38 = new Objeto("ModelosOBJ/pared.obj", "ModelosOBJ/pared.mtl", sProgram_textphong);
            pared39 = new Objeto("ModelosOBJ/pared.obj", "ModelosOBJ/pared.mtl", sProgram_textphong);
            pared40 = new Objeto("ModelosOBJ/pared.obj", "ModelosOBJ/pared.mtl", sProgram_textphong);
            araña = new Objeto("ModelosOBJ/spider.obj", "ModelosOBJ/spider.mtl", sProgram_textphong);
            llave = new Objeto("ModelosOBJ/llave.obj", "ModelosOBJ/llave.mtl", sProgram_textphong);
            cadena = new Objeto("ModelosOBJ/cadenas.obj", "ModelosOBJ/cadenas.mtl", sProgram_textphong);
            //cubo = new Objeto("ModelosOBJ/cubonormal.obj", "ModelosOBJ/cubonormal.mtl", sProgram_bump);
            cofre = new Objeto("ModelosOBJ/cofre.obj", "ModelosOBJ/cofre.mtl", sProgram_textphong);
            bolsas = new Objeto("ModelosOBJ/bolsas.obj", "ModelosOBJ/bolsas.mtl", sProgram_textphong);
            armas = new Objeto("ModelosOBJ/armas.obj", "ModelosOBJ/armas.mtl", sProgram_textphong);
            puerta = new Objeto("ModelosOBJ/puerta.obj", "ModelosOBJ/puerta.mtl", sProgram_textphong);
            puerta2 = new Objeto("ModelosOBJ/puerta.obj", "ModelosOBJ/puerta.mtl", sProgram_textphong);




            //Agrego los objetos a una lista de objetos sobre los que se puede colisionar
            colisiones.Add(barril1);
            colisiones.Add(barril2);
            colisiones.Add(estatua);
            colisiones.Add(pared1);
            colisiones.Add(pared2);
            colisiones.Add(pared3);
            colisiones.Add(pared4);
            colisiones.Add(pared5);
            colisiones.Add(pared6);
            colisiones.Add(pared7);
            colisiones.Add(pared8);
            colisiones.Add(pared9);
            colisiones.Add(pared10);
            colisiones.Add(pared11);
            colisiones.Add(pared12);
            colisiones.Add(pared13);
            colisiones.Add(pared14);
            colisiones.Add(pared15);
            colisiones.Add(pared16);
            colisiones.Add(pared17);
            colisiones.Add(pared18);
            colisiones.Add(pared19);
            colisiones.Add(pared20);
            colisiones.Add(pared21);
            colisiones.Add(pared22);
            colisiones.Add(pared23);
            colisiones.Add(pared24);
            colisiones.Add(pared25);
            colisiones.Add(pared26);
            colisiones.Add(pared27);
            colisiones.Add(pared28);
            colisiones.Add(pared29);
            colisiones.Add(pared30);
            colisiones.Add(pared31);
            colisiones.Add(pared32);
            colisiones.Add(pared33);
            colisiones.Add(pared34);
            colisiones.Add(pared35);
            colisiones.Add(pared36);
            colisiones.Add(pared37);
            colisiones.Add(pared38);
            colisiones.Add(pared39);
            colisiones.Add(pared40);
            colisiones.Add(araña);
            colisiones.Add(llave);
            colisiones.Add(cofre);
            colisiones.Add(bolsas);
            colisiones.Add(puerta);

        }

        private int CargarTextura(String imagenTex)
        {
            int texId = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, texId);


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
        private void glControl3_Paint(object sender, PaintEventArgs e)
        {
            Matrix4 modelMatrix = Matrix4.Identity; //Por ahora usamos la identidad.
            Matrix4 viewMatrix = myCamera.getViewMatrix();
            Matrix4 projMatrix = myCamera.getProjectionMatrix();
            Matrix4 mvMatrix = Matrix4.Mult(viewMatrix, modelMatrix);
            Matrix3 normalMatrix = Matrix3.Transpose(Matrix3.Invert(new Matrix3(modelMatrix)));
            Vector4 figColor = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);

            gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit); //Borramos el contenido del glControl.
            gl.Viewport(viewport); //Especificamos en que parte del glControl queremos dibujar.

            //1. DIBUJAMOS LOS EJES.(CON EL SHADER DE EJES)
            sProgram_ejes.Activate();
            sProgram_ejes.SetUniformValue("projMat", projMatrix);
            sProgram_ejes.SetUniformValue("mMat", modelMatrix);
            sProgram_ejes.SetUniformValue("vMat", viewMatrix);
            myAxis.Dibujar(sProgram_ejes);
            sProgram_ejes.Deactivate();

            //2. DIBUJAMOS EL MODELO (CON EL SHADER DE LUCES)  _________________ textshader_PHONG
            sProgram_textphong.Activate(); //Activamos el programa de shaders
                                           //Seteamos los valores uniformes.
            sProgram_textphong.SetUniformValue("projMatrix", projMatrix);
            sProgram_textphong.SetUniformValue("modelMatrix", modelMatrix);
            sProgram_textphong.SetUniformValue("viewMatrix", viewMatrix);
            sProgram_textphong.SetUniformValue("normalMatrix", normalMatrix);
            sProgram_textphong.SetUniformValue("cameraPosition", myCamera.getPosition());

            sProgram_textphong.SetUniformValue("A", 0.3f);
            sProgram_textphong.SetUniformValue("B", 0.07f);
            sProgram_textphong.SetUniformValue("C", 0.0008f);

            sProgram_textphong.SetUniformValue("myLight.position", myLight.Position);
            sProgram_textphong.SetUniformValue("myLight.Ia", myLight.Iambient);
            sProgram_textphong.SetUniformValue("myLight.Id", myLight.Idiffuse);
            sProgram_textphong.SetUniformValue("myLight.Is", myLight.Ispecular);
            //sProgram_textphong.SetUniformValue("myLight.coneAngle", myLight.ConeAngle);
            //sProgram_textphong.SetUniformValue("myLight.coneDirection", myLight.ConeDirection);
            sProgram_textphong.SetUniformValue("myLight.enabled", myLight.Enabled);
            sProgram_textphong.SetUniformValue("tim", timer);

            //----------------------------------------------------- DIBUJAMOS --------------------------------------------------------------
            Matrix4 transform;
            Matrix3 normalMatrixTransform;

            //TECHO
            transform = Matrix4.Mult(Matrix4.CreateTranslation(0.0f, 9.5f, 0.0f), modelMatrix);
            normalMatrixTransform = Matrix3.Transpose(Matrix3.Invert(new Matrix3(transform)));
            transform = Matrix4.Mult(Matrix4.CreateScale(1.1f), transform);
            sProgram_textphong.SetUniformValue("modelMatrix", transform);
            sProgram_textphong.SetUniformValue("normalMatrix", normalMatrixTransform);
            techo.paint(transform);

            //CUBO
            transformcubo = Matrix4.Mult(Matrix4.CreateTranslation(translationX1, 2.0f, translationZ1), modelMatrix);
            normalMatrixTransform = Matrix3.Transpose(Matrix3.Invert(new Matrix3(transformcubo)));
            sProgram_textphong.SetUniformValue("modelMatrix", transformcubo);
            sProgram_textphong.SetUniformValue("normalMatrix", normalMatrixTransform);
            objMovimiento.paint(transformcubo);

            //ESTATUA
            transform = Matrix4.Mult(Matrix4.CreateTranslation(17.0f, 0.0f, -17.0f), modelMatrix);
            transform = Matrix4.Mult(Matrix4.CreateRotationY(-45 * pi / 180), transform);
            transform = Matrix4.Mult(Matrix4.CreateScale(1.5f), transform);
            normalMatrixTransform = Matrix3.Transpose(Matrix3.Invert(new Matrix3(transform)));
            sProgram_textphong.SetUniformValue("modelMatrix", transform);
            sProgram_textphong.SetUniformValue("normalMatrix", normalMatrixTransform);
            estatua.paint(transform);

            //BARRIL 1
            transform = Matrix4.Mult(Matrix4.CreateTranslation(17.0f, 0.0f, -1.0f), modelMatrix);
            normalMatrixTransform = Matrix3.Transpose(Matrix3.Invert(new Matrix3(transform)));
            sProgram_textphong.SetUniformValue("modelMatrix", transform);
            sProgram_textphong.SetUniformValue("normalMatrix", normalMatrixTransform);
            barril1.paint(transform);

            //BARRIL 2
            transform = Matrix4.Mult(Matrix4.CreateTranslation(17.0f, 0.0f, -2.5f), modelMatrix);
            normalMatrixTransform = Matrix3.Transpose(Matrix3.Invert(new Matrix3(transform)));
            sProgram_textphong.SetUniformValue("modelMatrix", transform);
            sProgram_textphong.SetUniformValue("normalMatrix", normalMatrixTransform);
            barril2.paint(transform);

            // ARAÑA
            transform = Matrix4.Mult(Matrix4.CreateTranslation(5.0f, 4.0f, -53.4f), modelMatrix);
            transform = Matrix4.Mult(Matrix4.CreateRotationX(90 * pi / 180), transform);
            transform = Matrix4.Mult(Matrix4.CreateScale(0.3f), transform);
            transform = Matrix4.Mult(Matrix4.CreateTranslation(0.0f, 0.0f, 2 * (float)Math.Sin(timer / 10)), transform);
            normalMatrixTransform = Matrix3.Transpose(Matrix3.Invert(new Matrix3(transform)));
            sProgram_textphong.SetUniformValue("modelMatrix", transform);
            sProgram_textphong.SetUniformValue("normalMatrix", normalMatrixTransform);
            araña.paint(transform);            

            //PISO
            transform = Matrix4.Mult(Matrix4.CreateTranslation(0.0f, -0.5f, 0.0f), modelMatrix);
            transform = Matrix4.Mult(Matrix4.CreateScale(1.1f), modelMatrix);
            normalMatrixTransform = Matrix3.Transpose(Matrix3.Invert(new Matrix3(transform)));
            sProgram_textphong.SetUniformValue("modelMatrix", transform);
            sProgram_textphong.SetUniformValue("normalMatrix", normalMatrixTransform);

            piso.paint(transform);

            //PARED 1
            Matrix4 m1 = Matrix4.Mult(Matrix4.CreateTranslation(0.0f, 0.0f, 0.0f), modelMatrix);
            normalMatrixTransform = Matrix3.Transpose(Matrix3.Invert(new Matrix3(m1)));
            sProgram_textphong.SetUniformValue("modelMatrix", m1);
            sProgram_textphong.SetUniformValue("normalMatrix", normalMatrixTransform);
            pared1.paint(m1);

            //PARED 2
            Matrix4 m2 = Matrix4.Mult(Matrix4.CreateTranslation(0.0f, 0.0f, -9.2f), m1);
            normalMatrixTransform = Matrix3.Transpose(Matrix3.Invert(new Matrix3(m2)));
            sProgram_textphong.SetUniformValue("modelMatrix", m2);
            sProgram_textphong.SetUniformValue("normalMatrix", normalMatrixTransform);
            pared2.paint(m2);

            //PARED 3
            Matrix4 m3 = Matrix4.Mult(Matrix4.CreateTranslation(0.0f, 0.0f, -9.2f), m2);
            normalMatrixTransform = Matrix3.Transpose(Matrix3.Invert(new Matrix3(m3)));
            sProgram_textphong.SetUniformValue("modelMatrix", m3);
            sProgram_textphong.SetUniformValue("normalMatrix", normalMatrixTransform);
            pared3.paint(m3);

            //PARED 4
            Matrix4 m4 = Matrix4.Mult(Matrix4.CreateTranslation(0.0f, 0.0f, -9.2f), m3);
            normalMatrixTransform = Matrix3.Transpose(Matrix3.Invert(new Matrix3(m4)));
            sProgram_textphong.SetUniformValue("modelMatrix", m4);
            sProgram_textphong.SetUniformValue("normalMatrix", normalMatrixTransform);
            pared4.paint(m4);

            //PARED 5
            Matrix4 m5 = Matrix4.Mult(Matrix4.CreateTranslation(0.0f, 0.0f, -9.2f), m4);
            normalMatrixTransform = Matrix3.Transpose(Matrix3.Invert(new Matrix3(m5)));
            sProgram_textphong.SetUniformValue("modelMatrix", m5);
            sProgram_textphong.SetUniformValue("normalMatrix", normalMatrixTransform);
            pared5.paint(m5);

            //PARED 6
            Matrix4 m6 = Matrix4.Mult(Matrix4.CreateTranslation(0.0f, 0.0f, -9.2f), m5);
            normalMatrixTransform = Matrix3.Transpose(Matrix3.Invert(new Matrix3(m6)));
            sProgram_textphong.SetUniformValue("modelMatrix", m6);
            sProgram_textphong.SetUniformValue("normalMatrix", normalMatrixTransform);
            pared6.paint(m6);

            //PARED 7
            Matrix4 m7 = Matrix4.Mult(Matrix4.CreateTranslation(0.0f, 0.0f, 0.0f), modelMatrix); //Reseteo la posicion
            normalMatrixTransform = Matrix3.Transpose(Matrix3.Invert(new Matrix3(m7)));
            sProgram_textphong.SetUniformValue("modelMatrix", m7);
            sProgram_textphong.SetUniformValue("normalMatrix", normalMatrixTransform);
            pared7.paint(m7);

            //PARED 8
            Matrix4 m8 = Matrix4.Mult(Matrix4.CreateTranslation(8.9f, 0.0f, 0.0f), m7);
            normalMatrixTransform = Matrix3.Transpose(Matrix3.Invert(new Matrix3(m8)));
            sProgram_textphong.SetUniformValue("modelMatrix", m8);
            sProgram_textphong.SetUniformValue("normalMatrix", normalMatrixTransform);
            pared8.paint(m8);

            //PARED 9
            transform = Matrix4.Mult(Matrix4.CreateTranslation(18.4f, 0.0f, 0.0f), m1);
            normalMatrixTransform = Matrix3.Transpose(Matrix3.Invert(new Matrix3(transform)));
            sProgram_textphong.SetUniformValue("modelMatrix", transform);
            sProgram_textphong.SetUniformValue("normalMatrix", normalMatrixTransform);
            pared9.paint(transform);

            //PARED 10
            transform = Matrix4.Mult(Matrix4.CreateTranslation(18.4f, 0.0f, 0.0f), m2);
            normalMatrixTransform = Matrix3.Transpose(Matrix3.Invert(new Matrix3(transform)));
            sProgram_textphong.SetUniformValue("modelMatrix", transform);
            sProgram_textphong.SetUniformValue("normalMatrix", normalMatrixTransform);
            pared10.paint(transform);

            //PARED 11
            transform = Matrix4.Mult(Matrix4.CreateTranslation(0.35f, 0.0f, -18.8f), m8);
            normalMatrixTransform = Matrix3.Transpose(Matrix3.Invert(new Matrix3(transform)));
            sProgram_textphong.SetUniformValue("modelMatrix", transform);
            sProgram_textphong.SetUniformValue("normalMatrix", normalMatrixTransform);
            pared11.paint(transform);

            //PARED 12
            transform = Matrix4.Mult(Matrix4.CreateTranslation(9.2f, 0.0f, 0.0f), m3);
            normalMatrixTransform = Matrix3.Transpose(Matrix3.Invert(new Matrix3(transform)));
            sProgram_textphong.SetUniformValue("modelMatrix", transform);
            sProgram_textphong.SetUniformValue("normalMatrix", normalMatrixTransform);
            pared12.paint(transform);

            //PARED 13
            transform = Matrix4.Mult(Matrix4.CreateTranslation(9.2f, 0.0f, 0.0f), m4);
            normalMatrixTransform = Matrix3.Transpose(Matrix3.Invert(new Matrix3(transform)));
            sProgram_textphong.SetUniformValue("modelMatrix", transform);
            sProgram_textphong.SetUniformValue("normalMatrix", normalMatrixTransform);
            pared13.paint(transform);

            //PARED 14
            transform = Matrix4.Mult(Matrix4.CreateTranslation(9.2f, 0.0f, 0.0f), m6);
            normalMatrixTransform = Matrix3.Transpose(Matrix3.Invert(new Matrix3(transform)));
            sProgram_textphong.SetUniformValue("modelMatrix", transform);
            sProgram_textphong.SetUniformValue("normalMatrix", normalMatrixTransform);
            pared14.paint(transform);

            //PARED 15
            transform = Matrix4.Mult(Matrix4.CreateTranslation(0.0f, 0.0f, -55.2f), m7);
            normalMatrixTransform = Matrix3.Transpose(Matrix3.Invert(new Matrix3(transform)));
            sProgram_textphong.SetUniformValue("modelMatrix", transform);
            sProgram_textphong.SetUniformValue("normalMatrix", normalMatrixTransform);
            pared15.paint(transform);

            //PARED 16
            Matrix4 m16 = Matrix4.Mult(Matrix4.CreateTranslation(8.9f, 0.0f, 0.0f), m8);
            normalMatrixTransform = Matrix3.Transpose(Matrix3.Invert(new Matrix3(m16)));
            sProgram_textphong.SetUniformValue("modelMatrix", m16);
            sProgram_textphong.SetUniformValue("normalMatrix", normalMatrixTransform);
            pared16.paint(m16);

            //PARED 17
            Matrix4 m17 = Matrix4.Mult(Matrix4.CreateTranslation(8.9f, 0.0f, 0.0f), m16);
            normalMatrixTransform = Matrix3.Transpose(Matrix3.Invert(new Matrix3(m17)));
            sProgram_textphong.SetUniformValue("modelMatrix", m17);
            sProgram_textphong.SetUniformValue("normalMatrix", normalMatrixTransform);
            pared17.paint(m17);

            //PARED 18
            Matrix4 m18 = Matrix4.Mult(Matrix4.CreateTranslation(8.9f, 0.0f, 0.0f), m17);
            normalMatrixTransform = Matrix3.Transpose(Matrix3.Invert(new Matrix3(m18)));
            sProgram_textphong.SetUniformValue("modelMatrix", m18);
            sProgram_textphong.SetUniformValue("normalMatrix", normalMatrixTransform);
            pared18.paint(m18);

            //PARED 19
            transform = Matrix4.Mult(Matrix4.CreateTranslation(0.0f, 0.0f, -55.2f), m8);
            normalMatrixTransform = Matrix3.Transpose(Matrix3.Invert(new Matrix3(transform)));
            sProgram_textphong.SetUniformValue("modelMatrix", transform);
            sProgram_textphong.SetUniformValue("normalMatrix", normalMatrixTransform);
            pared19.paint(transform);

            //PARED 20
            transform = Matrix4.Mult(Matrix4.CreateTranslation(0.0f, 0.0f, -55.2f), m16);
            normalMatrixTransform = Matrix3.Transpose(Matrix3.Invert(new Matrix3(transform)));
            sProgram_textphong.SetUniformValue("modelMatrix", transform);
            sProgram_textphong.SetUniformValue("normalMatrix", normalMatrixTransform);
            pared20.paint(transform);

            //PARED 21
            transform = Matrix4.Mult(Matrix4.CreateTranslation(0.0f, 0.0f, -55.2f), m17);
            normalMatrixTransform = Matrix3.Transpose(Matrix3.Invert(new Matrix3(transform)));
            sProgram_textphong.SetUniformValue("modelMatrix", transform);
            sProgram_textphong.SetUniformValue("normalMatrix", normalMatrixTransform);
            pared21.paint(transform);

            //PARED 22
            transform = Matrix4.Mult(Matrix4.CreateTranslation(0.0f, 0.0f, -55.2f), m18);
            normalMatrixTransform = Matrix3.Transpose(Matrix3.Invert(new Matrix3(transform)));
            sProgram_textphong.SetUniformValue("modelMatrix", transform);
            sProgram_textphong.SetUniformValue("normalMatrix", normalMatrixTransform);
            pared22.paint(transform);

            //PARED 23
            transform = Matrix4.Mult(Matrix4.CreateTranslation(44.5f, 0.0f, 0.0f), m6);
            normalMatrixTransform = Matrix3.Transpose(Matrix3.Invert(new Matrix3(transform)));
            sProgram_textphong.SetUniformValue("modelMatrix", transform);
            sProgram_textphong.SetUniformValue("normalMatrix", normalMatrixTransform);
            pared23.paint(transform);

            // PARED 24
            transform = Matrix4.Mult(Matrix4.CreateTranslation(44.5f, 0.0f, 0.0f), m5);
            normalMatrixTransform = Matrix3.Transpose(Matrix3.Invert(new Matrix3(transform)));
            sProgram_textphong.SetUniformValue("modelMatrix", transform);
            sProgram_textphong.SetUniformValue("normalMatrix", normalMatrixTransform);
            pared24.paint(transform);

            // PARED 25
            transform = Matrix4.Mult(Matrix4.CreateTranslation(44.5f, 0.0f, 0.0f), m4);
            normalMatrixTransform = Matrix3.Transpose(Matrix3.Invert(new Matrix3(transform)));
            sProgram_textphong.SetUniformValue("modelMatrix", transform);
            sProgram_textphong.SetUniformValue("normalMatrix", normalMatrixTransform);
            pared25.paint(transform);

            // PARED 26
            transform = Matrix4.Mult(Matrix4.CreateTranslation(44.5f, 0.0f, 0.0f), m3);
            normalMatrixTransform = Matrix3.Transpose(Matrix3.Invert(new Matrix3(transform)));
            sProgram_textphong.SetUniformValue("modelMatrix", transform);
            sProgram_textphong.SetUniformValue("normalMatrix", normalMatrixTransform);
            pared26.paint(transform);

            // PARED 27
            transform = Matrix4.Mult(Matrix4.CreateTranslation(44.5f, 0.0f, 0.0f), m2);
            normalMatrixTransform = Matrix3.Transpose(Matrix3.Invert(new Matrix3(transform)));
            sProgram_textphong.SetUniformValue("modelMatrix", transform);
            sProgram_textphong.SetUniformValue("normalMatrix", normalMatrixTransform);
            pared27.paint(transform);

            // PARED 28
            transform = Matrix4.Mult(Matrix4.CreateTranslation(44.5f, 0.0f, 0.0f), m1);
            normalMatrixTransform = Matrix3.Transpose(Matrix3.Invert(new Matrix3(transform)));
            sProgram_textphong.SetUniformValue("modelMatrix", transform);
            sProgram_textphong.SetUniformValue("normalMatrix", normalMatrixTransform);
            pared28.paint(transform);

            // PARED 29
            transform = Matrix4.Mult(Matrix4.CreateTranslation(0.0f, 0.0f, -27.6f), m8);
            normalMatrixTransform = Matrix3.Transpose(Matrix3.Invert(new Matrix3(transform)));
            sProgram_textphong.SetUniformValue("modelMatrix", transform);
            sProgram_textphong.SetUniformValue("normalMatrix", normalMatrixTransform);
            pared29.paint(transform);

            // PARED 30
            transform = Matrix4.Mult(Matrix4.CreateTranslation(0.0f, 0.0f, -27.6f), m16);
            normalMatrixTransform = Matrix3.Transpose(Matrix3.Invert(new Matrix3(transform)));
            sProgram_textphong.SetUniformValue("modelMatrix", transform);
            sProgram_textphong.SetUniformValue("normalMatrix", normalMatrixTransform);
            pared30.paint(transform);

            // PARED 31
            transform = Matrix4.Mult(Matrix4.CreateTranslation(0.0f, 0.0f, -46.0f), m16);
            normalMatrixTransform = Matrix3.Transpose(Matrix3.Invert(new Matrix3(transform)));
            sProgram_textphong.SetUniformValue("modelMatrix", transform);
            sProgram_textphong.SetUniformValue("normalMatrix", normalMatrixTransform);
            pared31.paint(transform);

            // PARED 32
            transform = Matrix4.Mult(Matrix4.CreateTranslation(18.4f, 0.0f, 0.0f), m5);
            normalMatrixTransform = Matrix3.Transpose(Matrix3.Invert(new Matrix3(transform)));
            sProgram_textphong.SetUniformValue("modelMatrix", transform);
            sProgram_textphong.SetUniformValue("normalMatrix", normalMatrixTransform);
            pared32.paint(transform);

            // PARED 33
            transform = Matrix4.Mult(Matrix4.CreateTranslation(0.0f, 0.0f, -36.8f), m18);
            normalMatrixTransform = Matrix3.Transpose(Matrix3.Invert(new Matrix3(transform)));
            sProgram_textphong.SetUniformValue("modelMatrix", transform);
            sProgram_textphong.SetUniformValue("normalMatrix", normalMatrixTransform);
            pared33.paint(transform);

            // PARED 34
            transform = Matrix4.Mult(Matrix4.CreateTranslation(0.0f, 0.0f, -36.8f), m17);
            normalMatrixTransform = Matrix3.Transpose(Matrix3.Invert(new Matrix3(transform)));
            sProgram_textphong.SetUniformValue("modelMatrix", transform);
            sProgram_textphong.SetUniformValue("normalMatrix", normalMatrixTransform);
            pared34.paint(transform);

            // PARED 35
            transform = Matrix4.Mult(Matrix4.CreateTranslation(36.8f, 0.0f, 0.0f), m5);
            normalMatrixTransform = Matrix3.Transpose(Matrix3.Invert(new Matrix3(transform)));
            sProgram_textphong.SetUniformValue("modelMatrix", transform);
            sProgram_textphong.SetUniformValue("normalMatrix", normalMatrixTransform);
            pared35.paint(transform);

            // PARED 36
            transform = Matrix4.Mult(Matrix4.CreateTranslation(0.0f, 0.0f, -27.6f), m18);
            normalMatrixTransform = Matrix3.Transpose(Matrix3.Invert(new Matrix3(transform)));
            sProgram_textphong.SetUniformValue("modelMatrix", transform);
            sProgram_textphong.SetUniformValue("normalMatrix", normalMatrixTransform);
            pared36.paint(transform);

            // PARED 37
            transform = Matrix4.Mult(Matrix4.CreateTranslation(0.0f, 0.0f, -18.4f), m17);
            normalMatrixTransform = Matrix3.Transpose(Matrix3.Invert(new Matrix3(transform)));
            sProgram_textphong.SetUniformValue("modelMatrix", transform);
            sProgram_textphong.SetUniformValue("normalMatrix", normalMatrixTransform);
            pared37.paint(transform);

            // PARED 38
            transform = Matrix4.Mult(Matrix4.CreateTranslation(27.6f, 0.0f, 0.0f), m2);
            normalMatrixTransform = Matrix3.Transpose(Matrix3.Invert(new Matrix3(transform)));
            sProgram_textphong.SetUniformValue("modelMatrix", transform);
            sProgram_textphong.SetUniformValue("normalMatrix", normalMatrixTransform);
            pared38.paint(transform);

            // PARED 39
            transform = Matrix4.Mult(Matrix4.CreateTranslation(27.6f, 0.0f, 0.0f), m1);
            normalMatrixTransform = Matrix3.Transpose(Matrix3.Invert(new Matrix3(transform)));
            sProgram_textphong.SetUniformValue("modelMatrix", transform);
            sProgram_textphong.SetUniformValue("normalMatrix", normalMatrixTransform);
            pared39.paint(transform);

            // PARED 40
            transform = Matrix4.Mult(Matrix4.CreateTranslation(35.6f, 0.0f, 0.0f), m2);
            normalMatrixTransform = Matrix3.Transpose(Matrix3.Invert(new Matrix3(transform)));
            sProgram_textphong.SetUniformValue("modelMatrix", transform);
            sProgram_textphong.SetUniformValue("normalMatrix", normalMatrixTransform);
            pared40.paint(transform);

            // CADENAS
            transform = Matrix4.Mult(Matrix4.CreateTranslation(5.00f, 4.5f, -34.0f), m18);
            transform = Matrix4.Mult(Matrix4.CreateScale(0.5f), transform);
            normalMatrixTransform = Matrix3.Transpose(Matrix3.Invert(new Matrix3(transform)));
            sProgram_textphong.SetUniformValue("modelMatrix", transform);
            sProgram_textphong.SetUniformValue("normalMatrix", normalMatrixTransform);

            cadena.paint(transform);

            transform = Matrix4.Mult(Matrix4.CreateTranslation(6.00f, 4.5f, -35.0f), m18);
            transform = Matrix4.Mult(Matrix4.CreateScale(0.5f), transform);
            normalMatrixTransform = Matrix3.Transpose(Matrix3.Invert(new Matrix3(transform)));
            sProgram_textphong.SetUniformValue("modelMatrix", transform);
            sProgram_textphong.SetUniformValue("normalMatrix", normalMatrixTransform);

            cadena.paint(transform);

            transform = Matrix4.Mult(Matrix4.CreateTranslation(6.00f, 4.5f, -33.0f), m18);
            transform = Matrix4.Mult(Matrix4.CreateScale(0.5f), transform);
            normalMatrixTransform = Matrix3.Transpose(Matrix3.Invert(new Matrix3(transform)));
            sProgram_textphong.SetUniformValue("modelMatrix", transform);
            sProgram_textphong.SetUniformValue("normalMatrix", normalMatrixTransform);

            cadena.paint(transform);

            transform = Matrix4.Mult(Matrix4.CreateTranslation(4.00f, 4.5f, -36.0f), m18);
            transform = Matrix4.Mult(Matrix4.CreateScale(0.5f), transform);
            normalMatrixTransform = Matrix3.Transpose(Matrix3.Invert(new Matrix3(transform)));
            sProgram_textphong.SetUniformValue("modelMatrix", transform);
            sProgram_textphong.SetUniformValue("normalMatrix", normalMatrixTransform);

            cadena.paint(transform);

            // COFRE
            transform = Matrix4.Mult(Matrix4.CreateTranslation(5.5f, 0.0f, -16.0f), m17);
            transform = Matrix4.Mult(Matrix4.CreateScale(1.6f), transform);
            normalMatrixTransform = Matrix3.Transpose(Matrix3.Invert(new Matrix3(transform)));
            sProgram_textphong.SetUniformValue("modelMatrix", transform);
            sProgram_textphong.SetUniformValue("normalMatrix", normalMatrixTransform);

            cofre.paint(transform);

            // BOLSAS
            transform = Matrix4.Mult(Matrix4.CreateTranslation(5.5f, 0.0f, -1.0f), m16);
            normalMatrixTransform = Matrix3.Transpose(Matrix3.Invert(new Matrix3(transform)));
            sProgram_textphong.SetUniformValue("modelMatrix", transform);
            sProgram_textphong.SetUniformValue("normalMatrix", normalMatrixTransform);

            bolsas.paint(transform);

            // ARMAS
            transform = Matrix4.Mult(Matrix4.CreateTranslation(0.0f, 3.0f, -28.4f), m17);
            transform = Matrix4.Mult(Matrix4.CreateScale(2.0f), transform);
            normalMatrixTransform = Matrix3.Transpose(Matrix3.Invert(new Matrix3(transform)));
            sProgram_textphong.SetUniformValue("modelMatrix", transform);
            sProgram_textphong.SetUniformValue("normalMatrix", normalMatrixTransform);

            armas.paint(transform);

            transform = Matrix4.Mult(Matrix4.CreateTranslation(2.0f, 3.0f, -28.4f), m16);
            transform = Matrix4.Mult(Matrix4.CreateScale(2.0f), transform);
            normalMatrixTransform = Matrix3.Transpose(Matrix3.Invert(new Matrix3(transform)));
            sProgram_textphong.SetUniformValue("modelMatrix", transform);
            sProgram_textphong.SetUniformValue("normalMatrix", normalMatrixTransform);

            armas.paint(transform);

            // PUERTAS
            if (encontroLlave == false)
            {
                transform = Matrix4.Mult(Matrix4.CreateTranslation(1.2f, -3.1f, 0.7f), m18);
                transform = Matrix4.Mult(Matrix4.CreateScale(1.0f), transform);
                normalMatrixTransform = Matrix3.Transpose(Matrix3.Invert(new Matrix3(transform)));
                sProgram_textphong.SetUniformValue("modelMatrix", transform);
                sProgram_textphong.SetUniformValue("normalMatrix", normalMatrixTransform);

                puerta.paint(transform);

                transform = Matrix4.Mult(Matrix4.CreateTranslation(35.4f, 0.0f, 0.0f), m1);
                transform = Matrix4.Mult(Matrix4.CreateScale(1.2f), transform);
                normalMatrixTransform = Matrix3.Transpose(Matrix3.Invert(new Matrix3(transform)));
                sProgram_textphong.SetUniformValue("modelMatrix", transform);
                sProgram_textphong.SetUniformValue("normalMatrix", normalMatrixTransform);

                pared1.paint(transform);

                // LLAVE
                transform = Matrix4.Mult(Matrix4.CreateTranslation(40.8f, 4.0f, -40.0f), modelMatrix);
                transform = Matrix4.Mult(Matrix4.CreateScale(0.03f), transform);
                transform = Matrix4.Mult(Matrix4.CreateRotationY((timer * 5) % 360 * pi / 180), transform);
                normalMatrixTransform = Matrix3.Transpose(Matrix3.Invert(new Matrix3(transform)));
                sProgram_textphong.SetUniformValue("modelMatrix", transform);
                sProgram_textphong.SetUniformValue("normalMatrix", normalMatrixTransform);
                llave.paint(transform);
            }


            sProgram_textphong.Deactivate(); //Desactivamos el programa de shader.


            /*
            //--------------------------- NORMAL MAPPING --------------------------------------------------
            sProgram_bump.Activate(); //Activamos el programa de shaders

            //sProgram_bump.SetUniformValue("lColor", new Vector4(0.5f, 0.5f, 0.5f, 1.0f));
            sProgram_bump.SetUniformValue("lPos", myCamera.getPosition());
            sProgram_bump.SetUniformValue("projMatrix", projMatrix);
            sProgram_bump.SetUniformValue("viewMatrix", viewMatrix);
            sProgram_bump.SetUniformValue("light_diffuse", new Vector4(myLight.Idiffuse, 1.0f));
            sProgram_bump.SetUniformValue("light_specular", new Vector4(myLight.Ispecular, 1.0f));


            // CUBO
            sProgram_bump.SetUniformValue("lPos", myLightCubo.Position);
            sProgram_bump.SetUniformValue("light_diffuse", new Vector4(myLightCubo.Idiffuse, 1.0f));
            sProgram_bump.SetUniformValue("light_specular", new Vector4(myLightCubo.Ispecular, 1.0f));
            transform = Matrix4.Mult(Matrix4.CreateTranslation(3.6f, 3.0f, -10.0f), modelMatrix);
            transform = Matrix4.Mult(Matrix4.CreateRotationY(rotY * pi / 180), transform);
            transform = Matrix4.Mult(Matrix4.CreateRotationZ(rotZ * pi / 180), transform);
            normalMatrixTransform = Matrix3.Transpose(Matrix3.Invert(new Matrix3(transform)));
            sProgram_bump.SetUniformValue("modelMatrix", transform);
            //sProgram_bump.SetUniformValue("normalMatrix", normalMatrixTransform);
            cubo.paintBump(transform);


            sProgram_bump.Deactivate(); //Desactivamos el programa de shader.

            */
            glControl3.SwapBuffers(); //Intercambiamos buffers frontal y trasero, para evitar flickering.
        }

        private void glControl3_Resize(object sender, EventArgs e)
        {   //Actualizamos el viewport para que dibuje en el centro de la pantalla.
            Size size = glControl3.Size;
            if (size.Width < size.Height)
            {
                viewport.X = 0;
                viewport.Y = (size.Height - size.Width) / 2;
                viewport.Width = size.Width;
                viewport.Height = size.Width;
            }
            else
            {
                viewport.X = (size.Width - size.Height) / 2;
                viewport.Y = 0;
                viewport.Width = size.Height;
                viewport.Height = size.Height;
            }
            glControl3.Invalidate(); //Invalidamos el glControl para que se redibuje.(llama al metodo Paint)
        }

        private void SetupShaders()
        {
            //===== SHADER DE LUCES ===== PHONG
            //1. Creamos los shaders, a partir de archivos.
            String vShaderFile = "files/shaders/phong/vshader_p.glsl";
            String fShaderFile = "files/shaders/phong/fshader_p.glsl";
            Shader vShader = new Shader(ShaderType.VertexShader, vShaderFile);
            Shader fShader = new Shader(ShaderType.FragmentShader, fShaderFile);
            //2. Los compilamos
            vShader.Compile();
            fShader.Compile();
            //3. Creamos el Programa de shader con ambos.
            sProgram_phong = new ShaderProgram();
            sProgram_phong.AddShader(vShader);
            sProgram_phong.AddShader(fShader);
            //4. Construimos (linkeamos) el programa.
            sProgram_phong.Build();
            //5. Ya podemos eliminar los shaders compilados. (Si no los vamos a usar en otro programa)
            vShader.Delete();
            fShader.Delete();

            //===== SHADER DE LUCES ===== GOURAUD
            //1. Creamos los shaders, a partir de archivos.
            vShaderFile = "files/shaders/gouraud/vshader_g.glsl";
            fShaderFile = "files/shaders/gouraud/fshader_g.glsl";
            vShader = new Shader(ShaderType.VertexShader, vShaderFile);
            fShader = new Shader(ShaderType.FragmentShader, fShaderFile);
            //2. Los compilamos
            vShader.Compile();
            fShader.Compile();
            //3. Creamos el Programa de shader con ambos.
            sProgram_gouraud = new ShaderProgram();
            sProgram_gouraud.AddShader(vShader);
            sProgram_gouraud.AddShader(fShader);
            //4. Construimos (linkeamos) el programa.
            sProgram_gouraud.Build();
            //5. Ya podemos eliminar los shaders compilados. (Si no los vamos a usar en otro programa)
            vShader.Delete();
            fShader.Delete();

            //===== SHADER PARA LOS EJES =====
            vShader = new Shader(ShaderType.VertexShader, "files/shaders/ejes/vshader0.glsl");
            fShader = new Shader(ShaderType.FragmentShader, "files/shaders/ejes/fshader0.glsl");
            //2. Los compilamos
            vShader.Compile();
            fShader.Compile();
            //3. Creamos el Programa de shader con ambos.
            sProgram_ejes = new ShaderProgram();
            sProgram_ejes.AddShader(vShader);
            sProgram_ejes.AddShader(fShader);
            //4. Construimos (linkeamos) el programa.
            sProgram_ejes.Build();
            //5. Ya podemos eliminar los shaders compilados. (Si no los vamos a usar en otro programa)
            vShader.Delete();
            fShader.Delete();

            //===== SHADER DE TEXTURAS Y LUCES ===== PHONG
            vShader = new Shader(ShaderType.VertexShader, "files/shaders/texturasphong/vtextshader_p.glsl");
            fShader = new Shader(ShaderType.FragmentShader, "files/shaders/texturasphong/ftextshader_p.glsl");
            //2. Los compilamos
            vShader.Compile();
            fShader.Compile();
            //3. Creamos el Programa de shader con ambos.
            sProgram_textphong = new ShaderProgram();
            sProgram_textphong.AddShader(vShader);
            sProgram_textphong.AddShader(fShader);
            //4. Construimos (linkeamos) el programa.
            sProgram_textphong.Build();
            //5. Ya podemos eliminar los shaders compilados. (Si no los vamos a usar en otro programa)
            vShader.Delete();
            fShader.Delete();

            //===== SHADER DE NORMAL MAPPING ===== 
            //1. Creamos los shaders, a partir de archivos.
            vShaderFile = "files/shaders/normalMap/vBump.glsl";
            fShaderFile = "files/shaders/normalMap/fBump.glsl";
            vShader = new Shader(ShaderType.VertexShader, vShaderFile);
            fShader = new Shader(ShaderType.FragmentShader, fShaderFile);
            //2. Los compilamos
            vShader.Compile();
            fShader.Compile();
            //3. Creamos el Programa de shader con ambos.
            sProgram_bump = new ShaderProgram();
            sProgram_bump.AddShader(vShader);
            sProgram_bump.AddShader(fShader);
            //4. Construimos (linkeamos) el programa.
            sProgram_bump.Build();
            //5. Ya podemos eliminar los shaders compilados. (Si no los vamos a usar en otro programa)
            vShader.Delete();
            fShader.Delete();

        }

        private void logContextInfo()
        {
            String version, renderer, shaderVer, vendor;//, extensions;
            version = gl.GetString(StringName.Version);
            renderer = gl.GetString(StringName.Renderer);
            shaderVer = gl.GetString(StringName.ShadingLanguageVersion);
            vendor = gl.GetString(StringName.Vendor);
            //extensions = gl.GetString(StringName.Extensions);
            log("========= CONTEXT INFORMATION =========");
            log("Renderer:       {0}", renderer);
            log("Vendor:         {0}", vendor);
            log("OpenGL version: {0}", version);
            log("GLSL version:   {0}", shaderVer);
            //log("Extensions:" + extensions);
            log("===== END OF CONTEXT INFORMATION =====");

        }
        private void log(String format, params Object[] args)
        {
            System.Diagnostics.Debug.WriteLine(String.Format(format, args), "[CGUNS]");
        }

        private void glControl3_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            e.IsInputKey = true;
        }

        private void glControl3_KeyPressed(object sender, KeyEventArgs e)
        {
            bool colisionXPos = false;
            bool colisionXNeg = false;
            bool colisionZPos = false;
            bool colisionZNeg = false;

            float x = Math.Abs(myCamera.direccion.X);
            float z = Math.Abs(myCamera.direccion.Z);
            float c = 1.0f;
            float prom_x = ((x * c) / (x + z));
            float prom_z = ((z * c) / (x + z));

            switch (e.KeyCode)
            {
                case Keys.L:
                    if (myLight.Enabled == 0)
                        myLight.Enabled = 1;
                    else
                        myLight.Enabled = 0;
                    break;
                case Keys.S:
                    foreach (Objeto o in colisiones)
                    {
                        if (objMovimiento.Bb.colisionXPos(o.Bb))
                            colisionXPos = true;
                        if (objMovimiento.Bb.colisionXNeg(o.Bb))
                            colisionXNeg = true;
                        if (objMovimiento.Bb.colisionZPos(o.Bb))
                            colisionZPos = true;
                        if (objMovimiento.Bb.colisionZNeg(o.Bb))
                            colisionZNeg = true;
                        if (o.Equals(llave) && (colisionXPos || colisionXNeg || colisionZPos || colisionZNeg))
                            encontroLlave = true;
                        if (o.Equals(cofre) && (colisionXPos || colisionXNeg || colisionZPos || colisionZNeg))
                            gano = true;
                    }
                    if (encontroLlave)
                        colisiones.Remove(llave);
                    if (myCamera.direccion.Z <= 0 && myCamera.direccion.X >= 0 && !colisionZPos && !colisionXNeg)
                    {
                        translationX1 -= prom_x;
                        translationZ1 += prom_z;
                        myCamera.Atras(transformcubo);
                        myLight.Position = transformcubo.Row3 + new Vector4(0.0f, 2.0f, 0.0f, 1.0f);
                    }
                    else if (myCamera.direccion.Z > 0 && myCamera.direccion.X >= 0 && !colisionZNeg && !colisionXNeg)
                    {
                        translationX1 -= prom_x;
                        translationZ1 -= prom_z;
                        myCamera.Atras(transformcubo);
                        myLight.Position = transformcubo.Row3 + new Vector4(0.0f, 2.0f, 0.0f, 1.0f);
                    }
                    else if (myCamera.direccion.Z <= 0 && myCamera.direccion.X < 0 && !colisionZPos && !colisionXPos)
                    {
                        translationX1 += prom_x;
                        translationZ1 += prom_z;
                        myCamera.Atras(transformcubo);
                        myLight.Position = transformcubo.Row3 + new Vector4(0.0f, 2.0f, 0.0f, 1.0f);
                    }
                    else if (myCamera.direccion.Z > 0 && myCamera.direccion.X < 0 && !colisionZNeg && !colisionXPos)
                    {
                        translationX1 += prom_x;
                        translationZ1 -= prom_z;
                        myCamera.Atras(transformcubo);
                        myLight.Position = transformcubo.Row3 + new Vector4(0.0f, 2.0f, 0.0f, 1.0f);
                    }
                    break;
                case Keys.W:
                    foreach (Objeto o in colisiones)
                    {
                        if (objMovimiento.Bb.colisionXPos(o.Bb))
                            colisionXPos = true;
                        if (objMovimiento.Bb.colisionXNeg(o.Bb))
                            colisionXNeg = true;
                        if (objMovimiento.Bb.colisionZPos(o.Bb))
                            colisionZPos = true;
                        if (objMovimiento.Bb.colisionZNeg(o.Bb))
                            colisionZNeg = true;
                        if (o.Equals(llave) && (colisionXPos || colisionXNeg || colisionZPos || colisionZNeg))
                            encontroLlave = true;
                        if (o.Equals(cofre) && (colisionXPos || colisionXNeg || colisionZPos || colisionZNeg))
                            gano = true;
                    }
                    if (encontroLlave)
                        colisiones.Remove(llave);
                    //if (gano)

                    if (myCamera.direccion.Z <= 0 && myCamera.direccion.X >= 0 && !colisionZNeg && !colisionXPos)
                    {
                        translationX1 += prom_x;
                        translationZ1 -= prom_z;
                        myCamera.Adelante(transformcubo);
                        myLight.Position = transformcubo.Row3 + new Vector4(0.0f, 2.0f, 0.0f, 1.0f);
                    }
                    else if (myCamera.direccion.Z > 0 && myCamera.direccion.X >= 0 && !colisionZPos && !colisionXPos)
                    {
                        translationX1 += prom_x;
                        translationZ1 += prom_z;
                        myCamera.Adelante(transformcubo);
                        myLight.Position = transformcubo.Row3 + new Vector4(0.0f, 2.0f, 0.0f, 1.0f);
                    }
                    else if (myCamera.direccion.Z <= 0 && myCamera.direccion.X < 0 && !colisionZNeg && !colisionXNeg)
                    {
                        translationX1 -= prom_x;
                        translationZ1 -= prom_z;
                        myCamera.Adelante(transformcubo);
                        myLight.Position = transformcubo.Row3 + new Vector4(0.0f, 2.0f, 0.0f, 1.0f);
                    }
                    else if (myCamera.direccion.Z > 0 && myCamera.direccion.X < 0 && !colisionZPos && !colisionXNeg)
                    {
                        translationX1 -= prom_x;
                        translationZ1 += prom_z;
                        myCamera.Adelante(transformcubo);
                        myLight.Position = transformcubo.Row3 + new Vector4(0.0f, 2.0f, 0.0f, 1.0f);
                    }
                    break;

                case Keys.D:
                    myCamera.MirarDerecha(Matrix4.CreateRotationY(-1.5f * pi / 180));

                    myLight.ConeDirection = myLight.ConeDirection + new Vector3(transformcubo.Row3.X, 0.0f, 0.0f);
                    myLight.ConeDirection = myLight.ConeDirection + new Vector3(transformcubo.Row3.Z, 0.0f, 0.0f);
                    break;

                case Keys.A:
                    myCamera.MirarDerecha(Matrix4.CreateRotationY(1.5f * pi / 180));
                    myLight.ConeDirection = myLight.ConeDirection + new Vector3(transformcubo.Row3.X, 0.0f, 0.0f);
                    myLight.ConeDirection = myLight.ConeDirection + new Vector3(transformcubo.Row3.Z, 0.0f, 0.0f);
                    break;
                case Keys.Escape:
                    this.Close();
                    a.Close();
                    break;
            }
            glControl3.Invalidate(); //Notar que renderizamos para CUALQUIER tecla que sea presionada.
        }

        private void playSound()
        {
            SoundPlayer simpleSound = new SoundPlayer(@"C:\Users\Maxi\Desktop\got.wav");
            simpleSound.Play();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer++;
            if (timer == 1000)
            {
                if (MessageBox.Show("Game Over", "Black Out", MessageBoxButtons.OK, MessageBoxIcon.Information)== DialogResult.OK)
                {
                    a.Close();
                    this.Close();
                }
            }
            rotY++;
            rotZ++;
            glControl3.Invalidate();
        }
    }
 }