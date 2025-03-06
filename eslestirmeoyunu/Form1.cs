using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;


namespace eslestirmeoyunu
{
    public partial class Form1 : Form
    {
        Random random = new Random();

        List<string> icons = new List<string>()
        {
           "!", "b", "d", "f", "r", "t", "z", "q", "m", "w", "u", "g", "h", "j", "k", "l", "n", "o", "x", "e",
           "!", "b", "d", "f", "r", "t", "z", "q", "m", "w", "u", "g", "h", "j", "k", "l", "n", "o", "x", "e"
        };

        Label firstClicked, secondClicked;
        System.Windows.Forms.Timer showIconsTimer = new System.Windows.Forms.Timer();  // �konlar� g�stermek i�in timer
        System.Windows.Forms.Timer selectionTimer;
        // Oyuncular�n skorlar�n� tutacak de�i�kenler
        int player1Score = 0;
        int player2Score = 0;
        bool isPlayer1Turn = true; // Hangi oyuncunu


        public Form1()
        {
            InitializeComponent();
            AssignIconsToSquares();

            showIconsTimer.Interval = 5000; // 5 saniye
            showIconsTimer.Tick += HideIcons; // Timer s�resi dolunca HideIcons metodunu �a��r
            ShowAllIcons(); // Oyun

           
        }

        private void AssignIconsToSquares()
        {
            Label label;
            int randomNumber;
            for (int i = 0; i < tableLayoutPanel1.Controls.Count; i++)
            {
                if (tableLayoutPanel1.Controls[i] is Label)
                    label = (Label)tableLayoutPanel1.Controls[i];
                else
                    continue;
                randomNumber = random.Next(0, icons.Count);
                label.Text = icons[randomNumber];
                label.ForeColor = label.BackColor; // �konlar� ba�lang��ta g�r�nmez yap

                icons.RemoveAt(randomNumber);
            }

        }
        private void ShowAllIcons()
        {
            // T�m ikonlar� g�r�n�r yap
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;
                if (iconLabel != null)
                {
                    iconLabel.ForeColor = Color.Black; // �konlar� g�ster
                }
            }
            showIconsTimer.Start(); // 5 saniyelik timer'� ba�lat
        }

        private void HideIcons(object sender, EventArgs e)
        {
            showIconsTimer.Stop(); // Timer durdur
            // T�m ikonlar� tekrar gizle
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;
                if (iconLabel != null)
                {
                    iconLabel.ForeColor = iconLabel.BackColor; // �konlar� gizle
                }
            }
        }
        private void StartSelectionTimer()
        {
            // �kinci se�im i�in 5 saniye s�re ver
            selectionTimer = new System.Windows.Forms.Timer();
            selectionTimer.Interval = 5000; // 5 saniye
            selectionTimer.Tick += (s, e) =>
            {
                // S�re dolduysa ve ikinci buton se�ilmediyse
                if (secondClicked == null)
                {
                    // S�re doldu, s�ra di�er oyuncuya ge�er
                    isPlayer1Turn = !isPlayer1Turn;
                    statusLabel.Text = isPlayer1Turn ? "S�ra: Oyuncu 1" : "S�ra: Oyuncu 2"; // S�radaki oyuncuyu g�ster

                    // �lk se�ilen butonu tekrar gizle
                    HideSingleIcon(firstClicked);
                    firstClicked = null; // �lk butonu s�f�rla
                }

                selectionTimer.Stop(); // Timer'� durdur
            };
            selectionTimer.Start();
        }
        private void HideSingleIcon(Label iconLabel)
        {
            if (iconLabel != null)
            {
                iconLabel.ForeColor = iconLabel.BackColor; // �konu gizle
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label_Click(object sender, EventArgs e)
        {
            // E�er zaten iki t�klama yap�lm��sa, bir �ey yapma
            if (firstClicked != null && secondClicked != null)
                return;

            Label clickedLabel = sender as Label;

            // E�er t�klanan alan bir etiket de�ilse, bir �ey yapma
            if (clickedLabel == null)
                return;

            // E�er t�klanan etiket zaten g�r�n�rse, bir �ey yapma
            if (clickedLabel.ForeColor == Color.Black)
                return;

            // E�er ilk t�klama yap�lmad�ysa
            if (firstClicked == null)
            {
                firstClicked = clickedLabel;
                firstClicked.ForeColor = Color.Black;

                StartSelectionTimer(); // 5 saniye i�inde ikinci se�im yap�lmazsa s�ray� de�i�tiren timer ba�lat
                return;
            }

            // �kinci t�klama yap�l�nca
            secondClicked = clickedLabel;
            secondClicked.ForeColor = Color.Black;
            selectionTimer.Stop(); // �kinci t�klama yap�l�rsa timer durdurulur

            // E�le�me varsa
            if (firstClicked.Text == secondClicked.Text)
            {
                // E�le�me varsa, do�ru oyuncuya puan ver
                if (isPlayer1Turn)
                {
                    player1Score++; // Oyuncu 1 skoru art�r
                    player1ScoreLabel.Text = "Oyuncu 1 Skor: " + player1Score.ToString(); // Skoru g�ncelle
                }
                else
                {
                    player2Score++; // Oyuncu 2 skoru art�r
                    player2ScoreLabel.Text = "Oyuncu 2 Skor: " + player2Score.ToString(); // Skoru g�ncelle
                }

                firstClicked = null;
                secondClicked = null;

                // E�er bir oyuncu 11 puana ula��rsa
                if (player1Score == 11 || player2Score == 11)
                {
                    MessageBox.Show((isPlayer1Turn ? "Oyuncu 1" : "Oyuncu 2") + " kazand�!");
                    ResetGame();
                }

                return;
            }

            // E�le�me yoksa s�ray� de�i�tir ve timer'� ba�lat
            timer1.Start();
            isPlayer1Turn = !isPlayer1Turn; // S�ray� de�i�tir
            statusLabel.Text = isPlayer1Turn ? "S�ra: Oyuncu 1" : "S�ra: Oyuncu 2"; // S�radaki oyuncuyu g�ster
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            firstClicked.ForeColor = firstClicked.BackColor;
            secondClicked.ForeColor = secondClicked.BackColor;

            firstClicked = null;
            secondClicked = null;
        }
        // Oyun s�f�rlama fonksiyonu
        private void ResetGame()
        {
            player1Score = 0;
            player2Score = 0;
            player1ScoreLabel.Text = "Oyuncu 1 Skor: 0"; // Oyuncu 1 skoru s�f�rla
            player2ScoreLabel.Text = "Oyuncu 2 Skor: 0"; // Oyuncu 2 skoru s�f�rla

            // �konlar� tekrar kar��t�r ve yerle�tir
            icons = new List<string>()
            {
                "!", "b", "d", "f", "r", "t", "z", "q", "m", "w", "u", "g", "h", "j", "k", "l", "n", "o", "x", "e",
                "!", "b", "d", "f", "r", "t", "z", "q", "m", "w", "u", "g", "h", "j", "k", "l", "n", "o", "x", "e"
            };
            AssignIconsToSquares(); // �konlar� tekrar da��t
        }

        private void label41_Click(object sender, EventArgs e)
        {

        }

        private void label41_Click_1(object sender, EventArgs e)
        {

        }

        private void label42_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void statusLabel_Click(object sender, EventArgs e)
        {

        }

    }
}
