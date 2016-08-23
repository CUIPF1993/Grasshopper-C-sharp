using System;
using System.Collections.Generic;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel;
using Rhino.Geometry;
using GH_IO.Serialization;

namespace TriState
{
    public class TriStateComponent : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public TriStateComponent()
          : base("TriState", "Nickname",
              "Description",
              "Category", "Subcategory")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Rhino.Commands.TransformCommand a = new Rhino.Commands.TransformCommand()
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
            get { return new Guid("{f78f05a1-00eb-42ed-970d-b5e02f32ff8b}"); }
        }
    }
}


namespace MyTypes
{
    public class TriStateType : GH_Goo<int>
    {
        public TriStateType()
        {
            this.Value = -1;
        }

        public TriStateType(int tristateValue)
        {
            this.Value = tristateValue;
        }

        public TriStateType(TriStateType tristateSource)
        {
            this.Value = tristateSource.Value;
        }

        public override IGH_Goo Duplicate()
        {
            return new TriStateType(this);
        }

        public override int Value
        {
            get { return base.Value; }
            set
            {
                if (value < -1) { value = -1; }
                if (value > 1) { value = 1; }
                base.Value = value;
            }
        }

        public override bool IsValid
        {
            get
            {
                return true;
            }
        }

        public override string TypeName
        {
            get
            {
                return "TriState";
            }
        }

        public override string TypeDescription
        {
            get
            {
                return "A Tristate Value (True,False or Unknow)";
            }
        }

        public override string ToString()
        {
            if (this.Value == 0) { return "False"; }
            if (this.Value > 0) { return "True"; }
            return "Unknow";
        }

        public override bool Write(GH_IWriter writer)
        {
            writer.SetInt32("tri", this.Value);
            return true;
        }

        public override bool Read(GH_IReader reader)
        {
            this.Value = reader.GetInt32("tri");
            return true;
        }

        public override object ScriptVariable()
        {
            return base.ScriptVariable();
        }

        public override bool CastTo<Q>(ref Q target)
        {
            if (typeof(Q).IsAssignableFrom(typeof(int)))
            {
                object ptr = this.Value;
                target = (Q)ptr;
                return true;
            }

            if (typeof(Q).IsAssignableFrom(typeof(GH_Integer)))
            {
                object ptr = new GH_Integer(this.Value);
                target = (Q)ptr;
                return true;
            }
            return false;
        }

        public override bool CastFrom(object source)
        {
            if (source == null)
            {
                return false;
            }

            int val;
            if (GH_Convert.ToInt32(source, out val, GH_Conversion.Both))
            {
                this.Value = val;
                return true;
            }

            string str = null;
            if (GH_Convert.ToString(source, out str, GH_Conversion.Both))
            {
                switch (str.ToUpperInvariant())
                {
                    case "FALSE":
                    case "F":
                    case "NO":
                    case "N":
                        this.Value = 0;
                        return true;

                    case "TRUE":
                    case "T":
                    case "YES":
                    case "Y":
                        this.Value = +1;
                        return true;

                    case "UNKNOWN":
                    case "UNSET":
                    case "MAYBE":
                    case "DUNNO":
                    case "?":
                        this.Value = -1;
                        return true;
                }
            }
            return false;
        }
    }

    public class TriStateParameter : GH_PersistentParam<TriStateType>
    {
        public TriStateParameter() :
          base("TriState", "Tri", "Represents a collection of TriState values", "Params", "Primitive")
        { }

        public override System.Guid ComponentGuid
        {
            get { return new Guid("{2FEEF1A2-A764-431d-8C78-9BF92C78BAE1}"); }
        }

        protected override GH_GetterResult Prompt_Singular(ref TriStateType value)
        {
            Rhino.Input.Custom.GetOption go = new Rhino.Input.Custom.GetOption();
            go.SetCommandPrompt("TriState value");
            go.AcceptNothing(true);
            go.AddOption("True");
            go.AddOption("False");
            go.AddOption("Unknown");

            switch (go.Get())
            {
                case Rhino.Input.GetResult.Option:
                    if (go.Option().EnglishName == "True") { value = new TriStateType(1); }
                    if (go.Option().EnglishName == "False") { value = new TriStateType(0); }
                    if (go.Option().EnglishName == "Unknown") { value = new TriStateType(-1); }
                    return GH_GetterResult.success;

                case Rhino.Input.GetResult.Nothing:
                    return GH_GetterResult.accept;

                default:
                    return GH_GetterResult.cancel;
            }
        }
        protected override GH_GetterResult Prompt_Plural(ref List<TriStateType> values)
        {
            List<TriStateType> value = new List<TriStateType>();

            while (true)
            {
                TriStateType val = null;
                switch (Prompt_Singular(ref val))
                {
                    case GH_GetterResult.success:
                        value.Add(val);
                        break;

                    case GH_GetterResult.accept:
                        return GH_GetterResult.success;

                    case GH_GetterResult.cancel:
                        value = null;
                        return GH_GetterResult.cancel;
                }
            }
      }
    }
}