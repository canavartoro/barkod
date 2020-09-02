using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;

namespace UymBarkod.Barcode
{
    public class Ean13 : ShapeBase
    {
        #region Özel değişkenler
        private string _sName = "EAN13";


        private float _fMinimumAllowableScale = .8f;
        private float _fMaximumAllowableScale = 2.0f;

        // This is the nomimal size recommended by the EAN.
        private int _fWidth = 113;
        private int _fHeight = 78;
        private float _fFontSize = 8.0f;
        private float _fScale = 1.0f;
        private bool showtext = true;

        // Left Hand Digits.
        private string[] _aOddLeft = { "0001101", "0011001", "0010011", "0111101",
                                          "0100011", "0110001", "0101111", "0111011",
                                          "0110111", "0001011" };

        private string[] _aEvenLeft = { "0100111", "0110011", "0011011", "0100001",
                                           "0011101", "0111001", "0000101", "0010001",
                                           "0001001", "0010111" };

        // Right Hand Digits.
        private string[] _aRight = { "1110010", "1100110", "1101100", "1000010",
                                        "1011100", "1001110", "1010000", "1000100",
                                        "1001000", "1110100" };

        private string _sQuiteZone = "000000000";

        private string _sLeadTail = "101";

        private string _sSeparator = "01010";

        private string _sCountryCode = "00";
        private string _sManufacturerCode;
        private string _sProductCode;
        private string _sChecksumDigit;

        #endregion

        #region Constructor


        public Ean13()
        {
            this.shapeColor = Color.Transparent;
        }

        public Ean13(string mfgNumber, string productId)
        {
            this.CountryCode = "00";
            this.ManufacturerCode = mfgNumber;
            this.ProductCode = productId;
            this.CalculateChecksumDigit();
        }

        public Ean13(string countryCode, string mfgNumber, string productId)
        {
            this.CountryCode = countryCode;
            this.ManufacturerCode = mfgNumber;
            this.ProductCode = productId;
            this.CalculateChecksumDigit();
        }

        public Ean13(string countryCode, string mfgNumber, string productId, string checkDigit)
        {
            this.CountryCode = countryCode;
            this.ManufacturerCode = mfgNumber;
            this.ProductCode = productId;
            this.ChecksumDigit = checkDigit;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Tests whether the mouse hits this shape
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public override bool Hit(System.Drawing.Point p)
        {
            System.Drawing.Rectangle r = new System.Drawing.Rectangle(p, new System.Drawing.Size(5, 5));
            return rectangle.Contains(r);
        }

        /// <summary>
        /// Paints the shape on the canvas
        /// </summary>
        /// <param name="g"></param>
        public override void Paint(System.Drawing.Graphics g)
        {

            g.FillRectangle(shapeBrush, rectangle);
            if (hovered || isSelected)
                g.DrawRectangle(new System.Drawing.Pen(Color.Red, 2F), rectangle);

            //g.DrawString(rectangle.Location.ToString(), font, Brushes.Black, rectangle.X + 10, rectangle.Y + 10);

            //g.DrawString(gm.Size.ToString(), font, Brushes.Black, rectangle.X + 10, rectangle.Y + 30);


            //well, a lot should be said here like
            //the fact that one should measure the text before drawing it,
            //resize the width and height if the text if bigger than the rectangle,
            //alignment can be set and changes the drawing as well...
            //here we keep it really simple:

            if (text != string.Empty)
                this.DrawEan13Barcode(g, new System.Drawing.Point(rectangle.X + 2, rectangle.Y + 2));
            //g.DrawString(text,font,Brushes.Black, rectangle.X+10,rectangle.Y+10);
        }
        /// <summary>
        /// Invalidates the shape
        /// </summary>
        public override void Invalidate()
        {
            System.Drawing.Rectangle r = rectangle;
            r.Offset(-5, -5);
            r.Inflate(20, 20);
        }


        public void DrawEan13Barcode(System.Drawing.Graphics g, System.Drawing.Point pt)
        {

            g.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.White), 0, 0, Width,Height);

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

            int width = (int)(this._fWidth * this.Scale);
            int height = (int)(this._fHeight * this.Scale);

            //	EAN13 Barcode should be a total of 113 modules wide.
            int lineWidth = (int)(width / 113f);

            // Save the GraphicsState.
            System.Drawing.Drawing2D.GraphicsState gs = g.Save();

