/*/ role exeProgram; outputPath %folders.Workspace%\exe\Helper_002; nuget -\YamlDotNet; /*/

using System;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using System.Windows.Forms;

#pragma warning disable CS1591  // Missing XML comment for publicly visible type
// or member

namespace main {
	
	public class ProgramInfo {
		public string Path { get; set; }
		public string Title { get; set; }
		public string ClassName { get; set; }
		public string Owner { get; set; }
	}

	public class Config {
		public Dictionary<int, ProgramInfo> Programs { get; set; }
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
			
			static void Main(string[] args) {
				script.setup(trayIcon: true, sleepExit: true);
				// Получаем путь к каталогу, в котором находится исполняемый файл
				string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
				string configFilePath = Path.Combine(baseDirectory, "config.yaml");
				
				var deserializer = new DeserializerBuilder()
				.WithNamingConvention(CamelCaseNamingConvention.Instance).Build();

				try { 	
					Config config;
					using (var reader = new StreamReader(configFilePath))
					{
						config = deserializer.Deserialize<Config>(reader);
					}
					
						foreach (var arg in args) {
							if (int.TryParse(arg, out int param) && config.Programs.ContainsKey(param)) {
								var programInfo = config.Programs[param];
								WindowHelper.FindAndLocateWindow(programInfo);
							} else {
								Console.WriteLine($"Неверный или неизвестный параметрпараметр: {arg}");
							}
						}
				}
				catch (Exception ex) {
					Console.WriteLine(ex.Message);
				}
			}
		}
	}
}
