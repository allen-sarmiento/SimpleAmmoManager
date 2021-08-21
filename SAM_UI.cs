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
        public static NativeMenu aboutMod_sMenu = new NativeMenu("Ammo Manager", "About Mod", "Author, Version");

        // Current SubMenu
        public static NativeItem weapon = new NativeItem("Weapon:", "", "Null");
        public static NativeItem fullAmmo = new NativeItem("Full Ammo");
        public static NativeItem emptyAmmo = new NativeItem("Empty Ammo");
        public static NativeItem setAmmo = new NativeItem("Set Ammo");
        public static List<NativeItem> current_sMenu_Items = new List<NativeItem>() { weapon, fullAmmo, emptyAmmo, setAmmo };

        // About Mod SubMenu
        // public static NativeMenu settings_sMenu = new NativeMenu("About Mod", "Settings");
        public static NativeItem author = new NativeItem("Author:", "", "mathcaicedtea");
        public static NativeItem version = new NativeItem("Version:", "", $"{SAM_Script.VERSION}");

        public static List<NativeMenu> sMenuList = new List<NativeMenu>() { current_sMenu, aboutMod_sMenu };

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
                switch (Function.Call<WeaponGroup>(Hash.GET_WEAPONTYPE_GROUP, wHash))
                {
                    case WeaponGroup.AssaultRifle:
                        as_swg.addWeapon(wHash);
                        break;
                    case WeaponGroup.Heavy:
                        if (wHash == WeaponHash.GrenadeLauncherSmoke) // Skip exception
                            continue;
                        he_swg.addWeapon(wHash);
                        break;
                    case WeaponGroup.MG:
                        mg_swg.addWeapon(wHash);
                        break;
                    case WeaponGroup.Pistol:
                        pi_swg.addWeapon(wHash);
                        break;
                    case WeaponGroup.Shotgun:
                        if (wHash == WeaponHash.Musket) // Correct exception
                        {
                            sn_swg.addWeapon(wHash);
                            break;
                        }
                        sh_swg.addWeapon(wHash);
                        break;
                    case WeaponGroup.SMG:
                        sm_swg.addWeapon(wHash);
                        break;
                    case WeaponGroup.Sniper:
                        sn_swg.addWeapon(wHash);
                        break;
                    case WeaponGroup.Thrown:
                        if (wHash == WeaponHash.Ball || wHash == WeaponHash.BZGas) // Skip exception
                            continue;
                        th_swg.addWeapon(wHash);
                        break;
                    case WeaponGroup.FireExtinguisher:
                        th_swg.addWeapon(wHash);
                        break;
                    case WeaponGroup.PetrolCan:
                        th_swg.addWeapon(wHash);
                        break;
                }
            }
        }

        private static void initAboutMod_sMenu()
        {
            // pool.Add(settings_sMenu);
            // aboutMod_sMenu.AddSubMenu(settings_sMenu);
            // initSettings_sMenu();

            aboutMod_sMenu.Add(author);
            aboutMod_sMenu.Add(version);
        }

        private static void initSettings_sMenu()
        {
            
        }

        public static void hideSubMenus()
        {
            foreach (NativeMenu sMenu in sMenuList)
            {
                if (sMenu.Visible)
                    sMenu.Visible = false;
            }
            foreach (NativeMenu sMenu in SAM_WG.nMenuList)
            {
                if (sMenu.Visible)
                    sMenu.Visible = false;
            }
        }

        public static void listener()
        {
            updateCurrent_sMenu();
            updateWeapon_sMenu();
        }

        private static void updateWeapon_sMenu()
        {
            foreach (SAM_WG swg in SAM_WG.SAM_WGs)
            {
                swg.weaponList.ItemChanged += (o, e) =>
                {
                    checkWeaponOwnership(swg);
                };

                swg.nMenu.Opening += (o, e) =>
                {
                    checkWeaponOwnership(swg);
                };
                swg.fullAmmo.Activated += (o, e) =>
                {
                    WeaponHash wHash = swg.wHashList.ElementAt(swg.weaponList.SelectedIndex);
                    Function.Call(Hash.SET_PED_AMMO, Game.Player.Character, wHash, 9999);
                };
                swg.emptyAmmo.Activated += (o, e) =>
                {
                    WeaponHash wHash = swg.wHashList.ElementAt(swg.weaponList.SelectedIndex);
                    Function.Call(Hash.SET_PED_AMMO, Game.Player.Character, wHash, 0);
                };
                swg.setAmmo.Activated += (o, e) =>
                {
                    WeaponHash wHash = swg.wHashList.ElementAt(swg.weaponList.SelectedIndex);
                    string input = Game.GetUserInput(WindowTitle.EnterSynopsis, $"{SAM_Script.defaultSetAmmoAmt}", 3);
                    if (Int16.TryParse(input, out short amt))
                    {
                        Function.Call(Hash.SET_PED_AMMO, Game.Player.Character, wHash, amt);
                        Audio.PlaySoundFrontend("WEAPON_AMMO_PURCHASE", "HUD_AMMO_SHOP_SOUNDSET");
                    }
                    else
                    {
                        int inputErr = Notification.Show(NotificationIcon.Blocked, "AmmoManager", "Invalid Input", "Numerical input only.", true, true);
                        Audio.PlaySoundFrontend("ERROR", "HUD_FRONTEND_DEFAULT_SOUNDSET");
                    }
                };
            }
        }

        private static void checkWeaponOwnership(SAM_WG swg)
        {
            WeaponHash wHash = swg.wHashList.ElementAt(swg.weaponList.SelectedIndex); // Get weapon hash
            if (!Function.Call<bool>(Hash.HAS_PED_GOT_WEAPON, Game.Player.Character, wHash, false))
            {
                foreach (NativeItem item in swg.items)
                {
                    if (item != swg.weaponList)
                        item.Enabled = false;
                }
            }
            else
            {
                foreach (NativeItem item in swg.items)
                {
                    if (item != swg.weaponList)
                        item.Enabled = true;
                }
            }
        }

        private static void updateCurrent_sMenu()
        {
            current_sMenu.Opening += (o, e) =>
            {
                WeaponHash wHash = Function.Call<WeaponHash>(Hash.GET_SELECTED_PED_WEAPON, Game.Player.Character); // Get current weapon

                // Update alt. title & enable/disable elements
                if (Function.Call<bool>(Hash.IS_PED_ARMED, Game.Player.Character, 6))
                {
                    weapon.AltTitle = $"{SAM_WG.GetDisplayName(wHash.ToString())}";
                    foreach (NativeItem item in current_sMenu_Items)
                        item.Enabled = true;
                }
                else
                {
                    weapon.AltTitle = "N/A";
                    foreach (NativeItem item in current_sMenu_Items)
                        item.Enabled = false;
                }
                current_sMenu.Recalculate();
            };

            fullAmmo.Activated += (o, e) =>
            {
                WeaponHash wHash = Function.Call<WeaponHash>(Hash.GET_SELECTED_PED_WEAPON, Game.Player.Character);
                Function.Call(Hash.SET_PED_AMMO, Game.Player.Character, wHash, 9999);
            };
            emptyAmmo.Activated += (o, e) =>
            {
                WeaponHash wHash = Function.Call<WeaponHash>(Hash.GET_SELECTED_PED_WEAPON, Game.Player.Character);
                Function.Call(Hash.SET_PED_AMMO, Game.Player.Character, wHash, 0);
            };
            setAmmo.Activated += (o, e) =>
            {
                WeaponHash wHash = Function.Call<WeaponHash>(Hash.GET_SELECTED_PED_WEAPON, Game.Player.Character);
                string input = Game.GetUserInput(WindowTitle.EnterSynopsis, $"{SAM_Script.defaultSetAmmoAmt}", 3);
                if (Int16.TryParse(input, out short amt))
                {
                    Function.Call(Hash.SET_PED_AMMO, Game.Player.Character, wHash, amt);
                    Audio.PlaySoundFrontend("WEAPON_AMMO_PURCHASE", "HUD_AMMO_SHOP_SOUNDSET");
                }
                else
                {
                    int inputErr = Notification.Show(NotificationIcon.Blocked, "AmmoManager", "Invalid Input", "Numerical input only.", true, true);
                    Audio.PlaySoundFrontend("ERROR", "HUD_FRONTEND_DEFAULT_SOUNDSET");
                }
            };
        }

        /// <summary>
        /// Gets clip size and max ammo for given weapon hash.
        /// </summary>
        /// <returns></returns>
        private static int[] getAmmoInfo(WeaponHash wHash)
        {
            int max = Function.Call<int>(Hash.GET_MAX_AMMO_IN_CLIP, Game.Player.Character, wHash, 1);
            int clip = Function.Call<int>(Hash.GET_MAX_AMMO, Game.Player.Character, wHash);
            return new int[] {max, clip};
        }
    }
}
