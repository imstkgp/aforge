// AForge Image Processing Library
// AForge.NET framework
//
// Copyright � Andrew Kirillov, 2005-2008
// andrew.kirillov@aforgenet.com
//

namespace AForge.Imaging.Filters
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;

    /// <summary>
    /// Gaussian blur filter.
    /// </summary>
    /// 
    /// <remarks><para>The filter performs <see cref="Convolution">convolution filter</see> using
    /// the kernel, which is calculate with the help of <see cref="AForge.Math.Gaussian.Kernel2D"/>
    /// method and then converted to integer kernel by dividing all elements by the element with the
    /// smallest value. Using the kernel the convolution filter is known as Gaussian blur.</para>
    /// 
    /// <para>Using <see cref="Sigma"/> property it is possible to configure
    /// <see cref="AForge.Math.Gaussian.Sigma">sigma value of Gaussian function</see>.</para>
    /// 
    /// <para>For the list of supported pixel formats, see the documentation to <see cref="Convolution"/>
    /// filter.</para>
    /// 
    /// <para>Sample usage:</para>
    /// <code>
    /// // create filter with kernel size equal to 11
    /// // and Gaussia sigma value equal to 4.0
    /// GaussianBlur filter = new GaussianBlur( 4, 11 );
    /// // apply the filter
    /// filter.ApplyInPlace( image );
    /// </code>
    ///
    /// <para><b>Initial image:</b></para>
    /// <img src="img/imaging/sample1.jpg" width="480" height="361" />
    /// <para><b>Result image:</b></para>
    /// <img src="img/imaging/gaussian_blur.jpg" width="480" height="361" />
    /// </remarks>
    /// 
    /// <seealso cref="Convolution"/>
    /// 
    public sealed class GaussianBlur : Convolution
    {
        private double sigma = 1.4;
        private int    size = 5;

        /// <summary>
        /// Gaussian sigma value, [0.5, 5.0].
        /// </summary>
        /// 
        /// <remarks><para>Sigma value for Gaussian function used to calculate
        /// the kernel.</para>
        /// 
        /// <para>Default value is set to <b>1.4</b>.</para>
        /// </remarks>
        /// 
        public double Sigma
        {
            get { return sigma; }
            set
            {
                // get new sigma value
                sigma = Math.Max( 0.5, Math.Min( 5.0, value ) );
                // create filter
                CreateFilter( );
            }
        }

        /// <summary>
        /// Kernel size, [3, 5].
        /// </summary>
        /// 
        /// <remarks><para>Size of Gaussian kernel.</para>
        /// 
        /// <para>Default value is set to <b>5</b>.</para>
        /// </remarks>
        /// 
        public int Size
        {
            get { return size; }
            set
            {
                size = Math.Max( 3, Math.Min( 21, value | 1 ) );
                CreateFilter( );
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GaussianBlur"/> class.
        /// </summary>
        /// 
        public GaussianBlur( )
        {
            CreateFilter( );
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GaussianBlur"/> class.
        /// </summary>
        /// 
        /// <param name="sigma">Gaussian sigma value.</param>
        /// 
        public GaussianBlur( double sigma )
        {
            Sigma = sigma;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GaussianBlur"/> class.
        /// </summary>
        /// 
        /// <param name="sigma">Gaussian sigma value.</param>
        /// <param name="size">Kernel size.</param>
        /// 
        public GaussianBlur( double sigma, int size )
        {
            Sigma = sigma;
            Size = size;
        }

        // Private members
        #region Private Members

        // Create Gaussian filter
        private void CreateFilter( )
        {
            // create Gaussian function
            AForge.Math.Gaussian gaus = new AForge.Math.Gaussian( sigma );
            // create kernel
            double[,] kernel = gaus.Kernel2D( size );
            double min = kernel[0, 0];
            // integer kernel
            int[,] intKernel = new int[size, size];
            int divisor = 0;

            for ( int i = 0; i < size; i++ )
            {
                for ( int j = 0; j < size; j++ )
                {
                    double v = kernel[i, j] / min;

                    if ( v > ushort.MaxValue )
                    {
                        v = ushort.MaxValue;
                    }
                    intKernel[i, j] = (int) v;

                    // collect divisor
                    divisor += intKernel[i, j];
                }
            }

            // update filter
            this.Kernel = intKernel;
            this.Divisor = divisor;
        }
        #endregion
    }
}