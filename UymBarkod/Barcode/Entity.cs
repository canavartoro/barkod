using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Web;

namespace UymBarkod.Barcode
{
    public abstract class Entity
    {
        #region Fields
        /// <summary>
        /// tells whether the current entity is hovered by the mouse
        /// </summary>
        protected internal bool hovered = false;
        /// <summary>
        /// tells whether the entity is selected
        /// </summary>
        protected bool isSelected = false;

        /// <summary>
        /// Default font for drawing text
        /// </summary>
        protected Font font = new Font("Verdana", 10F);

        /// <summary>
        /// Default black pen
        /// </summary>
        protected Pen blackPen = new Pen(Brushes.Black, 1F);

        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets whether the entity is selected
        /// </summary>
        [Browsable(false)]
        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; }
        }

        [Browsable(true)]
        public virtual Font LFont
        {
            get { return font; }
            set { font = value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Default ctor
        /// </summary>
        public Entity()
        {
        }



        #endregion

        #region Methods
        /// <summary>
        /// Paints the entity on the control
        /// </summary>
        /// <param name="g">the graphics object to paint on</param>
        public abstract void Paint(Graphics g);
        /// <summary>
        /// Tests whether the shape is hit by the mouse
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public abstract bool Hit(Point p);
        /// <summary>
        /// Invalidates the entity
        /// </summary>
        public abstract void Invalidate();

        #endregion
    }
}