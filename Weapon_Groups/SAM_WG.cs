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

namespace SimpleAmmoManager.Weapon_Groups
{
    public class SAM_WG
    {
        // Static Members
        public static List<SAM_WG> SAM_WGs = new List<SAM_WG>();

        // Instance Members
        public NativeMenu nMenu;
        public NativeListItem<string> nListItem;
        public WeaponGroup wpnGrp;
        private List<string> tempList;

        public SAM_WG(WeaponGroup wpnGrp, string title)
        {
            SAM_WGs.Add(this); // Update class list
            this.wpnGrp = wpnGrp; // Set weapon group
            nMenu = new NativeMenu("Ammo Manager", title); // Set display title
            nListItem = new NativeListItem<string>("Weapon"); // Set list item to empty list
            tempList = new List<string>();

            // Update UI
            SAM_UI.pool.Add(nMenu);
            SAM_UI.mainMenu.AddSubMenu(nMenu);
            nMenu.Add(nListItem);
        }

        public void addWeapon(string wHash)
        {
            tempList.Add(GetDisplayName(wHash));
        }

        public static void init()
        {
            foreach (SAM_WG swg in SAM_WGs)
            {
                swg.nListItem.Items = swg.tempList;
            }
        }

        private static string GetDisplayName(string str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                Console.WriteLine(i);
                if (Char.IsUpper(str[i])) // If Upper
                {
                    // If succeeded by lower
                    if (i > 0 && i < str.Length - 1 && Char.IsLower(str[i + 1]))
                    {
                        str = str.Insert(i, " ");
                        i++;
                    }
                    // If preceeded by lower
                    if (i > 0 && i < str.Length && Char.IsLower(str[i - 1]))
                    {
                        str = str.Insert(i, " ");
                        i++;
                    }

                }
            }
            return str;
        }
    }
}
