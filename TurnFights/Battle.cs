using System;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace TurnFights
{
    public partial class Battle : Form
    {
        public int Text_Speed;
        public List<Element> Elements;
        int f1MHP, f2MHP;
        Fighter fighter1, fighter2;
        bool waitingMove = true;
        int moveChosen;
        public struct FighterUi
        {
            public Label fighterName, fighterStats;
            public List<Button> moveButtons;
            public List<Label> moveInfos;
            public FighterUi(Label name, Label stats, List<Button> moves, List<Label> infos)
            {
                fighterName = name;
                fighterStats = stats;
                moveButtons = moves;
                moveInfos = infos;
            }
        }
        public List<FighterUi> fighterUis = [new(), new()];
        public Battle()
        {
            InitializeComponent();
        }
        public async void FightLoop(Fighter f1, Fighter f2)
        {
            Text_Speed = Base.Text_Speed; Elements = Base.Elements;

            fighter1 = f1;
            fighter2 = f2;
            List<Fighter> f = [fighter1, fighter2];

            f1MHP = fighter1.hp;
            f2MHP = fighter2.hp;

            fighterUis[0] = new FighterUi(fighter1_Name, fighter1_Stats, [fighter1_move1_btt, fighter1_move2_btt, fighter1_move3_btt, fighter1_move4_btt], [fighter1_move1_info, fighter1_move2_info, fighter1_move3_info, fighter1_move4_info]);
            fighterUis[1] = new FighterUi(fighter2_Name, fighter2_Stats, [fighter2_move1_btt, fighter2_move2_btt, fighter2_move3_btt, fighter2_move4_btt], [fighter2_move1_info, fighter2_move2_info, fighter2_move3_info, fighter2_move4_info]);

            fighter1TurnBox.Visible = false;
            fighter2TurnBox.Visible = false;

            for (int i = 0; i < fighterUis.Count; i++)
            {
                fighterUis[i].fighterName.Text = f[i].name;
                int k = 0;
                foreach (Button b in fighterUis[i].moveButtons)
                {
                    b.Enabled = false;
                    b.Text = f[i].skills[k].skillType.ToString();
                    k++;
                }
                for (int j = 0; j < fighterUis[i].moveInfos.Count; j++)
                {
                    switch (f[i].skills[j].skillType)
                    {
                        //check if maybe skilltype variable is useless by using skills.type
                        case Skill.SkillType.Attack:
                            Attack atk = f[i].skills[j] as Attack;
                            fighterUis[i].moveInfos[j].Text = $"{Elements[atk.element].name} attack\n{atk.dmg} dmg";
                            break;
                        case Skill.SkillType.Heal:
                            Heal h = f[i].skills[j] as Heal;
                            if (h.PercentageHealing)
                                fighterUis[i].moveInfos[j].Text = $"Heal {h.healing} % of total HP";
                            else
                                fighterUis[i].moveInfos[j].Text = $"Heal {h.healing} points of HP";
                            break;
                        case Skill.SkillType.StatChange:
                            //probably better to do this in an event based style
                            StatChanger changer = f[i].skills[j] as StatChanger;
                            if (changer.changedStat == StatChanger.Stats.DMG)
                            {

                            }
                            else if (changer.changedStat == StatChanger.Stats.DEF)
                            {

                            }
                            else //change AGI
                            {

                            }
                            break;
                        case Skill.SkillType.State:
                            break;
                        case Skill.SkillType.XAttack:
                            break;
                        case Skill.SkillType.Defense:
                            break;
                    }
                }
            }

            UpdateHPs(f1.hp, f2.hp);

            await Task.Delay(1000 / Text_Speed);
            int turn = (int)Random.Shared.NextInt64(0, 2); //0 or 1
            Fighter mover = turn == 0 ? fighter1 : fighter2;
            Fighter target = turn == 0 ? fighter2 : fighter1;
            while (true)
            {
                if (fighter1.hp <= 0) { Winner(fighter1.name, fighter2.name, 1); return; }
                if (fighter2.hp <= 0) { Winner(fighter1.name, fighter2.name, 0); return; }

                label1.Text = $"It's {mover.name}'s turn!\nChoose a move!";
                foreach (Button b in fighterUis[turn].moveButtons)
                {
                    b.Enabled = true;
                }
                if (turn == 0) fighter1TurnBox.Visible = true; else fighter2TurnBox.Visible = true;

                Debug.WriteLine(fighter2_Name.TextAlign);


                while (waitingMove) { await Task.Delay(1); }
                waitingMove = true;
                fighter1TurnBox.Visible = false; fighter2TurnBox.Visible = false;
                foreach (Button b in fighterUis[turn].moveButtons)
                {
                    b.Enabled = false;
                }
                Skill move = mover.skills[moveChosen];

                //check skill type



                if (Random.Shared.NextSingle() * 100 < target.agi) //dodge chance
                {
                    label1.Text = $"{target.name}\nhas dodged the attack!";
                    await Task.Delay(1000 / Text_Speed);
                    (mover, target) = (target, mover);
                    TurnEnd();
                    continue;
                }
                float dmgMult = 1; label1.Text = "";
                if (Elements[target.element].weaks.Contains(atk.element))
                {
                    dmgMult = 2;
                    label1.Text = "Critical hit! ";
                }
                else if (Elements[target.element].resists.Contains(atk.element))
                {
                    dmgMult = 0.5f;
                    label1.Text = "Attack resisted! ";
                }
                int damage = (int)(atk.dmg * dmgMult) - target.df;
                damage = damage < 0 ? 0 : damage;
                label1.Text += $"The attack deals {damage} dmg to\n{target.name}!";
                target.hp -= damage;
                UpdateHPs(f1.hp, f2.hp);
                await Task.Delay(1500 / Text_Speed);
                label1.Text = $"{target.name}'s\nhp is now {target.hp}";
                await Task.Delay(1500 / Text_Speed);

                TurnEnd();


                turn = turn == 0 ? 1 : 0;
                (mover, target) = (target, mover);
            }
        }
        public void CalculateDamage()
        {

        }
        public void TurnEnd() { }
        public void UpdateHPs(int hp1, int hp2)
        {
            fighter1_Stats.Text = $"HP: {fighter1.hp} / {f1MHP}\nElement: {Elements[fighter1.element].name}" +
                $"\nDef: {fighter1.df}\nAgi: {fighter1.agi}";
            fighter2_Stats.Text = $"HP: {fighter2.hp} / {f2MHP}\nElement: {Elements[fighter2.element].name}" +
                $"\nDef: {fighter2.df}\nAgi: {fighter2.agi}";
        }
        public async void Winner(string p1, string p2, int p)
        {
            //Deactivate all buttons
            for (int i = 0; i < fighterUis.Count; i++)
            {
                foreach (Button b in fighterUis[i].moveButtons)
                {
                    b.Enabled = false;
                }
            }
            if (p == 0)
            {
                label1.Text = $"{p2}'s\nfighter has fainted!";
                await Task.Delay(1000 / Text_Speed);
                label1.Text = $"{p1}\nwins!";
            }
            else
            {
                label1.Text = $"{p1}'s\nfighter has fainted!";
                await Task.Delay(1000 / Text_Speed);
                label1.Text = $"{p2}\nwins!";
            }
            await Task.Delay(1000 / Text_Speed);
            label1.Text = "What a wonderful spar,\nbut it's time to leave now";
            await Task.Delay(1000 / Text_Speed);
            label1.Text = "I hope to see you soon...";
        }

        private void Battle_Load(object sender, EventArgs e) { }
        private void fighter1_move1_btt_Click(object sender, EventArgs e) { waitingMove = false; moveChosen = 0; }
        private void fighter1_move2_btt_Click(object sender, EventArgs e) { waitingMove = false; moveChosen = 1; }
        private void fighter1_move3_btt_Click(object sender, EventArgs e) { waitingMove = false; moveChosen = 2; }
        private void fighter1_move4_btt_Click(object sender, EventArgs e) { waitingMove = false; moveChosen = 3; }
        private void fighter2_move1_btt_Click(object sender, EventArgs e) { waitingMove = false; moveChosen = 0; }
        private void fighter2_move2_btt_Click(object sender, EventArgs e) { waitingMove = false; moveChosen = 1; }
        private void fighter2_move3_btt_Click(object sender, EventArgs e) { waitingMove = false; moveChosen = 2; }
        private void fighter2_move4_btt_Click(object sender, EventArgs e) { waitingMove = false; moveChosen = 3; }
    }
}
