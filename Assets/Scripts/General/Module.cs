using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ModuleType { Core, Energy, Computation, Power, Armor, Weapon, Repair, Battery, None, Locked }
public enum DamageType { Kinetic, Thermal, Radiation, Shock }

public class Module
{
    public ModuleType type;
    public int hp;
    public int maxHp;
    public int load;
    public int elecContribution;
    public int elecRestore;
    public int computingContribution;
    public Sprite Icon;

    public GameObject obj;

    public Module(ModuleType _type, int _maxHp, int _load, int _elecContribution)
    {
        type = _type;
        hp = maxHp = _maxHp;
        load = _load;
        elecContribution = _elecContribution;
        elecRestore = computingContribution = 0;
        Icon = null;

        switch (type)
        {
            case ModuleType.Core:
                Icon = Resources.Load<Sprite>("Images/001");
                break;
            case ModuleType.Energy:
                Icon = Resources.Load<Sprite>("Images/002");
                break;
            case ModuleType.Battery:
                Icon = Resources.Load<Sprite>("Images/003");
                break;
            case ModuleType.Armor:
                Icon = Resources.Load<Sprite>("Images/004");
                break;
            case ModuleType.Computation:
                Icon = Resources.Load<Sprite>("Images/005");
                break;
            case ModuleType.Weapon:
                Icon = Resources.Load<Sprite>("Images/007");
                break;
            case ModuleType.Power:
                Icon = Resources.Load<Sprite>("Images/009");
                break;
            default:
                break;
        }
    }

    public static Module sample(ModuleType type)
    {
        switch (type)
        {
            case ModuleType.Core:
                return CoreModule.sample();
            case ModuleType.Energy:
                return EnergyModule.sample();
            case ModuleType.Armor:
                return ArmorModule.sample();
            case ModuleType.Computation:
                return ComputationModule.sample();
            case ModuleType.Power:
                return PowerModule.sample();
            case ModuleType.Weapon:
                return WeaponModule.sample();
            case ModuleType.Repair:
                return RepairModule.sample();
            case ModuleType.Battery:
                return BatteryModule.sample();
            case ModuleType.Locked:
                return new Module(ModuleType.Locked, 0, 0, 0);
            case ModuleType.None:
                return new Module(ModuleType.None, 0, 0, 0);
            default:
                return new Module(ModuleType.None, 0, 0, 0);
        }
    }

    public virtual int ReturnElcContriution()
    {
        return elecContribution;
    }

    public virtual int ReturnElcRestore()
    {
        return elecRestore;
    }

    public virtual int ReturnComputContribution()
    {
        return computingContribution;
    }

    public virtual int ReturnComputUsing()
    {
        return 0;
    }
}

public class CoreModule : Module
{

    public float beta;
    public float omega;

    public CoreModule(int _maxhp, int _load, int _elecContribution, int _elecRestore,
      int _computingContribution, float _beta, float _omega) : base(ModuleType.Core, _maxhp, _load, _elecContribution)
    {
        elecRestore = _elecRestore;
        computingContribution = _computingContribution;
        beta = _beta;
        omega = _omega;
    }

    public static CoreModule sample()
    {
        return new CoreModule(500, 1, 2, 50, 100, 20f, 30f);
    }
}

public class EnergyModule : Module
{
    public EnergyModule(int _maxhp, int _load, int _elecContribution, int _elecRestore) :
      base(ModuleType.Energy, _maxhp, _load, _elecContribution)
    {
        elecRestore = _elecRestore;
    }

    public static EnergyModule sample()
    {
        return new EnergyModule(200, 1, 10, 100);
    }
}

public class ComputationModule : Module
{
    public ComputationModule(int _maxhp, int _load, int _elecContribution, int _computingContribution) :
      base(ModuleType.Computation, _maxhp, _load, _elecContribution)
    {
        computingContribution = _computingContribution;
    }

    public static ComputationModule sample()
    {
        return new ComputationModule(200, 1, -1, 400);
    }
}

public class PowerModule : Module
{
    public float beta;
    public float omega;

    public PowerModule(int _maxhp, int _load, int _elecContribution, int _computingContribution, float _beta, float _omega) :
      base(ModuleType.Power, _maxhp, _load, _elecContribution)
    {
        computingContribution = _computingContribution;
        beta = _beta;
        omega = _omega;
    }

    public static PowerModule sample()
    {
        return new PowerModule(300, 1, -1, -100, 80, 120);
    }

    public override int ReturnComputContribution()
    {
        return 0;
    }

    public override int ReturnComputUsing()
    {
        return -computingContribution;
    }
}

public class ArmorModule : Module
{
    public int[] resistance;

    public ArmorModule(int _maxhp, int _load, int _elecContribution, int _kinetic, int _thermal, int _radiation, int _shock) :
      base(ModuleType.Armor, _maxhp, _load, _elecContribution)
    {
        resistance = new int[] { _kinetic, _thermal, _radiation, _shock };
    }

    public static ArmorModule sample()
    {
        return new ArmorModule(1000, 1, 0, 0, 0, 0, 0);
    }
}

public class WeaponModule : Module
{
    public DamageType dmgType;
    public int singleDmg;
    public float shootingSpeed;
    public int jointNumber;

    public float lastShootTime;

    public WeaponModule(int _maxhp, int _load, int _elecContribution, int _computingContribution,
      DamageType _dmgType, int _singleDmg, float _shootingSpeed, int _jointNumber) :
      base(ModuleType.Weapon, _maxhp, _load, _elecContribution)
    {
        computingContribution = _computingContribution;
        dmgType = _dmgType;
        singleDmg = _singleDmg;
        shootingSpeed = _shootingSpeed;
        jointNumber = _jointNumber;
        lastShootTime = -10f;
    }

    public static WeaponModule sample()
    {
        return new WeaponModule(300, 1, -5, -100, DamageType.Kinetic, 2, 0.2f, 1);
    }

    public override int ReturnComputContribution()
    {
        return 0;
    }

    public override int ReturnComputUsing()
    {
        return -computingContribution;
    }
}

public class RepairModule : Module
{
    public RepairModule(int _maxhp, int _load, int _elecContribution) :
      base(ModuleType.Repair, _maxhp, _load, _elecContribution)
    {
    }

    public static RepairModule sample()
    {
        return new RepairModule(100, 1, 0);
    }
}

public class BatteryModule : Module
{
    public BatteryModule(int _maxhp, int _load, int _elecRestore) :
      base(ModuleType.Battery, _maxhp, _load, 0)
    {
        elecRestore = _elecRestore;
    }

    public static BatteryModule sample()
    {
        return new BatteryModule(200, 1, 450);
    }
}