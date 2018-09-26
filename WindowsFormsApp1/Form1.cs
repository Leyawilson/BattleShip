using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        List<Button> playerPosition;
        List<Button> enemyPosition;
        Random rand = new Random();
        int totalShips = 3;
        int totalEnemy = 3;
        int rounds = 10;
        int playerTotalScore = 0;
        int enemyTotalScore = 0;


        public Form1()
        {
            InitializeComponent();
            loadbuttons();
            attackButton.Enabled = false;//Disable player attackk button
            enemyLocationList.Text = null;//nullify enemy location drop box

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void playerPicksPosition(object sender, EventArgs e)
        {
            //Will allow player to pick 3 positions in map
            if (totalShips > 0)
            {//check which button got clicked
                var button = (Button)sender;
                button.Enabled = false;
                button.Tag = "playerShip";
                button.BackColor = System.Drawing.Color.Blue;
                totalShips--;
            }
            if(totalShips == 0)
            {
                //player picked all 3 ships
                attackButton.Enabled = true;
                attackButton.BackColor = System.Drawing.Color.Red;
                helpText.Top = 55;
                helpText.Left = 230;
                helpText.Text = "2) Now pick an attack position from drop down";
            }
        }

        private void attackEnemyPosition(object sender, EventArgs e)
        {
            //function to allow player to move on enemy locations
            if(enemyLocationList.Text!= "")
                {
                var attackPos = enemyLocationList.Text;
                attackPos = attackPos.ToLower();
                //index of enemy location
                int index = enemyPosition.FindIndex(a => a.Name == attackPos);
                //check if there is more rounds to player
                if(enemyPosition[index].Enabled && rounds > 0)
                {
                    rounds--;
                    roundText.Text = "Rounds" + rounds;
                    //if location picked by player has enemy ship
                    if (enemyPosition[index].Tag == "enemyship")
                    {
                        enemyPosition[index].Enabled = false;
                        enemyPosition[index].BackgroundImage = Properties.Resources.fireIcon;
                        enemyPosition[index].BackColor = System.Drawing.Color.Blue;
                        playerTotalScore++;
                        playerScore.Text = "" + playerTotalScore;
                        enemyPlayTime.Start();
                    }
                    else
                    {
                        //if player took different location
                        enemyPosition[index].Enabled = false;
                        enemyPosition[index].BackgroundImage = Properties.Resources.missIcon;
                        enemyPosition[index].BackColor = System.Drawing.Color.Blue;
                        enemyPlayTime.Start();
                    }

                }
            }
            else
            {
                //Alert player to pick location.
                MessageBox.Show("Choose a location from drop down list");
            }
        }

        private void enemyAttackPlayer(object sender, EventArgs e)
        {
            //to make move for player by cpu.
            //if player positon is >0 and more rounds to play
            if(playerPosition.Count > 0 && rounds > 0)
            {
                rounds--;
                roundText.Text = "Rounds" + rounds;
                int index = rand.Next(playerPosition.Count);
                if(playerPosition[index].Tag == "playerShip")
                {
                    playerPosition[index].BackgroundImage = Properties.Resources.fireIcon;
                    enemyMoves.Text = "" + playerPosition[index].Text;
                    playerPosition[index].Enabled = false;
                    playerPosition[index].BackColor = System.Drawing.Color.Blue;
                    playerPosition.RemoveAt(index);
                    enemyTotalScore++;
                    eEnemyScore.Text = "" + enemyTotalScore;
                    enemyPlayTime.Stop();
                 }
                else
                {
                    //if player tag isnt palyer ship
                    playerPosition[index].BackgroundImage = Properties.Resources.missIcon;
                    enemyMoves.Text = "" + playerPosition[index].Text;
                    playerPosition[index].Enabled = false;
                    playerPosition[index].BackColor = System.Drawing.Color.Blue;
                    playerPosition.RemoveAt(index);
                    enemyPlayTime.Stop();

                }
            }
            //check if we won/draw/lost
            if(rounds < 1 || playerTotalScore > 2 || enemyTotalScore > 2)
            {
                if( playerTotalScore > enemyTotalScore)
                {
                    MessageBox.Show(" Player Win!!!");
                }
                if(playerTotalScore == enemyTotalScore)
                {
                    MessageBox.Show("No one wins, it's a Draw");
                }
                if(enemyTotalScore > playerTotalScore)
                {
                    MessageBox.Show(" You lost, Better luck next time !!");
                }
            }
        }

        private void enemyPickPositions(object sender, EventArgs e)
        {

            //allow cpu to pick 3 position on map
            int index = rand.Next(enemyPosition.Count);
            if(enemyPosition[index].Enabled == true && enemyPosition[index].Tag == null)
            {
                enemyPosition[index].Tag = "enemyShip";
                totalEnemy--;
                Debug.WriteLine("Enemy Position " + enemyPosition[index].Text);
            }
            else
            {
                index = rand.Next(enemyPosition.Count);
            }
            if(totalEnemy < 1)
            {
                enemyPostitionPicker.Stop();
            }
        }
        private void loadbuttons()
        {
            playerPosition = new List<Button> { w1, w2, w3, w4, x1, x2, x3, x4, y1, y2, y3, y4, z1, z2, z3, z4 };
            enemyPosition = new List<Button> { a1,a2,a3,a4,b1,b2,b3,b4,c1,c2,c3,c4,d1,d2,d3,d4};
            //Add enemy location list to player
            for(int i = 0;i<enemyPosition.Count;i++)
            {
                enemyPosition[i].Tag = null;
                enemyLocationList.Items.Add(enemyPosition[i].Text);
            }

        }
    }
}
