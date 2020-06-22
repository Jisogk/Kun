using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StorageManager : MonoBehaviour {

    public static StorageManager instance = null;

    public ModuleSlotsUI mSlots;
    public ModuleStorageUI mStorage;

    public List<Module> ModuleList;
    public List<RobotDiagram> DiagramList;

    private GameObject icon;

    // Use this for initialization
    void Awake () {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        ModuleList = new List<Module>();
        DiagramList = new List<RobotDiagram>();

        DragAndDrop.OnLeftDrag += DragAndDrop_OnLeftDrag;
        DragAndDrop.OnEnter += DragAndDrop_OnEnter;
        DragAndDrop.OnExit += DragAndDrop_OnExit;
        DragAndDrop.OnLeftBeginDrag += DragAndDrop_OnLeftBeginDrag;
        DragAndDrop.OnLeftEndDrag += DragAndDrop_OnLeftEndDrag;

        if (DiagramList.Count == 0 || ModuleList.Count == 0)
        {
            initStandardSet();
            //PlayerManager.instance.initCurDiagram();
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddDiagram(RobotDiagram diagram)
    {
        DiagramList.Add(diagram);
    }

    public int GetDiagramCount()
    {
        return DiagramList.Count;
    }

    public void RemoveDiagram(int index)
    {
        if (index < ModuleList.Count - 1)
        {
            DiagramList.RemoveAt(index);
        }
    }

    public RobotDiagram GetDiagram(int index)
    {
        if (index < ModuleList.Count && DiagramList[index] != null)
        {
            return DiagramList[index];
        }
        else
        {
            return null;
        }
    }

    public RobotDiagram NewDiagram(int length, int width)
    {
        return new RobotDiagram(length, width);
    }

    public void AddModule(Module module)
    {
        ModuleList.Add(module);
    }

    public int GetModuleCount()
    {
        return ModuleList.Count;
    }

    public void RemoveModule(int index)
    {
        if (index < ModuleList.Count - 1)
        {
            ModuleList.RemoveAt(index);
        }
    }

    public Module GetModule(int index)
    {
        if (index < ModuleList.Count && ModuleList[index] != null)
        {
            return ModuleList[index];
        }
        else
        {
            return null;
        }
    }

    public Module NewModule(ModuleType type)
    {
        return Module.sample(type);
    }

    private void initStandardSet()
    {
        AddDiagram(new RobotDiagram(3,3, "Perfect"));

        AddModule(NewModule(ModuleType.Core));
        AddModule(NewModule(ModuleType.Energy));
        AddModule(NewModule(ModuleType.Computation));
        AddModule(NewModule(ModuleType.Power));
        AddModule(NewModule(ModuleType.Armor));
        AddModule(NewModule(ModuleType.Weapon));
        AddModule(NewModule(ModuleType.Battery));
    }

    public void DragAndDrop_OnEnter(Transform tf)
    {

    }

    public void DragAndDrop_OnExit()
    {
    }

    public void DragAndDrop_OnLeftBeginDrag(Transform tf)
    {
        //icon = new GameObject("Icon", typeof(Image));

        if (tf.transform.tag.Equals("ModuleSlot"))
        {
            ModuleSlot ms = tf.GetComponent<ModuleSlot>();
            if (PlayerManager.instance.ModuleMap[ms.rawIndex, ms.rolIndex] != null)
            {
                icon = new GameObject("Icon", typeof(Image));
                icon.GetComponent<Image>().sprite = PlayerManager.instance.ModuleMap[ms.rawIndex, ms.rolIndex].Icon;
                icon.transform.SetParent(GameObject.Find("Canvas").transform);
                icon.transform.SetAsLastSibling();
            }
            else
            {
                return;
            }
        }
        else if (tf.transform.tag.Equals("ModuleSelection"))
        {
            ModuleSelectUI ms = tf.GetComponent<ModuleSelectUI>();

            if (StorageManager.instance.ModuleList[ms.Index] != null)
            {
                icon = new GameObject("Icon", typeof(Image));
                icon.GetComponent<Image>().sprite = StorageManager.instance.ModuleList[ms.Index].Icon;
                icon.transform.SetParent(GameObject.Find("Canvas").transform);
                icon.transform.SetAsLastSibling();
            }
            else
            {
                return;
            }
        }
        else
        { return; }
    }

    public void DragAndDrop_OnLeftEndDrag(Transform prevTransfrom, Transform enterTransfrom)
    {
        Destroy(icon);
        ModuleSelectUI msu;
        ModuleSlot prevms;
        ModuleSlot enterms;

        if (prevTransfrom.tag.Equals("ModuleSelection"))
        {
            msu = prevTransfrom.GetComponent<ModuleSelectUI>();

            if (enterTransfrom.tag.Equals("ModuleSelection"))
            { }
            else if (enterTransfrom.tag.Equals("ModuleSlot"))
            {
                enterms = enterTransfrom.GetComponent<ModuleSlot>();
                PlayerManager.instance.ModuleMap[enterms.rawIndex, enterms.rolIndex] = ModuleList[msu.Index];
                mSlots.UpdateModuleShow();
            }
            else
            { }
        }
        else if(prevTransfrom.tag.Equals("ModuleSlot"))
        {
            prevms = prevTransfrom.GetComponent<ModuleSlot>();

            if (enterTransfrom.tag.Equals("ModuleSlot"))
            {
                enterms = enterTransfrom.GetComponent<ModuleSlot>();
                PlayerManager.instance.SwitchModule(prevms.rawIndex, prevms.rolIndex, enterms.rawIndex, enterms.rolIndex);
                mSlots.UpdateModuleShow();
            }
            else
            {
                PlayerManager.instance.ModuleMap[prevms.rawIndex, prevms.rolIndex] = null ;
            }
            mSlots.UpdateModuleShow();
        }
    }

    public void DragAndDrop_OnLeftDrag()
    {
        //icon.transform.position = eventData.position;
    }
}
