﻿using InjectionDll;
using InjectionDll.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheatDxX
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            if (_InjectProcess == null)
            {
                buttonLoad.Enabled = false;
                AttachProcess();
            }
            else
            {
                _InjectProcess.CaptureInterface.Disconnect();
                _InjectProcess = null;
            }
            if (_InjectProcess != null)
            {
                buttonLoad.Text = "Detach";
                buttonLoad.Enabled = true;
            }
            else
            {
                buttonLoad.Text = "Inject";
                buttonLoad.Enabled = true;
            }
        }

        MainClass _InjectProcess;
        private void AttachProcess()
        {
            string exeName = "minitri";
            Process[] processes = Process.GetProcessesByName(exeName);
            foreach (Process process in processes)
            {
                if (process.MainWindowHandle == IntPtr.Zero)
                {
                    continue;
                }

                // Skip if the process is already hooked (and we want to hook multiple applications)
                //if (HookManager.IsHooked(process.Id))
                //{
                //    continue;
                //}

                Direct3DVersion direct3DVersion = Direct3DVersion.AutoDetect;

                ConfigDll c = new ConfigDll()
                {
                    Direct3DVersion = direct3DVersion,
                    ShowOverlay = true
                };

                var interfaceDll = new InterfaceDll();
                interfaceDll.RemoteMessage += InterfaceDll_RemoteMessage;

                _InjectProcess = new MainClass(process, c, interfaceDll);
                break;
            }
        }

        private void InterfaceDll_RemoteMessage(MessageReceivedEventArgs message)
        {
            txtDebugLog.Invoke(new MethodInvoker(delegate ()
                {
                    txtDebugLog.Text = String.Format("{0}\r\n{1}", message, txtDebugLog.Text);
                })
            );
        }
    }
}
