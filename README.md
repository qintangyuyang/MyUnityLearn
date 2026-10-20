# MyUnityLearn · Unity 6 农场游戏

> 一个用 **Unity 6（6000.4.0f1）** 从零搭建的 2D 农场经营游戏，用于系统练习 Unity 2D 开发的核心能力。
>
> 项目跟随 Udemy 农场游戏课程实现，代码在课程基础上加入了自己的注释与重构。

![Unity](https://img.shields.io/badge/Unity-6000.4.0f1-black?logo=unity)
![URP](https://img.shields.io/badge/URP-2D-blue)
![Language](https://img.shields.io/badge/C%23-9.0-239120?logo=csharp)
![Status](https://img.shields.io/badge/status-持续开发中-yellow)

---

## 目录

- [游戏简介](#游戏简介)
- [技术栈](#技术栈)
- [已实现功能](#已实现功能)
- [操作说明](#操作说明)
- [场景说明](#场景说明)
- [工程结构](#工程结构)
- [脚本清单](#脚本清单)
- [核心系统设计](#核心系统设计)
- [运行方式](#运行方式)
- [学习路线](#学习路线)
- [已知问题与后续计划](#已知问题与后续计划)
- [素材与致谢](#素材与致谢)

---

## 游戏简介

一款 2D 俯视角农场经营小游戏。玩家在自己的农场里**翻地、浇水、播种、收获**，把作物拿到**商店**换取金币，再用金币**买更多种子**；同时受**昼夜时间**限制——回到床上睡觉或在时间耗尽时结束当天，第二天作物才会生长。

**核心循环**：

```
翻地 → 浇水 → 播种 → （睡觉/时间耗尽）→ 作物生长 → 收获 → 商店卖钱 → 买种子 → 循环
```

---

## 技术栈

| 分类 | 技术/工具 | 版本 |
|---|---|---|
| 引擎 | Unity | **6000.4.0f1 (Unity 6)** |
| 渲染管线 | Universal Render Pipeline（2D Renderer） | 17.4.0 |
| 输入 | **新 Input System**（`InputActionReference`） | 1.19.0 |
| 2D 工具 | Tilemap / Tilemap Extras / Sprite Atlas / Aseprite Importer | 1.0.0 / 7.0.1 / — / 4.0.1 |
| UI | UGUI + **TextMesh Pro** | 2.0.0 |
| 动画 | Animator 状态机 + Animation Clip | — |
| 语言 | C# | — |

---

## 已实现功能

### 农场与作物
- **动态网格生成**：由 `minPoint` / `maxPoint` 两个锚点自动铺出耕地网格，并用 LayerMask 检测障碍物、跳过不可用格子
- **6 段生长状态机**：`barren`（荒地）→ `ploughed`（翻耕）→ `planted`（播种）→ `growing1` → `growing2` → `ripe`（成熟）
- **8 种作物**：南瓜、生菜、胡萝卜、干草、土豆、草莓、番茄、牛油果，每种有独立贴图、种子价格、作物售价和**生长失败概率**
- **浇水机制**：只有浇过水的土地才会在第二天生长；生长有随机失败概率

### 工具系统
- 4 种工具：**锄头（翻地）/ 浇水壶 / 种子 / 篮子（收获）**
- 鼠标控制的**工具指示器**，限制在玩家周围一定范围内（`toolRange`），并对齐到格子中心
- 工具切换带动画（`Animator` Trigger）与音效

### 时间与昼夜
- 时间从 `dayStart` 推进到 `dayEnd`，屏幕实时显示 AM/PM
- 时间耗尽或玩家**回床睡觉** → 进入 `DayEnd` 过场场景（显示天数）
- 新的一天从床边醒来，作物统一结算生长

### 经济系统
- 金币 UI 实时显示
- **商店**：购买种子（按单价×数量扣钱）、出售作物（按数量×单价加钱）
- 背包：种子与作物的数量展示、种子选择

### 场景与流程
- 多场景切换（农场 ↔ 室内）
- 用 `PlayerPrefs["Transition"]` 记录过场标记，实现**从正确的门口/床边出现**
- 主菜单（含飘落物体动画）、暂停菜单、返回主菜单

### 表现与音频
- 摄像机跟随 + 边界钳制（`clampMin` / `clampMax`）
- 多首 BGM 轮播、标题音乐、**音效随机音高**（避免重复听感疲劳）

---

## 操作说明

| 操作 | 按键 | 说明 |
|---|---|---|
| 移动 | `W` `A` `S` `D` / 方向键 | 由 `Move` Action 驱动 |
| 使用工具 | `E` / 鼠标左键 | 对指示器所在格子使用当前工具 |
| 切换工具 | `1` `2` `3` `4` | 锄头 / 浇水壶 / 种子 / 篮子 |
| 循环切换工具 | `Tab` | 依次轮换 |
| 打开/关闭背包 | `I` | — |
| 打开/关闭商店 | `B`（仅编辑器） | 靠近商店区域按 `E` / `Space` 也可打开 |
| 睡觉 | 靠近床按 `Space` / `E` / 鼠标左键 | 直接结束当天 |
| 暂停 | `Esc` / `P` | — |

> **调试快捷键**
> - `N`：手动推进所有作物一个生长阶段（`GrowBlock.Update` 中的调试代码）
> - `B`：打开商店，仅 `#if UNITY_EDITOR` 下有效

---

## 场景说明

| 场景 | 路径 | 作用 |
|---|---|---|
| `MainMenu` | `Assets/Scenes/MainMenu.unity` | 主菜单，开始游戏入口 |
| `Main` | `Assets/Scenes/Main.unity` | **主农场场景**（核心玩法） |
| `Indoors` | `Assets/Scenes/Indoors.unity` | 室内场景（床、商店等） |
| `DayEnd` | `Assets/Scenes/DayEnd.unity` | 每日结算过场 |
| `Indoors-Copy` | `Assets/Scenes/Indoors-Copy.unity` | ⚠️ 调试用的副本，可删除 |
| `SampleScene` | `Assets/Scenes/SampleScene.unity` | URP 模板自带的空场景，可删除 |

---

## 工程结构

```
MyUnityLearn/
├── Assets/
│   ├── Scenes/                 # 游戏场景
│   ├── Scripts/                # 23 个 C# 脚本（全部游戏逻辑）
│   ├── Settings/               # URP 2D 配置（Renderer2D、UniversalRP 等）
│   ├── TextMesh Pro/           # TMP 资源（自动导入）
│   ├── _Udemy Farming Assets/  # 课程素材：美术、音频、字体
│   │   ├── Art/                # Tiny Wonder Farm 图集、精灵图
│   │   ├── Audio/              # Music（BGM）/ SFX（音效）
│   │   └── Font/               # Pixelnauts 像素字体
│   ├── InputSystem_Actions.inputactions   # 输入配置（Player / UI 两套）
│   └── DefaultVolumeProfile.asset
├── Packages/
│   └── manifest.json           # 依赖清单
├── ProjectSettings/            # 工程设置（含 ProjectVersion.txt）
├── .gitignore                  # Unity 官方模板
└── README.md
```

> `Library/`、`Temp/`、`Logs/`、`obj/` 等由 `.gitignore` 排除，不纳入版本控制。

---

## 脚本清单

| 脚本 | 职责 |
|---|---|
| `PlayerController` | 玩家主控：移动、左右翻转、工具切换与使用、动画驱动、UI 打开时锁定移动 |
| `GridController` | 生成耕地网格、按坐标查询格子、处理障碍格 |
| `GridInfo` | **跨场景保存网格数据**（单例 + `DontDestroyOnLoad`），按天结算作物生长 |
| `GrowBlock` | 单个耕地格：翻地、浇水、播种、成熟、收获、贴图更新 |
| `CropController` | 作物数据表（8 种作物的贴图/价格/失败率）与种子/作物数量增删 |
| `CropDisplay` / `SeedDisplay` | 背包中作物 / 种子的格子显示与种子选择 |
| `InventoryController` | 背包开关与整体刷新 |
| `ShopController` | 商店开关（与背包互斥） |
| `ShopSeedDisplay` / `ShopCropDisplay` | 商店中种子购买 / 作物出售 |
| `CurrencyController` | 金币的增减、余额校验、UI 同步 |
| `TimeController` | 昼夜时间推进、结束当天、开始新的一天 |
| `DayEndController` | 每日结算过场：显示天数，任意键进入下一天 |
| `BedController` | 靠近床时允许睡觉（结束当天） |
| `ShopActivator` | 靠近商店区域时允许打开商店 |
| `AreaSwitcher` | 场景切换触发器 + 过场落点定位 |
| `CameraController` | 摄像机跟随玩家并钳制在场景边界内 |
| `UIController` | UI 总控：时间/金钱文字、工具栏图标、暂停、返回主菜单 |
| `AudioManager` | BGM 轮播、标题音乐、音效播放（支持随机音高） |
| `MainMenuController` | 主菜单按钮（开始 / 退出） |
| `MainMenuFallingObject` | 主菜单飘落物体的下落与旋转 |
| `MainMenuBGObjectSpawner` | 主菜单飘落物体的定时生成 |

---

## 核心系统设计

### 1. 网格系统（`GridController` + `GridInfo` + `GrowBlock`）

- `GridController` 在 `Start` 时由两个锚点算出网格尺寸，批量 `Instantiate` 出 `GrowBlock`
- 用 `Physics2D.OverlapBox` + `gridBlockers` LayerMask 判断某格是否被障碍物占用（设为 `prevenetUse`）
- `GridInfo` 作为**跨场景数据容器**：场景切换时把每个格子的状态存进 `theGrid[y].blocks[x]`，切回来时再回填，从而实现"离开农场再回来作物还在"

### 2. 作物生长状态机（`GrowBlock.GrowthState`）

```
barren ──翻地──> ploughed ──浇水+播种──> planted ──过夜且已浇水──> growing1
   ↑                                                                  │
   └──────────────────────── 收获/枯萎 ←────── ripe ←── growing2 ←─────┘
```

- 生长只在**过夜结算**时发生（`GridInfo.GrowCrop`），并有 `growthFailChance` 随机失败
- 未浇水的土地不会生长；`ploughed` 状态过夜后会退回 `barren`（需要重新翻地）

### 3. 单例管理器

`PlayerController`、`GridInfo`、`TimeController`、`CropController`、`CurrencyController`、`UIController`、`AudioManager` 都采用统一的**单例 + `DontDestroyOnLoad`** 模式：

```csharp
private void Awake()
{
    if (instance == null) { instance = this; DontDestroyOnLoad(gameObject); }
    else { Destroy(gameObject); }
}
```

保证跨场景时只有一份实例，且数据不丢失。

### 4. 时间与过场

- `TimeController` 每帧推进 `currentTime`，超过 `dayEnd` 自动 `EndDay()`
- `EndDay()` → 结算作物 → 写 `PlayerPrefs["Transition"] = "Wake Up"` → 加载 `DayEnd` 场景
- `DayEndController` 等玩家任意键 → `StartDay()` → 加载 `wakeUpScene`
- `AreaSwitcher.Start()` 读取 `PlayerPrefs["Transition"]`，匹配则把玩家移动到指定 `startPoint`

---

## 运行方式

### 环境要求
- **Unity Hub** + **Unity 6000.4.0f1**（必须匹配，版本不同会触发升级/降级）
- 首次打开会自动导入 `_Udemy Farming Assets`，耗时几分钟属正常

### 步骤

1. `git clone https://github.com/qintangyuyang/MyUnityLearn.git`
2. Unity Hub → **Add** → 选择工程根目录 → 用 **6000.4.0f1** 打开
3. 在 `Project` 窗口打开 `Assets/Scenes/MainMenu.unity`
4. 点击 **Play**

> ⚠️ **输入系统提示**：本工程使用**新 Input System**（`com.unity.inputsystem`）。
> 如果后续手写了旧的 `Input.GetAxis` 之类的代码，需要把
> `Project Settings → Player → Other Settings → Active Input Handling` 改为 **Both**。

### 建议的编辑器设置
`Project Settings → Editor`：
- **Version Control Mode** = `Visible Meta Files`
- **Asset Serialization** = `Force Text`

（保证场景/Prefab 可 diff、可合并；`.meta` 文件必须提交）

---

## 学习路线

提交历史完整记录了功能演进过程，可作为学习索引：

| 阶段 | 提交主题 | 涉及知识点 |
|---|---|---|
| 1 | 初始化工程、导入素材 | 工程结构、资源管理、`.gitignore` |
| 2 | 创建主场景、瓦片地块 | **Tilemap**、2D 场景搭建 |
| 3 | 玩家移动、左右翻转 | **新 Input System**、Rigidbody2D、**Animator 状态机** |
| 4 | 摄像机跟随 | 正交相机、视口计算、边界钳制 |
| 5 | 摄像机范围限制、Tilemap 碰撞 | Tilemap Collider、`Physics2D` |
| 6 | 耕地地块、生长状态枚举、工具 UI | **enum 状态机**、UI 图标切换 |
| 7 | 工具使用动画 | Animator Trigger、动画事件 |
| 8 | 限制移动、种植、生长、收获 | 状态流转、工具系统 |
| 9 | 铺满网格、网格指示器 | 程序化生成、坐标换算、指针交互 |
| 10 | 场景切换、室内外切换 | `SceneManager`、`PlayerPrefs` 过场标记 |
| 11 | 多场景保存网格信息 | **跨场景数据持久化**（单例 + DontDestroyOnLoad） |
| 12 | 床与睡觉、时间显示 | 触发器交互、时间系统、AM/PM 格式化 |
| 13 | 一天的开始与结束、过场场景 | 场景流程编排 |
| 14 | 醒来在床边 | 过场落点定位 |
| 15 | 背包展示 | UI 刷新、数据驱动显示 |
| 16 | 商店、购买、金钱 UI | 经济系统、条件购买 |
| 17 | 出售作物、区域打开商店 | 触发器 + 交互 |
| 18 | 主菜单、飘落物动画 | 菜单流程、随机动画 |
| 19 | 暂停游戏 | `Time.timeScale` |
| 20 | 音乐与音效 | `AudioSource`、BGM 轮播、随机音高 |

---

## 已知问题与后续计划

### 待改进
- [ ] **存档系统**：目前进度（金币、天数、作物状态）只存在内存里，**退出游戏后重置**。计划接入 JSON 存档
- [ ] 清理调试代码：`GrowBlock` 里的 `N` 键、`UIController` 里的 `B` 键
- [ ] 删除 `Indoors-Copy` 与 `SampleScene` 两个冗余场景
- [ ] 统一命名：部分字段存在拼写问题（`gorwFailChance` → `growFailChance`、`prevenetUse` → `preventUse`、`thSR` → `soilRenderer`）
- [ ] `GrowBlock.Update` 里每帧检测键盘，建议收敛到统一输入处理

### 计划新增
- [ ] 多季节/天气系统
- [ ] 作物成长动画与粒子效果
- [ ] 任务/成就系统
- [ ] 存档与读档 UI
- [ ] 打包发布（Windows）

---

## 素材与致谢

- **美术**：Tiny Wonder Farm（像素风农场素材）
- **音频**：课程附带 BGM / SFX
- **字体**：Pixelnauts（SDF 字体资源由 TextMesh Pro 生成）
- **课程**：Udemy 农场游戏开发课程（本仓库为个人学习实现，非官方仓库）

> 素材版权归原作者所有，仅用于个人学习，不用于商业用途。

---

## 相关仓库

- [my-study-notes](https://github.com/qintangyuyang/my-study-notes) — C# / Unity / Lua 学习笔记与算法练习
- [unity-lab](https://github.com/qintangyuyang/unity-lab) — Unity 2022 练习工程（早期：2D 移动、碰撞、UI 锚点等基础练习）
