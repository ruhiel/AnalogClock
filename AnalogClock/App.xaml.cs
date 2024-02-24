using System;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Threading;
using System.Windows;
using System.Windows.Forms;

namespace AnalogClock
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private Mutex? _Mutex;
        private MainWindow? _Window = null;  //２重起動防止用
        //常駐終了時に開放するために保存しておく
        private ContextMenuStrip? _Menu = null;
        //常駐終了時に開放するために保存しておく
        private NotifyIcon? _NotifyIcon = null;

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            if (_Mutex == null)
            {
                return;
            }

            // ミューテックスの開放
            _Mutex.ReleaseMutex();
            _Mutex.Close();
            _Mutex = null;
        }

        /// <summary>
        /// 常駐開始時の初期化処理
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            var hasHandle = false;

            // 初期所有権なしでMutexを生成
            // Mutex名を固有にするためGUIDを使用する
            // GUIDはVisual studio > [ツール] > [GUIDの生成]から生成
            _Mutex = new Mutex(false, "{46DD35B8-506A-421F-BBF0-E464BC2DC944}");

            //---------------------------------------------------------
            // 二重起動のチェック
            //---------------------------------------------------------
            try
            {
                // Mutexの所有権を要求
                hasHandle = _Mutex.WaitOne(0, false);
            }
            catch (AbandonedMutexException)
            {
                // 別アプリがMutexオブジェクトを開放しないで終了した場合
                hasHandle = true;
            }

            if (hasHandle == false)
            {
                // 所有権が得られなかった場合、起動済みと判断して終了
                System.Windows.MessageBox.Show("二重起動のため、プログラムを終了します");
                _Mutex.Close();
                _Mutex = null;
                Shutdown();
                return;
            }

            //---------------------------------------------------------
            // アプリケーション開始
            //---------------------------------------------------------
            //継承元のOnStartupを呼び出す
            base.OnStartup(e);

            //アイコンの取得
            var icon = GetResourceStream(new Uri(@"Image\image.ico", UriKind.Relative)).Stream;

            //コンテキストメニューを作成
            _Menu = CreateMenu();

            //通知領域にアイコンを表示

            _NotifyIcon = new NotifyIcon
            {
                Visible = true,
                Icon = new Icon(icon),
                Text = System.Diagnostics.Process.GetCurrentProcess().ProcessName,
                ContextMenuStrip = _Menu
            };

            //アイコンがクリックされたら設定画面を表示
            _NotifyIcon.MouseClick += (s, er) =>
            {
                if (er.Button == MouseButtons.Left)
                {
                    ShowMainWindow();
                }
            };

            ShowMainWindow();
        }

        /// <summary>
        /// 設定画面を表示
        /// </summary>
        private void ShowMainWindow()
        {
            if (_Window == null)
            {
                _Window = new MainWindow();

                //ウィンドウを画面中央に表示
                _Window.WindowStartupLocation = WindowStartupLocation.CenterScreen;

                //Windowsを表示する
                _Window.Show();
            }
            else
            {
                //Windowsを表示する
                _Window.Show();
            }
        }

        /// <summary>
        /// コンテキストメニューの表示
        /// </summary>
        /// <returns></returns>
        private ContextMenuStrip CreateMenu()
        {
            var menu = new ContextMenuStrip();
            menu.Items.Add($"{System.Diagnostics.Process.GetCurrentProcess().ProcessName}を終了", null, (s, e) => { Shutdown(); });
            return menu;
        }
    }
}
