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
        System.Windows.Forms.Timer showIconsTimer = new System.Windows.Forms.Timer();  // Ýkonlarý göstermek için timer
        System.Windows.Forms.Timer selectionTimer;
        // Oyuncularýn skorlarýný tutacak deðiþkenler
        int player1Score = 0;
        int player2Score = 0;
        bool isPlayer1Turn = true; // Hangi oyuncunu


        public Form1()
        {
            InitializeComponent();
            AssignIconsToSquares();

            showIconsTimer.Interval = 5000; // 5 saniye
            showIconsTimer.Tick += HideIcons; // Timer süresi dolunca HideIcons metodunu çaðýr
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
                label.ForeColor = label.BackColor; // Ýkonlarý baþlangýçta görünmez yap

                icons.RemoveAt(randomNumber);
            }

        }
        private void ShowAllIcons()
        {
            // Tüm ikonlarý görünür yap
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;
                if (iconLabel != null)
                {
                    iconLabel.ForeColor = Color.Black; // Ýkonlarý göster
                }
            }
            showIconsTimer.Start(); // 5 saniyelik timer'ý baþlat
        }

        private void HideIcons(object sender, EventArgs e)
        {
            showIconsTimer.Stop(); // Timer durdur
            // Tüm ikonlarý tekrar gizle
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;
                if (iconLabel != null)
                {
                    iconLabel.ForeColor = iconLabel.BackColor; // Ýkonlarý gizle
                }
            }
        }
        private void StartSelectionTimer()
        {
            // Ýkinci seçim için 5 saniye süre ver
            selectionTimer = new System.Windows.Forms.Timer();
            selectionTimer.Interval = 5000; // 5 saniye
            selectionTimer.Tick += (s, e) =>
            {
                // Süre dolduysa ve ikinci buton seçilmediyse
                if (secondClicked == null)
                {
                    // Süre doldu, sýra diðer oyuncuya geçer
                    isPlayer1Turn = !isPlayer1Turn;
                    statusLabel.Text = isPlayer1Turn ? "Sýra: Oyuncu 1" : "Sýra: Oyuncu 2"; // Sýradaki oyuncuyu göster

                    // Ýlk seçilen butonu tekrar gizle
                    HideSingleIcon(firstClicked);
                    firstClicked = null; // Ýlk butonu sýfýrla
                }

                selectionTimer.Stop(); // Timer'ý durdur
            };
            selectionTimer.Start();
        }
        private void HideSingleIcon(Label iconLabel)
        {
            if (iconLabel != null)
            {
                iconLabel.ForeColor = iconLabel.BackColor; // Ýkonu gizle
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
            // Eðer zaten iki týklama yapýlmýþsa, bir þey yapma
            if (firstClicked != null && secondClicked != null)
                return;

            Label clickedLabel = sender as Label;

            // Eðer týklanan alan bir etiket deðilse, bir þey yapma
            if (clickedLabel == null)
                return;

            // Eðer týklanan etiket zaten görünürse, bir þey yapma
            if (clickedLabel.ForeColor == Color.Black)
                return;

            // Eðer ilk týklama yapýlmadýysa
            if (firstClicked == null)
            {
                firstClicked = clickedLabel;
                firstClicked.ForeColor = Color.Black;

                StartSelectionTimer(); // 5 saniye içinde ikinci seçim yapýlmazsa sýrayý deðiþtiren timer baþlat
                return;
            }

            // Ýkinci týklama yapýlýnca
            secondClicked = clickedLabel;
            secondClicked.ForeColor = Color.Black;
            selectionTimer.Stop(); // Ýkinci týklama yapýlýrsa timer durdurulur

            // Eþleþme varsa
            if (firstClicked.Text == secondClicked.Text)
            {
                // Eþleþme varsa, doðru oyuncuya puan ver
                if (isPlayer1Turn)
                {
                    player1Score++; // Oyuncu 1 skoru artýr
                    player1ScoreLabel.Text = "Oyuncu 1 Skor: " + player1Score.ToString(); // Skoru güncelle
                }
                else
                {
                    player2Score++; // Oyuncu 2 skoru artýr
                    player2ScoreLabel.Text = "Oyuncu 2 Skor: " + player2Score.ToString(); // Skoru güncelle
                }

                firstClicked = null;
                secondClicked = null;

                // Eðer bir oyuncu 11 puana ulaþýrsa
                if (player1Score == 11 || player2Score == 11)
                {
                    MessageBox.Show((isPlayer1Turn ? "Oyuncu 1" : "Oyuncu 2") + " kazandý!");
                    ResetGame();
                }

                return;
            }

            // Eþleþme yoksa sýrayý deðiþtir ve timer'ý baþlat
            timer1.Start();
            isPlayer1Turn = !isPlayer1Turn; // Sýrayý deðiþtir
            statusLabel.Text = isPlayer1Turn ? "Sýra: Oyuncu 1" : "Sýra: Oyuncu 2"; // Sýradaki oyuncuyu göster
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            firstClicked.ForeColor = firstClicked.BackColor;
            secondClicked.ForeColor = secondClicked.BackColor;

            firstClicked = null;
            secondClicked = null;
        }
        // Oyun sýfýrlama fonksiyonu
        private void ResetGame()
        {
            player1Score = 0;
            player2Score = 0;
            player1ScoreLabel.Text = "Oyuncu 1 Skor: 0"; // Oyuncu 1 skoru sýfýrla
            player2ScoreLabel.Text = "Oyuncu 2 Skor: 0"; // Oyuncu 2 skoru sýfýrla

            // Ýkonlarý tekrar karýþtýr ve yerleþtir
            icons = new List<string>()
            {
                "!", "b", "d", "f", "r", "t", "z", "q", "m", "w", "u", "g", "h", "j", "k", "l", "n", "o", "x", "e",
                "!", "b", "d", "f", "r", "t", "z", "q", "m", "w", "u", "g", "h", "j", "k", "l", "n", "o", "x", "e"
            };
            AssignIconsToSquares(); // Ýkonlarý tekrar daðýt
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
