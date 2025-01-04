using UnityEngine;
using System.Collections;

public static partial class EventEnum
{
    public const string GuideFinish = "GuideFinish"; //引导结束
    public const string UPDATE_ASSETS_COMPLATE = "UPDATE_ASSETS_COMPLATE"; //资源更新完毕
    public const string LOGIN_COMPLATE = "LOGIN_COMPLATE"; //登录成功
    public const string GDPR_ACCEPTED = "PRIVACY_ACCEPTED";
    public const string GDPR_REFUSED = "PRIVACY_REFUSED";
    public const string LOADING_PROGRESS = "LOADING_PROGRESS"; //加载进度
    public const string DOWNLOAD_ERROR = "DOWNLOAD_ERROR"; // 下载报错
    public const string DOWNLOAD_FINISH = "DOWNLOAD_FINISH"; // 文件更新完毕,只要有文件下载完成就会发送这个
    public const string SPEED_UP_LOADING_ANIMATION = "SPEED_UP_LOADING_ANIMATION"; // 加速loading界面的spine动画
    public const string LOADING_FINISH = "LOADING_FINISH"; // loading finish 加载完成 带loading条的
    public const string LOADING_START = "LOADING_START"; // loading start 开始加载 带loading条的
    public const string BackLogin = "BackLogin"; // 回到开始界面

    public const string SCREEN_ORIENTATION_CHANGED = "SCREEN_ORIENTATION_CHANGED"; // 屏幕方向改变
    public const string OnConfigHubUpdated = "OnConfigHubUpdated"; // 远程配置更新完毕
    public const string EnterHome = "EnterHome";
    public const string UserDataUpdate = "UserDataUpdate"; //游戏资源更新
    public const string EnergyChanged = "EnergyChanged"; //体力变化 
    public const string RoomAnimationEnd = "RoomAnimationEnd"; //房间动画播放完成
    public const string OnIAPItemPaid = "OnIAPItemPaid"; //充值事件
    public const string HOTEL_PURCHASE_SUCCESS = "HOTEL_PURCHASE_SUCCESS"; //充值事件
    public const string HOTEL_CLAIM_FREE_SUCCESS = "HOTEL_CLAIM_FREE_SUCCESS";
    public const string OnLevelUp = "OnLevelUp"; //升级
    public static readonly string STORE_SWITCH_TAG = "STORE_SWITCH_TAG";


    public const string BackHomeStep = "BackHomeStep";

    public const string UpdateRedPoint = "UpdateRedPoint";
    public const string UpdateDailyBonus = "UpdateDailyBonus";
    public const string RV_SHOP_DATA_CHANGE = "RvShopDataChange";
    public const string M3_RV_SHOP_REFRESH = "RvShopRefresh";
    public const string RV_SHOP_PURCHASE = "rvshoppurchase";

    public const string BUYSOURCES_PURCHASE = "buysourcespurchase";

    public static readonly string TASK_REFRESH = "TASK_REFRESH";
    public static readonly string TASK_COMPLETE_REFRESH = "TASK_COMPLETE_REFRESH";
    public static readonly string TASKBOX_PLAYANIM = "TASKBOX_PLAYANIM";

    public static readonly string IceBreak_Pack_Begin = "IceBreak_Pack_Begin";
    public static readonly string IceBreak_Pack_REFRESH = "IceBreak_Pack_REFRESH";
    public static readonly string IceBreak_Pack_Finish = "IceBreak_Pack_Finish";
    public static readonly string Daily_Pack_Begin = "Daily_Pack_Begin";
    public static readonly string Daily_Pack_REFRESH = "Daily_Pack_REFRESH";
    public static readonly string Daily_Pack_Time_REFRESH = "Daily_Pack_Time_REFRESH";
    public static readonly string Daily_Pack_Finish = "Daily_Pack_Finish";
    public static readonly string NewDaily_Pack_Begin = "NewDaily_Pack_Begin";
    public static readonly string NewDaily_Pack_REFRESH = "NewDaily_Pack_REFRESH";
    public static readonly string NewDaily_Pack_Time_REFRESH = "NewDaily_Pack_Time_REFRESH";
    public static readonly string NewDaily_Pack_Finish = "NewDaily_Pack_Finish";
    public static readonly string REWARD_POPUP = "REWARD_POPUP";
    public static readonly string NOTICE_POPUP = "NOTICE_POPUP";
    public static readonly string GLOBAL_MAIL_UPDATED = "GLOBAL_MAIL_UPDATED";
    public static readonly string GET_DECORATION_REWARD = "GET_DECORATION_REWARD";