            // Pixel cinsine çevirdim tüm ölçüleri
            //g.PageUnit = System.Drawing.GraphicsUnit.Point;

            // Set the PageScale to 1, so a millimeter will represent a true millimeter.
            //g.PageScale = 1;

            System.Drawing.SolidBrush brush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
            System.Drawing.SolidBrush brush2 = new System.Drawing.SolidBrush(System.Drawing.Color.White);


            int xPosition = pt.X;

            System.Text.StringBuilder strbEAN13 = new System.Text.StringBuilder();
            System.Text.StringBuilder sbTemp = new System.Text.StringBuilder();

            int xStart = pt.X;
            int yStart = pt.Y;
            int xEnd = 0;

            System.Drawing.Font font = new System.Drawing.Font("Arial", this._fFontSize * this.Scale);

            // Calculate the Check Digit.
            this.CalculateChecksumDigit();

            sbTemp.AppendFormat("{0}{1}{2}{3}",
                this.CountryCode,
                this.ManufacturerCode,
                this.ProductCode,
                this.ChecksumDigit);


            string sTemp = sbTemp.ToString();

            string sLeftPattern = "";

            // Convert the left hand numbers.
            sLeftPattern = ConvertLeftPattern(sTemp.Substring(0, 7));

            // Build the UPC Code.
            strbEAN13.AppendFormat("{0}{1}{2}{3}{4}{1}{0}",
                this._sQuiteZone, this._sLeadTail,
                sLeftPattern,
                this._sSeparator,
                ConvertToDigitPatterns(sTemp.Substring(7), this._aRight));

            string sTempUPC = strbEAN13.ToString();

            int fTextHeight = (int)(g.MeasureString(sTempUPC, font).Height);

            // Draw the barcode lines.
            for (int i = 0; i < strbEAN13.Length; i++)
            {
                if (sTempUPC.Substring(i, 1) == "1")
                {
                    if (xStart == pt.X)
                        xStart = xPosition;

                    // Save room for the UPC number below the bar code.
                    if ((i > 12 && i < 55) || (i > 57 && i < 101))
                        // Draw space for the number
                        g.FillRectangle(brush, xPosition, yStart, lineWidth, height - fTextHeight);
                    else
                        // Draw a full line.
                        if (showtext)
                        g.FillRectangle(brush, xPosition, yStart, lineWidth, height);
                    else
                        g.FillRectangle(brush, xPosition, yStart, lineWidth, height - fTextHeight);
                }

                xPosition += lineWidth;
                xEnd = xPosition;
            }

            // Draw the upc numbers below the line.
            xPosition = (int)(xStart - g.MeasureString(this.CountryCode.Substring(0, 1), font).Width);
            int yPosition = yStart + (height - fTextHeight);

            // Draw 1st digit of the country code.
            if (showtext)
                g.DrawString(sTemp.Substring(0, 1), font, brush, new System.Drawing.PointF(xPosition, yPosition));

            xPosition += (int)((g.MeasureString(sTemp.Substring(0, 1), font).Width + 43 * lineWidth) -
                (g.MeasureString(sTemp.Substring(1, 6), font).Width));

            // Draw MFG Number.
            if (showtext)
                g.DrawString(sTemp.Substring(1, 6), font, brush, new System.Drawing.PointF(xPosition, yPosition));

            xPosition += (int)(g.MeasureString(sTemp.Substring(1, 6), font).Width + (11 * lineWidth));

            // Draw Product ID.
            if (showtext)
                g.DrawString(sTemp.Substring(7), font, brush, new System.Drawing.PointF(xPosition, yPosition));

            // Restore the GraphicsState.
            g.Restore(gs);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        }



