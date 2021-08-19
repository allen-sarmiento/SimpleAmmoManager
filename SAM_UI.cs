using GTA;
using GTA.Math;
using GTA.Native;
using GTA.NaturalMotion;
using GTA.UI;
using Screen = GTA.UI.Screen;

using LemonUI;
using LemonUI.Elements;
using LemonUI.Extensions;
using LemonUI.Menus;
using LemonUI.Scaleform;
using LemonUI.TimerBars;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

using SimpleAmmoManager.Weapon_Groups;

namespace SimpleAmmoManager
{
    public static class SAM_UI
    {
        // Pool
        public static ObjectPool pool = new ObjectPool();

        // Weapon Groups
        public static readonly List<WeaponGroup> SAM_WeaponGroups = new List<WeaponGroup>()
        { WeaponGroup.AssaultRifle, WeaponGroup.Heavy, WeaponGroup.MG, WeaponGroup.Pistol, WeaponGroup.Shotgun,
          WeaponGroup.SMG, WeaponGroup.Sniper, WeaponGroup.Thrown, WeaponGroup.FireExtinguisher, WeaponGroup.PetrolCan};

        // Menus
        public static NativeMenu mainMenu = new NativeMenu("Ammo Manager", "Main Menu");
        public static NativeMenu aboutMod_sMenu = new NativeMenu("Ammo Manager", "About Mod", "Settings, Author, Version");
        public static SAM_WG as_swg = new SAM_WG(WeaponGroup.AssaultRifle, "Assault Rifles");
        public static SAM_WG he_swg = new SAM_WG(WeaponGroup.Heavy, "Heavy Weapons");
        public static SAM_WG mg_swg = new SAM_WG(WeaponGroup.MG, "Machine Guns");
        public static SAM_WG pi_swg = new SAM_WG(WeaponGroup.Pistol, "Pistols");
        public static SAM_WG sh_swg = new SAM_WG(WeaponGroup.Shotgun, "Shotguns");
        public static SAM_WG sm_swg = new SAM_WG(WeaponGroup.SMG, "SMGs");
        public static SAM_WG sn_swg = new SAM_WG(WeaponGroup.Sniper, "Sniper Rifles");
        public static SAM_WG th_swg = new SAM_WG(WeaponGroup.Thrown, "Throwables");

        public static void initUI()
        {
            pool.Add(mainMenu);

            // Initialize Weapon List in each Weapon Group
            foreach (WeaponHash wHash in Enum.GetValues(typeof(WeaponHash)))
            {
                string wpn_str = wHash.ToString();
                switch (Function.Call<WeaponGroup>(Hash.GET_WEAPONTYPE_GROUP, wHash))
                {
                    case WeaponGroup.AssaultRifle:
                        as_swg.addWeapon(wpn_str);
                        break;
                    case WeaponGroup.Heavy:
                        if (wHash == WeaponHash.GrenadeLauncherSmoke) // Skip exception
                            continue;
                        he_swg.addWeapon(wpn_str);
                        break;
                    case WeaponGroup.MG:
                        mg_swg.addWeapon(wpn_str);
                        break;
                    case WeaponGroup.Pistol:
                        pi_swg.addWeapon(wpn_str);
                        break;
                    case WeaponGroup.Shotgun:
                        sh_swg.addWeapon(wpn_str);
                        break;
                    case WeaponGroup.SMG:
                        sm_swg.addWeapon(wpn_str);
                        break;
                    case WeaponGroup.Sniper:
                        sn_swg.addWeapon(wpn_str);
                        break;
                    case WeaponGroup.Thrown:
                        th_swg.addWeapon(wpn_str);
                        break;
                    case WeaponGroup.FireExtinguisher:
                        th_swg.addWeapon(wpn_str);
                        break;
                    case WeaponGroup.PetrolCan:
                        th_swg.addWeapon(wpn_str);
                        break;
                }
            }

            mainMenu.DisableControls = true;
            aboutMod_sMenu.DisableControls = true;
            foreach(SAM_WG swg in SAM_WG.SAM_WGs)
            {
                swg.nMenu.DisableControls = true;
            }
        }
    }
}
