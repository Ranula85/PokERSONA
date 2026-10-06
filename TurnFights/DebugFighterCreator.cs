using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace TurnFights
{
    public partial class DebugFighterCreator : Form
    {
        public DebugFighterCreator()
        {
            InitializeComponent();
        }
        private async void DebugFighterCreator_Load(object sender, EventArgs e) { }
        bool waiting = true;
        public Fighter fighter;
        public async void GenerateFighter()
        {
            //Choose name
            terminal.Text = "Enter a name for the fighter";
            while (waiting) { await Task.Delay(1); }
            waiting = true;
            string name = input.Text;
            //Choose main element
            input.Text = "";
            terminal.Text = "Select a main element from the list:\n" +
                "0: Fire\n1: Ice\n2: Wind\n3: Rock\nr: random";
            while (waiting) { await Task.Delay(1); }
            waiting = true;
            terminal.Text = "";
            int mainElm = 0;
            if (input.Text == "r" || !int.TryParse(input.Text, out mainElm) || int.Parse(input.Text) >= Base.Elements.Count)
            {
                mainElm = (int)Random.Shared.NextInt64(0, Base.Elements.Count);
                terminal.Text = "Generated random main element (by choice or by invalid input)\n";
            }
            //Choose max hp
            input.Text = "";
            terminal.Text += "Input a max hp (or 'r' for a random amount):";
            while (waiting) { await Task.Delay(1); }
            waiting = true;
            terminal.Text = "";
            int hp = 0;
            if (input.Text == "r" || !int.TryParse(input.Text, out hp))
            {
                hp = (int)Random.Shared.NextInt64(Base.min_hp, Base.max_hp);
                terminal.Text = "Generated random HP (by choice or by invalid input)\n";
            }
            //Choose def
            input.Text = "";
            terminal.Text += "Input a def stat (or 'r' for a random amount):";
            while (waiting) { await Task.Delay(1); }
            waiting = true;
            terminal.Text = "";
            int def = 0;
            if (input.Text == "r" || !int.TryParse(input.Text, out def))
            {
                def = (int)Random.Shared.NextInt64(Base.min_def, Base.max_def);
                terminal.Text = "Generated random def (by choice or by invalid input)\n";
            }
            //Choose agi
            input.Text = "";
            terminal.Text += "Input an agi stat (or 'r' for a random amount):";
            while (waiting) { await Task.Delay(1); }
            waiting = true;
            terminal.Text = "";
            int agi = 0;
            if (input.Text == "r" || !int.TryParse(input.Text, out agi))
            {
                agi = (int)Random.Shared.NextInt64(Base.min_agi, Base.max_agi);
                terminal.Text = "Generated random agi (by choice or by invalid input)\n";
            }
            //Do all the choose moves part (this sucks)
            Skill[] moves = new Skill[Base.MoveN];
            terminal.Text += "Warning: From now on an invalid input will generate a default move list\n";
            for (int i = 0; i < Base.MoveN; i++)
            {
                input.Text = "";
                terminal.Text += "Select a move type from the list:\n" +
                "0: Attack\n1: Heal\n2: Defense\n3: Stat Changer\n4: Status\n5: XAttack\n";
                while (waiting) { await Task.Delay(1); }
                waiting = true;
                terminal.Text = "";
                try
                {
                    int num = int.Parse(input.Text);
                    if (num == 0) //attack
                    {
                        input.Text = "";
                        terminal.Text = "Input a line formatted as: [int damage] [int element]\n" +
                        "Element choices are:\n0: Fire\n1: Ice\n2: Wind\n3: Rock";
                        while (waiting) { await Task.Delay(1); }
                        waiting = true;
                        int[] ints = Array.ConvertAll(input.Text.Split(' '), int.Parse);
                        if (ints[1] > Base.Elements.Count - 1 || ints[1] < 0)
                        {
                            int.Parse("hello");
                        }
                        moves[i] = new Attack(ints[0], ints[1]);
                    }
                    else if (num == 1) //healing
                    {
                        input.Text = "";
                        terminal.Text = "Input a line formatted as: [int healing] [bool percentageHealing]\n";
                        while (waiting) { await Task.Delay(1); }
                        waiting = true;
                        string[] ints = input.Text.Split(' ');
                        moves[i] = new Heal(int.Parse(ints[0]), bool.Parse(ints[1]));
                    }
                    else if (num == 2) { moves[i] = new Defense(); } //generate defense
                    else if (num == 3) //generate stat changer
                    {
                        input.Text = "";
                        terminal.Text = "Input a line formatted as: [int stat] [float mod] [bool applyToSelf] [int turns]\n" +
                            "Stat choices are:\n0: DMG\n1: DEF\n2: AGI";
                        while (waiting) { await Task.Delay(1); }
                        waiting = true;
                        string[] ints = input.Text.Split(' ');
                        if (int.Parse(ints[0]) > 2 || int.Parse(ints[0]) < 0)
                        {
                            int.Parse("hello");
                        }
                        moves[i] = new StatChanger((StatChanger.Stats)int.Parse(ints[0]), float.Parse(ints[1]),
                            bool.Parse(ints[2]), int.Parse(ints[3]));
                    }
                    else if (num == 4) //generate state
                    {
                        input.Text = "";
                        terminal.Text = "Select a status from the list:\n" +
                            "0: Poison\n1: Spikes\n2: Overcharged";
                        while (waiting) { await Task.Delay(1); }
                        waiting = true;
                        List<State> states = [State.Poison, State.Spikes, State.Overcharged];
                        moves[i] = new State(states[int.Parse(input.Text)]);
                    }
                    else if (num == 5)
                    {
                        //select from list of XAttacks i guess? idk how ill do that
                        moves[i] = new Attack(0, 0); //for now
                    }
                    terminal.Text = "";
                }
                catch
                {
                    terminal.Text = "Invalid input, generating default attack list\n";
                    moves = [new Attack(10, 0), new Heal(20, false), new Defense(), new State(State.Poison)];
                    break;
                }
            }
            Startup.Instance.FighterDone(new(name, mainElm, hp, def, agi, moves));
            this.Close();
        }
        private void input_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                waiting = false;
                e.Handled = true;
            }
        }
    }
}