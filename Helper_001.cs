/*/ role exeProgram; outputPath %folders.Workspace%\exe\Helper_001; nuget -\System.Runtime.InteropServices; nuget -\Microsoft.Office.Interop.Word; /*/
//Version 1.0.0.3
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DrawingSize = System.Drawing.Size;
using DrawingPoint = System.Drawing.Point;
using MessageBox = System.Windows.Forms.MessageBox;
using System.Diagnostics;
using main;
using System.Reflection.Metadata;

#pragma warning disable CS1591  // Missing XML comment for publicly visible type
// or member

namespace main {
	
	public class ProgramInfo {
		public string Path { get; }
		public string Title { get; }
		public string ClassName { get; }
		public WOwner Owner { get; }
		
		// Конструктор
		public ProgramInfo(string path
				, string title
				, string className
				, WOwner owner = default) {
			Title = title;
			ClassName = className;
			Owner = owner;
			Path = path;
		}
	}
	
	public static class WindowHelper {
		public static string lastErrorMessage = "";
		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
		
		[StructLayout(LayoutKind.Sequential)]
		public struct RECT {
			public int Left;
			public int Top;
			public int Right;
			public int Bottom;
		}
		
		public static bool MoveWindowToPosition(int pos_x, int pos_y, int size_x, int size_y, ProgramInfo programInfo) {
			bool result = true;
			try {
				var hwnd = wnd.find(1, programInfo.Title, programInfo.ClassName, programInfo.Owner).Activate();
				// Перемещаем окно в заданную позицию и устанавливаем размеры
				hwnd.Move(pos_x, pos_y, size_x, size_y);
			}
			catch (Exception ex) {
				lastErrorMessage = ex.Message;
				result = false;
			}
			return result;
		}
		public static bool MoveWindowLeft(int dx, int dy, ProgramInfo programInfo) {
			int screenWidth;
			int screenHeight;
			bool result = true;
			
			try {
				var hwnd = wnd.find(1, programInfo.Title
									, programInfo.ClassName
									, programInfo.Owner).Activate();
				// Получаем размеры экрана
				screenWidth = Screen.PrimaryScreen.Bounds.Width - dx;
				screenHeight = Screen.PrimaryScreen.Bounds.Height - dy;
				// Располагаем w1 в левой части экрана
				hwnd.Move(0, 0, screenWidth / 2, screenHeight);
			}
			catch (Exception ex) {
				lastErrorMessage = ex.Message;
				result = false;
			}
			return result;
		}
		
		public static bool MoveWindowRight(int dx, int dy, ProgramInfo programInfo) {
			int screenWidth;
			int screenHeight;
			bool result = true;
			
			try {
				var hwnd = wnd.find(1, programInfo.Title
									, programInfo.ClassName
									, programInfo.Owner).Activate();
				// Получаем размеры экрана
				screenWidth = Screen.PrimaryScreen.Bounds.Width - dx;
				screenHeight = Screen.PrimaryScreen.Bounds.Height - dy;
				// Располагаем в левой части экрана
				hwnd.Move(screenWidth / 2 + dx, 0, screenWidth / 2, screenHeight);
			}
			catch (Exception) {
				result = false;
			}
			return result;
		}
		
		public static bool MoveWindowCenter(ProgramInfo programInfo) {
			int screenWidth;
			int screenHeight;
			bool result = true;
			
			try {
				var hwnd = wnd.find(1, programInfo.Title
									, programInfo.ClassName
									, programInfo.Owner).Activate();
				IntPtr hWndPtr = hwnd.Handle; // Предполагаем, что у hwnd есть свойство Handle для получения IntPtr
				// Получаем размеры экрана
				screenWidth = Screen.PrimaryScreen.Bounds.Width;
				screenHeight = Screen.PrimaryScreen.Bounds.Height;
				// Получаем размеры окна
				RECT rect;
				
				if (GetWindowRect(hWndPtr, out rect)) {
					int windowWidth = rect.Right - rect.Left;
					int windowHeight = rect.Bottom - rect.Top;
					
					// Располагаем окно в центре экрана
					hwnd.Move((screenWidth - windowWidth) / 2,
							  (screenHeight - windowHeight) / 2, windowWidth,
							  windowHeight);
				} else {
					result = false;
				}
			}
			catch (Exception) {
				result = false;
			}
			return result;
		}
		public static void FindAndLocateWindows(int dx, int dy, (ProgramInfo, ProgramInfo) programInfo) {
			
			if (!MoveWindowLeft(dx, dy, programInfo.Item1)) {
				System.Diagnostics.Process.Start($"{programInfo.Item1.Path}");
				MoveWindowLeft(dx, dy, programInfo.Item1);
			}
			if (!MoveWindowRight(dx, dy, programInfo.Item2)) {
				System.Diagnostics.Process.Start($"{programInfo.Item2.Path}");
				MoveWindowRight(dx, dy, programInfo.Item2);
			}
		}
		
		public static void FindAndLocateWindow(ProgramInfo programInfo) {
			
			if (!MoveWindowCenter(programInfo)) {
				System.Diagnostics.Process.Start($"{programInfo.Path}");
				MoveWindowCenter(programInfo);
			}
		}
		
		public class Program {
			
