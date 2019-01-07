using CGUNS.Meshes.FaceVertexList;
using OpenTK;
using System;
using System.Linq;

namespace BlackOut
{
    class BoundingBox
    {
        private float maxX = 0.0f;
        private float maxZ = 0.0f;
        private float minX = 0.0f;
        private float minZ = 0.0f;

        private Vector4 posicionMundo;

       public Vector4 PosicionMundo
        {
            get
            {
                return posicionMundo;
            }

            set
            {
                posicionMundo = value;
            }
        }

        public float MaxX
        {
            get
            {
                return maxX;
            }

            set
            {
                maxX = value;
            }
        }

        public float MaxZ
        {
            get
            {
                return maxZ;
            }

            set
            {
                maxZ = value;
            }
        }

        public float MinX
        {
            get
            {
                return minX;
            }

            set
            {
                minX = value;
            }
        }

        public float MinZ
        {
            get
            {
                return minZ;
            }

            set
            {
                minZ = value;
            }
        }

        public BoundingBox(FVLMesh objeto)
        {
            posicionMundo = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);

            if (objeto.VertexList.Count() > 0)
            {
                maxX = objeto.VertexList.First().X;
                maxZ = objeto.VertexList.First().Z;
                minX = objeto.VertexList.First().X;
                minZ = objeto.VertexList.First().Z;
                foreach(Vector3 vertex in objeto.VertexList)
                {
                    if (vertex.X > maxX)
                        maxX = vertex.X;
                    else if (vertex.X < minX)
                        minX = vertex.X;
                    if (vertex.Z > maxZ)
                        maxZ = vertex.Z;
                    else if (vertex.Z < minZ)
                        minZ = vertex.Z;
                }
            }
        }

        public BoundingBox(Objeto obj)
        {
            posicionMundo = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);

            if (obj.Meshes.Count() > 0)
            {
                maxX = obj.Meshes.First().VertexList.First().X;
                maxZ = obj.Meshes.First().VertexList.First().Z;
                minX = obj.Meshes.First().VertexList.First().X;
                minZ = obj.Meshes.First().VertexList.First().Z;
                foreach (FVLMesh mesh in obj.Meshes)
                {
                    if (mesh.VertexList.Count() > 0)
                    {
                        foreach (Vector3 vertex in mesh.VertexList)
                        {
                            if (vertex.X > maxX)
                                maxX = vertex.X;
                            else if (vertex.X < minX)
                                minX = vertex.X;
                            if (vertex.Z > maxZ)
                                maxZ = vertex.Z;
                            else if (vertex.Z < minZ)
                                minZ = vertex.Z;
                        }
                    }
                }
            }
        }
        
        public bool colisionXPos(BoundingBox box)
        {
            if (Math.Abs((posicionMundo.X + MaxX) - (box.PosicionMundo.X + box.MinX)) < 0.1f &&
                posicionMundo.Z + MaxZ >= box.PosicionMundo.Z + box.MinZ &&
                posicionMundo.Z + MinZ <= box.PosicionMundo.Z + box.MaxZ)
                return true;
            else return false;
        }
        public bool colisionXNeg(BoundingBox box)
        {      
            if (Math.Abs((posicionMundo.X + MinX) - (box.PosicionMundo.X + box.MaxX)) < 0.1f &&
                posicionMundo.Z + MaxZ >= box.PosicionMundo.Z + box.MinZ && 
                posicionMundo.Z + MinZ <= box.PosicionMundo.Z + box.MaxZ)
                return true;
            else return false;
        }
        public bool colisionZPos(BoundingBox box)
        {
            if (Math.Abs((posicionMundo.Z + MaxZ) - (box.PosicionMundo.Z + box.MinZ)) < 0.1f &&
                posicionMundo.X + MaxX >= box.PosicionMundo.X + box.MinX && 
                posicionMundo.X + MinX <= box.PosicionMundo.X + box.MaxX)
                return true;
            else return false;
        }
        public bool colisionZNeg(BoundingBox box)
        {
            if (Math.Abs((posicionMundo.Z + MinZ) - (box.PosicionMundo.Z + box.MaxZ)) < 0.1f &&
                posicionMundo.X + MaxX >= box.PosicionMundo.X + box.MinX && 
                posicionMundo.X + MinX <= box.PosicionMundo.X + box.MaxX)
                return true;
            else return false;
        }

    
    }
}



