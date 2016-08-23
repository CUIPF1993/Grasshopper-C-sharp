using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;


namespace test01
{
    public class CurveToPolyline : GH_Component
    {

        public CurveToPolyline()
          : base("CurveToPolyline", "CP",
              "Description",
              "user", "Subcategory")
        {
        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("curve", "C", "需要转换为多段线的曲线", GH_ParamAccess.item);
            pManager.AddNumberParameter("accuracy", "A", "贴合度，即容差", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddLineParameter("Line", "L", "直线", GH_ParamAccess.list);
        }


        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Curve curve = null;
            double accuracy = double.NaN;
            List<Line> line_list = new List<Line>();
            List<Curve> curve_list = new List<Curve>();

            if (!DA.GetData(0, ref curve)) { return; }
            if (!DA.GetData(1, ref accuracy)) { return; }

            curve_list.Add(curve);

            if (accuracy > 0.99999)
            {
                accuracy = 0.99999;
            }

            List<Line> line = Divide_curve(curve_list, accuracy, line_list);
            DA.SetDataList(0, line);
        }

        public bool Toogle(Curve curve, double accuracy)
        {
            //定义了一个方法去判定曲线是否需要优化，利用的数学原理为两点之间直线最短
            double curve_length = curve.GetLength();
            Point3d end_point = curve.PointAtEnd;
            Point3d star_point = curve.PointAtStart;
            Line line = new Line(star_point, end_point);
            double line_length = line.Length;

            double t1 = line_length / curve_length;
            //判断直线的长度与曲线的长度比值与容差的大小关系
            if (t1 > accuracy)
            {
                return false;
            }
            return true;
        }

        public List<Line> Divide_curve(List<Curve> curve_list, double accuracy, List<Line> line_list)
        {
            bool Bool = true;
            //设定一个布尔值，用于判定是否返回直线，还是进入下一次递归
            List<Curve> L_curve = new List<Curve>();
            //用来存放每次需要分割的曲线。          
            for (int i = 0; i < curve_list.Count; i++)
            {
                if (Toogle(curve_list[i], accuracy))
                {
                    Bool = false;
                    Interval domain = curve_list[i].Domain;
                    double parameter = (domain[1] - domain[0]) / 2.0+domain[0];
                    //求出曲线的中点，此处有陷阱，不可直接使用domain[1]/2
                    Curve[] split_curve = curve_list[i].Split(parameter);
                    L_curve.Add(split_curve[0]);
                    L_curve.Add(split_curve[1]);

                }
                else
                {
                    Point3d end_point = curve_list[i].PointAtEnd;
                    Point3d star_point = curve_list[i].PointAtStart;
                    Line line = new Line(star_point, end_point);
                    line_list.Add(line);
                }
            }
            if (Bool == true) { return line_list; }
            else
            {
                return Divide_curve(L_curve, accuracy, line_list);
            }
        }

        protected override System.Drawing.Bitmap  Icon
        {
            get
            {
                return test01.Properties.Resources.tu ;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("{b29c6aae-c280-4a3e-87f3-c6144b122b7c}"); }
        }
    }
}
