using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PizzeriaKG
{
    public partial class Form1 : Form
    {
        private bool lathato;

        private List<Pizza> pizzak = new List<Pizza>();
        private List<CheckBox> jeloloNegyzet = new List<CheckBox>();
        private List<RadioButton> rdBtnKicsiArak = new List<RadioButton>();
        private List<RadioButton> rdBtnNagyArak = new List<RadioButton>();
        private List<TextBox> txtDarabok = new List<TextBox>();

        private Panel pnlkozponti;
        private TextBox txtFizetendo;

        private int bal = 20;
        private int fent = 10;
        private int kozy = 40;
        private int meretY = 20;
        private int panelx = 200;
        private int meretChk = 120;
        private int meretAr = 50;
        private int meretFt = 40;
        private int meretDb = 50;
        private int koz = 3;

        public Form1()
        {
            InitializeComponent();
            openFileDialog1.InitialDirectory = Environment.CurrentDirectory;
            openFileDialog1.FileName = "";
            lathato = false;
            LathatosagBeallitas(lathato);

            pnlkozponti = new Panel();
            pnlkozponti.Size = new Size(800, 600);
            pnlkozponti.Location = new Point(10, 10);
            this.Controls.Add(pnlkozponti);

            txtFizetendo = new TextBox();
            txtFizetendo.Location = new Point(10, 620);
            txtFizetendo.Size = new Size(200, 30);
            this.Controls.Add(txtFizetendo);

            Button btnSzamol = new Button();
            btnSzamol.Text = "Számol";
            btnSzamol.Location = new Point(220, 620);
            btnSzamol.Click += new EventHandler(btnSzamol_Click);
            this.Controls.Add(btnSzamol);
        }

        public class Pizza
        {
            public string Nev { get; private set; }
            public int ArKicsi { get; set; }
            public int ArNagy { get; set; }

            public Pizza(string nev, int arKicsi, int arNagy)
            {
                Nev = nev;
                ArKicsi = arKicsi;
                ArNagy = arNagy;
            }

            public override string ToString()
            {
                return this.Nev;
            }
        }

        private void LathatosagBeallitas(bool lathatoE)
        {
            foreach (Control item in this.Controls)
            {
                item.Visible = lathatoE;
            }
            btnadatBe.Visible = !lathatoE;
        }

        private void btnadatBe_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string fajlNev = openFileDialog1.FileName;

                using (StreamReader olvasoCsatorna = new StreamReader(fajlNev))
                {
                    try
                    {
                        AdatBeolvasas(olvasoCsatorna);
                        ValasztekFeltoltes();
                        lathato = true;
                        LathatosagBeallitas(lathato);
                        btnadatBe.Visible = false;
                        this.BackgroundImage = null;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Hibaüzenet a fejlesztő számára");
                    }
                    finally
                    {
                        if (olvasoCsatorna != null)
                        {
                            olvasoCsatorna.Close();
                        }
                    }
                }
            }
        }

        private void ValasztekFeltoltes()
        {
            CheckBox checkBox;
            Label label;
            RadioButton radioButton;
            Panel panel;
            TextBox textBox;

            for (int i = 0; i < pizzak.Count; i++)
            {
                checkBox = new CheckBox();
                checkBox.TextAlign = ContentAlignment.MiddleLeft;
                checkBox.Text = pizzak[i].Nev;
                checkBox.Location = new Point(bal, fent + i * kozy);
                checkBox.Size = new Size(meretChk, meretY);
                jeloloNegyzet.Add(checkBox);
                pnlkozponti.Controls.Add(checkBox);

                panel = new Panel();
                panel.Size = new Size(panelx, meretY);
                panel.Location = new Point(bal + meretChk, fent + i * kozy);
                pnlkozponti.Controls.Add(panel);

                radioButton = new RadioButton();
                radioButton.Size = new Size(meretAr, meretY);
                radioButton.TextAlign = ContentAlignment.MiddleRight;
                radioButton.Text = pizzak[i].ArKicsi.ToString();
                radioButton.Location = new Point(0, 0);
                rdBtnKicsiArak.Add(radioButton);
                panel.Controls.Add(radioButton);

                radioButton = new RadioButton();
                radioButton.Size = new Size(meretAr, meretY);
                radioButton.TextAlign = ContentAlignment.MiddleRight;
                radioButton.Text = pizzak[i].ArNagy.ToString();
                radioButton.Location = new Point(meretAr + koz, 0);
                rdBtnNagyArak.Add(radioButton);
                panel.Controls.Add(radioButton);

                label = new Label();
                label.TextAlign = ContentAlignment.MiddleLeft;
                label.Text = " Ft";
                label.Location = new Point(2 * meretAr + koz, 0);
                label.Size = new Size(meretFt, meretY);
                panel.Controls.Add(label);

                textBox = new TextBox();
                textBox.Location = new Point(2 * meretAr + koz + meretFt + koz, 0);
                textBox.Size = new Size(meretDb, meretY);
                txtDarabok.Add(textBox);
                panel.Controls.Add(textBox);

                label = new Label();
                label.TextAlign = ContentAlignment.MiddleLeft;
                label.Text = " db";
                label.Location = new Point(2 * meretAr + koz + meretFt + koz + meretDb + koz, 0);
                label.Size = new Size(meretFt, meretY);
                panel.Controls.Add(label);
            }
        }

        private void btnSzamol_Click(object sender, EventArgs e)
        {
            bool vankijeloles = false;
            int db, osszeg = 0, ar = 0;

            for (int i = 0; i < jeloloNegyzet.Count; i++)
            {
                if (jeloloNegyzet[i].Checked)
                {
                    vankijeloles = true;
                    try
                    {
                        if (rdBtnKicsiArak[i].Checked)
                            ar = pizzak[i].ArKicsi;
                        else if (rdBtnNagyArak[i].Checked)
                            ar = pizzak[i].ArNagy;
                        else
                            throw new MissingFieldException();

                        db = int.Parse(txtDarabok[i].Text);

                        if (db <= 0) throw new Exception();

                        txtDarabok[i].BackColor = Color.White;
                        osszeg += ar * db;
                    }
                    catch (MissingFieldException)
                    {
                        MessageBox.Show("Nem választott méretet", "Hiba");
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Hibásan adta meg a darabszámot", "Hiba");
                        txtDarabok[i].BackColor = Color.Coral;
                        txtDarabok[i].Clear();
                    }
                }
            }

            if (!vankijeloles)
            {
                MessageBox.Show("Nincs kijelölve semmi", "Figyelmeztetés");
            }
            else
            {
                txtFizetendo.Text = osszeg + " Ft";
            }
        }

        private void AdatBeolvasas(StreamReader olvasoCsatorna)
        {
            pizzak.Clear();

            string sor;
            while ((sor = olvasoCsatorna.ReadLine()) != null)
            {
                string[] adatok = sor.Split(';');
                if (adatok.Length >= 3)
                {
                    string nev = adatok[0];
                    int arKicsi, arNagy;
                    if (int.TryParse(adatok[1], out arKicsi) && int.TryParse(adatok[2], out arNagy))
                    {
                        Pizza pizza = new Pizza(nev, arKicsi, arNagy);
                        pizzak.Add(pizza);
                    }
                }
            }
        }

        private void btnBezar_Click(object sender, EventArgs e)
        {
            DialogResult valasz = MessageBox.Show("Biztosan kilép?", "Megerősítés", MessageBoxButtons.YesNo);
            if (valasz == DialogResult.Yes) this.Close();
        }

        private void btnTorol_Click(object sender, EventArgs e)
        {
            txtFizetendo.Text = "";

            foreach (Control item in pnlkozponti.Controls)
            {
                if (item is CheckBox)
                {
                    ((CheckBox)item).Checked = false;
                }
                else if (item is TextBox)
                {
                    ((TextBox)item).Clear();
                }
                else if (item is Panel || item is GroupBox)
                {
                    foreach (Control child in item.Controls)
                    {
                        if (child is RadioButton)
                        {
                            ((RadioButton)child).Checked = false;
                        }
                        else if (child is TextBox)
                        {
                            ((TextBox)child).Clear();
                        }
                    }
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }
    }
}
