using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace MyMeshPipe
{
    public class MyMeshPipeInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "MyMeshPipe";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return null;
            }
        }
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("a54e9852-2fce-4c27-a0a0-f8c88e270e7d");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "CHINA";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "";
            }
        }
    }
}
