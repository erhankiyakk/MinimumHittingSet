using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;

namespace minimumHittingSet
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
    
        
        int altkümeElemanSayi = 0;
        int satir = -1;
        int[][] m_altkume;
        int m_a = -1;
        int m_satir = -1;
        void altKumeler(int[] elemanlarDizisi, int elemanlarBoyut, int altKümeElemaniSayisi, int index,int[] data, int i)
        {
            if (index == altKümeElemaniSayisi)
            {
                for (int j = 0; j < altKümeElemaniSayisi; j++)
                {
                    if (m_satir==satir|| m_a==-1)
                    {
                        m_a++;
                    }
                    m_altkume[satir][m_a] = data[j];
                    m_satir = satir;
                }
                satir++;
                m_a = -1;
                m_satir = -1;

                return;
            }
            
            if (i >= elemanlarBoyut)
                return;
            
            data[index] = elemanlarDizisi[i];
            altKumeler(elemanlarDizisi, elemanlarBoyut, altKümeElemaniSayisi, index + 1, data, i + 1);
            
            altKumeler(elemanlarDizisi, elemanlarBoyut, altKümeElemaniSayisi, index,data, i + 1);
        }
        
        void altKumeBul(int[] elemanlarDizisi,int elemanlarBoyut,int altKümeElemaniSayisi)
        {
            int[] geciciDizi = new int[altKümeElemaniSayisi];
            
            altKumeler(elemanlarDizisi, elemanlarBoyut, altKümeElemaniSayisi, 0, geciciDizi, 0);
        }

       bool kontrolum(int[] altKumeDizisi,int[,] asildizi,int satir)
       {
            int sayi=0;
            int[] kontroldizisi = new int[satir];
            while (sayi!=altkümeElemanSayi)
            {
                for (int i = 0; i < satir; i++)
                {
                    if (asildizi[i, altKumeDizisi[sayi]]==1)
                    {
                        kontroldizisi[i] = 1;
                    }
                }
                
                sayi++;
            }
            for (int i = 0; i < satir; i++)
            {
                if (kontroldizisi[i]==0)
                {
                    return false;
                }
            }
            return true;

       }

        int altKümeSayisiHesapla(int diziElemanSayisi,int altKümeElemanSayisi)
        {
            int a = diziElemanSayisi;
            int b = (diziElemanSayisi - altKümeElemanSayisi)+1;
            int carpım = 1;
            for (int i = b; i <= a; i++)
            {
                carpım *= i;
            }
            int c = Faktoriyel(altKümeElemanSayisi);
            int d = carpım/c;
            return d;
        }
        int Faktoriyel(int sayi)
        {
            if (sayi == 0)
            {
            return 1;
            }
            else {
            return sayi * Faktoriyel(sayi - 1); }
        }




        private int[] DiziyeAta(string[] dizi)
        {
            int[] asilDizi;
            int diziBoyut = dizi.Length;
            for (int i = (dizi.Length-1); 0 <= i; i--)
            {
                if (dizi[i] == "")
                {
                    diziBoyut--;
                }
            }
            asilDizi = new int[diziBoyut];
            for (int i = 0; i < diziBoyut; i++)
            {
                asilDizi[i] = Convert.ToInt32(dizi[i]);
            }

            return asilDizi;
        }
        //boş satırları silmek için
        string[] SatirDuzenle(string[] dizi)
        {
            int uzunluk = dizi.Length-1;
            while (dizi[uzunluk] == "" && 0<uzunluk)
            {
                uzunluk--;
            }
            uzunluk++;
            string[] asilDizi = new string[uzunluk];
            for (int i = 0; i < uzunluk; i++)
            {
                asilDizi[i] = dizi[i];
            }
            return asilDizi;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void btnGoster_Click(object sender, EventArgs e)
        {
            string[] satirlar;
            openFileDialog1.FileName = "Okunacak Dosyayı Seçiniz";
            openFileDialog1.Filter = "Yazı Dosyaları (*txt)|*.txt";
            openFileDialog1.ShowDialog();
            StreamReader okumaIslemi = new StreamReader(openFileDialog1.FileName);
            textBox1.Text = okumaIslemi.ReadToEnd();
            satirlar = System.IO.File.ReadAllLines(openFileDialog1.FileName);
            okumaIslemi.Close();
            satirlar = SatirDuzenle(satirlar);//boş satırları silmek için
            int[] elemanlar = DiziyeAta(satirlar[0].Split(' '));//U kümesi elemanlarını diziye atmak için
            int[,] matris = new int[satirlar.Length - 1, elemanlar.Length];
            for (int i = 0; i < satirlar.Length - 1; i++)
            {
                int[] satirEleman = DiziyeAta(satirlar[i + 1].Split(' '));
                for (int k = 0; k < satirEleman.Length; k++)
                {
                    matris[i, satirEleman[k]] = 1;
                }
            }


            int sayi = 1;
            string sonuc = "";
            while (sayi != elemanlar.Length)
            {
                altkümeElemanSayi = sayi;
                int altKümeSayisi = altKümeSayisiHesapla(elemanlar.Length, altkümeElemanSayi);
                m_altkume = new int[altKümeSayisi][];
                //m_alt için deneme

                for (int i = 0; i < altKümeSayisi; i++)
                {
                    m_altkume[i] = new int[altkümeElemanSayi];
                }
                altkümeElemanSayi = sayi;
                satir = 0;
                altKumeBul(elemanlar, elemanlar.Length, sayi);
                for (int k = 0; k < altKümeSayisi; k++)
                {
                    if (kontrolum(m_altkume[k], matris, satirlar.Length - 1))
                    {
                        for (int i = 0; i < altkümeElemanSayi; i++)
                        {
                            sonuc += m_altkume[k][i] + ",";
                        }
                        sayi = elemanlar.Length - 1;
                        break;
                    }
                }
                sayi++;


            }
            MessageBox.Show("Minimum Hitting Set :" + sonuc);
        }
    }
}

