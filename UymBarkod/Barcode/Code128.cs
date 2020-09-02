using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;

namespace UymBarkod.Barcode
{
    public enum CodeSet
    {
        CodeA,
        CodeB
    }

    public enum CodeSetAllowed
    {
        CodeA,
        CodeB,
        CodeAorB
    }

    public class Code128 : ShapeBase
    {
        private float _fFontSize;
        private float _fHeight;
        private float _fScale;
        private float _fWidth;
        private string _sName;
        private bool _yon;
        private const int cCODEA = 0x65;
        private const int cCODEB = 100;
        private static readonly int[,] cPatterns = new int[,] {
            { 2, 1, 2, 2, 2, 2, 0, 0 }, { 2, 2, 2, 1, 2, 2, 0, 0 }, { 2, 2, 2, 2, 2, 1, 0, 0 }, { 1, 2, 1, 2, 2, 3, 0, 0 }, { 1, 2, 1, 3, 2, 2, 0, 0 }, { 1, 3, 1, 2, 2, 2, 0, 0 }, { 1, 2, 2, 2, 1, 3, 0, 0 }, { 1, 2, 2, 3, 1, 2, 0, 0 }, { 1, 3, 2, 2, 1, 2, 0, 0 }, { 2, 2, 1, 2, 1, 3, 0, 0 }, { 2, 2, 1, 3, 1, 2, 0, 0 }, { 2, 3, 1, 2, 1, 2, 0, 0 }, { 1, 1, 2, 2, 3, 2, 0, 0 }, { 1, 2, 2, 1, 3, 2, 0, 0 }, { 1, 2, 2, 2, 3, 1, 0, 0 }, { 1, 1, 3, 2, 2, 2, 0, 0 },
            { 1, 2, 3, 1, 2, 2, 0, 0 }, { 1, 2, 3, 2, 2, 1, 0, 0 }, { 2, 2, 3, 2, 1, 1, 0, 0 }, { 2, 2, 1, 1, 3, 2, 0, 0 }, { 2, 2, 1, 2, 3, 1, 0, 0 }, { 2, 1, 3, 2, 1, 2, 0, 0 }, { 2, 2, 3, 1, 1, 2, 0, 0 }, { 3, 1, 2, 1, 3, 1, 0, 0 }, { 3, 1, 1, 2, 2, 2, 0, 0 }, { 3, 2, 1, 1, 2, 2, 0, 0 }, { 3, 2, 1, 2, 2, 1, 0, 0 }, { 3, 1, 2, 2, 1, 2, 0, 0 }, { 3, 2, 2, 1, 1, 2, 0, 0 }, { 3, 2, 2, 2, 1, 1, 0, 0 }, { 2, 1, 2, 1, 2, 3, 0, 0 }, { 2, 1, 2, 3, 2, 1, 0, 0 },
            { 2, 3, 2, 1, 2, 1, 0, 0 }, { 1, 1, 1, 3, 2, 3, 0, 0 }, { 1, 3, 1, 1, 2, 3, 0, 0 }, { 1, 3, 1, 3, 2, 1, 0, 0 }, { 1, 1, 2, 3, 1, 3, 0, 0 }, { 1, 3, 2, 1, 1, 3, 0, 0 }, { 1, 3, 2, 3, 1, 1, 0, 0 }, { 2, 1, 1, 3, 1, 3, 0, 0 }, { 2, 3, 1, 1, 1, 3, 0, 0 }, { 2, 3, 1, 3, 1, 1, 0, 0 }, { 1, 1, 2, 1, 3, 3, 0, 0 }, { 1, 1, 2, 3, 3, 1, 0, 0 }, { 1, 3, 2, 1, 3, 1, 0, 0 }, { 1, 1, 3, 1, 2, 3, 0, 0 }, { 1, 1, 3, 3, 2, 1, 0, 0 }, { 1, 3, 3, 1, 2, 1, 0, 0 },
            { 3, 1, 3, 1, 2, 1, 0, 0 }, { 2, 1, 1, 3, 3, 1, 0, 0 }, { 2, 3, 1, 1, 3, 1, 0, 0 }, { 2, 1, 3, 1, 1, 3, 0, 0 }, { 2, 1, 3, 3, 1, 1, 0, 0 }, { 2, 1, 3, 1, 3, 1, 0, 0 }, { 3, 1, 1, 1, 2, 3, 0, 0 }, { 3, 1, 1, 3, 2, 1, 0, 0 }, { 3, 3, 1, 1, 2, 1, 0, 0 }, { 3, 1, 2, 1, 1, 3, 0, 0 }, { 3, 1, 2, 3, 1, 1, 0, 0 }, { 3, 3, 2, 1, 1, 1, 0, 0 }, { 3, 1, 4, 1, 1, 1, 0, 0 }, { 2, 2, 1, 4, 1, 1, 0, 0 }, { 4, 3, 1, 1, 1, 1, 0, 0 }, { 1, 1, 1, 2, 2, 4, 0, 0 },
            { 1, 1, 1, 4, 2, 2, 0, 0 }, { 1, 2, 1, 1, 2, 4, 0, 0 }, { 1, 2, 1, 4, 2, 1, 0, 0 }, { 1, 4, 1, 1, 2, 2, 0, 0 }, { 1, 4, 1, 2, 2, 1, 0, 0 }, { 1, 1, 2, 2, 1, 4, 0, 0 }, { 1, 1, 2, 4, 1, 2, 0, 0 }, { 1, 2, 2, 1, 1, 4, 0, 0 }, { 1, 2, 2, 4, 1, 1, 0, 0 }, { 1, 4, 2, 1, 1, 2, 0, 0 }, { 1, 4, 2, 2, 1, 1, 0, 0 }, { 2, 4, 1, 2, 1, 1, 0, 0 }, { 2, 2, 1, 1, 1, 4, 0, 0 }, { 4, 1, 3, 1, 1, 1, 0, 0 }, { 2, 4, 1, 1, 1, 2, 0, 0 }, { 1, 3, 4, 1, 1, 1, 0, 0 },
            { 1, 1, 1, 2, 4, 2, 0, 0 }, { 1, 2, 1, 1, 4, 2, 0, 0 }, { 1, 2, 1, 2, 4, 1, 0, 0 }, { 1, 1, 4, 2, 1, 2, 0, 0 }, { 1, 2, 4, 1, 1, 2, 0, 0 }, { 1, 2, 4, 2, 1, 1, 0, 0 }, { 4, 1, 1, 2, 1, 2, 0, 0 }, { 4, 2, 1, 1, 1, 2, 0, 0 }, { 4, 2, 1, 2, 1, 1, 0, 0 }, { 2, 1, 2, 1, 4, 1, 0, 0 }, { 2, 1, 4, 1, 2, 1, 0, 0 }, { 4, 1, 2, 1, 2, 1, 0, 0 }, { 1, 1, 1, 1, 4, 3, 0, 0 }, { 1, 1, 1, 3, 4, 1, 0, 0 }, { 1, 3, 1, 1, 4, 1, 0, 0 }, { 1, 1, 4, 1, 1, 3, 0, 0 },
            { 1, 1, 4, 3, 1, 1, 0, 0 }, { 4, 1, 1, 1, 1, 3, 0, 0 }, { 4, 1, 1, 3, 1, 1, 0, 0 }, { 1, 1, 3, 1, 4, 1, 0, 0 }, { 1, 1, 4, 1, 3, 1, 0, 0 }, { 3, 1, 1, 1, 4, 1, 0, 0 }, { 4, 1, 1, 1, 3, 1, 0, 0 }, { 2, 1, 1, 4, 1, 2, 0, 0 }, { 2, 1, 1, 2, 1, 4, 0, 0 }, { 2, 1, 1, 2, 3, 2, 0, 0 }, { 2, 3, 3, 1, 1, 1, 2, 0 }
         };
        private const int cSHIFT = 0x62;
        private const int cSTARTA = 0x67;
        private const int cSTARTB = 0x68;
        private const int cSTOP = 0x6a;
        private bool QuietZone;

