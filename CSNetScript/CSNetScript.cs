public class MyForm : System.Windows.Forms.Form
{
    public MyForm()
    {
		this.Text = "CSNetScript";
		var txtINPUT = new System.Windows.Forms.TextBox();
		txtINPUT.Name = "txtINPUT";
		this.Controls.Add(txtINPUT);
		txtINPUT.BringToFront();
		txtINPUT.Dock = System.Windows.Forms.DockStyle.Fill;
		txtINPUT.Multiline = true;
		txtINPUT.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		var crlf = "\r\n";
		var mt = "";
		var qs = "\"";
		var s = " ";
		var notice_msg = mt;
	    
		try
		{
			var search_subdirs = System.IO.SearchOption.AllDirectories;
			var search_folder = System.IO.SearchOption.TopDirectoryOnly;
			var app_filepath = System.Reflection.Assembly.GetExecutingAssembly().Location;
			var app_folder = System.IO.Path.GetDirectoryName(app_filepath);
			var win_dir = System.Environment.ExpandEnvironmentVariables(@"%WINDIR%\Microsoft.NET\Framework64");
			var env_parms = System.Environment.GetCommandLineArgs();
			var utf8_format = new System.Text.UTF8Encoding(encoderShouldEmitUTF8Identifier:false,throwOnInvalidBytes:true);
			var sourcecode_filepath = mt;
			var sourcecode_filelist = new System.Collections.Generic.List<string>();
			var newexe_filepath = mt;
			var csc_path = mt;
			var csc_notice_msg = mt;
			try
			{
				foreach (var entry in System.IO.Directory.GetFiles(win_dir, "csc.exe", search_subdirs))
				{
					csc_path = entry;
				};
			} catch (System.Exception ex_skip) {}
			
			if (csc_path == mt)
			{
				notice_msg = "CSC path not found in %WINDIR%";
			}
			
			if (notice_msg == mt)
			{
			    var parm_seq_b1 = 0;
				if (env_parms.Length > 1)
				{
					foreach (var entry in env_parms)
					{
						parm_seq_b1 += 1;
						if (parm_seq_b1 == 2)
						{
							sourcecode_filepath = entry;
						}
						if (parm_seq_b1 >= 2)
						{
						    sourcecode_filelist.Add(entry);
						}
					}
				}
				
				if (sourcecode_filepath == mt)
				{
					notice_msg = "Please pass in a source code file" + crlf;
					notice_msg += "csc_path: " + csc_path;
					var apptext_content =
@"https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.textbox?view=windowsdesktop-6.0

In a hard-drive folder
- UrCSFilePath = %HOMEPATH%\Downloads\UrProgram.cs
- UrCMDFilePath = %HOMEPATH%\Downloads\UrProgramCompile.cmd
- UrEXEFilePath = %HOMEPATH%\Downloads\UrProgram.exe

[Notepad]
- [New text file]
- Paste in program text

Command-line program:
```
public class Program{public static void Main(string [] args){System.Console.WriteLine(""UrText"");}}
```

Windows program:
```
public class MyForm : System.Windows.Forms.Form
{
    public MyForm()
    {
		this.Text = ""C# Program"";
		var txtINPUT = new System.Windows.Forms.TextBox();
		txtINPUT.Name = ""txtINPUT"";
		this.Controls.Add(txtINPUT);
		txtINPUT.BringToFront();
		txtINPUT.Dock = System.Windows.Forms.DockStyle.Fill;
		txtINPUT.Multiline = true;
		txtINPUT.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		txtINPUT.Text = ""UrText"";
		//System.Windows.Forms.MessageBox.Show(""UrText"");
		//this.Close();
    }

    public static void Main()
    {
        System.Windows.Forms.Application.Run(new MyForm());
    }
}
```

- Save file as UrCSFilePath


[Command Line prompt]
- Execute command:

```
dir /s /b %WINDIR%\csc.exe
```

- Note: Output line like:

```
c:\windows\Microsoft.NET\Framework64\v4.0.30319
```

- UrFramework64Path = the found folder
- Copy UrFramework64Path to the Windows clipboard


[Notepad]
- [New text file]
- Paste in program

Compile command-line program:
```
del UrNewEXEFilePath
UrFramework64Path\csc.exe /target:exe /out:UrNewEXEFilePath UrUrCSFilePath
start """" UrNewEXEFilePath
```

Compile Windows program:
```
del UrNewEXEFilePath
UrFramework64Path\csc.exe /target:winexe /out:UrNewEXEFilePath UrUrCSFilePath
start """" UrNewEXEFilePath
```

- Save file as UrCMDFilePath


[Command Line prompt]
- Execute command: UrCMDFilePath
- Note: If command-line program, UrText is printed on a line of the console window
- Note: If Windows program, UrText is in a Windows Form textbox



Make a windows program to compile a source code file:

[CSNetScript.cmd]
```
del CSNetScript.exe
UrFramework64Path\csc.exe /target:winexe /out:CSNetScript.exe CSNetScript.cs
start """" CSNetScript.exe
```


[CSNetScript.cs]
```
public class MyForm : System.Windows.Forms.Form
{
    public MyForm()
    {
		this.Text = ""CSNetScript"";
		var txtINPUT = new System.Windows.Forms.TextBox();
		txtINPUT.Name = ""txtINPUT"";
		this.Controls.Add(txtINPUT);
		txtINPUT.BringToFront();
		txtINPUT.Dock = System.Windows.Forms.DockStyle.Fill;
		txtINPUT.Multiline = true;
		txtINPUT.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		var crlf = ""\r\n"";
		var mt = """";
		var qs = ""\"""";
		var s = "" "";
		var notice_msg = mt;
	    
		try
		{
			var search_subdirs = System.IO.SearchOption.AllDirectories;
			var search_folder = System.IO.SearchOption.TopDirectoryOnly;
			var app_filepath = System.Reflection.Assembly.GetExecutingAssembly().Location;
			var app_folder = System.IO.Path.GetDirectoryName(app_filepath);
			var win_dir = System.Environment.ExpandEnvironmentVariables(@""%WINDIR%\Microsoft.NET\Framework64"");
			var env_parms = System.Environment.GetCommandLineArgs();
			var utf8_format = new System.Text.UTF8Encoding(encoderShouldEmitUTF8Identifier:false,throwOnInvalidBytes:true);
			var sourcecode_filepath = mt;
			var sourcecode_filelist = new System.Collections.Generic.List<string>();
			var newexe_filepath = mt;
			var csc_path = mt;
			var csc_notice_msg = mt;
			try
			{
				foreach (var entry in System.IO.Directory.GetFiles(win_dir, ""csc.exe"", search_subdirs))
				{
					csc_path = entry;
				};
			} catch (System.Exception ex_skip) {}
			
			if (csc_path == mt)
			{
				notice_msg = ""CSC path not found in %WINDIR%"";
			}
			
			if (notice_msg == mt)
			{
			    var parm_seq_b1 = 0;
				if (env_parms.Length > 1)
				{
					foreach (var entry in env_parms)
					{
						parm_seq_b1 += 1;
						if (parm_seq_b1 == 2)
						{
							sourcecode_filepath = entry;
						}
						if (parm_seq_b1 >= 2)
						{
						    sourcecode_filelist.Add(entry);
						}
					}
				}
				
				if (sourcecode_filepath == mt)
				{
					notice_msg = ""Please pass in a source code file"" + crlf;
					notice_msg += ""csc_path: "" + csc_path;
					var apptext_content =
@"""";
					if (string.IsNullOrWhiteSpace(apptext_content) == false)
					{
						var appfile_basegroup = System.IO.Path.GetFileNameWithoutExtension(app_filepath);
						var apptext_filepath = System.IO.Path.Combine(app_folder, appfile_basegroup + "".cs.txt"");
						var file_exists = false;
						try
						{
							file_exists = System.IO.File.Exists(apptext_filepath);
						} catch (System.Exception ex_skip) {}
						
						if (file_exists == false)
						{
							System.IO.File.WriteAllText(
								apptext_filepath,
								apptext_content,
								utf8_format);
						}
					}
				}
			}
				
			if (notice_msg == mt)
			{
				foreach (var entry in sourcecode_filelist)
				{
					var file_exists = false;
					try
					{
						file_exists = System.IO.File.Exists(entry);
					} catch (System.Exception ex_skip) {}
					
					if (file_exists == false)
					{
						notice_msg += ""filename not found: "" + entry + crlf;
					}
				}
			}
				
			if (notice_msg == mt)
			{
				var parent_folder = System.IO.Path.GetDirectoryName(sourcecode_filepath);
				var file_basegroup = System.IO.Path.GetFileNameWithoutExtension(sourcecode_filepath);
				newexe_filepath = System.IO.Path.Combine(parent_folder, file_basegroup + "".exe"");
				var file_exists = false;
				try
				{
					file_exists = System.IO.File.Exists(newexe_filepath);
				} catch (System.Exception ex_skip) {}
				
				if (file_exists)
				{
					System.IO.File.Delete(newexe_filepath);
				}
				
				//write out .cmd
				//del %HOMEPATH%\Downloads\csc.exe
				//System.Diagnostics.Process.Start( csc_path /target:winexe /out:app_folder\csc.exe sourcecode_filepath
				var sourcecode_fileargs = mt;
				foreach (var entry in sourcecode_filelist)
				{
				    sourcecode_fileargs += s + qs + entry + qs;
				}
				
				var csc_args = ""/target:winexe /out:"" + newexe_filepath + sourcecode_fileargs;
				var startInfo = new System.Diagnostics.ProcessStartInfo();
				startInfo.FileName = csc_path;
				startInfo.Arguments = csc_args;
				startInfo.RedirectStandardOutput = true;
				startInfo.RedirectStandardError = true;
				startInfo.UseShellExecute = false;
				startInfo.CreateNoWindow = true;

				var processTemp = new System.Diagnostics.Process();
				processTemp.StartInfo = startInfo;
				processTemp.EnableRaisingEvents = true;
				try
				{
					processTemp.Start();
					processTemp.WaitForExit();
					csc_notice_msg = processTemp.StandardError.ReadToEnd();
					if (csc_notice_msg == mt)
					{
						csc_notice_msg = processTemp.StandardOutput.ReadToEnd();
					}
				} catch (System.Exception e) {throw;}
				
				file_exists = false;
				try
				{
					file_exists = System.IO.File.Exists(newexe_filepath);
				} catch (System.Exception ex_skip) {}
				
				if (file_exists == false)
				{
					notice_msg = csc_notice_msg + crlf + csc_args;
				}
			}
			
			if (notice_msg == mt)
			{
				//notice_msg = csc_notice_msg + System.Environment.NewLine + newexe_filepath;
				System.Diagnostics.Process.Start(newexe_filepath);
				this.Close();
			}
		}
		catch (System.Exception ex)
		{
		    notice_msg = ex.Message;
		}
		
		txtINPUT.Text = notice_msg;
		//System.Windows.Forms.MessageBox.Show(""UrText"");
		//this.Close();
    }

    public static void Main()
    {
        System.Windows.Forms.Application.Run(new MyForm());
    }
}
```
";
					if (string.IsNullOrWhiteSpace(apptext_content) == false)
					{
						var appfile_basegroup = System.IO.Path.GetFileNameWithoutExtension(app_filepath);
						var apptext_filepath = System.IO.Path.Combine(app_folder, appfile_basegroup + ".cs.txt");
						var file_exists = false;
						try
						{
							file_exists = System.IO.File.Exists(apptext_filepath);
						} catch (System.Exception ex_skip) {}
						
						if (file_exists == false)
						{
							System.IO.File.WriteAllText(
								apptext_filepath,
								apptext_content,
								utf8_format);
						}
					}
				}
			}
				
			if (notice_msg == mt)
			{
				foreach (var entry in sourcecode_filelist)
				{
					var file_exists = false;
					try
					{
						file_exists = System.IO.File.Exists(entry);
					} catch (System.Exception ex_skip) {}
					
					if (file_exists == false)
					{
						notice_msg += "filename not found: " + entry + crlf;
					}
				}
			}
				
			if (notice_msg == mt)
			{
				var parent_folder = System.IO.Path.GetDirectoryName(sourcecode_filepath);
				var file_basegroup = System.IO.Path.GetFileNameWithoutExtension(sourcecode_filepath);
				newexe_filepath = System.IO.Path.Combine(parent_folder, file_basegroup + ".exe");
				var file_exists = false;
				try
				{
					file_exists = System.IO.File.Exists(newexe_filepath);
				} catch (System.Exception ex_skip) {}
				
				if (file_exists)
				{
					System.IO.File.Delete(newexe_filepath);
				}
				
				//write out .cmd
				//del %HOMEPATH%\Downloads\csc.exe
				//System.Diagnostics.Process.Start( csc_path /target:winexe /out:app_folder\csc.exe sourcecode_filepath
				var sourcecode_fileargs = mt;
				foreach (var entry in sourcecode_filelist)
				{
				    sourcecode_fileargs += s + qs + entry + qs;
				}
				
				var csc_args = "/target:winexe /out:" + newexe_filepath + sourcecode_fileargs;
				var startInfo = new System.Diagnostics.ProcessStartInfo();
				startInfo.FileName = csc_path;
				startInfo.Arguments = csc_args;
				startInfo.RedirectStandardOutput = true;
				startInfo.RedirectStandardError = true;
				startInfo.UseShellExecute = false;
				startInfo.CreateNoWindow = true;

				var processTemp = new System.Diagnostics.Process();
				processTemp.StartInfo = startInfo;
				processTemp.EnableRaisingEvents = true;
				try
				{
					processTemp.Start();
					processTemp.WaitForExit();
					csc_notice_msg = processTemp.StandardError.ReadToEnd();
					if (csc_notice_msg == mt)
					{
						csc_notice_msg = processTemp.StandardOutput.ReadToEnd();
					}
				} catch (System.Exception e) {throw;}
				
				file_exists = false;
				try
				{
					file_exists = System.IO.File.Exists(newexe_filepath);
				} catch (System.Exception ex_skip) {}
				
				if (file_exists == false)
				{
					notice_msg = csc_notice_msg + crlf + csc_args;
				}
			}
			
			if (notice_msg == mt)
			{
				//notice_msg = csc_notice_msg + System.Environment.NewLine + newexe_filepath;
				System.Diagnostics.Process.Start(newexe_filepath);
				this.Close();
			}
		}
		catch (System.Exception ex)
		{
		    notice_msg = ex.Message;
		}
		
		txtINPUT.Text = notice_msg;
		//System.Windows.Forms.MessageBox.Show("UrText");
		//this.Close();
    }

    public static void Main()
    {
        System.Windows.Forms.Application.Run(new MyForm());
    }
}
