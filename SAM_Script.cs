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

/**
 * Notes:
 * - Musket is grouped as "Sniper" but occupies the Shotgun slot. [v]
 * - Exiting out of menu with 'Esc' opens in-game menu
 * - Ball under throwables [v]
 * - Feature: Option to delete or drop ammo when choosing 'Empty Ammo'
 * - Feature: Show Ammo Info
 * - Feature: Add All Weapons Menu [v]
 * - Can't set Mk2 ammo
 */

namespace SimpleAmmoManager
{
    [Serializable]
    public class SAM_Script : Script
    {
        // Script
        public static string VERSION = "v1.1.0";
        public static ScriptSettings scriptSettings;

        // Configurable Control Options
        public static Keys menuToggle;
        public static int defaultSetAmmoAmt;

        public SAM_Script()
        {
            // Important script stuff
            Tick += onTick;
            KeyDown += onKeyDown;
            KeyUp += onKeyUp;
            Aborted += onAborted;
            Interval = 0; // tick duration ms

            // Run on Execution
            if (IsExecuting)
            {
                // Load .ini
                Logger.Clear("SAM_Log");
                scriptSettings = ScriptSettings.Load("scripts\\SimpleAmmoManager\\SimpleAmmoManager.ini");
                menuToggle = scriptSettings.GetValue<Keys>("Settings", "Menu Toggle Key = ", Keys.F11);
                defaultSetAmmoAmt = scriptSettings.GetValue<int>("Settings", "Default [Set Ammo] Amount = ", 100);
                SAM_UI.initUI();
                SAM_WG.init();
            }

            SAM_UI.listener();
        }
        private void onTick(object sender, EventArgs e)
        {
            // Allows LemonUI to detect changes
            SAM_UI.pool.Process();
        }

        private void onKeyDown(object sender, KeyEventArgs e)
        {
            // Menu Visbility
            if (e.KeyCode == menuToggle)
            {
                if (!SAM_UI.pool.AreAnyVisible)
                    SAM_UI.mainMenu.Visible = true;
                else if (SAM_UI.mainMenu.Visible)
                    SAM_UI.mainMenu.Visible = false;
                else
                    SAM_UI.hideSubMenus();
            }
        }

        private void onKeyUp(object sender, KeyEventArgs e)
        {

        }

        private void onAborted(object sender, EventArgs e)
        {

        }
    }
}
