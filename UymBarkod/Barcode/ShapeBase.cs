using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Web;

namespace UymBarkod.Barcode
{
    public class ShapeBase : Entity
    {
        #region Fields
        protected string name = "name";
        /// <summary>
        /// the rectangle on which any shape lives
        /// </summary>
        protected Rectangle rectangle;
        /// <summary>
        /// the backcolor of the shapes
        /// </summary>
        protected Color shapeColor = Color.SteelBlue;
        /// <summary>
        /// the brush corresponding to the backcolor
        /// </summary>
        protected Brush shapeBrush;
        /// <summary>
        /// the text on the shape
        /// </summary>
        protected string text = string.Empty;

        #endregion

        #region Properties


        /// <summary>
        /// Gets or sets the width of the shape
        /// </summary>
        [Browsable(true), Description("The width of the shape"), Category("Layout")]
        public int Width
        {
            get { return this.rectangle.Width; }
            set {  }
        }

        [Browsable(true), Description("Name"), Category("Appearance")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }


        /// <summary>
        /// Gets or sets the height of the shape
        /// </summary>		
        [Browsable(true), Description("The height of the shape"), Category("Layout")]
        public int Height
        {
            get { return this.rectangle.Height; }
            set { Resize(this.Width, value); }
        }

        /// <summary>
        /// Gets or sets the text of the shape
        /// </summary>
        [Browsable(true), Description("The text shown on the shape"), Category("Appearance")]
        public virtual string Text
        {
            get { return text; }
            set { text = value; this.Invalidate(); }
        }

        /// <summary>
        /// the x-coordinate of the upper-left corner
        /// </summary>
        [Browsable(true), Description("The x-coordinate of the upper-left corner"), Category("Layout")]
        public int X
        {
            get { return rectangle.X; }
            set
            {
                Point p = new Point(value - rectangle.X, rectangle.Y);
            }
        }

        /// <summary>
        /// the y-coordinate of the upper-left corner
        /// </summary>
        [Browsable(true), Description("The y-coordinate of the upper-left corner"), Category("Layout")]
        public int Y
        {
            get { return rectangle.Y; }
            set
            {
                Point p = new Point(rectangle.X, value - rectangle.Y);
            }
        }
        /// <summary>
        /// The backcolor of the shape
        /// </summary>
        [Browsable(true), Description("The backcolor of the shape"), Category("Appearance")]
        public Color ShapeColor
        {
            get { return shapeColor; }
            set { shapeColor = value; SetBrush(); Invalidate(); }
        }

        /// <summary>
        /// Gets or sets the location of the shape;
        /// </summary>
        [Browsable(false), Category("Layout")]
        public Point Location
        {
            get { return new Point(this.rectangle.X, this.rectangle.Y); }
            set
            {
                //we use the move method but it requires the delta value, not an absolute position!
                Point p = new Point(value.X - rectangle.X, value.Y - rectangle.Y);
                //if you'd use this it would indeed move the shape but not the connector s of the shape
                //this.rectangle.X = value.X; this.rectangle.Y = value.Y; Invalidate();
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default ctor
        /// </summary>
        public ShapeBase()
        {
            Init();
        }

        #endregion

        #region Methods
        /// <summary>
        /// Summarizes the initialization used by the constructors
        /// </summary>
        private void Init()
        {
            rectangle = new Rectangle(0, 0, 100, 70);
            SetBrush();
        }


        /// <summary>
        /// Sets the brush corresponding to the backcolor
        /// </summary>
        private void SetBrush()
        {
            shapeBrush = new SolidBrush(shapeColor);

        }

        public void Resize(int width, int height)
        {
            this.rectangle.Height = height;
            this.rectangle.Width = width;
        }

        /// <summary>
        /// Overrides the abstract paint method
        /// </summary>
        /// <param name="g">a graphics object onto which to paint</param>
        public override void Paint(System.Drawing.Graphics g)
        {

            //if (rectangle.X < 1)
            //    rectangle.X = 0;
            //if (rectangle.Y < 1)
            //    rectangle.Y = 0;

            //if ((rectangle.X + this.Width) > gm.Width) rectangle.X = gm.Width - Convert.ToInt32(this.Width);
            //if ((rectangle.Y + this.Height) > gm.Height) rectangle.Y = gm.Height - Convert.ToInt32(this.Height);


            return;
        }

        /// <summary>
        /// Override the abstract Hit method
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public override bool Hit(System.Drawing.Point p)
        {
            return false;
        }

        /// <summary>
        /// Overrides the abstract Invalidate method
        /// </summary>
        public override void Invalidate()
        {
        }


        #endregion
    }
}