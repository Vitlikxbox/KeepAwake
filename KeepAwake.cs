using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

class KeepAwake : Form
{
    [DllImport("kernel32.dll")] static extern uint SetThreadExecutionState(uint esFlags);

    const uint ES_CONTINUOUS = 0x80000000, ES_SYSTEM_REQUIRED = 0x1, ES_DISPLAY_REQUIRED = 0x2;
    static readonly int[] Minutes = { 0, 15, 30, 60, 120, 240, 480 };

    Label statusLabel, infoLabel;
    ComboBox durationBox;
    CheckBox screenBox;
    Button toggleButton;
    NotifyIcon tray;
    Timer timer;
    bool active;
    DateTime? until;

    KeepAwake()
    {
        Text = "KeepAwake";
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        ClientSize = new Size(300, 210);
        Font = new Font("Segoe UI", 10);
        StartPosition = FormStartPosition.CenterScreen;

        statusLabel = new Label { Font = new Font("Segoe UI", 14, FontStyle.Bold), Location = new Point(15, 12), AutoSize = true };
        infoLabel = new Label { ForeColor = Color.Gray, Location = new Point(15, 45), AutoSize = true };
        Controls.Add(statusLabel);
        Controls.Add(infoLabel);

        Controls.Add(new Label { Text = "Длительность:", Location = new Point(15, 83), AutoSize = true });
        durationBox = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(130, 80), Width = 155 };
        durationBox.Items.AddRange(new object[] { "Бессрочно", "15 минут", "30 минут", "1 час", "2 часа", "4 часа", "8 часов" });
        durationBox.SelectedIndex = 0;
        Controls.Add(durationBox);

        screenBox = new CheckBox { Text = "Не выключать экран", Checked = true, Location = new Point(15, 117), AutoSize = true };
        screenBox.CheckedChanged += (s, e) => { if (active) Apply(); };
        Controls.Add(screenBox);

        toggleButton = new Button { Location = new Point(15, 152), Size = new Size(270, 42) };
        toggleButton.Click += (s, e) => { if (active) TurnOff(); else TurnOn(); };
        Controls.Add(toggleButton);

        var menu = new ContextMenuStrip();
        menu.Items.Add("Вкл / Выкл", null, (s, e) => { if (active) TurnOff(); else TurnOn(); });
        menu.Items.Add("Показать окно", null, (s, e) => ShowWindow());
        menu.Items.Add("Выход", null, (s, e) => Close());
        tray = new NotifyIcon { Icon = SystemIcons.Shield, Visible = true, ContextMenuStrip = menu };
        tray.DoubleClick += (s, e) => ShowWindow();

        timer = new Timer { Interval = 1000 };
        timer.Tick += (s, e) => Tick();

        Resize += (s, e) => { if (WindowState == FormWindowState.Minimized) Hide(); };
        UpdateUi();
    }

    void ShowWindow()
    {
        Show();
        WindowState = FormWindowState.Normal;
        Activate();
    }

    void TurnOn()
    {
        active = true;
        int m = Minutes[durationBox.SelectedIndex];
        until = m > 0 ? (DateTime?)DateTime.Now.AddMinutes(m) : null;
        Apply();
        timer.Start();
        UpdateUi();
    }

    void TurnOff()
    {
        active = false;
        until = null;
        SetThreadExecutionState(ES_CONTINUOUS);
        timer.Stop();
        UpdateUi();
    }

    void Apply()
    {
        uint flags = ES_CONTINUOUS | ES_SYSTEM_REQUIRED;
        if (screenBox.Checked) flags |= ES_DISPLAY_REQUIRED;
        SetThreadExecutionState(flags);
    }

    void Tick()
    {
        if (until.HasValue && DateTime.Now >= until.Value)
        {
            TurnOff();
            tray.ShowBalloonTip(3000, "KeepAwake", "Время вышло: компьютер снова может засыпать", ToolTipIcon.Info);
            return;
        }
        Apply();
        UpdateUi();
    }

    void UpdateUi()
    {
        durationBox.Enabled = !active;
        toggleButton.Text = active ? "Выключить" : "Включить";
        if (!active)
        {
            statusLabel.Text = "Выключено";
            statusLabel.ForeColor = Color.Gray;
            infoLabel.Text = "Компьютер засыпает как обычно";
            tray.Text = "KeepAwake: выключено";
            return;
        }
        statusLabel.Text = "Не даю уснуть";
        statusLabel.ForeColor = Color.ForestGreen;
        infoLabel.Text = until.HasValue
            ? "Осталось " + (until.Value - DateTime.Now).ToString(@"hh\:mm\:ss")
            : "Работает бессрочно";
        tray.Text = "KeepAwake: " + infoLabel.Text.ToLower();
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        SetThreadExecutionState(ES_CONTINUOUS);
        tray.Visible = false;
        tray.Dispose();
        base.OnFormClosing(e);
    }

    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.Run(new KeepAwake());
    }
}
