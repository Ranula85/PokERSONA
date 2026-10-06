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
        public int turnN;
        public delegate void TurnEvents(Fighter mover, Fighter target);
        public static event TurnEvents TurnEnd = (mover, target) => { }, DealDamage = (mover, target) => { };
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
        public static Battle Instance;
        public Battle()
        {
            Instance = this;
            InitializeComponent();
        }
        public async void PrepareFight(Fighter f1, Fighter f2)
        {
            Text_Speed = Base.Text_Speed; Elements = Base.Elements;

            fighter1 = f1;
            fighter2 = f2;
            List<Fighter> f = [fighter1, fighter2];

            fighter1.Info.StateTxts = new Dictionary<string, Label>() { 
                { "Poison", F1Poison }, { "Spikes", F1Spikes }, { "Overcharged", F1Overcharged }};
            fighter2.Info.StateTxts = new Dictionary<string, Label>() {
                { "Poison", F2Poison }, { "Spikes", F2Spikes }, { "Overcharged", F2Overcharged }};
            fighter1.Info.DmgTxt = new Label[] { F1DmgUp, F1DmgDown };
            fighter1.Info.DefTxt = new Label[] { F1DefUp, F1DefDown };
            fighter1.Info.AgiTxt = new Label[] { F1AgiUp, F1AgiDown };
            fighter2.Info.DmgTxt = new Label[] { F2DmgUp, F2DmgDown };
            fighter2.Info.DefTxt = new Label[] { F2DefUp, F2DefDown };
            fighter2.Info.AgiTxt = new Label[] { F2AgiUp, F2AgiDown };
            fighter1.Info.Defending = F1Defending;
            fighter2.Info.Defending = F2Defending;

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
                            StatChanger changer = f[i].skills[j] as StatChanger;
                            if (changer.applyToSelf)
                                fighterUis[i].moveInfos[j].Text = $"Increase self's {changer.changedStat.ToString()}";
                            else
                                fighterUis[i].moveInfos[j].Text = $"Decrease the opponent's {changer.changedStat.ToString()}";
                            break;
                        case Skill.SkillType.State:
                            State st = f[i].skills[j] as State;
                            if (st.afflictSelf)
                                fighterUis[i].moveInfos[j].Text = $"Inflicts {st.stateName} to oneself";
                            else
                                fighterUis[i].moveInfos[j].Text = $"Inflicts {st.stateName} to the opponent";
                            break;
                        case Skill.SkillType.XAttack:
                            //something fancy
                            break;
                        case Skill.SkillType.Defense:
                            Defense d = f[i].skills[j] as Defense;
                            fighterUis[i].moveInfos[j].Text = $"Reduce damage taken for one turn";
                            break;
                    }
                }
            }
            UpdateHPs();
            await Task.Delay(1000 / Text_Speed);
            FightLoop(f1, f2);
        }
        public async void FightLoop(Fighter f1, Fighter f2)
        {
            int turn = (int)Random.Shared.NextInt64(0, 2); //0 or 1
            Fighter mover = turn == 0 ? fighter1 : fighter2;
            Fighter target = turn == 0 ? fighter2 : fighter1;
            while (true)
            {
                if (fighter1.hp <= 0) { Winner(fighter1.name, fighter2.name, 1); return; }
                if (fighter2.hp <= 0) { Winner(fighter1.name, fighter2.name, 0); return; }

                //quick test
                if (fighter1.modDmg > 1.5f) F1DmgUp.Visible = true;

                label1.Text = $"It's {mover.name}'s turn!\nChoose a move!";
                foreach (Button b in fighterUis[turn].moveButtons)
                {
                    b.Enabled = true;
                }
                if (turn == 0) fighter1TurnBox.Visible = true; else fighter2TurnBox.Visible = true;

                while (waitingMove) { await Task.Delay(1); }
                waitingMove = true;
                fighter1TurnBox.Visible = false; fighter2TurnBox.Visible = false;
                foreach (Button b in fighterUis[turn].moveButtons)
                {
                    b.Enabled = false;
                }

                Skill move = mover.skills[moveChosen];
                if (move is Defense)
                {
                    mover.df = (int)(mover.df * 1.5f);
                    TurnEvents handler = (mover1, _) => { if (mover1.name == mover.name){
                            mover.df = (int)(mover.df / 1.5f); mover.Info.Defending.Visible = false; } };
                    handler += (mover1, _) => { if (mover1.name == mover.name) TurnEnd -= handler; };
                    TurnEnd += handler;
                    label1.Text = $"{mover.name}\nis defending!";
                    mover.Info.Defending.Visible = true;
                    await Task.Delay(1000 / Text_Speed);
                }
                else if (move is Heal)
                {
                    Heal h = move as Heal;
                    if (h.PercentageHealing)
                    {
                        mover.hp += (int)(mover.maxHp * h.healing / 100f);
                        label1.Text = $"{mover.name}\nhas healed {(int)(mover.hp * h.healing / 100f)} HP!";
                    }
                    else
                    {
                        mover.hp += h.healing;
                        label1.Text = $"{mover.name}\nhas healed {h.healing} HP!";
                    }
                    mover.hp = mover.hp > mover.maxHp ? mover.maxHp : mover.hp;
                    await Task.Delay(1000 / Text_Speed);
                }
                else if (move is State)
                {
                    State s = move as State;
                    if (s.afflictSelf)
                    {
                        s.ApplyState(mover);
                        label1.Text = $"{mover.name}\nhas inflicted themselves with {s.stateName}!";
                    }
                    else
                    {
                        s.ApplyState(target);
                        label1.Text = $"{mover.name}\nhas afflicted {target.name} with {s.stateName}!";
                    }
                    await Task.Delay(1000 / Text_Speed);
                }
                else if (move is StatChanger)
                {
                    StatChanger s = move as StatChanger;
                    if (s.applyToSelf)
                    {
                        s.ApplyChanger(mover);
                        label1.Text = $"{mover.name}\nhas increased their {s.changedStat}!";
                    }
                    else
                    {
                        s.ApplyChanger(target);
                        label1.Text = $"{mover.name}\nhas decreased {fighter2.name}'s {s.changedStat}!";
                    }
                    await Task.Delay(1000 / Text_Speed);
                }
                else if (move is Attack)
                {
                    Attack atk = move as Attack;
                    if (Random.Shared.NextSingle() * 100 < target.agi) //dodge chance
                    {
                        label1.Text = $"{target.name}\nhas dodged the attack!";
                        await Task.Delay(1000 / Text_Speed);
                    }
                    else
                    {
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

                        DealDamage(mover, target);

                        int damage = (int)(atk.dmg * mover.modDmg * dmgMult) - target.df;
                        damage = damage < 0 ? 0 : damage;
                        label1.Text += $"The attack deals {damage} dmg to\n{target.name}!";
                        Damage(target, damage);
                        await Task.Delay(1500 / Text_Speed);
                        label1.Text = $"{target.name}'s\nhp is now {target.hp}";
                        await Task.Delay(1500 / Text_Speed);
                    }
                }

                TurnEnd(mover, target); //any special events will take place here

                turn = turn == 0 ? 1 : 0;
                (mover, target) = (target, mover);
            }
        }
        public void Damage(Fighter target, int damage) //use this to deal the damages
        {
            if (target.name == fighter1.name)
                fighter1.hp -= damage;
            else
                fighter2.hp -= damage;
            UpdateHPs();
        }
        public void UpdateHPs()
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
