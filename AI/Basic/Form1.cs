using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AIVectors
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public string ItemName { get; set; }
        public int HaveScales { get; set; }
        public int NumberOfGills { get; set; }
        public int CanWalk { get; set; }
        public int IsFish {get; set;}
        private void Form1_Load(object sender, EventArgs e)
        {
            List<Form1> TeachingResources = new List<Form1>();
            //Categories:
            //Scale
            //Fins
            //Can walk
            TeachingResources.Add(new Form1()
            {
                ItemName = "Salmon",
                HaveScales = 1,
                NumberOfGills = 2,
                /*CanWalk = 0,*/
                IsFish = 1
            });
            TeachingResources.Add(new Form1()
            {
                ItemName = "Buffalo",
                HaveScales = 0,
                NumberOfGills = 0,
                /*CanWalk = 1,*/
                IsFish = 0
            });
            TeachingResources.Add(new Form1()
            {
                ItemName = "Shark",
                HaveScales = 1,
                NumberOfGills = 6,
                /*CanWalk = 0,*/
                IsFish = 1
            });
            TeachingResources.Add(new Form1()
            {
                ItemName = "Sea bass",
                HaveScales = 1,
                NumberOfGills = 2,
                /*CanWalk = 0,*/
                IsFish = 1
            });

            textBox4.Text = "3";
            textBox5.Text = "1";

            int totalXCorrect = 0;
            int totalYCorrect = 0;
            int totalZCorrect = 0;
            int countCorrect = 0;
            foreach(var Resource in TeachingResources.Where(x=>x.IsFish == 1))
            {
                totalXCorrect = totalXCorrect + Resource.HaveScales;
                totalYCorrect = totalYCorrect + Resource.NumberOfGills;
                totalZCorrect = totalZCorrect + Resource.IsFish;
                countCorrect++;
            }
            if(countCorrect != 0)
            {
                double averageX = totalXCorrect / countCorrect;
                double averageY = totalYCorrect / countCorrect;
                double averageZ = totalZCorrect / countCorrect;

                textBox2.Text = averageX + "," + averageY + "," + averageZ;
            }

            int totalXIncorrect = 0;
            int totalYIncorrect = 0;
            int totalZIncorrect = 0;
            int countIncorrect = 0;
            foreach (var Resource in TeachingResources.Where(x=>x.IsFish == 0))
            {
                totalXIncorrect = totalXIncorrect + Resource.HaveScales;
                totalYIncorrect = totalYIncorrect + Resource.NumberOfGills;
                totalZIncorrect = totalZIncorrect + Resource.IsFish;
                countIncorrect++;
            }
            if(countIncorrect != 0)
            {
                double averageX = totalXIncorrect / countIncorrect;
                double averageY = totalYIncorrect / countIncorrect;
                double averageZ = totalZIncorrect / countIncorrect;

                textBox3.Text = averageX + "," + averageY + "," + averageZ;
            }

           /* int numPoints = TeachingResources.Count;
            double meanX = TeachingResources.Where(x => x.IsFish == 1).Average(point => point.HaveScales);
            double meanY = TeachingResources.Where(x => x.IsFish == 1).Average(point => point.NumberOfGills);
            double meanZ = TeachingResources.Where(x => x.IsFish == 1).Average(point => point.IsFish);

            double sumXSquared = TeachingResources.Sum(point => point.HaveScales * point.HaveScales);
            double sumXY = TeachingResources.Sum(point => point.HaveScales * point.NumberOfGills);

            a = (sumXY / numPoints - meanX * meanY) / (sumXSquared / numPoints - meanX * meanX);
            b = (a * meanX - meanY);

            double a1 = a;
            double b1 = b;

            return points.Select(point => new XYPoint() { X = point.X, Y = a1 * point.X - b1 }).ToList();*/
        }

        public double CalculateEuclideanDistance(int x1, int y1, int z1, int x2, int y2, int z2)
        {
            double x = (x2 - x1) * (x2 - x1);
            double y = (y2 - y1) * (y2 - y1);
            double z = (z2 - z1) * (z2 - z1);
            double total = x + y + z;

            double distance = Math.Sqrt(total);
            return distance;
        }

        public int CalculateManhattanDistance(int x1, int y1, int z1, int x2, int y2, int z2)
        {
            int x = x2 - x1;
            int y = y2 - y1;
            int z = z2 - z1;

            int distance = x + y + z;
            return distance;
        }

        public double? MinkowskiMetric(List<Form1> Resources, int x2, int y2, int z2)
        {
            foreach(var Resource in Resources)
            {
                double absolute = Math.Abs(
                    CalculateEuclideanDistance(
                        Resource.HaveScales,
                        Resource.NumberOfGills,
                        Resource.IsFish,
                        x2,
                        y2,
                        z2
                    )
                );
                double absoluteSquare = absolute * absolute;

                if (absoluteSquare >= 0)
                {
                    return Math.Sqrt(absoluteSquare);
                }
            }
            return 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] ItemSearch = textBox1.Text.Split(',');
            List<Form1> ItemSearchList = new List<Form1>();

            ItemSearchList.Add(new Form1()
            {
                ItemName = "'" + ItemSearch[0] + "'",
                HaveScales = int.Parse(ItemSearch[1]),
                NumberOfGills = int.Parse(ItemSearch[2]),
                /*CanWalk = 0,*/
                IsFish = int.Parse(ItemSearch[3])
            });

            string[] ExistingCorrectModel = textBox2.Text.Split(',');
            string[] ExistingIncorrectModel = textBox3.Text.Split(',');
            if(ItemSearch != null && ExistingCorrectModel != null && ExistingIncorrectModel != null)
            {
                double? DistanceOfCorrectValue = MinkowskiMetric(ItemSearchList, int.Parse(ExistingCorrectModel[0]), int.Parse(ExistingCorrectModel[1]), int.Parse(ExistingCorrectModel[2]));
                double? DistanceOfIncorrectValue = MinkowskiMetric(ItemSearchList, int.Parse(ExistingIncorrectModel[0]), int.Parse(ExistingIncorrectModel[1]), int.Parse(ExistingIncorrectModel[2]));

                if(DistanceOfCorrectValue < DistanceOfIncorrectValue)
                {
                    label1.Text = ItemSearch[0] + " is a fish";
                    
                    if(int.Parse(ExistingCorrectModel[2]) == 1)
                    {
                        int averagetotalXCorrect = int.Parse(ExistingCorrectModel[0]);
                        int averagetotalYCorrect = int.Parse(ExistingCorrectModel[1]);
                        int averagetotalZCorrect = int.Parse(ExistingCorrectModel[2]);


                        double averageX = (((averagetotalXCorrect * int.Parse(textBox4.Text)) + int.Parse(ItemSearch[1])) / (int.Parse(textBox4.Text) + 1));
                        double averageY = (((averagetotalYCorrect * int.Parse(textBox4.Text)) + int.Parse(ItemSearch[2])) / (int.Parse(textBox4.Text) + 1));
                        double averageZ = (((averagetotalZCorrect * int.Parse(textBox4.Text)) + int.Parse(ItemSearch[3])) / (int.Parse(textBox4.Text) + 1));


                        textBox4.Text = (int.Parse(textBox4.Text) + 1).ToString();
                        textBox2.Text = averageX + "," + averageY + "," + averageZ;
                    }
                    else
                    {

                    }
                }
                else if(DistanceOfCorrectValue > DistanceOfIncorrectValue)
                {
                    label1.Text = ItemSearch[0] + " is not a fish";

                    if(int.Parse(ExistingIncorrectModel[2]) == 0)
                    {
                        int averagetotalXIncorrect = int.Parse(ExistingIncorrectModel[0]);
                        int averagetotalYIncorrect = int.Parse(ExistingIncorrectModel[1]);
                        int averagetotalZIncorrect = int.Parse(ExistingIncorrectModel[2]);


                        double averageX = (((averagetotalXIncorrect * int.Parse(textBox5.Text)) + int.Parse(ItemSearch[1])) / (int.Parse(textBox5.Text) + 1));
                        double averageY = (((averagetotalYIncorrect * int.Parse(textBox5.Text)) + int.Parse(ItemSearch[2])) / (int.Parse(textBox5.Text) + 1));
                        double averageZ = (((averagetotalZIncorrect * int.Parse(textBox5.Text)) + int.Parse(ItemSearch[3])) / (int.Parse(textBox5.Text) + 1));

                        textBox5.Text = (int.Parse(textBox5.Text) + 1).ToString();
                        textBox3.Text = averageX + "," + averageY + "," + averageZ;
                    }
                }
                else
                {
                    label1.Text = "NaN";
                }
            }
        }

        /*static void Main(string[] args)
        {
            List<XYPoint> points = new List<XYPoint>()
                                   {
                                       new XYPoint() {X = 1, Y = 12},
                                       new XYPoint() {X = 2, Y = 16},
                                       new XYPoint() {X = 3, Y = 34},
                                       new XYPoint() {X = 4, Y = 45},
                                       new XYPoint() {X = 5, Y = 47}
                                   };

            double a, b;

            List<XYPoint> bestFit = GenerateLinearBestFit(points, out a, out b);

            Console.WriteLine("y = {0:#.####}x {1:+#.####;-#.####}", a, -b);

            for (int index = 0; index < points.Count; index++)
            {
                Console.WriteLine("X = {0}, Y = {1}, Fit = {2:#.###}", points[index].X, points[index].Y, bestFit[index].Y);
            }
        }*/

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