			const string OBSIDIAN_PATH = @"C:\Program Files\Obsidian\Obsidian.exe";
			const string OBSIDIAN_CLASS_NAME = @"Chrome_WidgetWin_1";
			const string OBSIDIAN_TITLE = @"*Obsidian*";
			const string OBSIDIAN_OWNER = @"Obsidian.exe";
			
			const string OPERA_PATH = @"C:\Users\bgnic\AppData\Local\Programs\Opera\opera.exe";
			const string OPERA_CLASS_NAME = @"Chrome_WidgetWin_1";
			const string OPERA_TITLE = @"*";
			const string OPERA_OWNER = @"opera.exe";
			
			const string TODO_PATH = @"C:\Users\bgnic\AppData\Local\Swift\To-Do List\Swift To-Do List.exe";
			const string WORD_PATH = @"C:\Program Files\Microsoft Office\root\Office16\WINWORD.EXE";
			const string WORD_CLASS_NAME = @"OpusApp";
			const string WORD_TITLE = @"*Word";
			const string WORD_OWNER = "WINWORD.EXE";
			
			const string PYCHARM_PATH = @"C:\Program Files\JetBrains\PyCharm2024.3\bin\pycharm64.exe";
			const string SUBLIME_PATH = @"C:\Program Files\Sublime Text\sublime_text.exe";
			
			const string NETBEANS_PATH = @"C:\Program Files\NetBeans-23\netbeans\bin\netbeans64.exe";
			const string NETBEANS_CLASS_NAME = @"SunAwtFrame";
			const string NETBEANS_TITLE = @"*Apache NetBeans*";
			const string NETBEANS_OWNER = @"netbeans64.exe";
			
			const string VISUAL_STUDIO_CODE_TITLE = @"*Visual Studio Code";
			const string VISUAL_STUDIO_CLASS_NAME = @"Chrome_WidgetWin_1";
			const string VISUAL_STUDIO_OWNER = @"Code.exe";
			
			const string TOTAL_COMMANDER_PATH = @"C:\Total Commander Extended\Totalcmd64.exe";
			const string TOTAL_COMMANDER_TITLE = "*Total Commander (x64) 11.03*";
			const string TOTAL_COMMANDER_CLASS_NAME = "TTOTAL_CMD";
			
			const string VISUAL_STUDIO_CODE_PATH = @"C:\Users\bgnic\AppData\Local\Programs\Microsoft VS Code\Code.exe";
			const string VISUAL_STUDIO_CODE_HEADER = "* Visual Studio Code";
			const string VISUAL_STUDIO_CODE_CLASS_NAME = "Chrome_WidgetWin_1";
			const string VISUAL_STUDIO_CODE_OWNER = "Code.exe";
			
			const string SKYPE_PATH = @"C:\Program Files\WindowsApps\Microsoft.SkypeApp_15.132.3201.0_x64__kzf8qxf38zg5c\Skype\Skype.exe";
			const string SKYPE_TITLE = "Skype*";
			const string SKYPE_CLASS_NAME = "Chrome_WidgetWin_1";
			const string SKYPE_PROGRAM = "Skype.exe";
			
			const string SWIFT_TO_DO_LIST_PATH = @"C:\Users\bgnic\AppData\Local\Swift To-Do List\Swift To-Do List.exe";
			const string SWIFT_TO_DO_LIST_TITLE = "Swift To-Do List 11*";
			const string SWIFT_TO_DO_LIST_CLASS_NAME = "*.Window.*;";
			const string SWIFT_TO_DO_LIST_OWNER = "";
			
			const string FAR_MANAGER_PATH = @"C:\Program Files\ConEmu\ConEmu64.exe";
			const string FAR_MANAGER_TITLE = @"*Far*";
			const string FAR_MANAGER_CLASS_NAME = @"VirtualConsoleClass";
			const string FAR_MANAGER_OWNER = @"ConEmu64.exe";
			
			const string EMUL_PATH = @"K:\VK_Projects\Василий Воронин\Assembler 80\(1)";
			const string EMUL_TITLE = @"Модель лабораторного стенда УМПК-80";
			const string EMUL_CLASS_NAME = @"TLForm1";
			const string EMUL_OWNER = @"K580BM80.EXE";
			
			static void Main(string[] args) {
				script.setup(trayIcon: true, sleepExit: true);
				foreach (var arg in args) {
					if (int.TryParse(arg, out int param)) {
						switch (param) {
						case 1: {
									var far = new ProgramInfo(
											path: FAR_MANAGER_PATH
											, title: FAR_MANAGER_TITLE
											, className: FAR_MANAGER_CLASS_NAME
											, owner: FAR_MANAGER_OWNER);
											FindAndLocateWindow(far);
								Console.WriteLine("Far manager...");
							}
							
							break;
						case 2:
							Console.WriteLine("Обработка параметра 2");
							break;
						// Добавьте другие случаи по мере необходимости
						default:
							Console.WriteLine($"Неизвестный параметр: {param}");
							break;
						}
					} else {
						Console.WriteLine($"Неверный параметр: {arg}");
					}
					
				}
			}
			
			
/*			static void FindWindow3() {
				
				var word = new ProgramInfo(
						path: WORD_PATH
						, title: WORD_TITLE
						, className: WORD_CLASS_NAME
						, owner: WORD_OWNER);
				FindAndLocateWindow(word);
			}
*/
			
		}
	}
}





