using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Collections;

/*
	if you want to use this class,you need to add this string
	"using AllHotkey;" and add this class to the project
*/

namespace AllHotkey
{
    public delegate void HotkeyEventHandler(int HotKeyID);

    public class Hotkey : System.Windows.Forms.IMessageFilter
    {
        Hashtable keyIDs = new Hashtable();
        IntPtr hWnd;

        static public int Hotkey1;
        static public int Hotkey2;
        static public int Hotkey3;
        static public int Hotkey4;
		static public int Hotkey5;
		static public int Hotkey6;
		static public int Hotkey7;
		

        public event HotkeyEventHandler OnHotkey;

        public enum KeyFlags
        {
            MOD_ALT = 0x1,
            MOD_CONTROL = 0x2,
            MOD_SHIFT = 0x4,
            MOD_WIN = 0x8,
			MOD_None=0x00
        }
        [DllImport("user32.dll")]
        public static extern UInt32 RegisterHotKey(IntPtr hWnd, UInt32 id, UInt32 fsModifiers, UInt32 vk);

        [DllImport("user32.dll")]
        public static extern UInt32 UnregisterHotKey(IntPtr hWnd, UInt32 id);

        [DllImport("kernel32.dll")]
        public static extern UInt32 GlobalAddAtom(String lpString);

        [DllImport("kernel32.dll")]
        public static extern UInt32 GlobalDeleteAtom(UInt32 nAtom);

        public Hotkey(IntPtr hWnd)
        {
            this.hWnd = hWnd;
            Application.AddMessageFilter(this);
        }

        public int RegisterHotkey(Keys Key, KeyFlags keyflags)
        {
            UInt32 hotkeyid = GlobalAddAtom(System.Guid.NewGuid().ToString());
            RegisterHotKey((IntPtr)hWnd, hotkeyid, (UInt32)keyflags, (UInt32)Key);
            keyIDs.Add(hotkeyid, hotkeyid);
            return (int)hotkeyid;
        }

        public void UnregisterHotkeys()
        {
            Application.RemoveMessageFilter(this);
            foreach (UInt32 key in keyIDs.Values)
            {
                UnregisterHotKey(hWnd, key);
                GlobalDeleteAtom(key);
            }
        }

        public bool PreFilterMessage(ref System.Windows.Forms.Message m)
        {
            if (m.Msg == 0x312)
            {
                if (OnHotkey != null)
                {
                    foreach (UInt32 key in keyIDs.Values)
                    {
                        if ((UInt32)m.WParam == key)
                        {
                            OnHotkey((int)m.WParam);
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
/*
		private void Form1_Load(object sender, EventArgs e)
        {
            //------------------------------------------------------
            AllHotkey.Hotkey hokey1 = new AllHotkey.Hotkey(this.Handle);
            AllHotkey.Hotkey.Hotkey1 = hokey1.RegisterHotkey(System.Windows.Forms.Keys.F2, AllHotkey.Hotkey.KeyFlags.MOD_CONTROL);
            hokey1.OnHotkey += new AllHotkey.HotkeyEventHandler(OnHotkey);

            AllHotkey.Hotkey hotkey2 = new AllHotkey.Hotkey(this.Handle);
            AllHotkey.Hotkey.Hotkey2 = hokey1.RegisterHotkey(System.Windows.Forms.Keys.F6, AllHotkey.Hotkey.KeyFlags.MOD_CONTROL);
            hotkey2.OnHotkey += new AllHotkey.HotkeyEventHandler(OnHotkey);

            AllHotkey.Hotkey hotkey3 = new AllHotkey.Hotkey(this.Handle);
            AllHotkey.Hotkey.Hotkey3 = hokey1.RegisterHotkey(System.Windows.Forms.Keys.F12, AllHotkey.Hotkey.KeyFlags.MOD_CONTROL);
            hotkey3.OnHotkey += new AllHotkey.HotkeyEventHandler(OnHotkey);

            AllHotkey.Hotkey hotkey4 = new AllHotkey.Hotkey(this.Handle);
            AllHotkey.Hotkey.Hotkey4 = hokey1.RegisterHotkey(System.Windows.Forms.Keys.F7, AllHotkey.Hotkey.KeyFlags.MOD_CONTROL);
            hotkey3.OnHotkey += new AllHotkey.HotkeyEventHandler(OnHotkey);
            //------------------------------------------------------
        }

        public void OnHotkey(int HotkeyID)
        {
            if (HotkeyID == Hotkey.Hotkey1)
            {
                
            }

            if (HotkeyID == Hotkey.Hotkey2)
            {
                
            }

            if (HotkeyID == Hotkey.Hotkey3)
            {
                
            }

            if (HotkeyID == Hotkey.Hotkey4)
            {
                
            }
        }
*/
