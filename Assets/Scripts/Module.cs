using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ModuleType { Core, Energy, Computation, Power, Armor, Weapon }

public class Module {
  public ModuleType type;
  public int hp;
  public int maxHp;
  public int load;
  public int elecContribution;
  public int elecRestore;
  public int computingContribution;
  public int force;
  public int power;

  public Module(ModuleType _type, int _hp, int _maxhp, int _load, int _elecContribution, 
                int _elecRestore=0, int _computingContribution=0, int _force=0, int _power=0)
  {
    type = _type;
    hp = _hp;
    maxHp = _maxhp;
    load = _load;
    elecContribution = _elecContribution;
    elecRestore = _elecRestore;
    computingContribution = _computingContribution;
    force = _force;
    power = _power;
  }
}
