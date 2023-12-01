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
      checkBox1.Checked = false;
      checkBox2.Checked = false;
      checkBox3.Checked = false;
      label2.Text = "";
    }

    void Rectangle(string func, double start, double end, double presision) {
      double answer = 0;
      while (start < end) {
        answer += Math.Abs(presision * Func(start + presision/2));
        start += presision;
      }
      label2.Text += "\nметод прямоугольников: " + Convert.ToString(answer);
    } 
    void Trapeze(string func, double start, double end, double presision) {
      double answer = 0;
      while (start < end) {
        answer += Math.Abs((Func(start) + Func(start + presision)) / 2 * presision);
        start += presision;
      }
      label2.Text += "\nметод трапеций: " + Convert.ToString(answer);
    }
    void Simpson(string func, double start, double end, double presision) {
      double Chet = 0;
      double neChet = 0;
      double myStart = start + presision;
      int turn = 2;
      while (myStart < end) {
        if (turn % 2 == 0) {
          Chet += Math.Abs(Func(myStart));
        } else {
          neChet += Math.Abs(Func(myStart));
        }
        turn += 1;
        myStart += presision;
      }
      double answer = presision / 3 * (Func(start) + 4 * Chet + 2 * neChet + Func(end));
      label2.Text += "\nметод Симпсона: " + Convert.ToString(answer);
    }

    private void рассчитатьToolStripMenuItem_Click(object sender, EventArgs e) {
      try {
        label2.Text = "";
        string func = textBox1.Text;
        double start = Convert.ToDouble(textBox2.Text);
        double end = Convert.ToDouble(textBox3.Text);
        double presision = Convert.ToDouble(textBox4.Text);
        if ((start >= end) || ((end - start) < presision) || (presision <= 0)) {
          Mistake();
        } else {
          DrawChart(end, start, func);
          if (checkBox1.Checked) Rectangle(func, start, end, presision);
          if (checkBox2.Checked) Trapeze(func, start, end, presision);
          if (checkBox3.Checked) Simpson(func, start, end, presision);
        }
      } catch {
        Mistake();
      }
    }
    void Mistake() {
      label2.Text = "Некорректный ввод";
    }
  }
}