        public Code128()
        {
            this._sName = "CODE128";
            this.QuietZone = true;
            this._fWidth = 0x71;
            this._fHeight = 0x4e;
            this._fFontSize = 8f;
            this._fScale = 1;
            this._yon = true;
            base.shapeColor = Color.Transparent;
        }

        public Code128(string mfgNumber, string productId)
        {
            this._sName = "CODE128";
            this.QuietZone = true;
            this._fWidth = 0x71;
            this._fHeight = 0x4e;
            this._fFontSize = 8f;
            this._fScale = 1;
            this._yon = true;
        }

        public Code128(string countryCode, string mfgNumber, string productId)
        {
            this._sName = "CODE128";
            this.QuietZone = true;
            this._fWidth = 0x71;
            this._fHeight = 0x4e;
            this._fFontSize = 8f;
            this._fScale = 1;
            this._yon = true;
        }

        public Code128(string countryCode, string mfgNumber, string productId, string checkDigit)
        {
            this._sName = "CODE128";
            this.QuietZone = true;
            this._fWidth = 0x71;
            this._fHeight = 0x4e;
            this._fFontSize = 8f;
            this._fScale = 1;
            this._yon = true;
        }

        public static bool CharCompatibleWithCodeset(int CharAscii, CodeSet currcs)
        {
            CodeSetAllowed allowed = CodesetAllowedForChar(CharAscii);
            return (((allowed == CodeSetAllowed.CodeAorB) || ((allowed == CodeSetAllowed.CodeA) && (currcs == CodeSet.CodeA))) || ((allowed == CodeSetAllowed.CodeB) && (currcs == CodeSet.CodeB)));
        }

