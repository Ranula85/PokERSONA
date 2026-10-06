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
     * Added state visualization, however poison and the others dont seem to work, also defending text ends too early
     * so yeah you should probably do a good check to see what the game is doing right now and fix things
     * 
     * Make the moves menu compatible with different N of moves
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
            new Element("Dark",[6,5],[0,7], 0.9f, 1.2f, 0.8f, 1)     //7
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
        public float modifier;
        public bool applyToSelf;
        public int Turns;  //how long the effect lasts
        int _elapsed = -1; //turns passed, starts at -1 to account for the turn when it was applied
        public void ApplyChanger(Fighter target)
        {
            switch (changedStat)
            {
                case Stats.DMG:
                    target.modDmg *= modifier;
                    target.Info.DmgTxt[modifier > 1 ? 0 : 1].Visible = true;
                    break;
                case Stats.DEF:
                    target.df = (int)(target.df * modifier);
                    target.Info.DefTxt[modifier > 1 ? 0 : 1].Visible = true;
                    break;
                case Stats.AGI:
                    target.agi = (int)(target.agi * modifier);
                    target.Info.AgiTxt[modifier > 1 ? 0 : 1].Visible = true;
                    break;
            }
            Battle.TurnEnd += (_, _) => { _elapsed++; if (_elapsed == Turns) UnApplyChanger(target); };
        }
        public void UnApplyChanger(Fighter target)
        {
            Debug.WriteLine($"State changer {changedStat} {modifier} of {target.name} has ended");
            switch (changedStat)
            {
                case Stats.DMG:
                    target.modDmg /= modifier;
                    target.Info.DmgTxt[modifier > 1 ? 0 : 1].Visible = false;
                    break;
                case Stats.DEF:
                    target.df = (int)(target.df / modifier);
                    target.Info.DefTxt[modifier > 1 ? 0 : 1].Visible = false;
                    break;
                case Stats.AGI:
                    target.agi = (int)(target.agi / modifier);
                    target.Info.AgiTxt[modifier > 1 ? 0 : 1].Visible = false;
                    break;
            }
        }
        public StatChanger(StatChanger changer) {
            skillType = SkillType.StatChange; changedStat = changer.changedStat; modifier = changer.modifier;
            applyToSelf = changer.applyToSelf; Turns = changer.Turns; }
        public StatChanger(Stats change, float mod, bool toSelf, int time = 6) {
            skillType = SkillType.StatChange;  changedStat = change; modifier = mod;
            applyToSelf = toSelf; Turns = time; }
        public static StatChanger
            DmgUp = new StatChanger(Stats.DMG, 1.5f, true, 3),
            DmgDown = new StatChanger(Stats.DMG, 0.5f, false, 3),
            DefUp = new StatChanger(Stats.DEF, 1.5f, true, 3),
            DefDown = new StatChanger(Stats.DEF, 0.5f, false, 3),
            AgiUp = new StatChanger(Stats.AGI, 1.5f, true, 3),
            AgiDown = new StatChanger(Stats.AGI, 0.5f, false, 3);
    }
    public class State : Skill
    {
        //this first bit declares a subscriber that takes a function of type TurnEvents, when constructing States subscriber is given
        //the function of adding the TurnEvent function it holds into an event from Battle, the one we prefer, kudos to Gemini for this
        public string stateName;
        public Action<Battle.TurnEvents> Subscriber;
        public Action<Battle.TurnEvents> UnSubscriber;
        public Action<Fighter, Fighter> action; //the action to perform in the event
        public bool afflictSelf; //whether or not to afflict the state to caster
        public bool afflictedsTurn; //whether or not the action will take place in the afflicted's turn
        public int Turns;  //how long the state lasts
        int _elapsed = -1; //turns passed, starts at -1 to account for the turn when it was applied
        public void ApplyState(Fighter afflicted)
        {
            //the state can be afflicted to either the mover or the target and take effect on mover's or target's turn
            Battle.TurnEvents handler = afflictedsTurn ? (mover, target) => { if (mover == afflicted) { action(mover, target); } } : 
                (mover, target) => { if (target == afflicted) { action(mover, target); } };
            Subscriber(handler);
            Battle.TurnEnd += (_, _) => { _elapsed++; if (_elapsed == Turns) RemoveState(afflicted); };
            afflicted.Info.StateTxts[stateName].Visible = true;
        }
        public void RemoveState(Fighter afflicted)
        {
            Battle.TurnEvents handler = afflictedsTurn ? (mover, target) => { if (mover == afflicted) { action(mover, target); } } :
                (mover, target) => { if (target == afflicted) { action(mover, target); } };
            UnSubscriber(handler);
            afflicted.Info.StateTxts[stateName].Visible = false;
        }
        public State(State state) {
            skillType = SkillType.State; stateName = state.stateName; Subscriber = state.Subscriber; UnSubscriber = state.UnSubscriber;
            action = state.action; afflictSelf = state.afflictSelf; afflictedsTurn = state.afflictedsTurn; Turns = state.Turns; }
        public State(string name, Action<Battle.TurnEvents> s, Action<Battle.TurnEvents> ns, Action<Fighter, Fighter> act, bool toSelf, bool tTurn, int time = 6)
            {
            skillType = SkillType.State; stateName = name; Subscriber = s; UnSubscriber = ns; action = act; afflictSelf = toSelf; afflictedsTurn = tTurn; 
            Turns = time; }
        public readonly static State Poison = new State("Poison", s: handler => Battle.TurnEnd += handler, //damages mover in his turn
            ns: handler => Battle.TurnEnd -= handler,
            act: (mover, target) => { Battle.Instance.Damage(mover, 10); }, toSelf: false, tTurn: true),
            Spikes = new State("Spikes", s: handler => Battle.DealDamage += handler, //deals damage to mover when attacking target
                ns: handler => Battle.DealDamage -= handler,
                act: (mover, target) => { Battle.Instance.Damage(mover, 15); }, toSelf: true, tTurn: false),
            Overcharged = new State("Overcharged", s: handler => Battle.DealDamage += handler, //makes next attack double damage
                ns: handler => Battle.DealDamage -= handler,
                act: (mover, target) => { mover.modDmg = 2; }, toSelf: true, tTurn: true, 2);
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
        public float modDmg = 1; //used by statchangers and states
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
        public UiInfo Info;
        public struct UiInfo
        {
            public Dictionary<string, Label> StateTxts;
            public Label[] DmgTxt, DefTxt, AgiTxt; //up // down
            public Label Defending;
        }
        public Fighter() { }
    }
}