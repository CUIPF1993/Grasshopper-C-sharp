using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace test01
{
    public class CruveToPolylineInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "CurveToPolyline";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return null ;
            }
        }
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "用多条直线去描述一条曲线,采用的算法为二分法，和递归";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("09768b64-4e6f-407d-977e-e527e17a82f8");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "CPF";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "邮箱:756206487@qq.com";
            }
        }
    }
}
