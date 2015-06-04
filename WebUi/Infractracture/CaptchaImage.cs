using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace WebUi.Infractracture
{
    public class CaptchaImage : IDisposable
    {
        public const string captchaValueKey = "CaptchaImageText";

        public string Text { get; private set; }

        public Bitmap Image { get; private set; }

        public int Width { get; private set; }

        public int Height { get; private set; }

        private string familyName;

        private readonly Random random = new Random();

        public CaptchaImage(string text, int width, int height)
        {
            Text = text;
            SetDimensions(width, height);
            GenerateImage();
        }

        public CaptchaImage(string text, int width, int height, string familyName)
        {
            Text = text;
            SetDimensions(width, height);
            SetFamilyName(familyName);
            GenerateImage();
        }

        // ====================================================================
        // This member overrides Object.Finalize.
        // ====================================================================
        ~CaptchaImage()
        {
            Dispose(false);
        }

        // ====================================================================
        // Releases all resources used by this object.
        // ====================================================================
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        // ====================================================================
        // Custom Dispose method to clean up unmanaged resources.
        // ====================================================================
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                // Dispose of the bitmap.
                Image.Dispose();
        }

        // ====================================================================
        // Sets the image aWidth and aHeight.
        // ====================================================================
        private void SetDimensions(int aWidth, int aHeight)
        {
            // Check the aWidth and aHeight.
            if (aWidth <= 0)
                throw new ArgumentOutOfRangeException("aWidth", aWidth, "Argument out of range, must be greater than zero.");
            if (aHeight <= 0)
                throw new ArgumentOutOfRangeException("aHeight", aHeight, "Argument out of range, must be greater than zero.");
            Width = aWidth;
            Height = aHeight;
        }

        // ====================================================================
        // Sets the font used for the image text.
        // ====================================================================
        private void SetFamilyName(string aFamilyName)
        {
            // If the named font is not installed, default to a system font.
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

        // ====================================================================
        // Creates the bitmap image.
        // ====================================================================
        private void GenerateImage()
        {
            // Create a new 32-bit bitmap image.
            Bitmap bitmap = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);

            // Create a graphics object for drawing.
            Graphics g = Graphics.FromImage(bitmap);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rect = new Rectangle(0, 0, Width, Height);

            // Fill in the background.
            HatchBrush hatchBrush = new HatchBrush(HatchStyle.Divot, Color.DeepSkyBlue, Color.White);
            g.FillRectangle(hatchBrush, rect);

            // Set up the text font.
            SizeF size;
            float fontSize = rect.Height + 1;
            Font font;
            // Adjust the font size until the text fits within the image.
            do
            {
                fontSize--;
                font = new Font(familyName, fontSize, FontStyle.Bold);
                size = g.MeasureString(Text, font);
            } while (size.Width > rect.Width);

            // Set up the text format.
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;

            // Create a path using the text and warp it randomly.
            GraphicsPath path = new GraphicsPath();
            path.AddString(Text, font.FontFamily, (int)font.Style, font.Size, rect, format);
            float v = 7F;
            PointF[] points =
            {
                new PointF(random.Next(rect.Width) / v, random.Next(rect.Height) / v),
                new PointF(rect.Width - random.Next(rect.Width) / v, random.Next(rect.Height) / v),
                new PointF(random.Next(rect.Width) / v, rect.Height - random.Next(rect.Height) / v),
                new PointF(rect.Width - random.Next(rect.Width) / v, rect.Height - random.Next(rect.Height) / v)
            };
            Matrix matrix = new Matrix();
            matrix.Translate(0F, 0F);
            path.Warp(points, rect, matrix, WarpMode.Perspective, 0F);

            // Draw the text.
            hatchBrush = new HatchBrush(HatchStyle.Divot, Color.Gold, Color.Goldenrod);
            g.FillPath(hatchBrush, path);

            // Add some random noise.
            int m = Math.Max(rect.Width, rect.Height);
            for (int i = 0; i < (int)(rect.Width * rect.Height / 30F); i++)
            {
                int x = random.Next(rect.Width);
                int y = random.Next(rect.Height);
                int w = random.Next(m / 50);
                int h = random.Next(m / 50);
                g.FillEllipse(hatchBrush, x, y, w, h);
            }

            // Clean up.
            font.Dispose();
            hatchBrush.Dispose();
            g.Dispose();

            // Set the image.
            Image = bitmap;
        }
    }
}