        public System.Drawing.Bitmap CreateBitmap()
        {
            float tempWidth = (this.Width * this.Scale) * 100;
            float tempHeight = (this.Height * this.Scale) * 100;

            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap((int)tempWidth, (int)tempHeight);

            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bmp);
            this.DrawEan13Barcode(g, new System.Drawing.Point(0, 0));
            g.Dispose();
            return bmp;
        }


        private string ConvertLeftPattern(string sLeft)
        {
            switch (sLeft.Substring(0, 1))
            {
                case "0":
                    return CountryCode0(sLeft.Substring(1));

                case "1":
                    return CountryCode1(sLeft.Substring(1));

                case "2":
                    return CountryCode2(sLeft.Substring(1));

                case "3":
                    return CountryCode3(sLeft.Substring(1));

                case "4":
                    return CountryCode4(sLeft.Substring(1));

                case "5":
                    return CountryCode5(sLeft.Substring(1));

                case "6":
                    return CountryCode6(sLeft.Substring(1));

                case "7":
                    return CountryCode7(sLeft.Substring(1));

                case "8":
                    return CountryCode8(sLeft.Substring(1));

                case "9":
                    return CountryCode9(sLeft.Substring(1));

                default:
                    return "";
            }
        }


        private string CountryCode0(string sLeft)
        {
            // 0 Odd Odd  Odd  Odd  Odd  Odd 
            return ConvertToDigitPatterns(sLeft, this._aOddLeft);
        }


        private string CountryCode1(string sLeft)
        {
            // 1 Odd Odd  Even Odd  Even Even 
            System.Text.StringBuilder sReturn = new StringBuilder();
            // The two lines below could be replaced with this:
            // sReturn.Append( ConvertToDigitPatterns( sLeft.Substring( 0, 2 ), this._aOddLeft ) );
            sReturn.Append(ConvertToDigitPatterns(sLeft.Substring(0, 1), this._aOddLeft));
            sReturn.Append(ConvertToDigitPatterns(sLeft.Substring(1, 1), this._aOddLeft));
            sReturn.Append(ConvertToDigitPatterns(sLeft.Substring(2, 1), this._aEvenLeft));
            sReturn.Append(ConvertToDigitPatterns(sLeft.Substring(3, 1), this._aOddLeft));
            // The two lines below could be replaced with this:
            // sReturn.Append( ConvertToDigitPatterns( sLeft.Substring( 4, 2 ), this._aEvenLeft ) );
            sReturn.Append(ConvertToDigitPatterns(sLeft.Substring(4, 1), this._aEvenLeft));
            sReturn.Append(ConvertToDigitPatterns(sLeft.Substring(5, 1), this._aEvenLeft));
            return sReturn.ToString();
        }


        private string CountryCode2(string sLeft)
        {
            // 2 Odd Odd  Even Even Odd  Even 
            System.Text.StringBuilder sReturn = new StringBuilder();
            sReturn.Append(ConvertToDigitPatterns(sLeft.Substring(0, 1), this._aOddLeft));
            sReturn.Append(ConvertToDigitPatterns(sLeft.Substring(1, 1), this._aOddLeft));
            sReturn.Append(ConvertToDigitPatterns(sLeft.Substring(2, 1), this._aEvenLeft));
            sReturn.Append(ConvertToDigitPatterns(sLeft.Substring(3, 1), this._aEvenLeft));
            sReturn.Append(ConvertToDigitPatterns(sLeft.Substring(4, 1), this._aOddLeft));
            sReturn.Append(ConvertToDigitPatterns(sLeft.Substring(5, 1), this._aEvenLeft));
            return sReturn.ToString();
        }


        private string CountryCode3(string sLeft)
        {
            // 3 Odd Odd  Even Even Even Odd 
            System.Text.StringBuilder sReturn = new StringBuilder();
            sReturn.Append(ConvertToDigitPatterns(sLeft.Substring(0, 1), this._aOddLeft));
            sReturn.Append(ConvertToDigitPatterns(sLeft.Substring(1, 1), this._aOddLeft));
            sReturn.Append(ConvertToDigitPatterns(sLeft.Substring(2, 1), this._aEvenLeft));
            sReturn.Append(ConvertToDigitPatterns(sLeft.Substring(3, 1), this._aEvenLeft));
            sReturn.Append(ConvertToDigitPatterns(sLeft.Substring(4, 1), this._aEvenLeft));
            sReturn.Append(ConvertToDigitPatterns(sLeft.Substring(5, 1), this._aOddLeft));
            return sReturn.ToString();
        }


        private string CountryCode4(string sLeft)
        {
            // 4 Odd Even Odd  Odd  Even Even 
            System.Text.StringBuilder sReturn = new StringBuilder();
            sReturn.Append(ConvertToDigitPatterns(sLeft.Substring(0, 1), this._aOddLeft));
            sReturn.Append(ConvertToDigitPatterns(sLeft.Substring(1, 1), this._aEvenLeft));
            sReturn.Append(ConvertToDigitPatterns(sLeft.Substring(2, 1), this._aOddLeft));
            sReturn.Append(ConvertToDigitPatterns(sLeft.Substring(3, 1), this._aOddLeft));
            sReturn.Append(ConvertToDigitPatterns(sLeft.Substring(4, 1), this._aEvenLeft));
            sReturn.Append(ConvertToDigitPatterns(sLeft.Substring(5, 1), this._aEvenLeft));
            return sReturn.ToString();
        }


        private string CountryCode5(string sLeft)
        {
            // 5 Odd Even Even Odd  Odd  Even 
            System.Text.StringBuilder sReturn = new StringBuilder();
            sReturn.Append(ConvertToDigitPatterns(sLeft.Substring(0, 1), this._aOddLeft));
            sReturn.Append(ConvertToDigitPatterns(sLeft.Substring(1, 1), this._aEvenLeft));
            sReturn.Append(ConvertToDigitPatterns(sLeft.Substring(2, 1), this._aEvenLeft));
            sReturn.Append(ConvertToDigitPatterns(sLeft.Substring(3, 1), this._aOddLeft));
            sReturn.Append(ConvertToDigitPatterns(sLeft.Substring(4, 1), this._aOddLeft));
            sReturn.Append(ConvertToDigitPatterns(sLeft.Substring(5, 1), this._aEvenLeft));
            return sReturn.ToString();
        }


        private string CountryCode6(string sLeft)
        {
            // 6 Odd Even Even Even Odd  Odd 
            System.Text.StringBuilder sReturn = new StringBuilder();
            sReturn.Append(ConvertToDigitPatterns(sLeft.Substring(0, 1), this._aOddLeft));
            sReturn.Append(ConvertToDigitPatterns(sLeft.Substring(1, 1), this._aEvenLeft));
            sReturn.Append(ConvertToDigitPatterns(sLeft.Substring(2, 1), this._aEvenLeft));
            sReturn.Append(ConvertToDigitPatterns(sLeft.Substring(3, 1), this._aEvenLeft));
            sReturn.Append(ConvertToDigitPatterns(sLeft.Substring(4, 1), this._aOddLeft));
            sReturn.Append(ConvertToDigitPatterns(sLeft.Substring(5, 1), this._aOddLeft));
            return sReturn.ToString();
        }


        private string CountryCode7(string sLeft)
        {
            // 7 Odd Even Odd  Even Odd  Even
            System.Text.StringBuilder sReturn = new StringBuilder();
            sReturn.Append(ConvertToDigitPatterns(sLeft.Substring(0, 1), this._aOddLeft));
            sReturn.Append(ConvertToDigitPatterns(sLeft.Substring(1, 1), this._aEvenLeft));
            sReturn.Append(ConvertToDigitPatterns(sLeft.Substring(2, 1), this._aOddLeft));
            sReturn.Append(ConvertToDigitPatterns(sLeft.Substring(3, 1), this._aEvenLeft));
            sReturn.Append(ConvertToDigitPatterns(sLeft.Substring(4, 1), this._aOddLeft));
            sReturn.Append(ConvertToDigitPatterns(sLeft.Substring(5, 1), this._aEvenLeft));
            return sReturn.ToString();
        }


        private string CountryCode8(string sLeft)
        {
            // 8 Odd Even Odd  Even Even Odd 
            System.Text.StringBuilder sReturn = new StringBuilder();
            sReturn.Append(ConvertToDigitPatterns(sLeft.Substring(0, 1), this._aOddLeft));
            sReturn.Append(ConvertToDigitPatterns(sLeft.Substring(1, 1), this._aEvenLeft));
            sReturn.Append(ConvertToDigitPatterns(sLeft.Substring(2, 1), this._aOddLeft));
            sReturn.Append(ConvertToDigitPatterns(sLeft.Substring(3, 1), this._aEvenLeft));
            sReturn.Append(ConvertToDigitPatterns(sLeft.Substring(4, 1), this._aEvenLeft));
            sReturn.Append(ConvertToDigitPatterns(sLeft.Substring(5, 1), this._aOddLeft));
            return sReturn.ToString();
        }


        private string CountryCode9(string sLeft)
        {
            // 9 Odd Even Even Odd  Even Odd 
            System.Text.StringBuilder sReturn = new StringBuilder();
            sReturn.Append(ConvertToDigitPatterns(sLeft.Substring(0, 1), this._aOddLeft));
            sReturn.Append(ConvertToDigitPatterns(sLeft.Substring(1, 1), this._aEvenLeft));
            sReturn.Append(ConvertToDigitPatterns(sLeft.Substring(2, 1), this._aEvenLeft));
            sReturn.Append(ConvertToDigitPatterns(sLeft.Substring(3, 1), this._aOddLeft));
            sReturn.Append(ConvertToDigitPatterns(sLeft.Substring(4, 1), this._aEvenLeft));
            sReturn.Append(ConvertToDigitPatterns(sLeft.Substring(5, 1), this._aOddLeft));
            return sReturn.ToString();
        }


        private string ConvertToDigitPatterns(string inputNumber, string[] patterns)
        {
            System.Text.StringBuilder sbTemp = new StringBuilder();
            int iIndex = 0;
            for (int i = 0; i < inputNumber.Length; i++)
            {
                iIndex = Convert.ToInt32(inputNumber.Substring(i, 1));
                sbTemp.Append(patterns[iIndex]);
            }
            return sbTemp.ToString();
        }


        public void CalculateChecksumDigit()
        {
            string sTemp = this.CountryCode + this.ManufacturerCode + this.ProductCode;
            int iSum = 0;
            int iDigit = 0;

            // Calculate the checksum digit here.
            for (int i = sTemp.Length; i >= 1; i--)
            {
                iDigit = Convert.ToInt32(sTemp.Substring(i - 1, 1));
                if (i % 2 == 0)
                {	// odd
                    iSum += iDigit * 3;
                }
                else
                {	// even
                    iSum += iDigit * 1;
                }
            }

            int iCheckSum = (10 - (iSum % 10)) % 10;
            this.ChecksumDigit = iCheckSum.ToString();

        }

        #endregion

        #region -- Attributes/Properties --

        public string Name
        {
            get
            {
                return _sName;
            }
            set
            {
                _sName = value;
            }
        }

        public float MinimumAllowableScale
        {
            get
            {
                return _fMinimumAllowableScale;
            }
        }

        public float MaximumAllowableScale
        {
            get
            {
                return _fMaximumAllowableScale;
            }
        }

        public new float Width
        {
            get
            {
                return _fWidth;
            }
            set { this._fWidth = (int)value; }
        }

        public new float Height
        {
            get
            {
                return _fHeight;
            }
            set
            {
                if (value > 37)
                {
                    _fHeight = Convert.ToInt32(value);
                }
            }
        }

        public float FontSize
        {
            get
            {
                return _fFontSize;
            }
        }

        public override Font LFont
        {
            get
            {
                return base.LFont;
            }
            set
            {
                base.LFont = value;
                this.Scale = (value.Size / 8);
                base.Width = (int)(base.Width * this.Scale);
                base.Height = (int)(base.Height * this.Scale);
            }
        }

        public bool ShowText
        {
            get { return showtext; }
            //set { showtext = value; }
        }


        public float Scale
        {
            get
            {
                return _fScale;
            }

            set
            {
                if (value < this._fMinimumAllowableScale || value > this._fMaximumAllowableScale)
                    return;
                _fScale = value;
            }
        }

        public string CountryCode
        {
            get
            {
                return _sCountryCode;
            }

            set
            {
                while (value.Length < 2)
                {
                    value = "0" + value;
                }
                _sCountryCode = value;
            }
        }

        public string ManufacturerCode
        {
            get
            {
                return _sManufacturerCode;
            }

            set
            {
                _sManufacturerCode = value;
            }
        }

        public string ProductCode
        {
            get
            {
                return _sProductCode;
            }

            set
            {
                _sProductCode = value;
            }
        }

        public string ChecksumDigit
        {
            get
            {
                return _sChecksumDigit;
            }

            set
            {
                int iValue = Convert.ToInt32(value);
                if (iValue < 0 || iValue > 9)
                    throw new Exception("The Check Digit must be between 0 and 9.");
                _sChecksumDigit = value;
            }
        }
        public override string Text
        {
            get
            {
                return this.text;
            }
            set
            {
                try
                {
                    long i = Convert.ToInt64(value);
                    string s = i.ToString();
                    s = "000000000000".Substring(0, 12 - s.Length) + s;
                    this.text = s;
                    this.CountryCode = s.Substring(0, 2);
                    this.ManufacturerCode = s.Substring(2, 5);
                    this.ProductCode = s.Substring(7, 5);
                    this.CalculateChecksumDigit();
                }
                catch { }

            }

        }

        #endregion -- Attributes/Properties --

    }
}