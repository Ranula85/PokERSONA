using System.Runtime.InteropServices;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
namespace TurnFights
{
    public partial class Startup : Form
    {
        int Text_Speed;
        public Startup()
        {
            InitializeComponent();
        }
        private async void Form1_Load(object sender, EventArgs e)
        {
            Text_Speed = Base.Text_Speed;
            label1.Text = "Welcome to the fighter arena!";
            await Task.Delay(2000 / Text_Speed);
            label1.Text = "Player 1, what must I call?";
            textBox1.Visible = textBox1.Enabled = true;
            textBox1.Text = "Player1";
            button1.Visible = button1.Enabled = true;
        }
        string p1 = "", p2 = "";
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private async void button1_Click(object sender, EventArgs e)
        {
            if (p1 == "")
            {
                p1 = textBox1.Text;
                label1.Text = "And player 2, what about you?";
                textBox1.Text = "Player2";
            }
            else
            {
                p2 = textBox1.Text;
                button1.Visible = button1.Enabled = false;
                textBox1.Visible = textBox1.Enabled = false;
                Intro();
            }
        }
        async void Intro()
        {
            label1.Text = "Perfect, it's time to create your fighters...";
            await Task.Delay(2000 / Text_Speed);
            Fighter fighter1 = CreateFighter(p1);
            Fighter fighter2 = CreateFighter(p2);
            label1.Text = "Very well, the fighters are ready...";
            await Task.Delay(1000 / Text_Speed);
            label1.Text = "Well now...";
            await Task.Delay(3000 / Text_Speed);
            Battle bat = new Battle();
            bat.Show();
            bat.FightLoop(fighter1, fighter2);
            this.Hide();
        }
        Fighter CreateFighter(string name)
        {
            int MoveN = Base.MoveN; List<Element> Elements = Base.Elements;
            Attack[] atts = new Attack[MoveN];
            int main_elm = (int)Random.Shared.NextInt64(0, Elements.Count);
            for (int i = 0; i < MoveN; i++)
            {
                int elm;
                List<int> banned = Elements[main_elm].weaks;
                banned.Add(main_elm); //added bc main element is already account for in the following chance
                if (Random.Shared.NextSingle() < 0.8f) //85% chance for attack to be of main element
                    elm = main_elm;
                else
                {
                    elm = (int)Random.Shared.NextInt64(0, Elements.Count);
                    while (banned.Contains(elm)) { elm = (int)Random.Shared.NextInt64(0, Elements.Count); }
                }
                int dmg = (int)(Random.Shared.NextInt64(Base.min_dmg, Base.max_dmg) * Elements[main_elm].mod_dmg);
                atts[i] = new Attack(dmg, elm);
            }
            int hp = (int)(Random.Shared.NextInt64(Base.min_hp, Base.max_hp) * Elements[main_elm].mod_hp);
            int df = (int)(Random.Shared.NextInt64(Base.min_def, Base.max_def) * Elements[main_elm].mod_df);
            int agi = (int)(Random.Shared.NextInt64(Base.min_agi, Base.max_agi) * Elements[main_elm].mod_agi);

            return new Fighter(name, main_elm, hp, df, agi, atts);
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}