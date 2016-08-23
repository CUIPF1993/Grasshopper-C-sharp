using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Grasshopper.Kernel.Components;

namespace MyMeshPipe
{
    public class MyMeshPipeComponent : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public MyMeshPipeComponent()
          : base("MyMeshPipe", "MeshP",
              "mesh pipe",
              "user", "Subcategory")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("curve","C","Cruve to mesh pipe",GH_ParamAccess.item);
            pManager.AddNumberParameter("radius", "R", "radius for pipe", GH_ParamAccess.item);
            pManager.AddIntegerParameter ("radseg", "S", "radius of segments", GH_ParamAccess.item);
            pManager.AddIntegerParameter ("segments","S", "number of segments", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddMeshParameter("meshPipe", "MP", "msehpipe", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Curve curve = null;
            double radius = double.NaN;
            Int32 radSeg = new Int32();
            Int32 num = new Int32();

            if (!DA.GetData(0, ref curve)) { return; }
            if (!DA.GetData(1, ref radius)) { return;}
            if(!DA.GetData(2, ref radSeg)) { return; }
            if (!DA.GetData(3, ref num)) { return; }

            Point3d[] points = curvePoints(curve, num);
            Plane[] planes = curveFrame(curve, num);
            Mesh mesh_list = meshpipe(curve, planes, radius, radSeg, points);

            DA.SetData(0, mesh_list);
        }

        public Point3d[] curvePoints(Curve curve, int num)
        {
            double[] pointAt = curve.DivideByCount(num, true);
            Point3d[] points = new Point3d[num + 1];
            for (int i = 0; i < pointAt.Length; i++)
            {
                points[i] = curve.PointAt(pointAt[i]);
            }
            return points;
        }

        public Plane[] curveFrame(Curve curve, int num)
        {
            double[] pointAt = curve.DivideByCount(num, true);
            Plane[] planes = new Plane[num+1];
            for ( int i = 0; i < pointAt.Length ; i++)
            {
                Vector3d vec1 = curve.TangentAt(pointAt[i]);
                Vector3d vec2 = new Vector3d(0.0, 0.0, 1.0);
                if (vec1.IsParallelTo(vec2) != 0)
                {
                    Plane plane2 = new Plane(curve.PointAt(pointAt[i]), vec1);
                    planes[i] = plane2;

                }
                else
                {
                    Vector3d vec3 = Vector3d.CrossProduct(vec2, vec1);
                    Vector3d vec4 = Vector3d.CrossProduct(vec3, vec1);
                    Plane plane2 = new Plane(curve.PointAt(pointAt[i]), vec4, vec3);
                    planes[i] = plane2;
                }
            }
            return planes;

        }
        public Mesh meshpipe(Curve curve, Plane[] plane,double radius,Int32 radSeg,Point3d[] points)
        {
            Point3d[,] cir_Points = new Point3d[plane.Length, radSeg+1];
            for (int i = 0; i < plane.Length;i++)
            {
                Circle cir = new Circle(plane[i], points[i], radius);               
                for (int k = 0; k < radSeg; k++)
                {
                    double at = (2 * k * Math.PI) / radSeg;
                    Point3d  point= cir.PointAt(at);
                    cir_Points[i,k] = point;
                }
                cir_Points[i, radSeg] = cir_Points[i, 0];
            }

            Mesh meshlist = new Mesh();

            for (int i = 0; i < plane.Length - 1; i++)
            {
                for (int k = 0; k < radSeg; k++)
                {
                    Point3d point1 = cir_Points[i, k];
                    Point3d point2 = cir_Points[i, k+1];
                    Point3d point3 = cir_Points[i+1, k+1];
                    Point3d point4 = cir_Points[i+1, k];
                    Mesh mesh= new Mesh();

                    mesh.Vertices.Add(point1);
                    mesh.Vertices.Add(point2);
                    mesh.Vertices.Add(point3);
                    mesh.Vertices.Add(point4);
                    mesh.Faces.AddFace(0, 1, 2, 3);
                    meshlist.Append(mesh);
                }
            }
                return meshlist;
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{8d9ba7eb-4e45-4a95-b51e-0d1ff0487dfa}"); }
        }
    }
}
