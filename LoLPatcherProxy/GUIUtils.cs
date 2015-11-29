using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoLPatcherProxy
{
    public static class GUIUtils
    {
        private static List<Control> _locked;
        private static Task _current;
        private static string _title;
        private static Queue<Action> _queue = new Queue<Action>();
        public static bool Locked = false;

        public static void Interrupt(this Form f)
        {

            
            Locked = false;
            f.Invoke((Action)delegate
            {
                f.Text = _title;
                f.Lock(false);
            });
        }

        public static Task PerformTask(this Form f, Action action)
        {
            if(Locked)
            {
                action();
            }
            Locked = true;
            _title = f.Text;
            f.Invoke((Action)delegate { f.Text += " | Working..."; f.Lock(true); });
            
            return _current = Task.Factory.StartNew(action).ContinueWith((t) =>
            {
                f.Interrupt();

            });

            
        }

        public static void Lock(this Form f, bool value)
        {
            if(value)
            {
                if (_locked != null)
                    f.Lock(false);
                _locked = new List<Control>();
                lock (_locked)
                {
                    foreach (Control c in f.Controls)
                    {
                        if (c.Enabled)
                        {
                            f.Invoke((Action)delegate { c.Enabled = false; });
                            _locked.Add(c);
                        }
                    }
                }
            }
            else
            {
                if (_locked != null)
                {
                    try
                    {
                        lock (_locked)
                        {
                            foreach (Control c in _locked)
                            {
                                f.Invoke((Action)delegate { c.Enabled = true; });
                            }
                            
                        }
                        _locked = null;
                    }
                    catch { }
                }
            }
        }

        public static void SelectItem(this ComboBox c, string item)
        {
            c.Invoke((Action)delegate 
            {
                c.SelectedIndex = c.Items.IndexOf(item);
            });
        }

    }
}
