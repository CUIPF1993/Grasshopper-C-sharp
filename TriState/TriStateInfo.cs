using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace TriState
{
    public class TriStateInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "TriState";
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
                return new Guid("cfbed035-c287-4678-b3e2-cd466b966a42");
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
