using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace TurnFights
{
    /*I should probably comment the functions
     * Add a debug command box to apply states, stat changers, and other utilities for debugging
     * PROGRESS:
     * 
     * Make the moves menu compatible with different N of moves -> HIGHEST PRIORITY
     * 
     * Implement XAttacks
     * Add QtE
     * Add physical attacks that do crits by chance
     * Add more specialization for each element
     * Add new stats (maybe)
     * Add title screen (maybe)
     * Add campaign mode (maybe)
     */
    public static class Base
    {
        public static int MoveN = 4, Text_Speed = 1,
            min_hp = 120, max_hp = 250, min_dmg = 10, max_dmg = 20,
            min_def = 0, max_def = 10, min_agi = 0, max_agi = 10,
            min_heal = 25, max_heal = 75, min_percHeal = 10, max_percHeal = 60;

        public static List<Element> Elements =
        [
            //       Name  Weaks  Resists HPmod  DMGmod  DEFmod  AGImod
            new Element("Fire", [2,7], [0], 0.8f, 1.2f, 0.5f, 1),     //0
            new Element("Ice",  [0,3], [1], 0.6f, 2,    0.5f, 0.5f),  //1
            new Element("Wind", [2],    [], 1.1f, 0.9f, 0.8f, 1.5f),  //2
            new Element("Rock", [4], [3,5], 1.8f, 0.5f, 1.5f, 0.3f),  //3
            new Element("Plant",[0],   [3], 1.2f, 0.7f, 1,    1),     //4
            new Element("Shock",[6],   [5], 1,    1.2f, 1,    1.3f),  //5
            new Element("Light",[7], [5,6], 1,    0.7f, 1.1f, 1),     //6
            new Element("Dark",[6,5],[0,7], 0.9f, 1.2f, 0.8f, 1)      //7
        ];
    }
    public class Element
    {
        public string name;
        public List<int> weaks, resists;
        public float mod_hp, mod_dmg, mod_df, mod_agi;
        public Element(string n, List<int> weks, List<int> resis, float modHP, float modDMG, float modDF, float modAGI)
        {
            name = n;
            weaks = new(weks);
            resists = new(resis);
            mod_hp = modHP;
            mod_dmg = modDMG;
            mod_df = modDF;
            mod_agi = modAGI;
        }
    }
    public class Skill
    {
        public enum SkillType { Attack, Heal, StatChange, State, XAttack, Defense }
        public SkillType skillType;
        public Skill() { }
    }
    public class Attack : Skill
    {
        public int dmg, element;
        public Attack(int dg, int elm) { skillType = SkillType.Attack; dmg = dg; element = elm; }
    }
    public class Heal : Skill
    {
        public bool PercentageHealing;
        public int healing;
        public Heal(int heal, bool percent) { skillType = SkillType.Heal;  healing = heal; PercentageHealing = percent; }
    }
    public class Defense : Skill
    {
        public static float ResistancePercent = 0.5f;
        public Defense() { skillType = SkillType.Defense; }
    }
    public class StatChanger : Skill
    {   
        public enum Stats { DMG, DEF, AGI }
        public Stats changedStat;
        public float modifier; //this being bigger or smaller than 0 determines whether it is a self buff or a debuff for opponent
        public int Duration;  //how long the state lasts
        int _elapsed;
        public StatChanger(StatChanger changer) {
            skillType = SkillType.StatChange; changedStat = changer.changedStat; modifier = changer.modifier; Duration = changer.Duration; }
        public StatChanger(Stats change, float mod, int dur) {
            skillType = SkillType.StatChange;  changedStat = change; modifier = mod; Duration = dur; }
        public static StatChanger
            DmgUp = new StatChanger(Stats.DMG, 1.5f, 3),
            DmgDown = new StatChanger(Stats.DMG, 0.5f, 3),
            DefUp = new StatChanger(Stats.DEF, 1.5f, 3),
            DefDown = new StatChanger(Stats.DEF, 0.5f, 3),
            AgiUp = new StatChanger(Stats.AGI, 1.5f, 3),
            AgiDown = new StatChanger(Stats.AGI, 0.5f, 3);
    }
    public class State : Skill
    {
        public string stateName;
        public bool ToSelf;
        public int Duration;  //how long the state lasts
        int _elapsed;
        public State(string nam, int dur, bool self) { skillType = SkillType.State; stateName = nam; Duration = dur; ToSelf = self; }
        public State(State state) { skillType = SkillType.State; stateName = state.stateName; Duration = state.Duration; ToSelf = state.ToSelf; }
        public static State
            Poison = new State("Poison", 3, false),
            Spikes = new State("Spikes", 3, true),
            Overcharged = new State("Overcharged", 2, true); //has to account for the turn where the effect is applied
    }
    public class XAttack : Skill
    {
        //dear god not yet
    }
    public class Fighter
    {
        public string name;
        public int element;
        public int maxHp, df, agi, hp;
        public Dictionary<StatChanger.Stats, float> statMods = new Dictionary<StatChanger.Stats, float>() {
            { StatChanger.Stats.DMG, 1 }, { StatChanger.Stats.DEF, 1 }, 
            { StatChanger.Stats.AGI, 1 } }; //this will store the sum of all active statchangers (the general modifier)
        public Skill[] skills;
        public Fighter(string nam, int element, int hpp, int dff, int ag, Skill[] skilll)
        {
            name = nam;
            this.element = element;
            maxHp = hp = hpp;
            df = dff;
            agi = ag;
            skills = skilll;
        }
        public Status status = new();
        public struct Status
        {
            public Dictionary<string, (bool, int)> StateFlags = new Dictionary<string, (bool, int)>() { //state, (active, turns left)
                { "Defending", (false, 0) },
                { "Poison", (false, 0) },
                { "Spikes", (false, 0) },
                { "Overcharged", (false, 0) } };
            public List<(StatChanger.Stats, float, int)> currentStatFlags = new List<(StatChanger.Stats, float, int)>(); //stat, mod, turns left
            public Status() { }
        }
        public UiInfo Info;
        public struct UiInfo
        {
            public Dictionary<string, Label> StateTxts;
            public Dictionary<StatChanger.Stats, Label[]> StatTxts; //stat, up // down
            public Label Defending;
        }
        public Fighter() { }
    }
}