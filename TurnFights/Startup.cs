using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;
using static TurnFights.Battle;
namespace TurnFights
{
    public partial class Startup : Form
    {
        int Text_Speed;
        public static Startup Instance;
        public Startup()
        {
            Instance = this;
            InitializeComponent();
        }
        private async void Form1_Load(object sender, EventArgs e)
        {
            Text_Speed = Base.Text_Speed;
            label1.Text = "Welcome to the fighter arena!";
            await Task.Delay(2000 / Text_Speed);
            label1.Text = "Player 1, what must I call?";
            textBox1.Visible = textBox1.Enabled = true;
            textBox1.Text = "_DEBUG1";
        }
        string p1 = "", p2 = "";
        bool _debug1, _debug2;
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (p1 == "")
                {
                    p1 = textBox1.Text;
                    if (p1 == "_DEBUG1") _debug1 = true;
                    label1.Text = "And player 2, what about you?";
                    textBox1.Text = "Player2";
                }
                else
                {
                    p2 = textBox1.Text;
                    if (p2 == "_DEBUG2") _debug2 = true;
                    else if (p2 == p1) p2 += "2";
                    textBox1.Visible = textBox1.Enabled = false;
                    Intro();
                }
                e.Handled = true;
            }
        }
        async void Intro()
        {
            label1.Text = "Perfect, it's time to create your fighters...";
            await Task.Delay(2000 / Text_Speed);
            Fighter fighter1 = new();
            if (_debug1)
            {
                DebugFighterCreator creator = new DebugFighterCreator();
                creator.Show();
                creator.GenerateFighter();
            }
            else
                CreateFighter(p1);
        }
        Fighter fighter1, fighter2;
        bool fighter1Done = false;
        public async void FighterDone(Fighter fighter)
        {
            if (!fighter1Done)
            {
                fighter1Done = true;
                fighter1 = fighter;
                if (_debug2)
                {
                    DebugFighterCreator creator = new DebugFighterCreator();
                    creator.Show();
                    creator.GenerateFighter();
                }
                else
                    CreateFighter(p2);
            }
            else
            {
                fighter2 = fighter;
                label1.Text = "Very well, the fighters are ready...";
                await Task.Delay(1000 / Text_Speed);
                label1.Text = "Well now...";
                await Task.Delay(3000 / Text_Speed);
                Battle bat = new Battle();
                bat.Show();
                bat.PrepareFight(fighter1, fighter2);
                this.Hide();
            }
        }
        void CreateFighter(string name)
        {
            int MoveN = Base.MoveN; List<Element> Elements = Base.Elements;
            int main_elm = (int)Random.Shared.NextInt64(0, Elements.Count);
            int hp = (int)(Random.Shared.NextInt64(Base.min_hp, Base.max_hp) * Elements[main_elm].mod_hp);
            int df = (int)(Random.Shared.NextInt64(Base.min_def, Base.max_def) * Elements[main_elm].mod_df);
            int agi = (int)(Random.Shared.NextInt64(Base.min_agi, Base.max_agi) * Elements[main_elm].mod_agi);

            Skill[] moves = new Skill[MoveN];
            for (int i = 0; i < MoveN; i++)
            {
                int num = (int)Random.Shared.NextInt64(3, 6);
                num = i < 3 ? i : num; //so basically the first 3 moves are always attack, heal, defense
                if (num == 0 || num == 3) //generate attack
                {
                    int elm;
                    List<int> banned = new(Elements[main_elm].weaks);
                    banned.Add(main_elm); //added bc main element is already account for in the following chance
                    if (Random.Shared.NextSingle() < 0.6f) //60% chance for attack to be of main element
                        elm = main_elm;
                    else
                    {
                        elm = (int)Random.Shared.NextInt64(0, Elements.Count);
                        while (banned.Contains(elm)) { elm = (int)Random.Shared.NextInt64(0, Elements.Count); }
                    }
                    int dmg = (int)(Random.Shared.NextInt64(Base.min_dmg, Base.max_dmg) * Elements[main_elm].mod_dmg);
                    moves[i] = new Attack(dmg, elm);
                }
                else if (num == 1) //generate healing
                {
                    if (Random.Shared.NextInt64(0, 2) == 0) //normal heal
                    {
                        int heal = (int)Random.Shared.NextInt64(Base.min_heal, Base.max_heal);
                        moves[i] = new Heal(heal, false);
                    }
                    else //percentage healing
                    {
                        int heal = (int)Random.Shared.NextInt64(Base.min_percHeal, Base.max_percHeal);
                        moves[i] = new Heal(heal, true);
                    }
                }
                else if (num == 2) { moves[i] = new Defense(); } //generate defense
                else if (num == 4) //generate stat changer
                {
                    List<StatChanger> changers = [StatChanger.DmgUp, StatChanger.DmgDown, StatChanger.DefUp, StatChanger.DefDown,
                    StatChanger.AgiUp, StatChanger.AgiDown];
                    moves[i] = new StatChanger(changers[(int)Random.Shared.NextInt64(0, changers.Count)]);
                }
                else if (num == 5) //generate state
                {
                    List<State> states = [State.Poison, State.Spikes, State.Overcharged];
                    moves[i] = new State(states[(int)Random.Shared.NextInt64(0, states.Count)]);
                }
                else
                {
                    Debug.WriteLine("An unexpected error happened while generating the skills: Skill index was out of range");
                }
                if (Random.Shared.NextSingle() < 0.05f) //5% chance of replacing last skill with an XAttack
                {
                    //commented out for now because there is no XAttack functionality (thats a whole new hell, finish this first)
                    //moves[moves.Length - 1] = new XAttack();
                }
            }

            FighterDone(new Fighter(name, main_elm, hp, df, agi, moves));
        }
    }
}