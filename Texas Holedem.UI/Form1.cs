using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Texas_Heldom.Library.Classes;
using Texas_Heldom.Library.Enums;
using Texas_Heldom.Library.Interfaces;
using Texas_Heldom.Library.Structs;



namespace Texas_Holedem.UI
{


    public partial class Form1 : Form
    {
        #region Variables
        public Table _table;
        private List<Label> _playerCardLabels = new List<Label>();
        private List<Label> _dealerCardLabels = new List<Label>();

        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            try
            {
                #region Deck Table

                //Deck deck = new Deck();

                //deck.ShuffleDack(8);

                //var card = deck.DrawCard();
                #endregion

                #region temp card condition
                // tempory genererated value to display the card

                //Card card = new Card(Values.King, Suits.Hearts);


                //for (int i = 0; i < 8; i++)
                //{
                //    _playerCardLabels.Add(CreateCard(_playerLblPos[i].X, _playerLblPos[i].Y, card));
                //    Controls.Add(_playerCardLabels[i]);
                //}
                //for (int i = 0; i < 5; i++)
                //{
                //    _dealerCardLabels.Add(CreateCard(_dealerLblPos[i].X, _dealerLblPos[i].Y, card));
                //    Controls.Add(_dealerCardLabels[i]);
                //}

                #endregion

                //Card card = new Card(Values.King, Suits.Hearts);

                //Player player = new Player("joshua");
                //player.ReceivedCard(card, false);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnNewTable_Click(object sender, EventArgs e)
        {
            lblWinner.Visible = false;
            try
            {
                List<string> names = new List<string>();
                lblPlayerName1.Text = string.Empty; lblPlayerName2.Text = string.Empty;
                lblPlayerName3.Text = string.Empty; lblPlayerName4.Text = string.Empty;

                if (!txtPlayerName1.Text.Equals(string.Empty))  
                {
                    names.Add(txtPlayerName1.Text);

                    lblPlayerName1.Text = names[0];
                }
                if (!txtPlayerName2.Text.Equals(string.Empty)) 
                
                {
                    names.Add(txtPlayerName2.Text); lblPlayerName2.Text = names[1];
                }
                if (!txtPlayerName3.Text.Equals(string.Empty))
              
                {
                    names.Add(txtPlayerName3.Text); lblPlayerName3.Text = names[2];
                }
                if (!txtPlayerName4.Text.Equals(string.Empty)) 
              
                {
                    names.Add(txtPlayerName4.Text); lblPlayerName4.Text = names[3];
                }

                ClearHandLabels();  // call clear hand method
                ClearPlayerCardsFromTable();
                ClearDealerCardsFromTable();
                btnNewHand.Enabled = true;
                _table = new Table(names.ToArray());
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void btnNewHand_Click(object sender, EventArgs e)
        {
            if (_table == null)
                return;
            ClearHandLabels();  // call clear hand method
            ClearPlayerCardsFromTable();
            _table.DealNewHand();
            DisplayPlayerHand();
            ClearDealerCardsFromTable();
            _table.evaluteHand();
            //FillHandValueLabels();
            btnDrawCard.Enabled = true;
            lblWinner.Hide();

        }

        private void btnDrawCard_Click(object sender, EventArgs e)
        {

            if (_table.Dealer.CardCount == 0)
            {
                _table.DelerDrawsCard(3);
                _table.evaluteHand();
            }
            else if (_table.Dealer.CardCount >= 3)
                _table.DelerDrawsCard();
            DisplayDealerCards();

            if (_table.Dealer.CardCount == 4)
            {
                _table.evaluteHand();
                FillHandValueLabels();
            }
            if (_table.Dealer.CardCount == 5)
            {
                _table.evaluteHand();
                btnDrawCard.Enabled = false;
                lblWinner.Visible = true;
            }
            FillHandValueLabels();
            DetermineWinner();
            //lblWinner.Visible = true; // commit it 
            //var result = DisplayWinner().ToString();
            //lblWinner.Text = "Congratulation\n" + result + " is WINNER :)";
        }

        #region METHODS

        private Label CreateCard(int x, int y, Card card)
        {
            Label lbl = new Label();
            lbl.Text = card.Output;
            //"{0}\n{1}",card.Value, card.Suit);
            lbl.Size = new Size(45, 60);
            lbl.Location = new Point(x, y);
            lbl.BorderStyle = BorderStyle.FixedSingle;
            lbl.Font = new Font("Consolas", 15);
            lbl.TextAlign = ContentAlignment.MiddleCenter;
            lbl.BackColor = Color.WhiteSmoke;
            lbl.ForeColor =
                card.Suit.Equals(Suits.Hearts) ||
                card.Suit.Equals(Suits.Diamonds) ?
                Color.Red : Color.Black;
            return lbl;
        }

        private Point[] _dealerLblPos = new Point[]
            {
                new Point(320, 265),
                new Point(370, 265),
                new Point(420, 265),
                new Point(470, 265),
                new Point(520, 265)
            };

        private Point[] _playerLblPos = new Point[]
        {
             new Point(630, 330), new Point(680, 330),
             new Point(500, 420), new Point(550, 420),
             new Point(300, 420), new Point(350, 420),
             new Point(160, 330), new Point(210, 330)
            };

        private void ClearPlayerCardsFromTable()
        {
            foreach (Label label in _playerCardLabels)
                Controls.Remove(label);
            _playerCardLabels.Clear();

        }

        private void ClearDealerCardsFromTable()
        {
            foreach (Label label in _dealerCardLabels)
                Controls.Remove(label);
            _dealerCardLabels.Clear();

        }

        private void ClearHandLabels()
        {
            lblHand1.Text = string.Empty;
            lblHand2.Text = string.Empty;
            lblHand3.Text = string.Empty;
            lblHand4.Text = string.Empty;
        }

        private void FillHandValueLabels()
        {
            lblHand1.Text = _table.Players[0].HandValue.ToString();
            lblHand2.Text = _table.Players[1].HandValue.ToString();
            if (_table.Players.Count == 3)
            {
                lblHand3.Text = _table.Players[2].HandValue.ToString();
        }
            if (_table.Players.Count == 4)
            {
                lblHand3.Text = _table.Players[2].HandValue.ToString();
                lblHand4.Text = _table.Players[3].HandValue.ToString();
        }
    }

        private void DisplayPlayerHand()
        {
            //var p = 0;
            //var counter = 0;
            //for (int i = 1; i <= 4; i++)
            //{
            //    TextBox playerTextBox = (TextBox)this.Controls.Find("txtPlayerName" + (i), true)[0];
            //    Label handLabel = (Label)this.Controls.Find("lblHand" + (i), true)[0];
            //    if (playerTextBox.Text != String.Empty)
            //    {
            //        _table.Players[p].PlayerCards.Add(_table.Players[p].Cards.ElementAt(0));
            //      _table.Players[p].PlayerCards.Add(_table.Players[p].Cards.ElementAt(1));
            //    for (int l = 0; l <= 1; l++)
            //    {
            //        _playerCardLabels.Add(CreateCard(_playerLblPos[l + counter].X, _playerLblPos[l + counter].Y, _table.Players[p].PlayerCards[l]));
            //    }
            //    p++;
            //    }
            //    counter += 2;
            //}
            //foreach (Label lbl in _playerCardLabels)
            //{
            //    this.Controls.Add(lbl);
            //}
            //_table.Players[0].PlayerCards.Add(_table.Players[0].Cards[0]);
            //_table.Players[0].PlayerCards.Add(_table.Players[0].Cards[1]);
            //_table.Players[1].PlayerCards.Add(_table.Players[1].Cards[0]);
            //_table.Players[1].PlayerCards.Add(_table.Players[1].Cards[1]);
            //_table.Players[2].PlayerCards.Add(_table.Players[2].Cards[0]);
            //_table.Players[2].PlayerCards.Add(_table.Players[2].Cards[1]);
            //_table.Players[3].PlayerCards.Add(_table.Players[3].Cards[0]);
            //_table.Players[3].PlayerCards.Add(_table.Players[3].Cards[1]);
            if (_table.Players.Count == 2)
            {
                _playerCardLabels.Add(CreateCard(_playerLblPos[0].X, _playerLblPos[0].Y, _table.Players[0].PlayerCards[0]));
                Controls.Add(_playerCardLabels[0]);
                _playerCardLabels.Add(CreateCard(_playerLblPos[1].X, _playerLblPos[1].Y, _table.Players[0].PlayerCards[1]));
                Controls.Add(_playerCardLabels[1]);
                _playerCardLabels.Add(CreateCard(_playerLblPos[2].X, _playerLblPos[2].Y, _table.Players[1].PlayerCards[0]));
                Controls.Add(_playerCardLabels[2]);
                _playerCardLabels.Add(CreateCard(_playerLblPos[3].X, _playerLblPos[3].Y, _table.Players[1].PlayerCards[1]));
                Controls.Add(_playerCardLabels[3]);
            }
            if (_table.Players.Count ==3)
            {
                _playerCardLabels.Add(CreateCard(_playerLblPos[0].X, _playerLblPos[0].Y, _table.Players[0].PlayerCards[0]));
                Controls.Add(_playerCardLabels[0]);
                _playerCardLabels.Add(CreateCard(_playerLblPos[1].X, _playerLblPos[1].Y, _table.Players[0].PlayerCards[1]));
                Controls.Add(_playerCardLabels[1]);
                _playerCardLabels.Add(CreateCard(_playerLblPos[2].X, _playerLblPos[2].Y, _table.Players[1].PlayerCards[0]));
                Controls.Add(_playerCardLabels[2]);
                _playerCardLabels.Add(CreateCard(_playerLblPos[3].X, _playerLblPos[3].Y, _table.Players[1].PlayerCards[1]));
                Controls.Add(_playerCardLabels[3]);
                _playerCardLabels.Add(CreateCard(_playerLblPos[4].X, _playerLblPos[4].Y, _table.Players[2].PlayerCards[0]));
                Controls.Add(_playerCardLabels[4]);
                _playerCardLabels.Add(CreateCard(_playerLblPos[5].X, _playerLblPos[5].Y, _table.Players[2].PlayerCards[1]));
                Controls.Add(_playerCardLabels[5]);

            }
            if (_table.Players.Count == 4)
            {
                _playerCardLabels.Add(CreateCard(_playerLblPos[0].X, _playerLblPos[0].Y, _table.Players[0].PlayerCards[0]));
                Controls.Add(_playerCardLabels[0]);
                _playerCardLabels.Add(CreateCard(_playerLblPos[1].X, _playerLblPos[1].Y, _table.Players[0].PlayerCards[1]));
                Controls.Add(_playerCardLabels[1]);
                _playerCardLabels.Add(CreateCard(_playerLblPos[2].X, _playerLblPos[2].Y, _table.Players[1].PlayerCards[0]));
                Controls.Add(_playerCardLabels[2]);
                _playerCardLabels.Add(CreateCard(_playerLblPos[3].X, _playerLblPos[3].Y, _table.Players[1].PlayerCards[1]));
                Controls.Add(_playerCardLabels[3]);
                _playerCardLabels.Add(CreateCard(_playerLblPos[4].X, _playerLblPos[4].Y, _table.Players[2].PlayerCards[0]));
                Controls.Add(_playerCardLabels[4]);
                _playerCardLabels.Add(CreateCard(_playerLblPos[5].X, _playerLblPos[5].Y, _table.Players[2].PlayerCards[1]));
                Controls.Add(_playerCardLabels[5]);
                _playerCardLabels.Add(CreateCard(_playerLblPos[6].X, _playerLblPos[6].Y, _table.Players[3].PlayerCards[0]));
                Controls.Add(_playerCardLabels[6]);
                _playerCardLabels.Add(CreateCard(_playerLblPos[7].X, _playerLblPos[7].Y, _table.Players[3].PlayerCards[1]));
                Controls.Add(_playerCardLabels[7]);
            }
              



        }

        private void DisplayDealerCards()
        {
            ClearDealerCardsFromTable();
            int fg = _table.Dealer.CardCount;
            for (int i = 0; i < fg; i++)
            {
                _dealerCardLabels.Add(CreateCard(_dealerLblPos[i].X, _dealerLblPos[i].Y, _table.Dealer.Cards[i]));
                Controls.Add(_dealerCardLabels[i]);
            }
        }


        public void DetermineWinner()
        {
            if (_table.Players.Count < 3)
            {
                DisplayWinner2(); return;
            }
            if (_table.Players.Count == 3)
            {
                DisplayWinner3(); return;
            }
            if (_table.Players.Count > 3)
            {
                DisplayWinner(); return;
            }
        }


        void DisplayWinner2()
        {

            List<Hands> tempHand = new List<Hands>();

            #region Comparing HandValue
            if (_table.Players[0].HandValue > _table.Players[1].HandValue)
            { lblWinner.Text = "Congratulation \n Winner : " + _table.Players[0].Name; return; }
            tempHand.Add(_table.Players[0].HandValue);
            if (_table.Players[1].HandValue > _table.Players[0].HandValue )
            { lblWinner.Text = "Congratulation \n Winner : " + _table.Players[1].Name; return; }
            tempHand.Add(_table.Players[1].HandValue);
            #endregion

            var maxHandValue = (int)tempHand.Max(); // Find max Hand value 
            Values pHC1, pHC2, pBC1, pBC2;
            switch (maxHandValue)
            {
                case (int)Hands.StraightFlush:
                    if (_table.Players[0].HandValue == Hands.StraightFlush) { pHC1 = _table.Players[0].HighCard1; pBC1 = _table.Players[0].BestCards[0].Value; } else { pHC1 = 0; pBC1 = 0; }
                    if (_table.Players[1].HandValue == Hands.StraightFlush) { pHC2 = _table.Players[1].HighCard1; pBC2 = _table.Players[1].BestCards[0].Value; } else { pHC2 = 0; pBC2 = 0; }
                    playerHighCard(pHC1, pHC2); break;

                case (int)Hands.Straight:
                    if (_table.Players[0].HandValue == Hands.Straight) { pHC1 = _table.Players[0].HighCard1; pBC1 = _table.Players[0].BestCards[0].Value; } else { pHC1 = 0; pBC1 = 0; }
                    if (_table.Players[1].HandValue == Hands.Straight) { pHC2 = _table.Players[1].HighCard1; pBC2 = _table.Players[1].BestCards[0].Value; } else { pHC2 = 0; pBC2 = 0; }
                    playerHighCard(pHC1, pHC2); break;

                case (int)Hands.FourOfAKind:
                    if (_table.Players[0].HandValue == Hands.FourOfAKind) { pHC1 = _table.Players[0].HighCard1; pBC1 = _table.Players[0].BestCards[0].Value; } else { pHC1 = 0; pBC1 = 0; }
                    if (_table.Players[1].HandValue == Hands.FourOfAKind) { pHC2 = _table.Players[1].HighCard1; pBC2 = _table.Players[1].BestCards[0].Value; } else { pHC2 = 0; pBC2 = 0; }
                    playerHighCard(pHC1, pHC2); break;

                case (int)Hands.FullHouse:
                    if (_table.Players[0].HandValue == Hands.FullHouse) { pHC1 = _table.Players[0].HighCard1; pBC1 = _table.Players[0].BestCards[0].Value; } else { pHC1 = 0; pBC1 = 0; }
                    if (_table.Players[1].HandValue == Hands.FullHouse) { pHC2 = _table.Players[1].HighCard1; pBC2 = _table.Players[1].BestCards[0].Value; } else { pHC2 = 0; pBC2 = 0; }
                    playerHighCard(pHC1, pHC2); break;

                case (int)Hands.Flush:
                    if (_table.Players[0].HandValue == Hands.Flush) { pHC1 = _table.Players[0].HighCard1; pBC1 = _table.Players[0].BestCards[0].Value; } else { pHC1 = 0; pBC1 = 0; }
                    if (_table.Players[1].HandValue == Hands.Flush) { pHC2 = _table.Players[1].HighCard1; pBC2 = _table.Players[1].BestCards[0].Value; } else { pHC2 = 0; pBC2 = 0; }
                    playerHighCard(pHC1, pHC2); break;

                case (int)Hands.ThreeOfAKind:
                    if (_table.Players[0].HandValue == Hands.ThreeOfAKind) { pHC1 = _table.Players[0].HighCard1; pBC1 = _table.Players[0].BestCards[0].Value; } else { pHC1 = 0; pBC1 = 0; }
                    if (_table.Players[1].HandValue == Hands.ThreeOfAKind) { pHC2 = _table.Players[1].HighCard1; pBC2 = _table.Players[1].BestCards[0].Value; } else { pHC2 = 0; pBC2 = 0; }
                    playerHighCard(pHC1, pHC2); break;

                case (int)Hands.TwoPair:
                    if (_table.Players[0].HandValue == Hands.TwoPair) { pHC1 = _table.Players[0].HighCard1; pBC1 = _table.Players[0].BestCards[0].Value; } else { pHC1 = 0; pBC1 = 0; }
                    if (_table.Players[1].HandValue == Hands.TwoPair) { pHC2 = _table.Players[1].HighCard1; pBC2 = _table.Players[1].BestCards[0].Value; } else { pHC2 = 0; pBC2 = 0; }
                    playerHighCard(pHC1, pHC2); break;

                case (int)Hands.Pair:
                    if (_table.Players[0].HandValue == Hands.Pair) { pHC1 = _table.Players[0].HighCard1; pBC1 = _table.Players[0].BestCards[0].Value; } else { pHC1 = 0; pBC1 = 0; }
                    if (_table.Players[1].HandValue == Hands.Pair) { pHC2 = _table.Players[1].HighCard1; pBC2 = _table.Players[1].BestCards[0].Value; } else { pHC2 = 0; pBC2 = 0; }
                    playerHighCard(pHC1, pHC2); break;

                case (int)Hands.Nothing:
                    if (_table.Players[0].HandValue == Hands.Nothing) { pHC1 = _table.Players[0].PlayerCards.Max().Value; ; pBC1 = _table.Players[0].BestCards[0].Value; } else { pHC1 = 0; pBC1 = 0; }
                    if (_table.Players[1].HandValue == Hands.Nothing) { pHC2 = _table.Players[1].PlayerCards.Max().Value; pBC2 = _table.Players[1].BestCards[0].Value; } else { pHC2 = 0; pBC2 = 0; }
                    playerHighCard(pHC1, pHC2); break;
            
            }
            void playerHighCard(Values p1, Values p2)
            {
                if (p1 > p2 )
                {
                    lblWinner.Text = "CONGRATULATION \n Winner : " + _table.Players[0].Name; return;
                }
                if (p2 > p1 )
                {
                    lblWinner.Text = "CONGRATULATION \n Winner : " + _table.Players[1].Name; return;
                }
               
                else playerBestCard(pBC1, pBC2);
            }
            void playerBestCard(Values b1, Values b2)
            {
                if (b1 > b2 )
                {
                    lblWinner.Text = "CONGRATULATION \n Winner : " + _table.Players[0].Name; return;
                }
                if (b2 > b1 )
                {
                    lblWinner.Text = "CONGRATULATION \n Winner : " + _table.Players[1].Name; return;
                }
                else { lblWinner.Text = "CONGRATULATION \n Draw "; }
                return;
            }


        }

        void DisplayWinner3()
        {

            List<Hands> tempHand = new List<Hands>();
            #region Comparing HandValue
            if (_table.Players[0].HandValue > _table.Players[1].HandValue &&
               _table.Players[0].HandValue > _table.Players[2].HandValue)
            { lblWinner.Text = "Congratulation \n Winner : " + _table.Players[0].Name; return; }
            tempHand.Add(_table.Players[0].HandValue);
            if (_table.Players[1].HandValue > _table.Players[0].HandValue &&
                _table.Players[1].HandValue > _table.Players[2].HandValue
               )
            { lblWinner.Text = "Congratulation \n Winner : " + _table.Players[1].Name; return; }
            tempHand.Add(_table.Players[1].HandValue);
            if (_table.Players[2].HandValue > _table.Players[0].HandValue &&
                _table.Players[2].HandValue > _table.Players[1].HandValue
               )
            { lblWinner.Text = "Congratulation \n Winner : " + _table.Players[2].Name; return; }
            tempHand.Add(_table.Players[2].HandValue);
            #endregion

            var maxHandValue = (int)tempHand.Max(); // Find max Hand value 
            Values pHC1, pHC2, pHC3, pBC1, pBC2, pBC3;

            switch (maxHandValue)
            {
                case (int)Hands.StraightFlush:
                    if (_table.Players[0].HandValue == Hands.StraightFlush) { pHC1 = _table.Players[0].HighCard1; pBC1 = _table.Players[0].BestCards[0].Value; } else { pHC1 = 0; pBC1 = 0; }
                    if (_table.Players[1].HandValue == Hands.StraightFlush) { pHC2 = _table.Players[1].HighCard1; pBC2 = _table.Players[1].BestCards[0].Value; } else { pHC2 = 0; pBC2 = 0; }
                    if (_table.Players[2].HandValue == Hands.StraightFlush) { pHC3 = _table.Players[2].HighCard1; pBC3 = _table.Players[2].BestCards[0].Value; } else { pHC3 = 0; pBC3 = 0; }
                    playerHighCard(pHC1, pHC2, pHC3); break;

                case (int)Hands.Straight:
                    if (_table.Players[0].HandValue == Hands.Straight) { pHC1 = _table.Players[0].HighCard1; pBC1 = _table.Players[0].BestCards[0].Value; } else { pHC1 = 0; pBC1 = 0; }
                    if (_table.Players[1].HandValue == Hands.Straight) { pHC2 = _table.Players[1].HighCard1; pBC2 = _table.Players[1].BestCards[0].Value; } else { pHC2 = 0; pBC2 = 0; }
                    if (_table.Players[2].HandValue == Hands.Straight) { pHC3 = _table.Players[2].HighCard1; pBC3 = _table.Players[2].BestCards[0].Value; } else { pHC3 = 0; pBC3 = 0; }
                    playerHighCard(pHC1, pHC2, pHC3); break;

                case (int)Hands.FourOfAKind:
                    if (_table.Players[0].HandValue == Hands.FourOfAKind) { pHC1 = _table.Players[0].HighCard1; pBC1 = _table.Players[0].BestCards[0].Value; } else { pHC1 = 0; pBC1 = 0; }
                    if (_table.Players[1].HandValue == Hands.FourOfAKind) { pHC2 = _table.Players[1].HighCard1; pBC2 = _table.Players[1].BestCards[0].Value; } else { pHC2 = 0; pBC2 = 0; }
                    if (_table.Players[2].HandValue == Hands.FourOfAKind) { pHC3 = _table.Players[2].HighCard1; pBC3 = _table.Players[2].BestCards[0].Value; } else { pHC3 = 0; pBC3 = 0; }
                    playerHighCard(pHC1, pHC2, pHC3); break;

                case (int)Hands.FullHouse:
                    if (_table.Players[0].HandValue == Hands.FullHouse) { pHC1 = _table.Players[0].HighCard1; pBC1 = _table.Players[0].BestCards[0].Value; } else { pHC1 = 0; pBC1 = 0; }
                    if (_table.Players[1].HandValue == Hands.FullHouse) { pHC2 = _table.Players[1].HighCard1; pBC2 = _table.Players[1].BestCards[0].Value; } else { pHC2 = 0; pBC2 = 0; }
                    if (_table.Players[2].HandValue == Hands.FullHouse) { pHC3 = _table.Players[2].HighCard1; pBC3 = _table.Players[2].BestCards[0].Value; } else { pHC3 = 0; pBC3 = 0; }
                    playerHighCard(pHC1, pHC2, pHC3); break;

                case (int)Hands.Flush:
                    if (_table.Players[0].HandValue == Hands.Flush) { pHC1 = _table.Players[0].HighCard1; pBC1 = _table.Players[0].BestCards[0].Value; } else { pHC1 = 0; pBC1 = 0; }
                    if (_table.Players[1].HandValue == Hands.Flush) { pHC2 = _table.Players[1].HighCard1; pBC2 = _table.Players[1].BestCards[0].Value; } else { pHC2 = 0; pBC2 = 0; }
                    if (_table.Players[2].HandValue == Hands.Flush) { pHC3 = _table.Players[2].HighCard1; pBC3 = _table.Players[2].BestCards[0].Value; } else { pHC3 = 0; pBC3 = 0; }
                    playerHighCard(pHC1, pHC2, pHC3); break;

                case (int)Hands.ThreeOfAKind:
                    if (_table.Players[0].HandValue == Hands.ThreeOfAKind) { pHC1 = _table.Players[0].HighCard1; pBC1 = _table.Players[0].BestCards[0].Value; } else { pHC1 = 0; pBC1 = 0; }
                    if (_table.Players[1].HandValue == Hands.ThreeOfAKind) { pHC2 = _table.Players[1].HighCard1; pBC2 = _table.Players[1].BestCards[0].Value; } else { pHC2 = 0; pBC2 = 0; }
                    if (_table.Players[2].HandValue == Hands.ThreeOfAKind) { pHC3 = _table.Players[2].HighCard1; pBC3 = _table.Players[2].BestCards[0].Value; } else { pHC3 = 0; pBC3 = 0; }
                    playerHighCard(pHC1, pHC2, pHC3); break;

                case (int)Hands.TwoPair:
                    if (_table.Players[0].HandValue == Hands.TwoPair) { pHC1 = _table.Players[0].HighCard1; pBC1 = _table.Players[0].BestCards[0].Value; } else { pHC1 = 0; pBC1 = 0; }
                    if (_table.Players[1].HandValue == Hands.TwoPair) { pHC2 = _table.Players[1].HighCard1; pBC2 = _table.Players[1].BestCards[0].Value; } else { pHC2 = 0; pBC2 = 0; }
                    if (_table.Players[2].HandValue == Hands.TwoPair) { pHC3 = _table.Players[2].HighCard1; pBC3 = _table.Players[2].BestCards[0].Value; } else { pHC3 = 0; pBC3 = 0; }
                    playerHighCard(pHC1, pHC2, pHC3); break;
                case (int)Hands.Pair:
                    if (_table.Players[0].HandValue == Hands.Pair) { pHC1 = _table.Players[0].HighCard1; pBC1 = _table.Players[0].BestCards[0].Value; } else { pHC1 = 0; pBC1 = 0; }
                    if (_table.Players[1].HandValue == Hands.Pair) { pHC2 = _table.Players[1].HighCard1; pBC2 = _table.Players[1].BestCards[0].Value; } else { pHC2 = 0; pBC2 = 0; }
                    if (_table.Players[2].HandValue == Hands.Pair) { pHC3 = _table.Players[2].HighCard1; pBC3 = _table.Players[2].BestCards[0].Value; } else { pHC3 = 0; pBC3 = 0; }
                    playerHighCard(pHC1, pHC2, pHC3); break;

                case (int)Hands.Nothing:
                    if (_table.Players[0].HandValue == Hands.Nothing) { pHC1 = _table.Players[0].PlayerCards.Max().Value; ; pBC1 = _table.Players[0].BestCards[0].Value; } else { pHC1 = 0; pBC1 = 0; }
                    if (_table.Players[1].HandValue == Hands.Nothing) { pHC2 = _table.Players[1].PlayerCards.Max().Value; pBC2 = _table.Players[1].BestCards[0].Value; } else { pHC2 = 0; pBC2 = 0; }
                    if (_table.Players[2].HandValue == Hands.Nothing) { pHC3 = _table.Players[2].PlayerCards.Max().Value; pBC3 = _table.Players[2].BestCards[0].Value; } else { pHC3 = 0; pBC3 = 0; }
                    playerHighCard(pHC1, pHC2, pHC3); break;
            }

            void playerHighCard(Values p1, Values p2, Values p3)
            {
                if (p1 > p2 && p1 > p3)
                {
                    lblWinner.Text = "CONGRATULATION \n Winner : " + _table.Players[0].Name; return;
                }
                if (p2 > p1 && p2 > p3)
                {
                    lblWinner.Text = "CONGRATULATION \n Winner : " + _table.Players[1].Name; return;
                }
                if (p3 > p1 && p3 > p2)
                {
                    lblWinner.Text = "CONGRATULATION \n Winner : " + _table.Players[2].Name; return;
                }
                else playerBestCard(pBC1, pBC2, pBC3);
            }

            void playerBestCard(Values b1, Values b2, Values b3)
            {
                if (b1 > b2 && b1 > b3)
                {
                    lblWinner.Text = "CONGRATULATION \n Winner : " + _table.Players[0].Name; return;
                }
                if (b2 > b1 && b2 > b3)
                {
                    lblWinner.Text = "CONGRATULATION \n Winner : " + _table.Players[1].Name; return;
                }
                if (b3 > b1 && b3 > b2)
                {
                    lblWinner.Text = "CONGRATULATION \n Winner : " + _table.Players[2].Name; return;
                }
                else { lblWinner.Text = "CONGRATULATION \n Draw "; }
                return;
            }
            #region Old methods
            void highCardPlayer()
                    {

                        if (_table.Players[0].HighCard1 > _table.Players[1].HighCard1 &&
                            _table.Players[0].HighCard1 > _table.Players[2].HighCard1)
                        { lblWinner.Text = "Congratulation \n Winner : " + _table.Players[0].Name; return; }
                        if (_table.Players[1].HighCard1 > _table.Players[0].HighCard1 &&
                            _table.Players[1].HighCard1 > _table.Players[2].HighCard1)
                        { lblWinner.Text = "Congratulation \n Winner : " + _table.Players[1].Name; return; }
                        if (_table.Players[2].HighCard1 > _table.Players[0].HighCard1 &&
                            _table.Players[2].HighCard1 > _table.Players[1].HighCard1)
                        { lblWinner.Text = "Congratulation \n Winner : " + _table.Players[2].Name; return; }

                        else CompareBestCards();

                    }
                    void CompareBestCards()
                    {
                        //var abc =_table.Players[0].BestCards[0].Value;
                        //for (int i = 4; i >= 0; i--) { }
                        //{ if (!_table.Players[0].BestCards[i].Value.Equals(_table.Players[1].BestCards[i].Value))
                        //     if (_table.Players[0].BestCards[i].Value > _table.Players[1].BestCards[i].Value)
                        //            _table.Players[0].Name : _table.Players[1].Name;

                        if (_table.Players[0].BestCards[0].Value > _table.Players[1].BestCards[0].Value &&
                            _table.Players[0].BestCards[0].Value > _table.Players[2].BestCards[0].Value)
                        { lblWinner.Text = "Congratulation \n Winner : " + _table.Players[0].Name; return; }
                        if (_table.Players[1].BestCards[0].Value > _table.Players[0].BestCards[0].Value &&
                            _table.Players[1].BestCards[0].Value > _table.Players[2].BestCards[0].Value)
                        { lblWinner.Text = "Congratulation \n Winner : " + _table.Players[1].Name; return; }
                        if (_table.Players[2].BestCards[0].Value > _table.Players[0].BestCards[0].Value &&
                            _table.Players[2].BestCards[0].Value > _table.Players[1].BestCards[0].Value)
                        { lblWinner.Text = "Congratulation \n Winner : " + _table.Players[2].Name; return; }

                    }
                    #endregion
        }

        void DisplayWinner()
        {
           
            List<Hands> tempHand = new List<Hands>();
            #region Comparing HandValue
            if (_table.Players[0].HandValue > _table.Players[1].HandValue &&
               _table.Players[0].HandValue > _table.Players[2].HandValue &&
               _table.Players[0].HandValue > _table.Players[3].HandValue)
            { lblWinner.Text = "Congratulation \n Winner : "+ _table.Players[0].Name; return; }
            tempHand.Add(_table.Players[0].HandValue);
            if (_table.Players[1].HandValue > _table.Players[0].HandValue &&
                _table.Players[1].HandValue > _table.Players[2].HandValue &&
                _table.Players[1].HandValue > _table.Players[3].HandValue)
            { lblWinner.Text = "Congratulation \n Winner : " + _table.Players[1].Name; return; }
            tempHand.Add(_table.Players[1].HandValue);
            if (_table.Players[2].HandValue > _table.Players[0].HandValue &&
                _table.Players[2].HandValue > _table.Players[1].HandValue &&
                _table.Players[2].HandValue > _table.Players[3].HandValue)
            { lblWinner.Text = "Congratulation \n Winner : " + _table.Players[2].Name; return; }
            tempHand.Add(_table.Players[2].HandValue);
            if (_table.Players[3].HandValue > _table.Players[0].HandValue &&
                _table.Players[3].HandValue > _table.Players[1].HandValue &&
                _table.Players[3].HandValue > _table.Players[2].HandValue)
            { lblWinner.Text = "Congratulation \n Winner : " + _table.Players[3].Name; return; }
            tempHand.Add(_table.Players[3].HandValue);
            #endregion
           
            var abc = _table.Players[0].Cards.Except(_table.Dealer.Cards).ToList();
            abc.Reverse();
            var maxHandValue = (int)tempHand.Max(); // Find max Hand value 
            //var temp = Enum.GetName(typeof(Hands), maxHandValue);
            Values pHC1, pHC2, pHC3, pHC4, pBC1, pBC2, pBC3, pBC4;

            switch (maxHandValue)
            {
                case (int)Hands.StraightFlush:
                    if (_table.Players[0].HandValue == Hands.StraightFlush) { pHC1 = _table.Players[0].HighCard1; pBC1 = _table.Players[0].BestCards[0].Value; } else { pHC1 = 0; pBC1 = 0; }
                    if (_table.Players[1].HandValue == Hands.StraightFlush) { pHC2 = _table.Players[1].HighCard1; pBC2 = _table.Players[1].BestCards[0].Value; } else { pHC2 = 0; pBC2 = 0; }
                    if (_table.Players[2].HandValue == Hands.StraightFlush) { pHC3 = _table.Players[2].HighCard1; pBC3 = _table.Players[2].BestCards[0].Value; } else { pHC3 = 0; pBC3 = 0; }
                    if (_table.Players[3].HandValue == Hands.StraightFlush) { pHC4 = _table.Players[3].HighCard1; pBC4 = _table.Players[3].BestCards[0].Value; } else { pHC4 = 0; pBC4 = 0; }
                    playerHighCard(pHC1, pHC2, pHC3, pHC4);  break;
                case (int)Hands.Straight:
                    if (_table.Players[0].HandValue == Hands.Straight) { pHC1 = _table.Players[0].HighCard1; pBC1 = _table.Players[0].BestCards[0].Value; } else { pHC1 = 0; pBC1 = 0; }
                    if (_table.Players[1].HandValue == Hands.Straight) { pHC2 = _table.Players[1].HighCard1; pBC2 = _table.Players[1].BestCards[0].Value; } else { pHC2 = 0; pBC2 = 0; }
                    if (_table.Players[2].HandValue == Hands.Straight) { pHC3 = _table.Players[2].HighCard1; pBC3 = _table.Players[2].BestCards[0].Value; } else { pHC3 = 0; pBC3 = 0; }
                    if (_table.Players[3].HandValue == Hands.Straight) { pHC4 = _table.Players[3].HighCard1; pBC4 = _table.Players[3].BestCards[0].Value; } else { pHC4 = 0; pBC4 = 0; }
                    playerHighCard(pHC1, pHC2, pHC3, pHC4);  break;

                case (int)Hands.FourOfAKind:
                    if (_table.Players[0].HandValue == Hands.FourOfAKind) { pHC1 = _table.Players[0].HighCard1; pBC1 = _table.Players[0].BestCards[0].Value; } else { pHC1 = 0; pBC1 = 0; }
                    if (_table.Players[1].HandValue == Hands.FourOfAKind) { pHC2 = _table.Players[1].HighCard1; pBC2 = _table.Players[1].BestCards[0].Value; } else { pHC2 = 0; pBC2 = 0; }
                    if (_table.Players[2].HandValue == Hands.FourOfAKind) { pHC3 = _table.Players[2].HighCard1; pBC3 = _table.Players[2].BestCards[0].Value; } else { pHC3 = 0; pBC3 = 0; }
                    if (_table.Players[3].HandValue == Hands.FourOfAKind) { pHC4 = _table.Players[3].HighCard1; pBC4 = _table.Players[3].BestCards[0].Value; } else { pHC4 = 0; pBC4 = 0; }
                    playerHighCard(pHC1, pHC2, pHC3, pHC4);  break;

                case (int)Hands.FullHouse:
                    if (_table.Players[0].HandValue == Hands.FullHouse) { pHC1 = _table.Players[0].HighCard1; pBC1 = _table.Players[0].BestCards[0].Value; } else { pHC1 = 0; pBC1 = 0; }
                    if (_table.Players[1].HandValue == Hands.FullHouse) { pHC2 = _table.Players[1].HighCard1; pBC2 = _table.Players[1].BestCards[0].Value; } else { pHC2 = 0; pBC2 = 0; }
                    if (_table.Players[2].HandValue == Hands.FullHouse) { pHC3 = _table.Players[2].HighCard1; pBC3 = _table.Players[2].BestCards[0].Value; } else { pHC3 = 0; pBC3 = 0; }
                    if (_table.Players[3].HandValue == Hands.FullHouse) { pHC4 = _table.Players[3].HighCard1; pBC4 = _table.Players[3].BestCards[0].Value; } else { pHC4 = 0; pBC4 = 0; }
                    playerHighCard(pHC1, pHC2, pHC3, pHC4);  break;

                case (int)Hands.Flush:
                    if (_table.Players[0].HandValue == Hands.Flush) { pHC1 = _table.Players[0].HighCard1; pBC1 = _table.Players[0].BestCards[0].Value; } else { pHC1 = 0; pBC1 = 0; }
                    if (_table.Players[1].HandValue == Hands.Flush) { pHC2 = _table.Players[1].HighCard1; pBC2 = _table.Players[1].BestCards[0].Value; } else { pHC2 = 0; pBC2 = 0; }
                    if (_table.Players[2].HandValue == Hands.Flush) { pHC3 = _table.Players[2].HighCard1; pBC3 = _table.Players[2].BestCards[0].Value; } else { pHC3 = 0; pBC3 = 0; }
                    if (_table.Players[3].HandValue == Hands.Flush) { pHC4 = _table.Players[3].HighCard1; pBC4 = _table.Players[3].BestCards[0].Value; } else { pHC4 = 0; pBC4 = 0; }
                    playerHighCard(pHC1, pHC2, pHC3, pHC4); break;

                case (int)Hands.ThreeOfAKind:
                    if (_table.Players[0].HandValue == Hands.ThreeOfAKind) { pHC1 = _table.Players[0].HighCard1; pBC1 = _table.Players[0].BestCards[0].Value; } else { pHC1 = 0; pBC1 = 0; }
                    if (_table.Players[1].HandValue == Hands.ThreeOfAKind) { pHC2 = _table.Players[1].HighCard1; pBC2 = _table.Players[1].BestCards[0].Value; } else { pHC2 = 0; pBC2 = 0; }
                    if (_table.Players[2].HandValue == Hands.ThreeOfAKind) { pHC3 = _table.Players[2].HighCard1; pBC3 = _table.Players[2].BestCards[0].Value; } else { pHC3 = 0; pBC3 = 0; }
                    if (_table.Players[3].HandValue == Hands.ThreeOfAKind) { pHC4 = _table.Players[3].HighCard1; pBC4 = _table.Players[3].BestCards[0].Value; } else { pHC4 = 0; pBC4 = 0; }
                    playerHighCard(pHC1, pHC2, pHC3, pHC4);  break;

                case (int)Hands.TwoPair:
                    if (_table.Players[0].HandValue == Hands.TwoPair) { pHC1 = _table.Players[0].HighCard1; pBC1 = _table.Players[0].BestCards[0].Value; } else { pHC1 = 0; pBC1 = 0; }
                    if (_table.Players[1].HandValue == Hands.TwoPair) { pHC2 = _table.Players[1].HighCard1; pBC2 = _table.Players[1].BestCards[0].Value; } else { pHC2 = 0; pBC2 = 0; }
                    if (_table.Players[2].HandValue == Hands.TwoPair) { pHC3 = _table.Players[2].HighCard1; pBC3 = _table.Players[2].BestCards[0].Value; } else { pHC3 = 0; pBC3 = 0; }
                    if (_table.Players[3].HandValue == Hands.TwoPair) { pHC4 = _table.Players[3].HighCard1; pBC4 = _table.Players[3].BestCards[0].Value; } else { pHC4 = 0; pBC4 = 0; }
                    playerHighCard(pHC1, pHC2, pHC3, pHC4);  break;
                case (int)Hands.Pair:
                    if (_table.Players[0].HandValue == Hands.Pair) { pHC1 = _table.Players[0].HighCard1; pBC1 = _table.Players[0].BestCards[0].Value; } else { pHC1 = 0; pBC1 = 0; }
                    if (_table.Players[1].HandValue == Hands.Pair) { pHC2 = _table.Players[1].HighCard1; pBC2 = _table.Players[1].BestCards[0].Value; } else { pHC2 = 0; pBC2 = 0; }
                    if (_table.Players[2].HandValue == Hands.Pair) { pHC3 = _table.Players[2].HighCard1; pBC3 = _table.Players[2].BestCards[0].Value; } else { pHC3 = 0; pBC3 = 0; }
                    if (_table.Players[3].HandValue == Hands.Pair) { pHC4 = _table.Players[3].HighCard1; pBC4 = _table.Players[3].BestCards[0].Value; } else { pHC4 = 0; pBC4 = 0; }
                    playerHighCard(pHC1, pHC2, pHC3, pHC4);  break;
                case (int)Hands.Nothing:
                    if (_table.Players[0].HandValue == Hands.Nothing) { pHC1 = _table.Players[0].PlayerCards.Max().Value; pBC1 = _table.Players[0].BestCards[0].Value;} else { pHC1 = 0; pBC1 = 0; }
                    if (_table.Players[1].HandValue == Hands.Nothing) { pHC2 = _table.Players[1].PlayerCards.Max().Value; pBC2 = _table.Players[1].BestCards[0].Value; } else { pHC2 = 0; pBC2 = 0; }
                    if (_table.Players[2].HandValue == Hands.Nothing) { pHC3 = _table.Players[2].PlayerCards.Max().Value; pBC3 = _table.Players[2].BestCards[0].Value; } else { pHC3 = 0; pBC3 = 0; }
                    if (_table.Players[3].HandValue == Hands.Nothing ) { pHC4 = _table.Players[3].PlayerCards.Max().Value; pBC4 = _table.Players[3].BestCards[0].Value; } else { pHC4 = 0; pBC4 = 0; }
                    playerHighCard(pHC1, pHC2, pHC3, pHC4); break;
            }

            void playerHighCard(Values p1,Values p2,Values p3, Values p4){
                if (p1 >p2 && p1 > p3 && p1> p4)
                { lblWinner.Text = "CONGRATULATION :)\n Winner : " + _table.Players[0].Name; return; }
                if (p2 > p1 && p2 > p3 && p1 > p4)
                { lblWinner.Text = "CONGRATULATION :)\n Winner : " + _table.Players[1].Name; return; }
                if (p3>p1 && p3>p2 && p3 >p4)
                { lblWinner.Text = "CONGRATULATION :)\n Winner : " + _table.Players[2].Name; return; }
                if (p4 > p1 && p4 > p2 && p4 > p3)
                { lblWinner.Text = "CONGRATULATION :)\n Winner : " + _table.Players[3].Name; return; }
                else playerBestCard(pBC1, pBC2, pBC3, pBC4);
            }
            void playerBestCard(Values b1, Values b2, Values b3, Values b4)
            {
                if (b1 > b2 && b1 > b3 && b1 > b4)
                { lblWinner.Text = "CONGRATULATIONS \n Winner : " + _table.Players[0].Name; return; }
                if (b2 > b1 && b2 > b3 && b1 > b4)
                { lblWinner.Text = "CONGRATULATIONS \n Winner : " + _table.Players[1].Name; return; }
                if (b3 > b1 && b3 > b2 && b3 > b4)
                { lblWinner.Text = "CONGRATULATIONS \n Winner : " + _table.Players[2].Name; return; }
                if (b4 > b1 && b4 > b2 && b4 > b3)
                { lblWinner.Text = "CONGRATULATIONS \n Winner : " + _table.Players[3].Name; return; }
                else { lblWinner.Text = "UNFORTUNATELY :( \n Draw "; }
                return;
               
                
            }

            #region old methods
            //void highCardPlayer()
            //{          

            //    if (_table.Players[0].HighCard1 > _table.Players[1].HighCard1 &&
            //        _table.Players[0].HighCard1 > _table.Players[2].HighCard1 &&
            //        _table.Players[0].HighCard1 > _table.Players[3].HighCard1)
            //    { lblWinner.Text = "Congratulation \n Winner : " + _table.Players[0].Name; return; }
            //    if (_table.Players[1].HighCard1 > _table.Players[0].HighCard1 &&
            //        _table.Players[1].HighCard1 > _table.Players[2].HighCard1 &&
            //        _table.Players[1].HighCard1 > _table.Players[3].HighCard1)
            //    { lblWinner.Text = "Congratulation \n Winner : " + _table.Players[1].Name; return; }
            //    if (_table.Players[2].HighCard1 > _table.Players[0].HighCard1 &&
            //        _table.Players[2].HighCard1 > _table.Players[1].HighCard1 &&
            //        _table.Players[2].HighCard1 > _table.Players[3].HighCard1)
            //    { lblWinner.Text = "Congratulation \n Winner : " + _table.Players[2].Name; return; }
            //    if (_table.Players[3].HighCard1 > _table.Players[0].HighCard1 &&
            //        _table.Players[3].HighCard1 > _table.Players[1].HighCard1 &&
            //        _table.Players[3].HighCard1 > _table.Players[2].HighCard1)
            //    { lblWinner.Text = "Congratulation \n Winner : " + _table.Players[3].Name; return; }
            //    else CompareBestCards();

            //}
            //void CompareBestCards()
            //{
            //    if (_table.Players[0].BestCards[0].Value > _table.Players[1].BestCards[0].Value &&
            //        _table.Players[0].BestCards[0].Value > _table.Players[2].BestCards[0].Value &&
            //        _table.Players[0].BestCards[0].Value > _table.Players[3].BestCards[0].Value)
            //    { lblWinner.Text = "Congratulation \n Winner : " + _table.Players[0].Name; return; }
            //    if (_table.Players[1].BestCards[0].Value > _table.Players[0].BestCards[0].Value &&
            //        _table.Players[1].BestCards[0].Value > _table.Players[2].BestCards[0].Value &&
            //        _table.Players[1].BestCards[0].Value > _table.Players[3].BestCards[0].Value)
            //    { lblWinner.Text = "Congratulation \n Winner : "+ _table.Players[1].Name; return; }
            //    if (_table.Players[2].BestCards[0].Value > _table.Players[0].BestCards[0].Value &&
            //        _table.Players[2].BestCards[0].Value > _table.Players[1].BestCards[0].Value &&
            //        _table.Players[2].BestCards[0].Value > _table.Players[3].BestCards[0].Value)
            //    { lblWinner.Text = "Congratulation \n Winner : " + _table.Players[2].Name; return;}
            //    if (_table.Players[3].BestCards[0].Value > _table.Players[0].BestCards[0].Value &&
            //        _table.Players[3].BestCards[0].Value > _table.Players[1].BestCards[0].Value &&
            //        _table.Players[3].BestCards[0].Value > _table.Players[2].BestCards[0].Value)
            //    { lblWinner.Text = "Congratulation \n Winner : " + _table.Players[3].Name; return;}
            //}
            #endregion
        }


        #endregion
    }
}


   
       
   