        public static CodeSetAllowed CodesetAllowedForChar(int CharAscii)
        {
            if ((CharAscii >= 0x20) && (CharAscii <= 0x5f))
            {
                return CodeSetAllowed.CodeAorB;
            }
            return ((CharAscii < 0x20) ? CodeSetAllowed.CodeA : CodeSetAllowed.CodeB);
        }

        public static int[] CodesForChar(int CharAscii, int LookAheadAscii, ref CodeSet CurrCodeSet)
        {
            int num = -1;
            if (!CharCompatibleWithCodeset(CharAscii, CurrCodeSet))
            {
                if ((LookAheadAscii != -1) && !CharCompatibleWithCodeset(LookAheadAscii, CurrCodeSet))
                {
                    switch (CurrCodeSet)
                    {
                        case CodeSet.CodeA:
                            num = 100;
                            CurrCodeSet = CodeSet.CodeB;
                            goto Label_0052;

                        case CodeSet.CodeB:
                            num = 0x65;
                            CurrCodeSet = CodeSet.CodeA;
                            goto Label_0052;
                    }
                }
                else
                {
                    num = 0x62;
                }
            }
            Label_0052:
            if (num != -1)
            {
                return new int[] { num, CodeValueForChar(CharAscii) };
            }
            return new int[] { CodeValueForChar(CharAscii) };
        }

        public static int CodeValueForChar(int CharAscii)
        {
            return ((CharAscii >= 0x20) ? (CharAscii - 0x20) : (CharAscii + 0x40));
        }

        public Bitmap CreateBitmap()
        {
            float num = (this.Width * this.Scale) * 100;
            float num2 = (this.Height * this.Scale) * 100f;
            Bitmap image = new Bitmap((int)num, (int)num2);
            Graphics.FromImage(image).Dispose();
            return image;
        }

        private static CodeSet GetBestStartSet(CodeSetAllowed csa1, CodeSetAllowed csa2)
        {
            int num = 0;
            num += (csa1 == CodeSetAllowed.CodeA) ? 1 : 0;
            num += (csa1 == CodeSetAllowed.CodeB) ? -1 : 0;
            num += (csa2 == CodeSetAllowed.CodeA) ? 1 : 0;
            num += (csa2 == CodeSetAllowed.CodeB) ? -1 : 0;
            return ((num > 0) ? CodeSet.CodeA : CodeSet.CodeB);
        }

        public override bool Hit(Point p)
        {
            Rectangle rect = new Rectangle(p, new Size(5, 5));
            return this.rectangle.Contains(rect);
        }

        public override void Invalidate()
        {
            Rectangle rc = base.rectangle;
            rc.Offset(-5, -5);
            rc.Inflate(20, 20);
        }


