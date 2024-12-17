//Version 1.0.0.2
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DrawingSize = System.Drawing.Size;
using DrawingPoint = System.Drawing.Point;
using MessageBox = System.Windows.Forms.MessageBox;
using main;

#pragma warning disable CS1591  // Missing XML comment for publicly visible type
// or member

namespace main
{

	public class ProgramInfo
	{
		public string Path { get; }
		public string Title { get; }
		public string ClassName { get; }
		public WOwner Owner { get; }

		// Конструктор
		public ProgramInfo(string path
				, string title
				, string className
				, WOwner owner = default)
		{
			Title = title;
			ClassName = className;
			Owner = owner;
			Path = path;
		}
	}

	public static class WindowHelper
	{
		public static string lastErrorMessage = "";
		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

		[StructLayout(LayoutKind.Sequential)]
		public struct RECT
		{
			public int Left;
			public int Top;
			public int Right;
			public int Bottom;
		}


		public static bool MoveWindowLeft(int dx, int dy, ProgramInfo programInfo)
		{
			int screenWidth;
			int screenHeight;
			bool result = true;

			try
			{
				var hwnd = wnd.find(1, programInfo.Title
									, programInfo.ClassName
									, programInfo.Owner).Activate();
				// Получаем размеры экрана
				screenWidth = Screen.PrimaryScreen.Bounds.Width - dx;
				screenHeight = Screen.PrimaryScreen.Bounds.Height - dy;
				// Располагаем w1 в левой части экрана
				hwnd.Move(0, 0, screenWidth / 2, screenHeight);
			}
			catch (Exception ex)
			{
				lastErrorMessage = ex.Message;
				result = false;
			}
			return result;
		}

