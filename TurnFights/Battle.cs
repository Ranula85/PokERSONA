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
            fighter1.Info.StatTxts = new Dictionary<StatChanger.Stats, Label[]>() {
                { StatChanger.Stats.DMG, new Label[] { F1DmgUp, F1DmgDown } },
                { StatChanger.Stats.DEF, new Label[] { F1DefUp, F1DefDown } },
                { StatChanger.Stats.AGI, new Label[] { F1AgiUp, F1AgiDown } }
            };
            fighter2.Info.StatTxts = new Dictionary<StatChanger.Stats, Label[]>() {
                { StatChanger.Stats.DMG, new Label[] { F2DmgUp, F2DmgDown } },
                { StatChanger.Stats.DEF, new Label[] { F2DefUp, F2DefDown } },
                { StatChanger.Stats.AGI, new Label[] { F2AgiUp, F2AgiDown } }
            };
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
                            if (changer.modifier > 1) //makes it a self buff
                                fighterUis[i].moveInfos[j].Text = $"Increase self's {changer.changedStat}";
                            else
                                fighterUis[i].moveInfos[j].Text = $"Decrease the opponent's {changer.changedStat}";
                            break;
                        case Skill.SkillType.State:
                            State st = f[i].skills[j] as State;
                            if (st.ToSelf)
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

                label1.Text = $"It's {mover.name}'s turn!\nChoose a move!";
                foreach (Button b in fighterUis[turn].moveButtons) //enable move buttons
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
                    mover.status.StateFlags["Defending"] = (true, 2);
                    label1.Text = $"{mover.name}\nstarts defending!";
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
                    UpdateHPs();
                    await Task.Delay(1000 / Text_Speed);
                }
                else if (move is State)
                {
                    State s = move as State;
                    if (s.ToSelf)
                    {
                        label1.Text = $"{mover.name}\nhas inflicted themselves with {s.stateName}!";
                        mover.status.StateFlags[s.stateName] = (true, s.Duration);
                        mover.Info.StateTxts[s.stateName].Visible = true;
                    }
                    else
                    {
                        label1.Text = $"{mover.name}\nhas afflicted {target.name} with {s.stateName}!";
                        target.status.StateFlags[s.stateName] = (true, s.Duration);
                        target.Info.StateTxts[s.stateName].Visible = true;
                    }
                    await Task.Delay(1000 / Text_Speed);
                }
                else if (move is StatChanger)
                {
                    //i was here
                    StatChanger s = move as StatChanger;
                    if (s.modifier > 1) //self buff
                    {
                        mover.status.currentStatFlags.Add((s.changedStat, s.modifier, s.Duration));
                        mover.statMods[s.changedStat] *= s.modifier;
                        label1.Text = $"{mover.name}\nhas increased their {s.changedStat}!";
                        mover.Info.StatTxts[s.changedStat][0].Visible = true;
                    }
                    else
                    {
                        target.status.currentStatFlags.Add((s.changedStat, s.modifier, s.Duration));
                        target.statMods[s.changedStat] *= s.modifier;
                        label1.Text = $"{mover.name}\nhas decreased {target.name}'s {s.changedStat}!";
                        target.Info.StatTxts[s.changedStat][1].Visible = true;
                    }
                    await Task.Delay(1000 / Text_Speed);
                }
                else if (move is Attack)
                {
                    Attack atk = move as Attack;
                    if (Random.Shared.NextSingle() * 100 < target.agi * target.statMods[StatChanger.Stats.AGI]) //dodge chance
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
                        int rawDmg = (int)(atk.dmg * mover.statMods[StatChanger.Stats.DMG] * dmgMult);
                        int targetDef = (int)(target.df * target.statMods[StatChanger.Stats.DEF]);
                        int damage = rawDmg - targetDef;

                        if (target.status.StateFlags["Defending"].Item1) { damage /= 2; label1.Text += $"{target.name}\nis defending! The attack is resisted"; }
                        if (target.status.StateFlags["Overcharged"].Item1) { damage *= 2; label1.Text += $"{target.name}\nis overcharged! The attack is stronger"; }
                        if (mover.status.StateFlags["Overcharged"].Item1) { damage *= 2; label1.Text += $"{mover.name}\nis overcharged! The attack is stronger"; }

                        damage = damage < 0 ? 0 : damage;
                        label1.Text += $"The attack deals {damage} dmg to\n{target.name}!";
                        Damage(target, damage);
                        await Task.Delay(1500 / Text_Speed);
                        label1.Text = $"{target.name}'s\nhp is now {target.hp}";

                        if (target.status.StateFlags["Spikes"].Item1)
                        {
                            Damage(mover, 15); label1.Text = $"{mover.name}\nis hurt by spikes!"; await Task.Delay(1000 / Text_Speed);
                        }

                        await Task.Delay(1500 / Text_Speed);
                    }
                }

                //CHECK TURN END STUFF
                if (mover.status.StateFlags["Poison"].Item1)
                {
                    Damage(mover, 10); label1.Text = $"{mover.name}\nis poisoned!"; await Task.Delay(1000 / Text_Speed);
                }

                int i = 0;
                foreach (var flag in mover.status.StateFlags)
                {
                    if (flag.Key == "Defending") continue; //this is handled separately
                    if (!flag.Value.Item1) continue; //if effect isnt active, continue
                    i = flag.Value.Item2 - 1;
                    mover.status.StateFlags[flag.Key] = (true, i);
                    if (i <= 0)
                    {
                        mover.status.StateFlags[flag.Key] = (false, 0);
                        label1.Text = $"{mover.name}\nis no longer affected by {flag.Key}!"; await Task.Delay(1000 / Text_Speed);
                        mover.Info.StateTxts[flag.Key].Visible = false;
                    }
                }
                if (target.status.StateFlags["Defending"].Item1)
                {
                    target.status.StateFlags["Defending"] = (false, 0);
                    target.Info.Defending.Visible = false;
                }
                for (int j = 0; j < mover.status.currentStatFlags.Count; j++)
                {
                    var statMod = mover.status.currentStatFlags[j];
                    mover.status.currentStatFlags[j] = (statMod.Item1, statMod.Item2, statMod.Item3 - 1);
                    if (mover.status.currentStatFlags[j].Item3 <= 0)
                    {
                        mover.statMods[statMod.Item1] /= statMod.Item2;
                        string modtype = statMod.Item2 < 1 ? "debuff" : "buff";
                        label1.Text = $"{mover.name}\n's {statMod.Item1} {modtype}'s effect has ended!";
                        mover.Info.StatTxts[statMod.Item1][0].Visible = false;
                        await Task.Delay(1000 / Text_Speed);
                        mover.status.currentStatFlags.RemoveAt(j);
                        j--; //this compensates for the removal of one element of the list, the next one will now be in this j position
                    }
                }
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

        private void button1_Click(object sender, EventArgs e)
        {
            HelpInfo helpMenu = new HelpInfo();
            helpMenu.Show();
        }
    }
}
