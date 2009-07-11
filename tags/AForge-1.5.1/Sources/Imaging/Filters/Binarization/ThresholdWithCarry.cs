// AForge Image Processing Library
// AForge.NET framework
//
// Copyright � Andrew Kirillov, 2005-2007
// andrew.kirillov@gmail.com
//

namespace AForge.Imaging.Filters
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;

    /// <summary>
    /// Threshold binarization with error carry.
    /// </summary>
    /// 
    /// <remarks></remarks>
    /// 
    public class ThresholdWithCarry : FilterGrayToGrayPartial
    {
        private byte threshold = 128;

        /// <summary>
        /// Threshold value.
        /// </summary>
        /// 
        /// <remarks>Default value is 128.</remarks>
        /// 
        public byte ThresholdValue
        {
            get { return threshold; }
            set { threshold = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThresholdWithCarry"/> class.
        /// </summary>
        /// 
        public ThresholdWithCarry( ) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThresholdWithCarry"/> class.
        /// </summary>
        /// 
        /// <param name="threshold">Threshold value.</param>
        /// 
        public ThresholdWithCarry( byte threshold )
        {
            this.threshold = threshold;
        }

        /// <summary>
        /// Process the filter on the specified image.
        /// </summary>
        /// 
        /// <param name="imageData">Image data.</param>
        /// <param name="rect">Image rectangle for processing by the filter.</param>
        /// 
        protected override unsafe void ProcessFilter( BitmapData imageData, Rectangle rect )
        {
            int startX  = rect.Left;
            int startY  = rect.Top;
            int stopX   = startX + rect.Width;
            int stopY   = startY + rect.Height;
            int offset  = imageData.Stride - rect.Width;

            // value which is caried from pixel to pixel
            short carry = 0;

            // do the job
            byte* ptr = (byte*) imageData.Scan0.ToPointer( );

            // allign pointer to the first pixel to process
            ptr += ( startY * imageData.Stride + startX );

            // for each line	
            for ( int y = startY; y < stopY; y++ )
            {
                carry = 0;

                // for each pixel
                for ( int x = startX; x < stopX; x++, ptr++ )
                {
                    carry += *ptr;

                    if ( carry >= threshold )
                    {
                        *ptr = (byte) 255;
                        carry -= 255;
                    }
                    else
                    {
                        *ptr = (byte) 0;
                    }
                }
                ptr += offset;
            }
        }
    }
}