		public static bool MoveWindowRight(int dx, int dy, ProgramInfo programInfo)
		{
			int screenWidth;
			int screenHeight;
			bool result = true;

			try
			{
				var hwnd = wnd.find(1, programInfo.Title
									, programInfo.ClassName
									, programInfo.Owner).Activate();
				// Получаем размеры экрана
				screenWidth = Screen.PrimaryScreen.Bounds.Width - dx;
				screenHeight = Screen.PrimaryScreen.Bounds.Height - dy;
				// Располагаем в левой части экрана
				hwnd.Move(screenWidth / 2 + dx, 0, screenWidth / 2, screenHeight);
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		public static bool MoveWindowCenter(ProgramInfo programInfo)
		{
			int screenWidth;
			int screenHeight;
			bool result = true;

			try
			{
				var hwnd = wnd.find(1, programInfo.Title
									, programInfo.ClassName
									, programInfo.Owner).Activate();
				IntPtr hWndPtr = hwnd.Handle; // Предполагаем, что у hwnd есть свойство Handle для получения IntPtr
											  // Получаем размеры экрана
				screenWidth = Screen.PrimaryScreen.Bounds.Width;
				screenHeight = Screen.PrimaryScreen.Bounds.Height;
				// Получаем размеры окна
				RECT rect;

				if (GetWindowRect(hWndPtr, out rect))
				{
					int windowWidth = rect.Right - rect.Left;
					int windowHeight = rect.Bottom - rect.Top;

					// Располагаем окно в центре экрана
					hwnd.Move((screenWidth - windowWidth) / 2,
							  (screenHeight - windowHeight) / 2, windowWidth,
							  windowHeight);
				}
				else
				{
					result = false;
				}
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		//-------------------------------------------------
		public static bool MoveWindowLeft(int dx, int dy,
										  string name, string cn, WOwner of = default)
		{
			int screenWidth;
			int screenHeight;
			bool result = true;

			try
			{
				var hwnd = wnd.find(1, name, cn, of).Activate();
				// Получаем размеры экрана
				screenWidth = Screen.PrimaryScreen.Bounds.Width - dx;
				screenHeight = Screen.PrimaryScreen.Bounds.Height - dy;
				// Располагаем w1 в левой части экрана
				hwnd.Move(0, 0, screenWidth / 2, screenHeight);
			}
			catch (Exception ex)
			{
				lastErrorMessage = ex.Message;
				result = false;
			}
			return result;
		}

		public static bool MoveWindowRight(int dx, int dy,
										   string name, string cn, WOwner of = default)
		{
			int screenWidth;
			int screenHeight;
			bool result = true;

			try
			{
				var hwnd = wnd.find(1, name, cn, of).Activate();
				// Получаем размеры экрана
				screenWidth = Screen.PrimaryScreen.Bounds.Width - dx;
				screenHeight = Screen.PrimaryScreen.Bounds.Height - dy;
				// Располагаем в левой части экрана
				hwnd.Move(screenWidth / 2 + dx, 0, screenWidth / 2, screenHeight);
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		public static bool MoveWindowCenter(string name, string cn, WOwner of = default)
		{
			int screenWidth;
			int screenHeight;
			bool result = true;

			try
			{
				var hwnd = wnd.find(1, name, cn, of).Activate();
				IntPtr hWndPtr = hwnd.Handle; // Предполагаем, что у hwnd есть свойство Handle для получения IntPtr
											  // Получаем размеры экрана
				screenWidth = Screen.PrimaryScreen.Bounds.Width;
				screenHeight = Screen.PrimaryScreen.Bounds.Height;
				// Получаем размеры окна
				RECT rect;

				if (GetWindowRect(hWndPtr, out rect))
				{
					int windowWidth = rect.Right - rect.Left;
					int windowHeight = rect.Bottom - rect.Top;

					// Располагаем окно в центре экрана
					hwnd.Move((screenWidth - windowWidth) / 2,
							  (screenHeight - windowHeight) / 2, windowWidth,
							  windowHeight);
				}
				else
				{
					result = false;
				}
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		//----------------------------------------------

		public static void FindAndLocateWindows(int dx, int dy, (ProgramInfo, ProgramInfo) programInfo)
		{

			if (!MoveWindowLeft(dx, dy, programInfo.Item1))
			{
				System.Diagnostics.Process.Start($"{programInfo.Item1.Path}");
				MoveWindowLeft(dx, dy, programInfo.Item1);
			}
			if (!MoveWindowRight(dx, dy, programInfo.Item2))
			{
				System.Diagnostics.Process.Start($"{programInfo.Item2.Path}");
				MoveWindowRight(dx, dy, programInfo.Item2);
			}
		}

		public static void FindAndLocateWindow(ProgramInfo programInfo)
		{

			if (!MoveWindowCenter(programInfo))
			{
				System.Diagnostics.Process.Start($"{programInfo.Path}");
				MoveWindowCenter(programInfo);
			}
		}


		public static void FindAndLocateWindows(int dx, int dy, (string, string) path
						, string name1, string cn1, string name2, string cn2, WOwner of1 = default, WOwner of2 = default)
		{

			if (!MoveWindowLeft(dx, dy, name1, cn1, of1))
			{
				System.Diagnostics.Process.Start($"{path.Item1}");
				MoveWindowLeft(dx, dy, name1, cn1, of1);
			}
			if (!MoveWindowRight(dx, dy, name2, cn2, of2))
			{
				System.Diagnostics.Process.Start($"{path.Item2}");
				MoveWindowRight(dx, dy, name2, cn2, of2);
			}
		}

		public static void FindAndLocateWindow(string path, string name, string cn, WOwner of = default)
		{

			if (!MoveWindowCenter(name, cn, of))
			{
				System.Diagnostics.Process.Start($"{path}");
				MoveWindowCenter(name, cn, of);
			}
		}

		public class Program
		{

			// Импортируем необходимые функции из user32.dll
			[DllImport("user32.dll")]
			public static extern bool ReleaseCapture();
			[DllImport("user32.dll")]
			public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

			const int WM_NCLBUTTONDOWN = 0xA1;
			const int HTCAPTION = 0x2;

			const string TOTAL_COMMANDER_PATH = @"C:\Total Commander Extended\Totalcmd64.exe";
			const string OPERA_PATH = @"C:\Users\bgnic\AppData\Local\Programs\Opera\opera.exe";
			const string OPERA_CLASS_NAME = @"Chrome_WidgetWin_1";
			const string OPERA_TITLE = @"*";
			const string TODO_PATH = @"C:\Users\bgnic\AppData\Local\Swift\To-Do List\Swift To-Do List.exe";
			const string WORD_PATH = @"C:\Program Files\Microsoft Office\root\Office16\WINWORD.EXE";
			
			const string PYCHARM_PATH = @"C:\Program Files\JetBrains\PyCharm2024.3\bin\pycharm64.exe";
			const string SUBLIME_PATH = @"C:\Program Files\Sublime Text\sublime_text.exe";
			const string NETBEANS_PATH = @"C:\Program Files\NetBeans-23\netbeans\bin\netbeans64.exe";
			
			const string VISUAL_STUDIO_CODE_PATH = @"C:\Users\bgnic\AppData\Local\Programs\Microsoft VS Code\Code.exe";
			const string VISUAL_STUDIO_CODE_TITLE = @"*Visual Studio Code";
			const string VISUAL_STUDIO_CLASS_NAME = @"Chrome_WidgetWin_1";	
			const string VISUAL_STUDIO_PROGRAM = @"Code.exe";	

			const string TOTAL_COMMANDER_TITLE = "*Total Commander (x64) 11.03*";
			const string TOTAL_COMMANDER_CLASS_NAME = "TTOTAL_CMD";
			const string VISUAL_STUDIO_CODE_HEADER = "* Visual Studio Code";
			const string VISUAL_STUDIO_CODE_CLASS_NAME = "Chrome_WidgetWin_1";
			const string VISUAL_STUDIO_CODE_PROGRAM = "Code.exe";
			
			const string SKYPE_PATH = @"C:\Program Files\WindowsApps\Microsoft.SkypeApp_15.132.3201.0_x64__kzf8qxf38zg5c\Skype\Skype.exe";
			const string SKYPE_TITLE = "Skype";
			const string SKYPE_CLASS_NAME = "Chrome_WidgetWin_1";
			const string SKYPE_PROGRAM = "Skype.exe";

			const string SWIFT_TO_DO_LIST_HEADER = "Swift To-Do List 11*";
			const string SWIFT_TO_DO_LIST_CLASS_NAME = "*.Window.*;";
			const string SWIFT_TO_DO_LIST_PROGRAM = "";

			static void Main()
			{
				script.setup(trayIcon: true, sleepExit: true);
				var form = new Form();
				form.Text = "Helper";
				form.Size = new DrawingSize(400, 450);
				form.TopMost = true;

				form.Load += (sender, e) => form.Location = new DrawingPoint(3000, 1600);

				// Добавляем обработчик события MouseDown для формы
				form.MouseDown += (sender, e) =>
				{
					if (e.Button == MouseButtons.Left)
					{
						ReleaseCapture();
						SendMessage(form.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
					}
				};

				// Создаем TabControl и добавляем его на форму
				var tabControl = new TabControl();
				tabControl.Dock = DockStyle.Fill;

				// Создаем вкладки
				var tabPage1 = new TabPage("Вкладка 1");
				var tabPage2 = new TabPage("Вкладка 2");
				var tabPage3 = new TabPage("Вкладка 1");
				var tabPage4 = new TabPage("Вкладка 2");

				// Добавляем вкладки в TabControl
				tabControl.TabPages.Add(tabPage1);
				tabControl.TabPages.Add(tabPage2);
				tabControl.TabPages.Add(tabPage3);
				tabControl.TabPages.Add(tabPage4);

				//			AddMouseDownHandler(form, form);
				AddMouseDownHandler(tabPage1, form);
				AddMouseDownHandler(tabPage2, form);
				AddMouseDownHandler(tabPage3, form);
				AddMouseDownHandler(tabPage4, form);

				// Добавляем TabControl на форму
				form.Controls.Add(tabControl);

				var button1 =
					CreateButton(tabPage1, @"Total Commander and Browser",
								 new DrawingPoint(10, 10), (sender, e) => FindWindow1());
				var button2 =
					CreateButton(tabPage1, "TODO! and Total Commander",
								 new DrawingPoint(10, 50), (sender, e) => FindWindow2());
				var button3 = CreateButton(tabPage1, "MS Word", new DrawingPoint(10, 90),
										   (sender, e) => FindWindow3());
				var button4 =
					CreateButton(tabPage1, "Skype and PyCharm", new DrawingPoint(10, 130),
								 (sender, e) => FindWindow4());
				var button5 =
					CreateButton(tabPage1, "Sublime and PyCharm", new DrawingPoint(10, 170),
								 (sender, e) => FindWindow5());
				var button6 =
					CreateButton(tabPage1, "Skype and Sublime", new DrawingPoint(10, 210),
								 (sender, e) => FindWindow6());
				var button7 =
					CreateButton(tabPage1, "admin", new DrawingPoint(10, 250),
								 (sender, e) => Click1());
				var button8 =
					CreateButton(tabPage1, "Total Commander - Net Beans", new DrawingPoint(10, 290),
								 (sender, e) => FindWindow8());
				var button9 =
					CreateButton(tabPage1, "Sublime - Net Beans", new DrawingPoint(10, 330),
								 (sender, e) => FindWindow9());
				var button10 =
					CreateButton(tabPage2, "Skype - VS Code", new DrawingPoint(10, 10),
								 (sender, e) => FindWindow10());
				
				

				form.ShowDialog();
			}

			static void AddMouseDownHandler(Control control, Form form)
			{
				control.MouseDown += (sender, e) =>
				{
					if (e.Button == MouseButtons.Left)
					{
						ReleaseCapture();
						SendMessage(form.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
					}
				};

				foreach (Control child in control.Controls)
				{
					AddMouseDownHandler(child, form);
				}
			}

			#region CreateButton
			static Button CreateButton(Control parent, string buttonText,
										   DrawingPoint location,
										   EventHandler clickHandler)
			{
				var button = new Button();
				button.Text = buttonText;
				button.Location = location;
				button.AutoSize = true;
				button.Click += clickHandler;
				parent.Controls.Add(button);
				return button;
			}

			static void FindWindow1()
			{
				var totalCommander = new ProgramInfo(
						path: TOTAL_COMMANDER_PATH
						, title: TOTAL_COMMANDER_TITLE
						, className: TOTAL_COMMANDER_CLASS_NAME);
				var opera = new ProgramInfo(
						path: OPERA_PATH
						, title: "*"
						, className: OPERA_CLASS_NAME
						, owner: "opera.exe");
				
				FindAndLocateWindows(
				dx: 512, dy: 128, (totalCommander, opera));
			}

			static void FindWindow2()
			{

				FindAndLocateWindows(
					dx: 512,
					dy: 128,
					path: (TODO_PATH, TOTAL_COMMANDER_PATH),
					name1: SWIFT_TO_DO_LIST_HEADER,
					cn1: SWIFT_TO_DO_LIST_CLASS_NAME,
					name2: TOTAL_COMMANDER_TITLE,
					cn2: TOTAL_COMMANDER_CLASS_NAME,
					of1: default,
					of2: default
				);
			}

			static void FindWindow3()
			{

				FindAndLocateWindow(
					path: WORD_PATH,
					name: "*Word",
					cn: "OpusApp"
				);
			}

			static void FindWindow4()
			{

				FindAndLocateWindows(
					dx: 512,
					dy: 128,
					path: (SKYPE_PATH, PYCHARM_PATH),
					name1: "Skype",
					cn1: "Chrome_WidgetWin_1",
					name2: "*",
					cn2: "SunAwtFrame",
					of1: default,
					of2: default
				);
			}

			static void FindWindow5()
			{
				FindAndLocateWindows(
					dx: 512,
					dy: 128,
					path: (SUBLIME_PATH, PYCHARM_PATH),
					name1: "*",
					cn1: "PX_WINDOW_CLASS",
					name2: "*",
					cn2: "SunAwtFrame",
					of1: default,
					of2: default
				);
			}
			static void FindWindow6()
			{

				FindAndLocateWindows(
					dx: 512,
					dy: 128,
					path: (SKYPE_PATH, SUBLIME_PATH),
					name1: "Skype",
					cn1: "Chrome_WidgetWin_1",
					name2: "*",
					cn2: "PX_WINDOW_CLASS",
					of1: default,
					of2: default
					);
			}

			static void FindWindow8()
			{

				FindAndLocateWindows(
					dx: 512,
					dy: 128,
					path: (TOTAL_COMMANDER_PATH, NETBEANS_PATH),
					name1: TOTAL_COMMANDER_TITLE,
					cn1: "TTOTAL_CMD",
					name2: "Apache NetBeans IDE 23",
					cn2: "SunAwtFrame",
					of1: default,
					of2: "netbeans64.exe"
					);
			}

			static void FindWindow9()
			{

				FindAndLocateWindows(
					dx: 512,
					dy: 168,
					path: (SUBLIME_PATH, NETBEANS_PATH),
					name1: "*",
					cn1: "PX_WINDOW_CLASS",
					name2: "Apache NetBeans IDE 23",
					cn2: "SunAwtFrame",
					of1: default,
					of2: "netbeans64.exe"
					);
			}
			
			static void FindWindow10()
			{
				var skype = new ProgramInfo(
						path: SKYPE_PATH
						, title: SKYPE_TITLE
						, className: SKYPE_CLASS_NAME);
				var vscode = new ProgramInfo(
						path: VISUAL_STUDIO_CODE_PATH
						, title: VISUAL_STUDIO_CODE_TITLE
						, className: VISUAL_STUDIO_CODE_CLASS_NAME);
				
				FindAndLocateWindows(
				dx: 512, dy: 128, (skype, vscode));
			}
			
			
			

			static void Click1()
			{

				using (opt.scope.all())
				{ //recorded
					opt.mouse.MoveSpeed = opt.key.KeySpeed = opt.key.TextSpeed = 8;
					var w1 = wnd.find(0, "Киноафиша - Opera", "Chrome_WidgetWin_1").Activate();
					mouse.click(w1, 806, 238); /* grouping  *//*image:WkJNG7UIAMTnGZf/iHSvC/0qjLUt0k2oRAIbk9ra3hIkyx2/ho9p5FmHWeItqy1oCzhJuDAMMHBMKNAQNseaQdQgCE/vetcyHRHLkJzipG4EkKFKBDJ/d9/cFgM21ldRW1aMu4tzTI0M4vBgH98vz/j//EB/Zysy0SBaaqtQWJDF7s4WTo6P4HNYMD83jfL664AbUcst/N5Wk+UVvPJBOulARIBdy8ApHo8OaNxZQqEiTaYXfrUJ9uXZbzo3WXv5wGK5tgtQBAmhUK6lhgGieXnDsvLx+gE=*/

					keys.send("^фв=", "Back*3 ^admin", "Tab ^фвы=", "Ins*2 Back*4 ^фв=", "Back*3 ^admin", "Tab Enter");
				}
			}
			#endregion
		}
	}
}







