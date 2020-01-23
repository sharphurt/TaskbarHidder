using System;
using System.ComponentModel;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

namespace TaskBarHidder
{
    public class TaskBarHidderForm : Form
    {
        private IContainer components;

        public static NotifyIcon NotifyIcon;
        private ContextMenuStrip _contextMenu;
        private ToolStripMenuItem _exitMenuItem;
        private Label _changingHotKeysHeaderLabel;
        public static Button[] KeyButtons = new Button[2];
        private Label _plusLabel;
        public static Panel ChangeHotKeyPanel;
        public static Button SaveChangesButton;
        private Button _changeHotKeyModeButton;
        public static CheckBox AutorunCheckBox;
        private Timer _timer;

        public TaskBarHidderForm()
        {
            FormEventHandlers.Form = this;
            InitializeForm();
            InitializeComponents();
            InitializeEvents();
        }

        private void InitializeForm()
        {
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(274, 105);
            Margin = new Padding(4, 5, 4, 5);
            Name = "TaskbarHidder";
            Text = "Taskbar Hidder App";
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = new Icon("tbH.ico");
            WindowState = FormWindowState.Minimized;
        }
        
        private void InitializeComponents()
        {
            components = new Container();

            _contextMenu = new ContextMenuStrip(components)
            {
                ImageScalingSize = new Size(32, 32),
                Size = new Size(103, 40)
            };

            NotifyIcon = new NotifyIcon(components)
            {
                ContextMenuStrip = _contextMenu,
                Icon = new Icon("pinned.ico"),
                Text = "TaskBar Hidder",
                Visible = true
            };

            _exitMenuItem = new ToolStripMenuItem
            {
                Size = new Size(103, 40),
                Text = "Exit"
            };

            _timer = new Timer
            {
                Enabled = true,
                Interval = 1000
            };

            _changingHotKeysHeaderLabel = new Label
            {
                Location = new Point(60, 0),
                Size = new Size(209, 23),
                TabIndex = 2,
                Text = "Настрока сочетания клавиш"
            };

            KeyButtons[0] = new Button
            {
                Location = new Point(20, 26),
                Size = new Size(96, 30),
                TabIndex = 0,
                Text = "Key #1",
                UseVisualStyleBackColor = true
            };

            KeyButtons[1] = new Button
            {
                Location = new Point(157, 26),
                Size = new Size(96, 30),
                TabIndex = 3,
                Text = "Key #2",
                UseVisualStyleBackColor = true
            };

            SaveChangesButton = new Button
            {
                Location = new Point(157, 66),
                Size = new Size(96, 30),
                TabIndex = 7,
                Text = "Сохранить",
                Visible = false,
                UseVisualStyleBackColor = true,
            };

            _plusLabel = new Label
            {
                Font = new Font("Segoe UI", 13.8F, FontStyle.Regular,
                    GraphicsUnit.Point, ((byte) (204))),
                Location = new Point(125, 26),
                Size = new Size(26, 26),
                TabIndex = 4,
                Text = "+",
            };

            AutorunCheckBox = new CheckBox
            {
                Location = new Point(12, 77),
                Size = new Size(138, 26),
                TabIndex = 6,
                Text = "Автозагрузка",
                UseVisualStyleBackColor = true
            };

            _changeHotKeyModeButton = new Button
            {
                Location = new Point(3, 3),
                Margin = new Padding(3, 2, 3, 2),
                Size = new Size(269, 30),
                TabIndex = 5,
                Text = "Настроить сочетание клавиш",
                UseVisualStyleBackColor = true
            };

            ChangeHotKeyPanel = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(274, 72),
                TabIndex = 5,
                Visible = false
            };

            ChangeHotKeyPanel.Controls.Add(_changingHotKeysHeaderLabel);
            ChangeHotKeyPanel.Controls.Add(_plusLabel);
            ChangeHotKeyPanel.Controls.Add(KeyButtons[0]);
            ChangeHotKeyPanel.Controls.Add(KeyButtons[1]);


            Controls.Add(SaveChangesButton);
            Controls.Add(AutorunCheckBox);
            Controls.Add(ChangeHotKeyPanel);
            Controls.Add(_changeHotKeyModeButton);

            _contextMenu.Items.AddRange(new ToolStripItem[] {_exitMenuItem});

            ChangeHotKeyPanel.SuspendLayout();
            ChangeHotKeyPanel.ResumeLayout(false);
            _contextMenu.SuspendLayout();
            _contextMenu.ResumeLayout(false);
            SuspendLayout();
            ResumeLayout(false);

            Icons.UpdateIcon();
        }

        private void InitializeEvents()
        {
            Load += FormEventHandlers.TaskBarHidderFormLoad;
            Resize += FormEventHandlers.WindowResize;
            NotifyIcon.MouseDoubleClick += FormEventHandlers.NotifyIconClick;
            _exitMenuItem.Click += FormEventHandlers.ExitMenuItemClick;
            _timer.Tick += FormEventHandlers.TimerTick;
            _changeHotKeyModeButton.Click += FormEventHandlers.ChangeHotKeyModeButtonClick;
            KeyButtons[0].Click += FormEventHandlers.ChangeKey1ButtonClick;
            KeyButtons[1].Click += FormEventHandlers.ChangeKey2ButtonClick;
            SaveChangesButton.Click += FormEventHandlers.SaveChangesButtonClick;
            AutorunCheckBox.CheckStateChanged += FormEventHandlers.AutorunCheckboxChanged;
        }
    }
}