        public Image MakeBarcodeImage(string InputData, int BarWeight, bool AddQuietZone)
        {
            Bitmap image = new Bitmap((int)this.Width, (int)this.Height);
            Graphics graphics = Graphics.FromImage(image);
            int[] numArray = StringToCode128(InputData);
            int num = (((numArray.Length - 3) * 0x15) + 0x2d) * BarWeight;
            int height = 40;
            if (AddQuietZone)
            {
                num += 20 * BarWeight;
            }
            int x = AddQuietZone ? (10 * BarWeight) : 0;
            for (int i = 0; i < numArray.Length; i++)
            {
                int num5 = numArray[i];
                for (int j = 0; j < 8; j += 2)
                {
                    int width = cPatterns[num5, j] * BarWeight;
                    int num8 = cPatterns[num5, j + 1] * BarWeight;
                    if (width > 0)
                    {
                        graphics.FillRectangle(Brushes.Black, x, 0, width, height);
                    }
                    x += width + num8;
                }
            }
            if (this._fScale == 1)
            {
                graphics.DrawString(InputData, new Font("Microsoft Sans Serif", 7f), Brushes.Black, new PointF((float)(x / 2), (float)height));
            }
            else
            {
                graphics.DrawString(InputData, new Font("Microsoft Sans Serif", 8f, FontStyle.Bold), Brushes.Black, new PointF((float)(x / 2), (float)height));
            }
            image.RotateFlip(RotateFlipType.Rotate90FlipX);
            return image;
        }

        public override void Paint(Graphics g)
        {
            int[] numArray;
            float num;
            float num2;
            float num3;
            int num4;
            int num5;
            int num6;
            float num7;
            float num8;
            if (this._yon)
            {

                g.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.White), 0, 0, Width, Height);