    public static readonly string GIFTBAGLINK_GET_REWARD = "GIFTBAGLINK_GET_REWARD";
    public static readonly string GIFTBAGLINK_PURCHASE_REFRESH = "GIFTBAGLINK_PURCHASE_REFRESH";
    public static readonly string TASKASSIST_PURCHASE_REFRESH = "TASKASSIST_PURCHASE_REFRESH";
    public static readonly string GARAGE_CLEANUP_PURCHASE_REFRESH = "GARAGE_CLEANUP_PURCHASE_REFRESH";
    public static readonly string GIFTBAGLINK_OPEN_REFRESH = "GIFTBAGLINK_OPEN_REFRESH";

    public static readonly string GIFT_BAG_BUY_BETTER_PURCHASE_REFRESH = "GIFT_BAG_BUY_BETTER_PURCHASE_REFRESH";
    public static readonly string GIFT_BAG_SEND_ONE_PURCHASE_REFRESH = "GIFT_BAG_SEND_ONE_PURCHASE_REFRESH";

    public static readonly string FAQ_SELECT_QUESTION = "FAQ_SELECT_QUESTION";
    public static readonly string FAQ_QUESTION_SERVER_BACK = "FAQ_QUESTION_SERVER_BACK";

    public const string MASTERCARD_PURCHASE = "MASTERCARD_PURCHASE";
    public const string MASTERCARD_GETREWARD = "MASTERCARD_GETREWARD";

    public static readonly string PIGBANK_PURCHASE_REFRESH = "PIGBANK_PURCHASE_REFRESH";
    public static readonly string PIGBANK_UI_REFRESH = "PIGBANK_UI_REFRESH";
    public static readonly string PIGBANK_VALUE_REFRESH = "PIGBANK_VALUE_REFRESH";
    public static readonly string PIGBANK_SHOW_BUTTON = "PIGBANK_SHOW_BUTTON";
    public static readonly string PIGBANK_INITIMAGE = "PIGBANK_INITIMAGE";

    public static readonly string VIEW_NEW_HEAD = "GET_NEW_HEAD"; // 浏览新头像
    public static readonly string GET_NEW_HEAD = "GET_NEW_HEAD"; // 获得新头像

    public static readonly string UPDATE_HEAD = "UPDATE_HEAD"; // 更新头像
    public static readonly string UPDATE_NAME = "UPDATE_NAME"; // 更新名字

    public static readonly string DAILY_RANK_UPDATE = "DAILY_RANK_UPDATE";

    public static readonly string GARAGE_CLEANUP_TURNIN = "GARAGE_CLEANUP_TURNIN";
    public static readonly string GARAGE_CLEANUP_LevelFinish = "GARAGE_CLEANUP_LevelFinish";
    public static readonly string GARAGE_CLEANUP_Finish = "GARAGE_CLEANUP_Finish";

    public static readonly string EASTER_CLAIM = "EASTER_CLAIM";

    public static readonly string EASTER_PACK_BEGIN = "EASTER_PACK_BEGIN";
    public static readonly string EASTER_PACK_REFRESH = "EASTER_PACK_REFRESH";
    public static readonly string EASTER_PACK_Finish = "EASTER_PACK_Finish";

    public static readonly string BATTLE_PASS_2_PURCHASE = "BATTLE_PASS_2_PURCHASE"; // battle pass 充值
    public static readonly string BATTLE_PASS_2_PURCHASE_UNLOCK = "BATTLE_PASS_2_PURCHASE_UNLOCK"; // battle pass 充值
    public static readonly string BATTLE_PASS_2_TASK_REFRESH = "BATTLE_PASS_2_TASK_REFRESH"; // battle task
    public static readonly string BATTLE_PASS_2_REFRESH = "BATTLE_PASS_2_REFRESH"; // battle task
    public static readonly string BATTLE_PASS_2_STORE_REFRESH = "BATTLE_PASS_2_STORE_REFRESH";
    public static readonly string BATTLE_PASS_2_TASK_COMPLETE = "BATTLE_PASS_2_TASK_COMPLETE"; // battle task
    public static readonly string BATTLE_PASS_2_COLLECT_LOOP = "BATTLE_PASS_2_COLLECT_LOOP"; // battle pass 循环奖励领取

    public static readonly string ASMR_BEGIN_DOWNLOAD = "ASMR_BEGIN_DOWNLOAD";
    public static readonly string ASMR_End_DOWNLOAD = "ASMR_End_DOWNLOAD";
    public static readonly string ASMR_DOWNLOAD_PROGRESS = "ASMR_DOWNLOAD_PROGRESS";
    public static readonly string ASMR_REFRESH_REDPOINT = "ASMR_REFRESH_REDPOINT";
}

// 1-服务顾客
// 2-合成物品
// 3-消耗资源
// 4-合成次数
// 5-获得资源
public enum TaskType
{
    Serve = 1,
    MergerItem,
    Consume,
    MergerNum,
    GetRes,
}