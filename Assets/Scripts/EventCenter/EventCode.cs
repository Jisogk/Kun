/// <summary>
/// 事件码
/// 指定事件类型
/// 总有一种这东西要写一百行的感觉（
/// </summary>

public enum EventCode
{
    NewDiagram, //在仓库中添加新设计图的事件
    SetNewDiagram, //从UI中选择新设计图的事件
    ChangeDiagram, //在仓库中完成设计图切换时的事件
    SwitchModule, //在电力方案中切换模块开关的事件
    StatusChanged, //刷新状态参数的事件
    ShowErrMsg, //显示错误信息的事件
    ShowWarningMsg, //显示警告的事件
    HideErrMsg, //隐藏错误信息的事件
    OnModuleDestory, //模块被摧毁时的事件
    OnCoreDestory, //核心被摧毁时的事件
    OnModuleDamage, //模块受到伤害的事件
    OnEnergyChange, //能量储备发生变化时的事件
}
