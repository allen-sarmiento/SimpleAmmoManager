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

        // Menus
        public static NativeMenu mainMenu = new NativeMenu("Ammo Manager", "Main Menu");
        public static NativeMenu current_sMenu = new NativeMenu("Ammo Manager", "Current Weapon");
        public static SAM_WG as_swg;
        public static SAM_WG he_swg;
        public static SAM_WG mg_swg;
        public static SAM_WG pi_swg;
        public static SAM_WG sh_swg;
        public static SAM_WG sm_swg;
        public static SAM_WG sn_swg;
        public static SAM_WG th_swg;
        public static NativeMenu aboutMod_sMenu = new NativeMenu("Ammo Manager", "About Mod", "Settings, Author, Version");

        // Current SubMenu
        public static NativeItem weapon = new NativeItem("Weapon:", "", "Null");
        public static NativeItem ammo = new NativeItem("Ammo");
        public static NativeItem fullAmmo = new NativeItem("Full Ammo");
        public static NativeItem emptyAmmo = new NativeItem("Empty Ammo");
        public static NativeItem setAmmo = new NativeItem("Set Ammo");
        public static List<NativeItem> current_sMenu_Items = new List<NativeItem>() { weapon, ammo, fullAmmo, emptyAmmo, setAmmo };

        // About Mod SubMenu
        public static NativeMenu settings_sMenu = new NativeMenu("About Mod", "Settings");
        public static NativeItem author = new NativeItem("Author:", "", "mathcaicedtea");
        public static NativeItem version = new NativeItem("Version:", "", $"{SAM_Script.VERSION}");

        public static void initUI()
        {
            pool.Add(mainMenu);

            pool.Add(current_sMenu);
            mainMenu.AddSubMenu(current_sMenu);
            initCurrent_sMenu();

            initWeapons_sMenu();

            pool.Add(aboutMod_sMenu);
            mainMenu.AddSubMenu(aboutMod_sMenu);
            initAboutMod_sMenu();
        }

        private static void initCurrent_sMenu()
        {
            current_sMenu.Add(weapon);
            current_sMenu.Add(ammo);
            current_sMenu.Add(fullAmmo);
            current_sMenu.Add(emptyAmmo);
            current_sMenu.Add(setAmmo);
        }

        private static void initWeapons_sMenu()
        {
            as_swg = new SAM_WG(WeaponGroup.AssaultRifle, "Assault Rifles");
            he_swg = new SAM_WG(WeaponGroup.Heavy, "Heavy Weapons");
            mg_swg = new SAM_WG(WeaponGroup.MG, "Machine Guns");
            pi_swg = new SAM_WG(WeaponGroup.Pistol, "Pistols");
            sh_swg = new SAM_WG(WeaponGroup.Shotgun, "Shotguns");
            sm_swg = new SAM_WG(WeaponGroup.SMG, "SMGs");
            sn_swg = new SAM_WG(WeaponGroup.Sniper, "Sniper Rifles");
            th_swg = new SAM_WG(WeaponGroup.Thrown, "Throwables/Other");

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
                        if (wHash == WeaponHash.Musket) // Correct exception
                        {
                            sn_swg.addWeapon(wpn_str);
                            break;
                        }
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
        }

        private static void initAboutMod_sMenu()
        {
            pool.Add(settings_sMenu);
            aboutMod_sMenu.AddSubMenu(settings_sMenu);
            initSettings_sMenu();

            aboutMod_sMenu.Add(author);
            aboutMod_sMenu.Add(version);
        }

        private static void initSettings_sMenu()
        {
            
        }

        public static void listener()
        {

        }

        public static void update()
        {
            // Current SubMenu
            current_sMenu.Opening += (o, e) =>
            {
                
                string wHash = Function.Call<WeaponHash>(Hash.GET_SELECTED_PED_WEAPON, Game.Player.Character).ToString(); // Get current weapon
                // Update alt. title
                if (Function.Call<bool>(Hash.IS_PED_ARMED, Game.Player.Character, 6))
                {
                    weapon.AltTitle = $"{SAM_WG.GetDisplayName(wHash)}";
                }
                
                else
                {
                    weapon.AltTitle = "N/A";
                    foreach(NativeItem item in current_sMenu_Items)
                    {
                        item.Enabled = false;
                        item.RightBadge = new ScaledTexture("commonmenu", "shop_lock");
                    }
                }
                current_sMenu.Recalculate();
                // Screen.ShowSubtitle("Here", 1000);
            };
        }
    }
}
