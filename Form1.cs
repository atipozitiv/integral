using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MathNet.Numerics;
using org.matheval;
using org.matheval.Functions;

namespace integral {
  public partial class Form1 : Form {
    public Form1() {
      InitializeComponent();
      MaximizeBox = false;
      FormBorderStyle = FormBorderStyle.Fixed3D;
      this.Text = "Вычисление определенного интеграла";
    }

    double Func(double x) {
      Expression func = new Expression(textBox1.Text.ToLower());
      func.Bind("x", x);
      double result = func.Eval<double>();
      return Convert.ToDouble(result);
    }
    void DrawChart(double end, double start, string func) {
      this.chart1.Series[0].Points.Clear();
      double y;
      while (start <= end) {
        y = Func(start);
        this.chart1.Series[0].Points.AddXY(start, y);
        start += 0.1;
      }
    }

    private void очиститьToolStripMenuItem_Click(object sender, EventArgs e) {
      textBox1.Text = "";
      textBox2.Text = "";
      textBox3.Text = "";
      textBox4.Text = "";
      textBox5.Text = "";
      checkBox1.Checked = false;
      checkBox2.Checked = false;
      checkBox3.Checked = false;
      label2.Text = "";
    }

    double Rectangle(string func, double start, double end, double N, double presision) {
      double answer = 0;
      double step = (end - start) / N;
      while (start < end) {
        answer += step * Func(start + step/2);
        start += step;
        if (start + step > end) step = (end - start) / 2;
      }
      return Math.Round(answer, presision.ToString().Length-2);  
    } 
    double Trapeze(string func, double start, double end, double N, double presision) {
      double answer = 0;
      double step = (end - start) / N;
      while (start < end) {
        answer += (Func(start) + Func(start + step)) / 2 * step;
        start += step;
        if (start + step > end) step = (end - start) / 2;
      }
      return Math.Round(answer, presision.ToString().Length-2);
    }
    double Simpson(string func, double start, double end, double N, double presision) {
      double answer = (2 * Trapeze(func, start, end, N, presision) + Rectangle(func, start, end, N, presision)) / 3;
      return Math.Round(answer, presision.ToString().Length - 2);
    }

    private void рассчитатьToolStripMenuItem_Click(object sender, EventArgs e) {
      try {
      
        label2.Text = "";
        string func = textBox1.Text;
        double start = Convert.ToDouble(textBox2.Text);
        double end = Convert.ToDouble(textBox3.Text);
        double presision = Convert.ToDouble(textBox4.Text);
        double N = 0;
        if (textBox5.Text != "")  N = Convert.ToDouble(textBox5.Text);
        if ((start >= end) || ((end - start) < presision) || (presision <= 0) || (N<0)) {
          Mistake();
        } else {
          DrawChart(end, start, func);
          if (N != 0) {
            if (checkBox1.Checked) label2.Text += "\nметод прямоугольников: " + Convert.ToString(Rectangle(func, start, end, N, presision));
            if (checkBox2.Checked) label2.Text += "\nметод трапеций: " + Convert.ToString(Trapeze(func, start, end, N, presision));
            if (checkBox3.Checked) label2.Text += "\nметод Симпсона: " + Convert.ToString(Simpson(func, start, end, N, presision));
          } else {
            double myN = 2;
            double myRec = Rectangle(func, start, end, myN, presision);
            double myTra = Trapeze(func, start, end, myN, presision);
            double mySim = Simpson(func, start, end, myN, presision);
            while ((Math.Abs(myRec - mySim) > presision) || (Math.Abs(myTra - mySim) > presision)) {
              myN += 1;
              myRec = Rectangle(func, start, end, myN, presision);
              myTra = Trapeze(func, start, end, myN, presision);
              mySim = Simpson(func, start, end, myN, presision);
            }
            if (checkBox1.Checked) label2.Text += "\nметод прямоугольников: " + Convert.ToString(Rectangle(func, start, end, myN, presision));
            if (checkBox2.Checked) label2.Text += "\nметод трапеций: " + Convert.ToString(Trapeze(func, start, end, myN, presision));
            if (checkBox3.Checked) label2.Text += "\nметод Симпсона: " + Convert.ToString(Simpson(func, start, end, myN, presision));
            label2.Text += "\nОптимально N: " + Convert.ToString(myN);
          }
        }
      } catch {
        Mistake();
      }
    }
    void Mistake() {
      label2.Text = "Некорректный ввод";
    }


    private void Form1_Load(object sender, EventArgs e) {

    }
  }
}