                g.FillRectangle(base.shapeBrush, base.rectangle);
                if (base.hovered || base.isSelected)
                {
                    g.DrawRectangle(new Pen(Color.Red, 2f), new Rectangle(base.Location, new Size((int)this.Width, (int)this.Height)));
                }
                if (base.text != string.Empty)
                {
                    numArray = StringToCode128(base.text);
                    num = (((numArray.Length - 3) * 0x15) + 0x2d) * this._fScale;
                    num2 = this._fHeight;
                    if (this.QuietZone)
                    {
                        num += 20 * this._fScale;
                    }
                    num3 = this.QuietZone ? (base.Location.X + (10 * this._fScale)) : base.Location.X;
                    for (num4 = 0; num4 < numArray.Length; num4++)
                    {
                        num5 = numArray[num4];
                        num6 = 0;
                        while (num6 < 8)
                        {
                            num7 = cPatterns[num5, num6] * this._fScale;
                            num8 = cPatterns[num5, num6 + 1] * this._fScale;
                            if (num7 > 0)
                            {
                                g.FillRectangle(Brushes.Black, num3, base.Location.Y, num7, num2);
                            }
                            num3 += num7 + num8;
                            num6 += 2;
                        }
                    }
                    if (this._fScale == 1)
                    {
                        g.DrawString(base.text, new Font("Microsoft Sans Serif", 7f), Brushes.Black, new PointF(base.Location.X + (base.text.Length * 6), base.Location.Y + num2));
                    }
                    else
                    {
                        g.DrawString(base.text, new Font("Microsoft Sans Serif", 8f, FontStyle.Bold), Brushes.Black, new PointF(base.Location.X + (base.text.Length * 10), base.Location.Y + num2));
                    }
                }
            }
            else
            {
                g.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.White), 0, 0, Height, Width);
                g.FillRectangle(base.shapeBrush, base.rectangle);
                if (base.hovered || base.isSelected)
                {
                    g.DrawRectangle(new Pen(Color.Red, 2f), new Rectangle(new Point(base.Location.X, base.Location.Y), new Size((int)this.Height, (int)this.Width)));
                }
                if (base.text != string.Empty)
                {
                    numArray = StringToCode128(base.text);
                    num = (((numArray.Length - 3) * 0x15) + 0x2d) * this._fScale;
                    num2 = this._fHeight;
                    if (this.QuietZone)
                    {
                        num += 20 * this._fScale;
                    }
                    num3 = this.QuietZone ? (base.Location.Y + (10 * this._fScale)) : base.Location.Y;
                    for (num4 = 0; num4 < numArray.Length; num4++)
                    {
                        num5 = numArray[num4];
                        for (num6 = 0; num6 < 8; num6 += 2)
                        {
                            num7 = cPatterns[num5, num6] * this._fScale;
                            num8 = cPatterns[num5, num6 + 1] * this._fScale;
                            if (num7 > 0)
                            {
                                g.FillRectangle(Brushes.Black, base.Location.X, num3, num2, num7);
                            }
                            num3 += num7 + num8;
                        }
                    }
                    StringFormat format = new StringFormat(StringFormatFlags.DirectionVertical);
                    if (this._fScale == 1)
                    {
                        g.DrawString(base.text, base.font, Brushes.Black, new PointF((float)(base.Location.X - (this._fScale * 0x10)), (float)(base.Location.Y + (base.text.Length * 3))), format);
                    }
                    else
                    {
                        g.DrawString(base.text, new Font("Microsoft Sans Serif", 8f, FontStyle.Bold), Brushes.Black, new PointF((float)(base.Location.X - (this._fScale * 0x10)), base.Location.Y + num2), format);
                    }
                }
            }
        }

        public void Resize(int width, int height)
        {
            base.Resize(width, height);
            this.Invalidate();
        }

        public static int StartCodeForCodeSet(CodeSet cs)
        {
            return ((cs == CodeSet.CodeA) ? 0x67 : 0x68);
        }

        public static int StopCode()
        {
            return 0x6a;
        }

        public static int[] StringToCode128(string AsciiData)
        {
            int num;
            byte[] bytes = Encoding.ASCII.GetBytes(AsciiData);
            CodeSetAllowed allowed = (bytes.Length > 0) ? CodesetAllowedForChar(bytes[0]) : CodeSetAllowed.CodeAorB;
            CodeSetAllowed allowed2 = (bytes.Length > 0) ? CodesetAllowedForChar(bytes[1]) : CodeSetAllowed.CodeAorB;
            CodeSet bestStartSet = GetBestStartSet(allowed, allowed2);
            ArrayList list = new ArrayList(bytes.Length + 3);
            list.Add(StartCodeForCodeSet(bestStartSet));
            for (num = 0; num < bytes.Length; num++)
            {
                int charAscii = bytes[num];
                int lookAheadAscii = (bytes.Length > (num + 1)) ? bytes[num + 1] : -1;
                list.AddRange(CodesForChar(charAscii, lookAheadAscii, ref bestStartSet));
            }
            int num4 = (int)list[0];
            for (num = 1; num < list.Count; num++)
            {
                num4 += num * ((int)list[num]);
            }
            list.Add(num4 % 0x67);
            list.Add(StopCode());
            return (list.ToArray(typeof(int)) as int[]);
        }

        public float FontSize
        {
            get
            {
                return this._fFontSize;
            }
        }

        public float Height
        {
            get
            {
                return (float)this._fHeight;
            }
            set
            {
                this._fHeight = Convert.ToInt32(value);
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
                this.Scale = (int)(value.Size / 8f);
                base.Width *= (int)this.Scale;
                base.Height *= (int)this.Scale;
            }
        }

        public string Name
        {
            get
            {
                return this._sName;
            }
            set { _sName = value; }
        }

        private bool QQuietZone
        {
            get
            {
                return this.QuietZone;
            }
            set
            {
                this.QuietZone = value;
            }
        }//kullanici gormesin

        public float Scale
        {
            get
            {
                return this._fScale;
            }
            set
            {
                if ((value < 0) || (value > 2))
                {
                    this._fScale = 1;
                }
                else
                {
                    this._fScale = value;
                }
            }
        }

        public override string Text
        {
            get
            {
                return base.text;
            }
            set
            {
                base.text = value.Replace(" ", "");
            }
        }

        public float Width
        {
            get
            {
                return this._fWidth;
            }
            set
            {
                this._fWidth = value;
            }
        }

        public bool Yon
        {
            get
            {
                return this._yon;
            }
            set
            {
                this._yon = value;
            }
        }
    }
}