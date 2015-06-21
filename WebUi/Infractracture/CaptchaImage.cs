using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace WebUi.Infractracture
{
    /// <summary>
    /// Captcha generator.
    /// </summary>
    public class CaptchaImage : IDisposable
    {
        #region Fields and Properties

        /// <summary>
        /// Constant to find captcha value in session
        /// </summary>
        public const string captchaValueKey = "CaptchaImageText";

        /// <summary>
        /// Text written on captcha
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// Captcha image
        /// </summary>
        public Bitmap Image
        {
            get
            {
                if (isDisposed) throw new ObjectDisposedException("CaptchaImage");
                return image;
            }
            private set { image = value; }
        }

        /// <summary>
        /// Captcha image width
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// Captcha image height
        /// </summary>
        public int Height { get; private set; }

        private string familyName;
        private bool isDisposed;
        private readonly object sinchronise;

        private readonly Random random = new Random();
        private Bitmap image;

        #endregion

        #region Constractor

        /// <summary>
        /// Create captha with generic serif font family.
        /// </summary>
        /// <param name="text">text on the captcha</param>
        /// <param name="width">captcha image width</param>
        /// <param name="height">captcha image height</param>
        public CaptchaImage(string text, int width, int height) : this(text,width,height,"")
        {
        }

        public CaptchaImage(string text, int width, int height, string familyName)
        {
            isDisposed = false;
            sinchronise = new object();
            Text = text;
            SetDimensions(width, height);
            SetFamilyName(familyName);
            GenerateImage();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
         public  void  Dispose()
        {
            lock (sinchronise)
            {
                if (!isDisposed)
                {
                    isDisposed = true;
                    Image.Dispose();
                }
            }

        }

        #endregion

        #region Private Methods

       private void SetDimensions(int aWidth, int aHeight)
        {
            if (aWidth <= 0)
                throw new ArgumentOutOfRangeException("aWidth", aWidth, "Argument out of range, must be greater than zero.");
            if (aHeight <= 0)
                throw new ArgumentOutOfRangeException("aHeight", aHeight, "Argument out of range, must be greater than zero.");
            Width = aWidth;
            Height = aHeight;
        }

        private void SetFamilyName(string aFamilyName)
        {
            try
            {
                Font font = new Font(aFamilyName, 12F);
                familyName = aFamilyName;
                font.Dispose();
            }
            catch (Exception)
            {
                familyName = FontFamily.GenericSerif.Name;
            }
        }

      
        private void GenerateImage()
        {
            Bitmap bitmap = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(bitmap);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rect = new Rectangle(0, 0, Width, Height);
            
            DrawBackground(rect, g);
            Font font = GetFontForImageSize(rect, g);

            var hatchBrush = new HatchBrush(HatchStyle.ZigZag, Color.Gold, Color.Goldenrod);
            DrawTransformedString(font, rect, g, hatchBrush);
            
            AddRandomNoise(rect, g, hatchBrush);

            font.Dispose();
            hatchBrush.Dispose();
            g.Dispose();
            
            Image = bitmap;
        }

        private void AddRandomNoise(Rectangle rect, Graphics g, HatchBrush hatchBrush)
        {
            int m = Math.Max(rect.Width, rect.Height);
            for (int i = 0; i < (int) (rect.Width*rect.Height/30F); i++)
            {
                int x = random.Next(rect.Width);
                int y = random.Next(rect.Height);
                int w = random.Next(m/50);
                int h = random.Next(m/50);
                g.FillEllipse(hatchBrush, x, y, w, h);
            }
        }

        private void DrawTransformedString(Font font, Rectangle rect, Graphics g, HatchBrush hatchBrush)
        {
            GraphicsPath path = GetStringPath(font, rect);
            TransformPath(rect, path);
            g.FillPath(hatchBrush, path);
        }

        private void TransformPath(Rectangle rect, GraphicsPath path)
        {
            float v = 7F;
            PointF[] points =
            {
                new PointF(random.Next(rect.Width)/v, random.Next(rect.Height)/v),
                new PointF(rect.Width - random.Next(rect.Width)/v, random.Next(rect.Height)/v),
                new PointF(random.Next(rect.Width)/v, rect.Height - random.Next(rect.Height)/v),
                new PointF(rect.Width - random.Next(rect.Width)/v, rect.Height - random.Next(rect.Height)/v)
            };
            Matrix matrix = new Matrix();
            matrix.Translate(0F, 0F);
            path.Warp(points, rect, matrix, WarpMode.Perspective, 0F);
        }

        private GraphicsPath GetStringPath(Font font, Rectangle rect)
        {
            StringFormat format = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            GraphicsPath path = new GraphicsPath();
            path.AddString(Text, font.FontFamily, (int) font.Style, font.Size, rect, format);
            return path;
        }

        private Font GetFontForImageSize(Rectangle rect, Graphics g)
        {
            SizeF size;
            float fontSize = rect.Height + 1;
            Font font;
            do
            {
                fontSize--;
                font = new Font(familyName, fontSize, FontStyle.Bold);
                size = g.MeasureString(Text, font);
            } while (size.Width > rect.Width);

            return font;
        }

        private void DrawBackground(Rectangle rect, Graphics g)
        {
            HatchBrush hatchBrush = new HatchBrush(HatchStyle.Divot, Color.DeepSkyBlue, Color.White);
            g.FillRectangle(hatchBrush, rect);
        }

        #endregion
    }
}