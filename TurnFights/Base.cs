using System;
using System.Collections.Generic;
using System.Text;

namespace TurnFights
{
    /* TODO:
     * IN ORDER TO ADD EVENT BASED STUFF LIKE STATES OR STAT CHANGES, YOU SHOULD PROBABLY REARRANGE A LOT OF CODE INTO FIGHTER CLASS
     * OR SOMETHING LIKE THAT SO THAT YOU CAN MORE EASILY SUBSCRIBE THINGS INTO IT AND NOT NEED TO PASS AROUND SO MANY VARIABLES
     * BEFORE DOING THAT, MAKE A GITHUB REPOSITORY JUST IN CASE
     * 
     * ERROR: Sometimes the turns break -> ill need to check on it once it appears
     * Fine tune weaks/resists (reciprocal weaknesses dont make much sense
     * Fine tune min/max stats
     * Add new elements
     * Introduce other move types through a move class (heal, (de)buffing, etc)
     * Add QtE
     * Add physical attacks that do crits by chance
     * Make the moves menu compatible with different N of moves
     * Add more specialization for each element
     * Add new stats (maybe)
     * Add title screen (maybe)
     * Add campaign mode (maybe)
     */
    public static class Base
    {
        public static int MoveN = 4, Text_Speed = 16,
            min_hp = 120, max_hp = 250, min_dmg = 10, max_dmg = 20,
            min_def = 0, max_def = 10, min_agi = 0, max_agi = 10;

        public static List<Element> Elements =
        [
            new Element("Fire", [1, 2], [0], 0.8f, 1.2f, 0.5f, 1),
            new Element("Ice",  [0,3],  [1], 0.6f, 2,    0.5f, 0.5f),
            new Element("Wind", [2,3],  [],  1.1f, 0.9f, 1,    1.5f),
            new Element("Rock", [0,1],  [3], 1.8f, 0.7f, 1.5f, 0.4f)
            //shock element
            //light/dark elements
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
            weaks = weks;
            resists = resis;
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
        public Attack(int dg, int elm)
        {
            skillType = SkillType.Attack;
            dmg = dg;
            element = elm;
        }
    }
    public class Heal : Skill
    {
        public bool PercentageHealing;
        public int healing;
        public Heal(int heal, bool percent) { skillType = SkillType.Heal;  healing = heal; PercentageHealing = percent; }
    }
    public class StatChanger : Skill
    {
        public enum Stats { DMG, DEF, AGI }
        public Stats changedStat;
        public float modifier;
    }
    public class State : Skill
    {

    }
    public class XAttack : Skill
    {

    }
    public class Defense : Skill
    {

    }
    public class Fighter
    {
        public string name;
        public int element;
        public int hp, df, agi;
        public Skill[] skills;
        public Fighter(string nam, int element, int hpp, int dff, int ag, Skill[] skilll)
        {
            name = nam;
            this.element = element;
            hp = hpp;
            df = dff;
            agi = ag;
            skills = skilll;
        }
    }
}