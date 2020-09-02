using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UymBarkod.Barcode;

namespace UymBarkod
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnEan13_Click(object sender, EventArgs e)
        {

            Ean13 barkod = new Ean13();
            barkod.Text = "869269150003";
            barkod.ShapeColor = System.Drawing.Color.White;
            barkod.Width = 133;
            barkod.Height = 98;
            barkod.X = 20;
            barkod.Y = 20;

            System.Drawing.Image img = new Bitmap(133, 98);
            barkod.Paint(Graphics.FromImage(img));
            img.Save(Server.MapPath("~") + "/aa.jpeg", System.Drawing.Imaging.ImageFormat.Bmp);

            imgBarkod.ImageUrl = "aa.jpeg";

        }

        protected void btnCode128_Click(object sender, EventArgs e)
        {

            Code128Yeni barkod = new Code128Yeni();
            barkod.Text = "869269150003";
            barkod.ShapeColor = System.Drawing.Color.White;
            barkod.Width = 266;
            barkod.Height = 196;
            barkod.X = 20;
            barkod.Y = 20;
            barkod.Yon = true;

            System.Drawing.Image img = new Bitmap(266, 196);
            barkod.Paint(Graphics.FromImage(img));
            img.Save(Server.MapPath("~") + "/bb.jpeg", System.Drawing.Imaging.ImageFormat.Bmp);

            imgBarkod.ImageUrl = "bb.jpeg";


            //Code128 barkod = new Code128();
            //barkod.Text = "abcd123456789dda";
            //barkod.ShapeColor = System.Drawing.Color.White;
            //barkod.Width = 266;
            //barkod.Height = 190;
            //barkod.X = 20;
            //barkod.Y = 20;
            //barkod.Yon = true;

            //System.Drawing.Image img = new Bitmap(133, 98);
            //barkod.Paint(Graphics.FromImage(img));
            //img.Save(Server.MapPath("~") + "/bb.jpeg", System.Drawing.Imaging.ImageFormat.Bmp);
        }
    }
}