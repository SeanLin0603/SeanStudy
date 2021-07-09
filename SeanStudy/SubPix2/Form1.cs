using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using static subpixel.sub;

namespace SubPix2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    var inputImage = new Image<Bgr, byte>(ofd.FileName);
                    picSrc.Image = inputImage.Bitmap;

                    Parameter parameter = new Parameter();
                    Result result = new Result();
                    Image<Gray, byte> image = inputImage.Convert<Gray, byte>();
                    Devernay(image, parameter, out result);
                    var ipol = drawOld(inputImage);
                    var pami = drawNew(ipol, result);
                }
            }
        }

        private Image<Bgr, byte> drawOld(Image<Bgr, byte> image)
        {
            int scale = 11;
            Image<Bgr, byte> drawImage = image.Clone();
            Size newFrame = new Size(drawImage.Width * scale, drawImage.Height * scale);
            drawImage = drawImage.Resize(scale, Inter.Nearest);

            subpixel.sub.Parameter parameter = new subpixel.sub.Parameter();
            subpixel.sub.Result result = new subpixel.sub.Result();
            SubPixel(image.Convert<Gray, byte>(), parameter, out result);

            // find the biggest contour
            int maxIdx = -1;
            int maxSize = -1;
            for (int i = 0; i < result.SubContours.Capacity; i++)
            {
                int size = result.SubContours[i].points.Length;
                if (size > maxSize)
                {
                    maxIdx = i;
                    maxSize = size;
                }
            }

            PointF[] subPixContour = result.SubContours[maxIdx].points;
            for (int i = 0; i < subPixContour.Length; i++)
            {
                PointF pt = subPixContour[i];

                // draw in center
                int r = (int)(pt.Y * scale + scale / 2);
                int c = (int)(pt.X * scale + scale / 2);

                // red means subpix points
                drawImage.Data[r, c, 0] = 255;
                drawImage.Data[r, c, 1] = 0;
                drawImage.Data[r, c, 2] = 0;
            }

            CvInvoke.Imwrite("oldMethod.png", drawImage);
            return drawImage;
        }

        private Image<Bgr, byte> drawNew(Image<Bgr, byte> image, Result result)
        {
            int scale = 11;
            Image<Bgr, byte> drawImage = image.Clone();
            Size newFrame = new Size(drawImage.Width * scale, drawImage.Height * scale);
            //drawImage = drawImage.Resize(scale, Inter.Nearest);

            // get max contour
            int maxIdx = -1, maxSize = -1;
            for (int i = 0; i < result.Points.Count; i++)
            {
                int size = result.Points[i].Count;
                if (size > maxSize)
                {
                    maxIdx = i;
                    maxSize = size;
                }
            }

            for (int i = 0; i < maxSize; i++)
            {
                int r = (int)(result.Points[maxIdx][i].Y * scale + scale / 2);
                int c = (int)(result.Points[maxIdx][i].X * scale + scale / 2);

                // BGR
                drawImage.Data[r, c, 0] = 0;
                drawImage.Data[r, c, 1] = 0;
                drawImage.Data[r, c, 2] = 255;
            }

            CvInvoke.Imwrite("newMethod.png", drawImage);
            return drawImage;
        }



        // ############################algorithms#########################################

        public class Parameter
        {
            public double Sigma;

            public double Threshold_H;

            public double Threshold_L;

            public Parameter()
            {
                Sigma = 0;
                Threshold_H = 4.2;
                Threshold_L = 0.81;
            }

            public Parameter(double sigma, double high, double low)
            {
                Sigma = sigma;
                Threshold_H = high;
                Threshold_L = low;
            }
        }

        public class Result
        {
            public List<List<PointF>> Points;

            public Result()
            {
                Points = new List<List<PointF>>();
            }
        }

        public class GradientResult
        {
            public Size ImageSize;

            public double[] Gx;
            public double[] Gy;
            public double[] ModG;

            //public Image<Gray, double> Gx;
            //public Image<Gray, double> Gy;
            //public Image<Gray, double> ModG;

            public GradientResult() { }
        }

        public class EdgeResult
        {
            public Size ImageSize;
            public double[] Ex;
            public double[] Ey;

            public EdgeResult() { }
        }

        public class ChainResult
        {
            public int[] Next;
            public int[] Prev;

            public ChainResult() { }
        }

        private bool greaterCustom(double a, double b)
        {
            const double epsilon = 2.2204460492503131e-16;
            if (a <= b) return false;
            if ((a - b) < 1000 * epsilon) return false;
            return true;
        }

        private double dist(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
        }

        private double chain(int from, int to, GradientResult gradientResult, EdgeResult edgeResult)
        {
            double dx, dy;

            int width = gradientResult.ImageSize.Width;
            int height = gradientResult.ImageSize.Height;

            if (edgeResult == null || gradientResult.Gx == null || gradientResult.Gy == null)
                throw new Exception("chain: invalid input");

            if (from < 0 || to < 0 || from >= width * height || to >= width * height)
                throw new Exception("chain: one of the points is out the image");

            //check that the points are different and valid edge points,otherwise return invalid chaining
            if (from == to)
                return 0.0; // same pixel, not a valid chaining
            if (edgeResult.Ex[from] < 0.0 || edgeResult.Ey[from] < 0.0 || edgeResult.Ex[to] < 0.0 || edgeResult.Ey[to] < 0.0)
                return 0.0; // one of them is not an edge point, not a valid chaining

            /* in a good chaining, the gradient should be roughly orthogonal
            to the line joining the two points to be chained:
            when Gy * dx - Gx * dy > 0, it corresponds to a forward chaining,
            when Gy * dx - Gx * dy < 0, it corresponds to a backward chaining.

            first check that the gradient at both points to be chained agree
            in one direction, otherwise return invalid chaining. */
            dx = edgeResult.Ex[to] - edgeResult.Ex[from];
            dy = edgeResult.Ey[to] - edgeResult.Ey[from];
            if ((gradientResult.Gy[from] * dx - gradientResult.Gx[from] * dy) * (gradientResult.Gy[to] * dx - gradientResult.Gx[to] * dy) <= 0.0)
                return 0.0; /* incompatible gradient angles, not a valid chaining */

            /* return the chaining score: positive for forward chaining,negative for backwards. 
            the score is the inverse of the distance to the chaining point, to give preference to closer points */
            if ((gradientResult.Gy[from] * dx - gradientResult.Gx[from] * dy) >= 0.0)
                return 1.0 / dist(edgeResult.Ex[from], edgeResult.Ey[from], edgeResult.Ex[to], edgeResult.Ey[to]); /* forward chaining  */
            else
                return -1.0 / dist(edgeResult.Ex[from], edgeResult.Ey[from], edgeResult.Ex[to], edgeResult.Ey[to]); /* backward chaining */
        }

        private bool getGradient(Image<Gray, byte> image, out GradientResult gradientResult)
        {
            if (image == null)
            {
                gradientResult = null;
                return false;
            }

            int width = image.Width;
            int height = image.Height;

            gradientResult = new GradientResult();
            gradientResult.ImageSize = new Size(width, height);
            gradientResult.Gx = new double[width * height];
            gradientResult.Gy = new double[width * height];
            gradientResult.ModG = new double[width * height];

            for (int x = 1; x < width - 1; x++)
            {
                for (int y = 1; y < height - 1; y++)
                {
                    double dx = image.Data[y, (x + 1), 0] - image.Data[y, (x - 1), 0];
                    double dy = image.Data[(y + 1), x, 0] - image.Data[(y - 1), x, 0];
                    gradientResult.Gx[x + y * width] = dx;
                    gradientResult.Gy[x + y * width] = dy;
                    gradientResult.ModG[x + y * width] = Math.Sqrt(dx * dx + dy * dy);
                }
            }

            return true;
        }

        private bool getEdgePoints(GradientResult gradientResult, out EdgeResult edgeResult)
        {
            if (gradientResult == null || gradientResult.Gx == null || gradientResult.Gy == null || gradientResult.ModG == null)
            {
                edgeResult = null;
                return false;
            }

            int width = gradientResult.ImageSize.Width;
            int height = gradientResult.ImageSize.Height;
            edgeResult = new EdgeResult();
            edgeResult.ImageSize = gradientResult.ImageSize;
            edgeResult.Ex = new double[width * height];
            edgeResult.Ey = new double[width * height];

            // initialize
            for (int i = 0; i < width * height; i++)
            {
                edgeResult.Ex[i] = -1;
                edgeResult.Ey[i] = -1;
            }

            for (int x = 2; x < width - 2; x++)
            {
                for (int y = 2; y < height - 2; y++)
                {
                    int dx = 0;
                    int dy = 0;

                    double mod = gradientResult.ModG[x + y * width];
                    double left = gradientResult.ModG[(x - 1) + y * width];
                    double right = gradientResult.ModG[(x + 1) + y * width];
                    double up = gradientResult.ModG[x + (y - 1) * width];
                    double down = gradientResult.ModG[x + (y + 1) * width];
                    double gx = Math.Abs(gradientResult.Gx[x + y * width]);
                    double gy = Math.Abs(gradientResult.Gy[x + y * width]);
                    /* when local horizontal maxima of the gradient modulus and the gradient direction 
                    is more horizontal (|Gx| >= |Gy|),=> a "horizontal" (H) edge found else, 
                    if local vertical maxima of the gradient modulus and the gradient direction is more
                    vertical (|Gx| <= |Gy|),=> a "vertical" (V) edge found */

                    /* it can happen that two neighbor pixels have equal value and are both	maxima, for example 
                    when the edge is exactly between both pixels. in such cases, as an arbitrary convention, 
                    the edge is marked on the left one when an horizontal max or below when a vertical max.
                    for	this the conditions are L < mod >= R and D < mod >= U,respectively. the comparisons are
                    done using the function greater() instead of the operators > or >= so numbers differing only
                    due to rounding errors are considered equal */
                    if (greaterCustom(mod, left) && !greaterCustom(right, mod) && gx >= gy)
                    {
                        dx = 1;
                    }
                    else if (greaterCustom(mod, down) && !greaterCustom(up, mod) && gx <= gy)
                    {
                        dy = 1;
                    }

                    /* Devernay sub-pixel correction
                    the edge point position is selected as the one of the maximum of a quadratic interpolation of the magnitude of
                    the gradient along a unidimensional direction. the pixel must be a local maximum. so we	have the values:

                    the x position of the maximum of the parabola passing through(-1,a), (0,b), and (1,c) is
                    offset = (a - c) / 2(a - 2b + c),and because b >= a and b >= c, -0.5 <= offset <= 0.5	*/
                    if (dx > 0 || dy > 0)
                    {
                        double a = gradientResult.ModG[(x - dx) + (y - dy) * width];
                        double b = gradientResult.ModG[x + y * width];
                        double c = gradientResult.ModG[(x + dx) + (y + dy) * width];
                        double offset = 0.5 * (a - c) / (a - b - b + c);

                        edgeResult.Ex[x + y * width] = x + offset * dx;
                        edgeResult.Ey[x + y * width] = y + offset * dy;
                    }
                }
            }

            return true;
        }

        private bool chainEdgePoints(GradientResult gradientResult, EdgeResult edgeResult, out ChainResult chainResult)
        {
            if (edgeResult.Ex == null || edgeResult.Ey == null || gradientResult.Gx == null || gradientResult.Gy == null)
            {
                chainResult = null;
                return false;
            }

            int x, y, i, j, alt;

            int width = gradientResult.ImageSize.Width;
            int height = gradientResult.ImageSize.Height;
            chainResult = new ChainResult();
            chainResult.Next = new int[width * height];
            chainResult.Prev = new int[width * height];

            /* initialize next and prev as non linked */
            for (i = 0; i < width * height; i++)
            {
                chainResult.Next[i] = -1;
                chainResult.Prev[i] = -1;
            }

            /* try each point to make local chains */
            for (x = 2; x < (width - 2); x++)   /* 2 pixel margin to include the tested neighbors */
            {
                for (y = 2; y < (height - 2); y++)
                {
                    if (edgeResult.Ex[x + y * width] >= 0.0 && edgeResult.Ey[x + y * width] >= 0.0) /* must be an edge point */
                    {
                        int from = x + y * width;  /* edge point to be chained			*/
                        double fwd_s = 0.0;  /* score of best forward chaining		*/
                        double bck_s = 0.0;  /* score of best backward chaining		*/
                        int fwd = -1;        /* edge point of best forward chaining */
                        int bck = -1;        /* edge point of best backward chaining*/

                        /* try all neighbors two pixels apart or less.
                        looking for candidates for chaining two pixels apart, in most such cases, 
                        is enough to obtain good chains of edge points that	accurately describes the edge.	*/
                        for (i = -2; i <= 2; i++)
                            for (j = -2; j <= 2; j++)
                            {
                                int to = x + i + (y + j) * width; /* candidate edge point to be chained */
                                double s = chain(from, to, gradientResult, edgeResult);  /* score from-to */

                                if (s > fwd_s)  /* a better forward chaining found    */
                                {
                                    fwd_s = s;  /* set the new best forward chaining  */
                                    fwd = to;
                                }
                                if (s < bck_s)  /* a better backward chaining found	  */
                                {
                                    bck_s = s;  /* set the new best backward chaining */
                                    bck = to;
                                }
                            }

                        /* before making the new chain, check whether the target was
                        already chained and in that case, whether the alternative
                        chaining is better than the proposed one.

                        x alt                        x alt
                        \                          /
                        \                        /
                        from x---------x fwd              bck x---------x from

                        we know that the best forward chain starting at from is from-fwd.
                        but it is possible that there is an alternative chaining arriving
                        at fwd that is better, such that alt-fwd is to be preferred to
                        from-fwd. an analogous situation is possible in backward chaining,
                        where an alternative link bck-alt may be better than bck-from.

                        before making the new link, check if fwd/bck are already chained,
                        and in such case compare the scores of the proposed chaining to
                        the existing one, and keep only the best of the two.

                        there is an undesirable aspect of this procedure: the result may
                        depend on the order of exploration. consider the following
                        configuration:

                        a x-------x b
                        /
                        /
                        c x---x d    with score(a-b) < score(c-b) < score(c-d)
                        or equivalently ||a-b|| > ||b-c|| > ||c-d||

                        let us consider two possible orders of exploration.

                        order: a,b,c
                        we will first chain a-b when exploring a. when analyzing the
                        backward links of b, we will prefer c-b, and a-b will be unlinked.
                        finally, when exploring c, c-d will be preferred and c-b will be
                        unlinked. the result is just the chaining c-d.

                        order: c,b,a
                        we will first chain c-d when exploring c. then, when exploring
                        the backward connections of b, c-b will be the preferred link;
                        but because c-d exists already and has a better score, c-b
                        cannot be linked. finally, when exploring a, the link a-b will
                        be created because there is no better backward linking of b.
                        the result is two chainings: c-d and a-b.

                        we did not found yet a simple algorithm to solve this problem. by
                        simple, we mean an algorithm without two passes or the need to
                        re-evaluate the chaining of points where one link is cut.

                        for most edge points, there is only one possible chaining and this
                        problem does not arise. but it does happen and a better solution
                        is desirable.
                        */
                        if (fwd >= 0 && chainResult.Next[from] != fwd &&
                            ((alt = chainResult.Prev[fwd]) < 0 || chain(alt, fwd, gradientResult, edgeResult) < fwd_s))
                        {
                            if (chainResult.Next[from] >= 0)     /* remove previous from-x link if one */
                                chainResult.Prev[chainResult.Next[from]] = -1;  /* only prev requires explicit reset  */
                            chainResult.Next[from] = fwd;         /* set next of from-fwd link          */
                            if (alt >= 0)            /* remove alt-fwd link if one         */
                                chainResult.Next[alt] = -1;         /* only next requires explicit reset  */
                            chainResult.Prev[fwd] = from;         /* set prev of from-fwd link          */
                        }
                        if (bck >= 0 && chainResult.Prev[from] != bck &&
                            ((alt = chainResult.Next[bck]) < 0 || chain(alt, bck, gradientResult, edgeResult) > bck_s))
                        {
                            if (alt >= 0)            /* remove bck-alt link if one         */
                                chainResult.Prev[alt] = -1;         /* only prev requires explicit reset  */
                            chainResult.Next[bck] = from;         /* set next of bck-from link          */
                            if (chainResult.Prev[from] >= 0)     /* remove previous x-from link if one */
                                chainResult.Next[chainResult.Prev[from]] = -1;  /* only next requires explicit reset  */
                            chainResult.Prev[from] = bck;         /* set prev of bck-from link          */
                        }
                    }
                }
            }

            return true;
        }

        private ChainResult thresholdsWithHysteresis(Parameter parameter, GradientResult gradientResult, ChainResult chainResult)
        {
            /* check input */
            if (chainResult == null || chainResult.Next == null || chainResult.Prev == null || gradientResult.ModG == null)
            {
                return null;
            }

            int width = gradientResult.ImageSize.Width;
            int height = gradientResult.ImageSize.Height;
            int i, j, k;

            bool[] valid = new bool[width * height];
            for (int a = 0; a < width * height; a++)
            {
                valid[a] = false;
            }

            /* validate all edge points over th_h or connected to them and over th_l */
            for (i = 0; i < width * height; i++)   /* prev[i]>=0 or next[i]>=0 implies an edge point */
            {
                if ((chainResult.Prev[i] >= 0 || chainResult.Next[i] >= 0) && !valid[i] && gradientResult.ModG[i] >= parameter.Threshold_H)
                {
                    valid[i] = true; /* mark as valid the new point */

                    /* follow the chain of edge points forwards */
                    for (j = i; j >= 0 && (k = chainResult.Next[j]) >= 0 && !valid[k]; j = chainResult.Next[j])
                        if (gradientResult.ModG[k] < parameter.Threshold_H)
                        {
                            chainResult.Next[j] = -1;  /* cut the chain when the point is below th_l */
                            chainResult.Prev[k] = -1;  /* j must be assigned to next[j] and not k,
						   so the loop is chained in this case */
                        }
                        else
                            valid[k] = true; /* otherwise mark the new point as valid */

                    /* follow the chain of edge points backwards */
                    for (j = i; j >= 0 && (k = chainResult.Prev[j]) >= 0 && !valid[k]; j = chainResult.Prev[j])
                        if (gradientResult.ModG[k] < parameter.Threshold_L)
                        {
                            chainResult.Prev[j] = -1;  /* cut the chain when the point is below th_l */
                            chainResult.Next[k] = -1;  /* j must be assigned to prev[j] and not k,
						   so the loop is chained in this case */
                        }
                        else
                            valid[k] = true; /* otherwise mark the new point as valid */
                }
            }
            /* remove any remaining non-valid chained point */
            for (i = 0; i < width * height; i++)   /* prev[i]>=0 or next[i]>=0 implies edge point */
            {
                if ((chainResult.Prev[i] >= 0 || chainResult.Next[i] >= 0) && !valid[i])
                {
                    chainResult.Prev[i] = chainResult.Next[i] = -1;
                }
            }

            return chainResult;
        }

        private Result exportPoint(EdgeResult edgeResult, ChainResult chainResult)
        {
            Result result = new Result();
            int width = edgeResult.ImageSize.Width;
            int height = edgeResult.ImageSize.Height;

            int k, n;

            for (int i = 0; i < width * height; i++)
            {
                if (chainResult.Next[i] >= 0 || chainResult.Prev[i] >= 0)
                {
                    List<PointF> contour = new List<PointF>();

                    for (k = i; (n = chainResult.Prev[k]) >= 0 && n != i; k = n);

                    do
                    {
                        PointF point = new PointF((float)edgeResult.Ex[k], (float)edgeResult.Ey[k]);

                        n = chainResult.Next[k];
                        chainResult.Next[k] = -1;
                        chainResult.Prev[k] = -1;
                        k = n;
                        contour.Add(point);
                    } while (k >= 0);

                    result.Points.Add(contour);
                }
            }

            return result;
        }

        public bool Devernay(Image<Gray, byte> image, Parameter parameter, out Result result)
        {
            result = new Result();
            GradientResult gradientResult = new GradientResult();
            EdgeResult edgeResult = new EdgeResult();
            ChainResult chainResult = new ChainResult();

            // compute gradient
            if (parameter.Sigma == 0)
            {
                getGradient(image, out gradientResult);
            }
            else
            {
                Size kernel = new Size(3, 3);
                CvInvoke.GaussianBlur(image, image, kernel, parameter.Sigma);
                getGradient(image, out gradientResult);
            }

            getEdgePoints(gradientResult, out edgeResult);

            chainEdgePoints(gradientResult, edgeResult, out chainResult);

            chainResult = thresholdsWithHysteresis(parameter, gradientResult, chainResult);

            result = exportPoint(edgeResult, chainResult);

            return true;
        }
    }
}
